using System;
using System.Collections.Generic;

using Common;
using Common.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ModelLoading
{
    class RenderEngine : AbstractRenderEngine
    {
        private ShaderManager ShaderManager { get; set; }

        public SkyboxRenderer Skybox { get; set; }

        public RenderEngine(int Width, int Height, Player Player) : base(Width, Height, Player)
        {
 
            GL.Viewport(0, 0, (int)Width, (int)Height);
            float aspect = Width / Height;

            Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 200);

            ShaderManager = new ShaderManager(this);

            Skybox = new SkyboxRenderer(ShaderManager, this);
        }

        public override void Render(IEnumerable<SimpleModel> models)
        {
            SetupVieport();

            GL.Disable(EnableCap.DepthTest);
            Skybox.Render();
            GL.Enable(EnableCap.DepthTest);

            foreach (var model in models)
            {
                BindBuffers(model);
                Draw(model);
            }

            GL.Flush();
        }

        private new void BindBuffers(SimpleModel model)
        {
            ShaderManager.BindBuffers(model, Player.FlashlightPosition, refreshVertices: true);
        }

        private new void Draw(SimpleModel model)
        {
            if (model.TextureId != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, ShaderManager.texcoord_buffer_address);
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(model.TextureCoordinates.Length * Vector2.SizeInBytes),
                  model.TextureCoordinates, BufferUsageHint.DynamicDraw);
                GL.VertexAttribPointer(ShaderManager.AttributeTexcoord_Parameter_Address, 2, VertexAttribPointerType.Float, false, 0, 0);

                GL.EnableVertexAttribArray(ShaderManager.AttributeTexcoord_Parameter_Address);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.Uniform1(ShaderManager.UniformTexture_Parameter_Address, 0);
                GL.BindTexture(TextureTarget.Texture2D, model.TextureId);
            }

            GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
