using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleLighting
{
    class GraphicSystem
    {

        public float Width { get; set; }
        public float Height { get; set; }

        private WaitCallback Refresh { get; set; }

        public Matrix4[] ModelView { get; set; }

        public Matrix4 Projection { get; set; }

        public Matrix4 ViewMatrix { get; set; }

        public ShaderManager Shaders { get; set; }

        public float green = 0;
        public long counter = 0;

        public GraphicSystem(int viewWidth, int viewHeight, Action refresh)
        {
            Width = viewWidth;
            Height = viewHeight;

            WaitCallback call = new WaitCallback(unused => refresh());
            Refresh = call;
            GL.Viewport(0, 0, (int)Width, (int)Height);
            Shaders = new ShaderManager(this);
        }

        public void Render(SimpleModel[] models)
        {
            var model = models.FirstOrDefault();
            if (model != null)
            {
                SetupViewport();
                Shaders.BindParams(model);
                Shaders.BindMatrices(ViewMatrix, Projection);
                Draw(model);
                Shaders.Unbind();
            }
        }

        private void SetupViewport()
        {
            ModelView = new Matrix4[] { Matrix4.Identity };

            float aspect = (float)Width / (float)Height;
            Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.01f, 300);
            ModelView[0] = Projection;

            Vector3 target = new Vector3(5, 5f, 0);
            Vector3 eye = new Vector3(0.0f, 0f, 50f);

            ViewMatrix = Matrix4.LookAt(eye, target, Vector3.UnitY);

            ModelView[0] = Matrix4.Mult(ViewMatrix, Projection);
        }

        private void Draw(SimpleModel m)
        {
            GL.Enable(EnableCap.DepthTest | EnableCap.LineSmooth);

            //GL.ClearColor(0, green, 0, 100);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.DrawArrays(PrimitiveType.Triangles, 0, m.Vertices.Length);

            GL.Flush();
            
        }
    }
}
