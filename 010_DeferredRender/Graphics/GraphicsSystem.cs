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

        public FrameBufferManager FrameBuf { get; set; }

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
            Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 400);

            Shaders.InitTexturelessNoLight();
            Shaders.InitTexturedNoLight();
            Shaders.InitSecondGBufferPassProgram();
            Shaders.InitOneQuadProgramProgram();
            Shaders.InitLightVolumeProgram();

            FrameBuf = new FrameBufferManager(_width, _height);
        }

        internal void Render(List<SimpleModel> models, List<SimpleModel> lights)
        {
            GL.Enable(EnableCap.DepthTest);
            FrameBuf.EnableMainFrameBuffer();
            RenderToCurrentTarget(models);
            FrameBuf.DisableMainFrameBuffer();
            DrawUsingGBuffer(lights);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        private void RenderToCurrentTarget(List<SimpleModel> models)
        {
            RebuildMatrices();

            GL.ClearColor(0, 0f, 0.1f, 1f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (var model in models)
            {
                model.Bind(ModelView, ModelViewProjection, Projection);

                GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);
            }
        }


        public void DrawUsingGBuffer(List<SimpleModel> lights)
        {
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(0, 0f, 0f, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);

            List<SimpleModel> modelsTemp = ConvertLightsToViewSpace(lights);

            Shaders.BindOneQuadScreenAndDraw(FrameBuf, _player.Position);

            Shaders.PrepareToDrawLights(FrameBuf, ModelView, ModelViewProjection);

            foreach (var light in modelsTemp)
            {
                // shift model to the light position
                Shaders.DrawLight(light, light.Vertices[0], light.Colors[0]);
            }


            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Flush();
        }

        private List<SimpleModel> ConvertLightsToViewSpace(List<SimpleModel> lights)
        {
            var result = new List<SimpleModel>(lights.Count * 2);

            for (int i = 0; i < lights.Count; i++)
            {
                var l = lights[i];

                var pos = new Vector4(l.Vertices[0], 1);
                var trans = Vector4.Transform(pos, ModelView);

                result.Add(new SimpleModel()
                {
                    Colors = l.Colors,
                    Normals = l.Normals,
                    Vertices = new[] { trans.Xyz },
                });

            }

            return result;
        }

        private void RebuildMatrices()
        {
            ModelView = Matrix4.LookAt(_player.Position, _player.Target, Vector3.UnitY);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }
    }
}