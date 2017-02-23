using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace LSystemsPlants.Core.Graphics
{
    class RenderEngine
    {

        private ShaderManager _shaders;

        public Matrix4 ModelView = Matrix4.Identity;

        public Matrix4 Projection = Matrix4.Identity;

        public Matrix4 ModelViewProjection = Matrix4.Identity;

        public int Height { get; set; }

        public int Width { get; set; }


        public RenderEngine(int width, int height, float zFar = 200)
        {
            Width = width;
            Height = height;

            GL.Viewport(0, 0, (int)width, (int)height);
            float aspect = width / height;

            //Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, zFar);
            Projection = Matrix4.CreateOrthographic(Width, Height, 0.2f, zFar);

            //ModelView = Matrix4.LookAt(Player.Position, Player.Target, Vector3.UnitY);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);

            _shaders = new ShaderManager();


        }

        public void Begin()
        {
            GL.ClearColor(System.Drawing.Color.LightGray);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void End()
        {
            GL.Flush();
        }

        public void Render(SimpleModel tree, double delta)
        {
            _shaders.BindBuffers(tree, new[] { ModelViewProjection });

            GL.EnableVertexAttribArray(_shaders.AttrVertexLocation);
            GL.EnableVertexAttribArray(_shaders.AttrColorLocation);
            GL.EnableVertexAttribArray(_shaders.AttrNormalLocation);

            GL.DrawArrays(PrimitiveType.Lines, 0, tree.Vertices.Length);
        }
    }
}
