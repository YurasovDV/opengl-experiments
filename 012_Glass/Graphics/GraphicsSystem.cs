using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
// using GL4 = OpenTK.Graphics.OpenGL4.GL;

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


         private OpenTK.Graphics.Color4 ClearColor = new OpenTK.Graphics.Color4(255, 165, 0, 1);

        public FrameBufferManager FrameBufferManager { get; set; }
        public float Aspect { get; private set; }

        public GraphicsSystem(int width, int height, Player player)
        {
            _width = width;
            _height = height;
            _player = player;
            InitGraphics();
        }

        private void InitGraphics()
        {
            Aspect = (float)_width / _height;
            GL.Viewport(0, 0, _width, _height);
            GL.Enable(EnableCap.DepthTest);
            Shaders.InitTexturelessNoLight();
            Shaders.InitRenderWithEnvironmentMap();

            FrameBufferManager = new FrameBufferManager(_width, _height);
        }

        public void Render(List<SimpleModel> models, List<SimpleModel> reflectiveModels)
        {
            FrameBufferManager.EnableReflectionsFrameBuffer();
            var cubeMap = FrameBufferManager.ReflectionsMapFrameBufferDescriptor.DiffuseTextureId;
            var reflective = reflectiveModels.Single();
            var center = new Vector3(0, 3, 0);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment0,
                TextureTarget.TextureCubeMapPositiveX,
                cubeMap,
                0);
            RebuildMatrices(center, center + Vector3.UnitX);
            GL.ClearColor(ClearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Render(models);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment0,
                TextureTarget.TextureCubeMapNegativeX,
                cubeMap,
                0);
            RebuildMatrices(center, center - Vector3.UnitX);
            GL.ClearColor(ClearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Render(models);


            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                        FramebufferAttachment.ColorAttachment0,
                        TextureTarget.TextureCubeMapPositiveY,
                        cubeMap,
                        0);
            RebuildMatrices(center, center + Vector3.UnitY);
            GL.ClearColor(ClearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Render(models);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                        FramebufferAttachment.ColorAttachment0,
                        TextureTarget.TextureCubeMapNegativeY,
                        cubeMap,
                        0);
            RebuildMatrices(center, center - Vector3.UnitY);
            GL.ClearColor(ClearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Render(models);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment0,
                TextureTarget.TextureCubeMapPositiveZ,
                cubeMap,
                0);
            RebuildMatrices(center, center + Vector3.UnitZ);
            GL.ClearColor(ClearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Render(models);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                    FramebufferAttachment.ColorAttachment0,
                    TextureTarget.TextureCubeMapNegativeZ,
                    cubeMap,
                    0);
            RebuildMatrices(center, center - Vector3.UnitZ);
            GL.ClearColor(ClearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Render(models);

            FrameBufferManager.DisableReflectionsFrameBuffer();

            DrawUsingFrameBuffer(models, reflectiveModels, cubeMap);

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

        private void DrawUsingFrameBuffer(List<SimpleModel> models, List<SimpleModel> reflectiveModels, int cubeMapTextureId)
        {
            RebuildMatrices();

            GL.ClearColor(ClearColor);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var model in models)
            {
                Render(model);
            }

            foreach (var model in reflectiveModels)
            {
                RenderWithEnvMap(model, cubeMapTextureId);
            }
        }

        private void RenderWithEnvMap(SimpleModel model, int cubeMapTextureId)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, cubeMapTextureId);
            Shaders.BindWithEnvironmentMap(model, ModelView, ModelViewProjection, Projection, cubeMapTextureId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);
        }

        private void RebuildMatrices()
        {
            ModelView = Matrix4.LookAt(_player.Position, _player.Target, Vector3.UnitY);
            Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, Aspect, 0.1f, 200);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }

        private void RebuildMatrices(Vector3 pos, Vector3 target)
        {
            ModelView = Matrix4.LookAt(pos, target, Vector3.UnitY);
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, Aspect, 0.1f, 200);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }
    }
}