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

            FrameBuf = new FrameBufferManager(_width, _height);
        }

        internal void Render(List<SimpleModel> models, List<SimpleModel> lights)
        {
            GL.Enable(EnableCap.DepthTest);
            FrameBuf.EnableMainFrameBuffer();
            RenderToCurrentTarget(models, lights);
            FrameBuf.DisableMainFrameBuffer();
            DrawUsingGBuffer(lights);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <param name="lights">is not used</param>
        private void RenderToCurrentTarget(List<SimpleModel> models, List<SimpleModel> lights)
        {
            RebuildMatrices();

            GL.ClearColor(0, 0f, 0.1f, 1f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (var model in models)
            {
                model.Bind(ModelView, ModelViewProjection, Projection);

                GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);
            }

            GL.Flush();

        }


        public void DrawUsingGBuffer(List<SimpleModel> lights)
        {
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(0, 0f, 0f, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            var modelsTemp = lights.Select(l => new SimpleModel()
            {
                Colors = l.Colors,
                Normals = l.Normals,
                Vertices = new[] { Vector3.Transform(l.Vertices[0], ModelView) },
            })
            .ToList();

            Shaders.BindOneQuadScreenAndDraw(FrameBuf, _player.Position, modelsTemp);

            GL.Flush();
        }


        private void RebuildMatrices()
        {
            ModelView = Matrix4.LookAt(_player.Position, _player.Target, Vector3.UnitY);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }
    }
}