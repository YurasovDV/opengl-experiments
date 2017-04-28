using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace SimpleShooter.Graphics.ShaderLoader
{
    class ShaderLoaderLine : IShaderLoader
    {
        public ShaderProgramDescriptor Load()
        {
            var result = new ShaderProgramDescriptor();
            var lineProgramId = GL.CreateProgram();

            var vert = GL.CreateShader(ShaderType.VertexShader);
            var vertText = File.ReadAllText(@"Content\Shaders\line.vert");
            GL.ShaderSource(vert, vertText);
            GL.CompileShader(vert);
            GL.AttachShader(lineProgramId, vert);


            int statusCode;
            GL.GetShader(vert, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vert, out info);
                throw new Exception("vertex shader: " + info);
            }

            var frag = GL.CreateShader(ShaderType.FragmentShader);
            var fragText = File.ReadAllText(@"Content\Shaders\line.frag");
            GL.ShaderSource(frag, fragText);
            GL.CompileShader(frag);
            GL.AttachShader(lineProgramId, frag);

            GL.LinkProgram(lineProgramId);

            GL.GetShader(frag, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(frag, out info);
                throw new Exception("fragment shader: " + info);
            }

            result.uniformMVP = GL.GetUniformLocation(lineProgramId, "uMVP");

            result.ProgramId = lineProgramId;

            result.AttribVerticesLocation = GL.GetAttribLocation(lineProgramId, "vPosition");
            result.AttribColorsLocation = GL.GetAttribLocation(lineProgramId, "vColor");

            GL.GenBuffers(1, out result.verticesBuffer);
            GL.GenBuffers(1, out result.colorsBuffer);

            return result;
        }
    }
}
