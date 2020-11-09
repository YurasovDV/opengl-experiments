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

        Vector3 _center;

        private OpenTK.Graphics.Color4 ClearColor = new OpenTK.Graphics.Color4(255, 165, 0, 1);

        public FrameBufferManager FrameBufferManager { get; set; }
        public float Aspect { get; private set; }
        SkyBoxRenderer _skybox;
        public GraphicsSystem(int width, int height, Player player, Vector3 mirrorCenter)
        {
            _width = width;
            _height = height;
            _player = player;
            _center = mirrorCenter;
            InitGraphics();
        }

        private void InitGraphics()
        {
            Aspect = (float)_width / _height;
            GL.Viewport(0, 0, _width, _height);
            GL.Enable(EnableCap.DepthTest);
            Shaders.InitTexturelessNoLight();
            Shaders.InitRenderWithEnvironmentMap();
            Shaders.InitSkybox();

            FrameBufferManager = new FrameBufferManager(_width, _height);
            _skybox = new SkyBoxRenderer(50);
        }

        public void Render(List<SimpleModel> models, List<SimpleModel> reflectiveModels)
        {
            FrameBufferManager.EnableReflectionsFrameBuffer();
            var cubeMap = FrameBufferManager.ReflectionsMapFrameBufferDescriptor.DiffuseTextureId;
            var reflective = reflectiveModels.Single();


            Vector3[] deltas = new[]
            {
                Vector3.UnitX,
                Vector3.UnitX * -1,
                Vector3.UnitY,
                Vector3.UnitY * -1,
                Vector3.UnitZ,
                Vector3.UnitZ * -1
            };

            Vector3[] normals = new[]
            {
                Vector3.UnitY,
                Vector3.UnitY,
                Vector3.UnitX,
                Vector3.UnitX,
                Vector3.UnitY,
                Vector3.UnitY,
            };
            for (int i = 0; i < 6; i++)
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                        FramebufferAttachment.ColorAttachment0,
                        TextureTarget.TextureCubeMapPositiveX + i,
                        cubeMap,
                        0);

                GL.ClearColor(ClearColor);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                RebuildMatrices(_center, _center + deltas[i], -normals[i]);

                GL.Disable(EnableCap.DepthTest);
                _skybox.Render(_center, ModelView, Projection);
                GL.Enable(EnableCap.DepthTest);

                Render(models);
            }

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

            GL.Disable(EnableCap.DepthTest);
            _skybox.Render(_player.Position, ModelView, Projection);
            GL.Enable(EnableCap.DepthTest);

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
            Shaders.BindWithEnvironmentMap(model, _player.Position, cubeMapTextureId, ModelView, ModelViewProjection, Projection);
            GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);
        }

        private void RebuildMatrices()
        {
            ModelView = Matrix4.LookAt(_player.Position, _player.Target, Vector3.UnitY);
            Projection = Matrix4.CreatePerspectiveFieldOfView(1f, Aspect, 0.1f, 200);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }

        private void RebuildMatrices(Vector3 pos, Vector3 target, Vector3 up)
        {
            ModelView = Matrix4.LookAt(pos, target, up);
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1, 0.1f, 200);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }
    }
}