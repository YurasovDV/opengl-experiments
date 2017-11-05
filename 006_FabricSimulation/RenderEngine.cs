using System;
using System.IO;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace FabricSimulation
{
    public class RenderEngine : AbstractRenderEngine
    {
        public int MainProgram { get; set; }

        public int Uniform_MVP_Location { get; set; }
        public int Uniform_MV_Location { get; set; }

        public int UniformLightPosition { get; set; }

        public int VertexAttribLocation { get; set; }
        public int ColorAttribLocation { get; set; }
        public int NormalAttribLocation { get; set; }



        private int vertexBufferAddress;
        private int colorBufferAddress;
        private int normalBufferAddress;


        public RenderEngine(int width, int height, AbstractPlayer player, float zFar = 200) : base(width, height, player, zFar)
        {
            InitShaders();
        }

        protected override void BindBuffers(SimpleModel model)
        {
            GL.UseProgram(MainProgram);

            var light = Player.FlashlightPosition;

            GL.UniformMatrix4(Uniform_MVP_Location, false, ref ModelViewProjection);
            GL.UniformMatrix4(Uniform_MV_Location, false, ref ModelView);
            GL.Uniform3(UniformLightPosition, ref light);



            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferAddress);
            GL.BufferData(BufferTarget.ArrayBuffer, 
                (IntPtr)(model.Vertices.Length * Vector3.SizeInBytes), 
                model.Vertices, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(VertexAttribLocation, 3, 
                VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(VertexAttribLocation);



            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferAddress);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Colors.Length * Vector3.SizeInBytes),
                model.Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(ColorAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(ColorAttribLocation);


            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferAddress);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Normals.Length * Vector3.SizeInBytes),
                model.Normals, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(NormalAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(NormalAttribLocation);

        }

        protected override void Draw(SimpleModel model)
        {

            GL.ClearColor(0, 0.1f, 0f, 100);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);
        }

        protected override void PostRender()
        {
            GL.Flush();
        }

        private void InitShaders()
        {
            MainProgram = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            string text = string.Empty;
            using (StreamReader rd = new StreamReader("Shaders\\fabricVertex.glsl"))
            {
                text = rd.ReadToEnd();
            }

            GL.ShaderSource(vertexShader, text);
            GL.CompileShader(vertexShader);
            GL.AttachShader(MainProgram, vertexShader);

            int status;
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out status);
            if (status != 1)
            {
                string error;
                GL.GetShaderInfoLog(vertexShader, out error);
                throw new Exception(error);
            }




            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

            text = string.Empty;
            using (StreamReader rd = new StreamReader("Shaders\\fabricFrag.glsl"))
            {
                text = rd.ReadToEnd();
            }

            GL.ShaderSource(fragmentShader, text);
            GL.CompileShader(fragmentShader);
            GL.AttachShader(MainProgram, fragmentShader);


            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out status);
            if (status != 1)
            {
                string error;
                GL.GetShaderInfoLog(fragmentShader, out error);
                throw new Exception(error);
            }



            GL.LinkProgram(MainProgram);

            GL.UseProgram(MainProgram);


            VertexAttribLocation = GL.GetAttribLocation(MainProgram, "vPos");
            ColorAttribLocation = GL.GetAttribLocation(MainProgram, "vColor");
            NormalAttribLocation = GL.GetAttribLocation(MainProgram, "vNormal");

            UniformLightPosition = GL.GetUniformLocation(MainProgram, "uLight");
            Uniform_MVP_Location = GL.GetUniformLocation(MainProgram, "uMVP");
            Uniform_MV_Location = GL.GetUniformLocation(MainProgram, "uMV");

            GL.GenBuffers(1, out vertexBufferAddress);
            GL.GenBuffers(1, out normalBufferAddress);
            GL.GenBuffers(1, out colorBufferAddress);
        }
    }
}
