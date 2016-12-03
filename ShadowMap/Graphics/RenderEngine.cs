using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ShadowMap
{
    public class RenderEngine : AbstractRenderEngine
    {
        public PrimitiveType RenderMode = PrimitiveType.Triangles;

        public ShaderManager Shaders { get; set; }

        public RenderEngine(int width, int height, IPlayer player, float zFar = 300)
            : base(width, height, player, zFar)
        {
            Shaders = new ShaderManager(this);
        }

        public new void PreRender()
        {
            GL.ClearColor(0, 0f, 0f, 100);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SetupVieport();
        }

        public void Draw(SimpleModel model, Vector3 light)
        {
            Shaders.BindBuffers(model, light);
            GL.DrawArrays(RenderMode, 0, model.Vertices.Length);
            Shaders.Release();
        }

        public new void PostRender()
        {
            GL.Flush();
        }

        internal void ChangeRenderMode()
        {
            if (RenderMode == PrimitiveType.Triangles)
            {
                RenderMode = PrimitiveType.Lines;
            }
            else
            {
                RenderMode = PrimitiveType.Triangles;
            }
        }
    }
}
