using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public SimpleModel Model { get; set; }

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

            var vertices = new Vector3[]
                {
                    new Vector3(0, 0, -10),
                    new Vector3(100, 100,-10),
                    new Vector3(0, 100, -10),
                    
                    new Vector3(0, 0, -10),
                    new Vector3(100, 0, -10),
                    new Vector3(100, 100, -10),
                    
                };


            var colors = Enumerable.Repeat(new Vector3(1, 0, 0), vertices.Length);
            var normals = Enumerable.Repeat(new Vector3(0, 1, 0), vertices.Length);

            Model = new SimpleModel()
            {
                Colors = colors.ToArray(),
                Normals = normals.ToArray(),
                Vertices = vertices.ToArray()
            };

        }

        public void Render(double delta)
        {
            GL.ClearColor(System.Drawing.Color.Green);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            this._shaders.BindBuffers(Model, new[] {ModelViewProjection });

            GL.EnableVertexAttribArray(_shaders.AttrVertexLocation);
            GL.EnableVertexAttribArray(_shaders.AttrColorLocation);
            GL.EnableVertexAttribArray(_shaders.AttrNormalLocation);

            GL.DrawArrays(PrimitiveType.Triangles, 0, Model.Vertices.Length);

            GL.Flush();
        }
    }
}
