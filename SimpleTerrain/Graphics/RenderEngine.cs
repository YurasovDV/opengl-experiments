using Common;
using Common.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace SimpleTerrain
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

        public void Draw(SimpleModel model, Vector3 light)
        {
            GL.ClearColor(0, 0f, 0f, 100);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SetupVieport();

            Shaders.BindBuffers(model, light);

            GL.DrawArrays(RenderMode, 0, model.Vertices.Length);
           
            GL.Flush();
            Shaders.Release();
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
