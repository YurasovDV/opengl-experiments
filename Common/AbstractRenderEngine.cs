using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Common
{
    public class AbstractRenderEngine 
    {
        public IPlayer Player { get; set; }

        public Matrix4 ModelView = Matrix4.Identity;

        public Matrix4 ModelViewProjection = Matrix4.Identity;

        public Matrix4 Projection;

        public int Height { get; set; }

        public int Width { get; set; }

        public AbstractRenderEngine(int width, int height, IPlayer player, float zFar = 200)
        {
            this.Width = width;
            this.Height = height;
            this.Player = player;

            GL.Viewport(0, 0, (int)width, (int)height);
            float aspect = width / height;

            Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, zFar);

        }


        public virtual void SetupVieport()
        {
            GL.Enable(EnableCap.DepthTest);
            ModelView = Matrix4.LookAt(Player.Position, Player.Target, Vector3.UnitY);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }

        protected virtual void PreRender()
        {

        }


        public virtual void Render(IEnumerable<SimpleModel> models)
        {
            SetupVieport();
            PreRender();
           

            foreach (var model in models)
            {
                BindBuffers(model);
                Draw(model);
            }

            PostRender();

        }

        protected virtual void PostRender()
        {
            
        }

        protected virtual void BindBuffers(SimpleModel model)
        {
            
        }

        protected virtual void Draw(SimpleModel model)
        {

        }
    }
}
