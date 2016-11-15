using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace SimpleTerrain
{
    public class RenderEngine : AbstractRenderEngine
    {
        public ShaderManager Shaders { get; set; }

        public RenderEngine(int width, int height, AbstractPlayer player, float zFar = 200)
            : base(width, height, player, zFar)
        {
            Shaders = new ShaderManager(this);
        }

        public void Draw(SimpleModel model, Vector3 light)
        {
            GL.ClearColor(0, 0f, 0f, 100);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SetupVieport();

            Shaders.BindBuffers(model, light);

            GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);
           
            GL.Flush();
            Shaders.Release();
        }
    }
}
