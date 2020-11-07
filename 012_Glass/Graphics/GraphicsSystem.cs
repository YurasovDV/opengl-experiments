using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Glass.Graphics
{
    internal class GraphicsSystem
    {
        private int _height;
        private int _width;
        private Player _player;

        public Matrix4 Projection;
        public Matrix4 ModelView;
        public Matrix4 ModelViewProjection;

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
            Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 200);

            Shaders.InitTexturelessNoLight();
            Shaders.InitRenderWithEnvironmentMap();

            FrameBufferManager = new FrameBufferManager(_width, _height);
        }

        public void Render(List<SimpleModel> models, List<SimpleModel> reflectiveModels)
        {
            FrameBufferManager.EnableReflectionsFrameBuffer();
            var cubeMap = FrameBufferManager.ReflectionsMapFrameBufferDescriptor.DiffuseTextureId;

            for (int i = 0; i < 6; i++)
            {

                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, 
                    TextureTarget.TextureCubeMapPositiveX + i, cubeMap, 0);

                // камера на 90 градусов и выбрать сторону
                RebuildMatrices();

                GL.ClearColor(0, 0, 0.0f, 1);

                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                Render(models);
            }


            FrameBufferManager.DisableReflectionsFrameBuffer();

            DrawUsingFrameBuffer(models, reflectiveModels);

            GL.Flush();
        }

        private void Render(List<SimpleModel> models)
        {
            var combinedModel = new SimpleModel();

            combinedModel.Colors = models.SelectMany(m => m.Colors).ToArray();
            combinedModel.Vertices = models.SelectMany(m => m.Vertices).ToArray();

            Shaders.BindTexturelessNoLight(combinedModel, ModelView, ModelViewProjection, Projection);

            GL.DrawArrays(PrimitiveType.Triangles, 0, combinedModel.Vertices.Length);
        }

        private void Render(SimpleModel model)
        {
            Shaders.BindTexturelessNoLight(model, ModelView, ModelViewProjection, Projection);

            GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);
        }

        private void DrawUsingFrameBuffer(List<SimpleModel> models, List<SimpleModel> reflectiveModels)
        {
            RebuildMatrices();

            GL.ClearColor(0, 0, 0.0f, 1);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var model in models)
            {
                Render(model);
            }            
            
            foreach (var model in reflectiveModels)
            {
                RenderWithEnvMap(model);
            }
        }

        private void RenderWithEnvMap(SimpleModel model)
        {
            Shaders.BindWithEnvironmentMap(model, ModelView, ModelViewProjection, Projection);
            GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);
        }

        private void RebuildMatrices()
        {
            ModelView = Matrix4.LookAt(_player.Position, _player.Target, Vector3.UnitY);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }
    }
}