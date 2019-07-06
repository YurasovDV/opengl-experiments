using System;
using System.Linq;
using System.Collections.Generic;
using Common;
using DeferredRender.Graphics.FrameBuffer;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics
{
    internal class GraphicsSystem
    {
        private int _height;
        private int _width;
        private Player _player;

        public Matrix4 Projection;
        public Matrix4 ModelView;
        public Matrix4 ModelViewProjection;

        public SimpleModel LightVolume { get; set; }

        public FrameBufferManager FrameBufferManager { get; set; }

        public GraphicsSystem(int width, int height, Player player)
        {
            _width = width;
            _height = height;
            _player = player;
            InitGraphics();
        }

        private void InitGraphics()
        {
            float aspect = (float)_width / _height;
            GL.Viewport(0, 0, _width, _height);
            Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 600);

            Shaders.InitTexturelessNoLight();
            Shaders.InitTexturedNoLight();
            Shaders.InitLightVolumeProgram();
            Shaders.InitFinalPassProgram();
            Shaders.InitOneQuadProgramProgram();

            FrameBufferManager = new FrameBufferManager(_width, _height);
        }

        internal void Render(List<SimpleModel> models, List<PointLight> lights)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            FrameBufferManager.EnableMainFrameBuffer();
            RenderToCurrentTarget(models, lights);
            FrameBufferManager.DisableMainFrameBuffer();

            FrameBufferManager.EnableSecondFrameBuffer();
            PerformLightingDrawCall(lights);
            FrameBufferManager.DisableSecondFrameBuffer();

            DrawUsingGBuffer();
        }

        /// <summary>
        /// second step
        /// </summary>
        /// <param name="lights"></param>
        private void PerformLightingDrawCall(List<PointLight> lights)
        {
            // GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.DepthTest);

            // we don't write to depth buffer
            GL.DepthMask(false);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);

            //GL.Enable(EnableCap.StencilTest);

            ClearColor();
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Shaders.PrepareToDrawLights(FrameBufferManager, ModelView, ModelViewProjection, LightVolume.Vertices);
            int len = LightVolume.Vertices.Length;
            foreach (var light in lights)
            {
                Shaders.DrawLight(light, len);
            }

            //GL.Disable(EnableCap.StencilTest);
        }

        /// <summary>
        /// third step
        /// </summary>
        public void DrawUsingGBuffer()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(0, 0f, 0f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.CullFace(CullFaceMode.Back);

            Shaders.BindOneQuadScreenAndDraw(FrameBufferManager, _player.Position);

            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Flush();
        }

        private void ClearColor()
        {
            GL.ClearColor(0, 0f, 0f, 1);
        }

        /// <summary>
        /// render all models using their shaders
        /// </summary>
        /// <param name="models"></param>
        private void RenderToCurrentTarget(List<SimpleModel> models, List<PointLight> lights)
        {
            RebuildMatrices();

            ClearColor();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (var model in models)
            {
                model.Bind(ModelView, ModelViewProjection, Projection);

                GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);
            }
            bool isFirst = true;
            foreach (var light in lights)
            {
                var mv = Matrix4.CreateTranslation(0, -0.05f, 0) * light.Transform;

                Shaders.BindTexturelessNoLightWithSharedBuffer(light.DenotationCube, mv * ModelView, mv * ModelViewProjection, Projection, isFirst);

                GL.DrawArrays(PrimitiveType.Triangles, 0, light.DenotationCube.Vertices.Length);
                isFirst = false;
            }
        }

        private void RebuildMatrices()
        {
            ModelView = Matrix4.LookAt(_player.Position, _player.Target, Vector3.UnitY);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }
    }
}