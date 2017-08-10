using System;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ShaderOnForm
{
    public class GraphicsManager
    {

        #region поля

        ShaderManager shaderManager;
        private Matrix4[] modelView;

        #endregion
        #region свойства
        public int Width { get; set; }

        public int Height { get; set; }

        public float MaxY { get; set; }

        public float MinY { get; set; }

        public float MaxX { get; set; }

        public float MinX { get; set; }

        public Vector3[] Vertices { get; set; }

        public Vector3[] colors { get; set; }

        public Matrix4[] ModelView
        {
            get { return modelView; }
            set { modelView = value; }
        }

        public float PointSize { get; set; }

        #endregion

        public Engine Engine { get; set; }
        Vector3 up = new Vector3(0, 1, 0);
        public GraphicsManager(Action renderAction)
        {
            Render = renderAction;
            shaderManager = new ShaderManager();
        }
        public HeightMap Map { get; set; }
        public Action Render { get; set; }
        public AbstractVehicle Vehicle
        {
            get
            {
                if (Engine != null)
                {
                    return Engine.Vehicle;
                }
                return null;
            }
        }

        public void InitGraphics(int controlWidth, int controlHeight)
        {
            Width = controlWidth;
            Height = controlHeight;
            ModelView = new Matrix4[] { Matrix4.Identity };
            GL.Viewport(0, 0, Width, Height);
            GL.PointSize(PointSize);

            float aspect = (float)Width / (float)Height;
            //var m1 = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.01f, 500f);


            Vertices = new Vector3[] 
            { 
                new Vector3(-0.8f, -0.8f, 0f),
                new Vector3( 0.8f, -0.8f, 0f),
                new Vector3( 0f,  0.8f, 0f)
            };

            colors = new Vector3[] 
            {
                new Vector3( 0f,  1f, 0f)
            };

            shaderManager.InitProgram();
            // TODO почему-то первый вызов не рисует нормально
            //RenderCall(Vertices, null);


            /*
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            float aspect = (float)width / (float)height;
            var projMatr = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 500f);
            GL.LoadMatrix(ref projMatr);
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();*/
            GL.Enable(EnableCap.DepthTest);



        }

        protected void BindBuffers(Vector3[] points, Vector3[] colors, Vector3[] normals)
        {
            shaderManager.BindShaderParams(points, colors, normals, ref ModelView[0]);
        }

        public void RenderCall(Vector3[] vertices, Vector3[] normals)
        {
            float aspect = (float)Width / (float)Height;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.01f, 500f);
            ModelView[0] = projection;

            Vector3 center = new Vector3(10, 0.8f, 0);

           

            Vector3 from = new Vector3(0.0f, 0.8f, 0f);
            if(Vehicle != null && Vehicle.Target != null)
            {
                from = new Vector3(Vehicle.X, Vehicle.Y, Vehicle.Z);
                center = new Vector3(Vehicle.Target[0], Vehicle.Target[1], Vehicle.Target[2]);
            }

             GL.Uniform3(shaderManager.UniformLightPosition_Parameter_Address, from);

            var viewMatrix = Matrix4.LookAt(from, center, Vector3.UnitY);

            //float aspect = (float)Width / (float)Height;
            //m1 = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.01f, 500f);

            ModelView[0] = Matrix4.Mult(viewMatrix, projection);

            RenderPoints(vertices, normals);
            Render();
            GL.Flush();
        }

        private void RenderPoints(Vector3[] vertices,Vector3[] normals )
        {
            colors = GetColors(vertices.Length); 

            BindBuffers(vertices, colors, normals);



            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.EnableVertexAttribArray(shaderManager.AttributeVPosition);
            GL.EnableVertexAttribArray(shaderManager.AttributeVColor);
            GL.EnableVertexAttribArray(shaderManager.AttributeTexcoord_Parameter_Address);

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);

            GL.DisableVertexAttribArray(shaderManager.AttributeVPosition);
            GL.DisableVertexAttribArray(shaderManager.AttributeVColor);
            GL.DisableVertexAttribArray(shaderManager.AttributeTexcoord_Parameter_Address);
        }

        private Vector3[] GetColors(int count)
        {
            Vector3 color1 = new Vector3(0, 0.2f, 0);
            Vector3 color2 = new Vector3(0f, 0.7f, 0);
            Vector3 color3 = new Vector3(0, 0.4f, 0f);
            var result = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                //var r = 1;
                //var g = MathHelperMINE.Sin[i % 360];
                //var b = MathHelperMINE.Cos[i % 360];
                //result[i] = new Vector3(r, g, b);
                if (i % 3 == 0)
                {
                    result[i] = color1;
                }
                else if (i % 3 == 1)
                {
                    result[i] = color3;
                }
                else
                {
                    result[i] = color2;
                }
            }
            return result;
        }

    /*    private void RenderAxes(Vector3[] axes)
        {
            Vertices = axes;
            colors = new Vector3[]
            {
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
            };

            BindBuffers(Vertices, colors);
            GL.EnableVertexAttribArray(shaderManager.AttributeVPosition);
            GL.EnableVertexAttribArray(shaderManager.AttributeVColor);
            GL.DrawArrays(PrimitiveType.Lines, 0, axes.Length);
            GL.DisableVertexAttribArray(shaderManager.AttributeVPosition);
            GL.DisableVertexAttribArray(shaderManager.AttributeVColor);
        }*/
    }
}
