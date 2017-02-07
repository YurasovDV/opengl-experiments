using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, zFar);

            _shaders = new ShaderManager();

        }

        public void Render(double delta)
        {


            GL.Flush();
        }
    }
}
