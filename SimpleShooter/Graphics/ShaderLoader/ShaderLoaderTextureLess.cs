using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace SimpleShooter.Graphics.ShaderLoader
{
    class ShaderLoaderTextureLess : IShaderLoader
    {
        public ShaderProgramDescriptor Load()
        {
            ShaderProgramDescriptor result = new ShaderProgramDescriptor();
            var textureLessProgId = GL.CreateProgram();

            var vert = GL.CreateShader(ShaderType.VertexShader);
            var vertText = File.ReadAllText(@"Content\Shaders\textureless.vert");
            GL.ShaderSource(vert, vertText);
            GL.CompileShader(vert);
            GL.AttachShader(textureLessProgId, vert);


            int statusCode;
            GL.GetShader(vert, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vert, out info);
                throw new Exception("vertex shader: " + info);
            }

            var frag = GL.CreateShader(ShaderType.FragmentShader);
            var fragText = File.ReadAllText(@"Content\Shaders\textureless.frag");
            GL.ShaderSource(frag, fragText);
            GL.CompileShader(frag);
            GL.AttachShader(textureLessProgId, frag);

            GL.LinkProgram(textureLessProgId);

            GL.GetShader(frag, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(frag, out info);
                throw new Exception("fragment shader: " + info);
            }

            result.uniformLightPos = GL.GetUniformLocation(textureLessProgId, "uLightPos");

            result.uniformMVP = GL.GetUniformLocation(textureLessProgId, "uMVP");
            result.uniformMV = GL.GetUniformLocation(textureLessProgId, "uMV");
            result.uniformProjection = GL.GetUniformLocation(textureLessProgId, "uP");

            result.ProgramId = textureLessProgId;
            // result.ShadersKind = ShadersNeeded.TextureLess;

            result.AttribVerticesLocation = GL.GetAttribLocation(textureLessProgId, "vPosition");
            result.AttribNormalsLocation = GL.GetAttribLocation(textureLessProgId, "vNormal");
            result.AttribColorsLocation = GL.GetAttribLocation(textureLessProgId, "vColor");

            GL.GenBuffers(1, out result.verticesBuffer);
            GL.GenBuffers(1, out result.colorsBuffer);
            GL.GenBuffers(1, out result.normalsBuffer);


            return result;
        }
    }
}
