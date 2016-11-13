using Common;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcTreeRevisited
{
    class RenderEngine : AbstractRenderEngine
    {
        //public ShaderManager ShaderManager { get; set; }

        public RenderEngine(int width, int height, AbstractPlayer player)
            : base(width, height, player, 900)
        {
            //ShaderManager = new ShaderManager(this);

            // Skybox = new SkyboxRenderer(ShaderManager, this);
        }

        protected override void PreRender()
        {
            //base.PreRender();

            // GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            /*GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(ShaderManager.UniformTexture_Parameter_Address, 0);

            GL.EnableVertexAttribArray(ShaderManager.VertexAttribLocation);
            GL.EnableVertexAttribArray(ShaderManager.ColorAttribLocation);
            GL.EnableVertexAttribArray(ShaderManager.AttributeTexcoord_Parameter_Address);*/

        }

        protected override void BindBuffers(SimpleModel model)
        {
            //RefreshKind refreshWhat = RefreshKind.Vertices | RefreshKind.TextureCoords | RefreshKind.Color;
            // if (ShaderManager.IsFirstDrawCall)
            // {
            //  refreshWhat = refreshWhat;
            // | RefreshKind.Color;
            //  }

            //ShaderManager.BindBuffers(model, Player.FlashlightPosition, refreshWhat);
        }

        protected override void Draw(SimpleModel model)
        {
            /*if (model.TextureId != -1)
            {
                GL.BindTexture(TextureTarget.Texture2D, model.TextureId);
            }

            GL.Disable(EnableCap.DepthTest);

            GL.DrawArrays(PrimitiveType.Quads, 0, model.Vertices.Length);

            GL.Enable(EnableCap.DepthTest);*/

        }

        protected override void PostRender()
        {

            /*GL.DisableVertexAttribArray(ShaderManager.VertexAttribLocation);
            GL.DisableVertexAttribArray(ShaderManager.ColorAttribLocation);
            GL.DisableVertexAttribArray(ShaderManager.AttributeTexcoord_Parameter_Address);*/

            //GL.BindTexture(TextureTarget.Texture2D, 0);
        }

    }
}
