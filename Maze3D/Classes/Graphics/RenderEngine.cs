using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze3D.Classes.Graphics
{
    class RenderEngine
    {
        private SimpleModel model;
        private ShaderManager ShaderManager { get; set; }
        private Player PlayerData;

        public float Height { get; set; }

        public float Width { get; set; }

        //private int green = 0;
       // private int greenCount = 0;

        public Matrix4 ModelView = Matrix4.Identity;
        public Matrix4 Projection = Matrix4.Identity;
        public Matrix4 ModelViewProjection = Matrix4.Identity;

        public RenderEngine(Player PlayerData, float w, float h)
        {
            this.PlayerData = PlayerData;
            Width = w;
            Height = h;
            GL.Viewport(0, 0, (int)Width, (int)Height);
            ShaderManager = new ShaderManager(this);

        }

        internal void SetMaze(SimpleModel model)
        {
            this.model = model;
        }

        internal void Render(long delta)
        {
            SetupVieport();
            BindBuffers();

            Draw();
        }

        private void Draw()
        {
            //GL.ClearColor(0, green, 0, 100);
            GL.ClearColor(0, 0f, 0.5f, 100);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.DepthTest);

            GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);
            /*greenCount++;
            if (greenCount % 50 == 0)
            {
                green++;
            }*/
            GL.Flush();
        }

        private void BindBuffers()
        {
            Vector3 light = new Vector3(PlayerData.Position.X, PlayerData.Position.Y+10, PlayerData.Position.Z-10);
            ShaderManager.BindBuffers(model, light);
        }

        private void SetupVieport()
        {

            GL.Enable(EnableCap.DepthTest);

            float aspect = Width / Height;

            Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 100);

            ModelView = Matrix4.LookAt(PlayerData.Position, PlayerData.Target, Vector3.UnitY);

            ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }
    }
}
