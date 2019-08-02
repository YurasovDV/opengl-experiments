using System;
using System.IO;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace LSystemsPlants.Core.Graphics
{
    class ShaderManager
    {
        public int MainProgramId { get; set; }

        public int AttrVertexLocation { get; set; }

        public int AttrColorLocation { get; set; }

        public int AttrNormalLocation { get; set; }

        public int UMvpLocation { get; set; }

        public int UMvLocation { get; set; }

        public int UPLocation { get; set; }

        public int vertexBufferAddress;
        public int colorBufferAddress;
        public int normalBufferAddress;


        public ShaderManager()
        {
            CreateMainProgram();

            vertexBufferAddress = GL.GenBuffer();
            colorBufferAddress = GL.GenBuffer();
            normalBufferAddress = GL.GenBuffer();
        }


        public void BindBuffers(SimpleModel model, Matrix4[] matrices)
        {
            GL.UseProgram(MainProgramId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferAddress);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * model.Vertices.Length), model.Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrVertexLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferAddress);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * model.Colors.Length), model.Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrColorLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferAddress);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * model.Normals.Length), model.Normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrNormalLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.UniformMatrix4(UMvpLocation, false, ref matrices[0]);
        }

        private void CreateMainProgram()
        {
            MainProgramId = GL.CreateProgram();

            var vertex = GL.CreateShader(ShaderType.VertexShader);
            string vert = File.ReadAllText(@"Assets\main.vert");
            GL.ShaderSource(vertex, vert);
            GL.CompileShader(vertex);
            GL.AttachShader(MainProgramId, vertex);
            GL.GetShader(vertex, ShaderParameter.CompileStatus, out int statusCode);
            if (statusCode != 1)
            {
                GL.GetShaderInfoLog(vertex, out string info);
                throw new Exception("vertex shader: " + info);
            }


            var fragment = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragment, File.ReadAllText(@"Assets\main.frag"));
            GL.CompileShader(fragment);
            GL.AttachShader(MainProgramId, fragment);
            GL.GetShader(fragment, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                GL.GetShaderInfoLog(fragment, out string info);
                throw new Exception("fragment shader: " + info);
            }


            GL.LinkProgram(MainProgramId);
            GL.UseProgram(MainProgramId);
            AttrColorLocation = GL.GetAttribLocation(MainProgramId, "vColor");
            AttrVertexLocation = GL.GetAttribLocation(MainProgramId, "vPos");
            AttrNormalLocation = GL.GetAttribLocation(MainProgramId, "vNormal");

            UMvpLocation = GL.GetUniformLocation(MainProgramId, "uMVP");
        }
    }
}
