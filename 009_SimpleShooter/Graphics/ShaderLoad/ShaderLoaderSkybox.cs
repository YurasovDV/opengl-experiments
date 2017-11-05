using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace SimpleShooter.Graphics.ShaderLoad
{
    class ShaderLoaderSkybox : IShaderLoader
    {
        public ShaderProgramDescriptor Load()
        {
            var result = new ShaderProgramDescriptor()
            {
                ShaderKind = ShadersNeeded.SkyBox
            };

            var ProgramIdForSky = GL.CreateProgram();

            var vert = GL.CreateShader(ShaderType.VertexShader);
            var vertText = File.ReadAllText(@"Content\Shaders\skybox.vert");
            GL.ShaderSource(vert, vertText);
            GL.CompileShader(vert);
            GL.AttachShader(ProgramIdForSky, vert);

            int statusCode;
            GL.GetShader(vert, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vert, out info);
                throw new Exception("vertex shader: " + info);
            }

            var frag = GL.CreateShader(ShaderType.FragmentShader);
            var fragText = File.ReadAllText(@"Content\Shaders\skybox.frag");
            GL.ShaderSource(frag, fragText);
            GL.CompileShader(frag);
            GL.AttachShader(ProgramIdForSky, frag);

            GL.GetShader(frag, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(frag, out info);
                throw new Exception("fragment shader: " + info);
            }

            GL.LinkProgram(ProgramIdForSky);
            result.ProgramId = ProgramIdForSky;

            GL.GenBuffers(1, out result.verticesBuffer);

            result.TextureSampler = GL.GetUniformLocation(ProgramIdForSky, "cubemap");
            result.AttribVerticesLocation = GL.GetAttribLocation(ProgramIdForSky, "position");
            result.uniformMV = GL.GetUniformLocation(ProgramIdForSky, "view");
            result.uniformProjection = GL.GetUniformLocation(ProgramIdForSky, "projection");

            return result;
        }
    }
}
