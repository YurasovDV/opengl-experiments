using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace SimpleShooter.Graphics.ShaderLoad
{
    class ShaderLoaderSimpleModel : IShaderLoader
    {
        public ShaderProgramDescriptor Load()
        {
            var result = new ShaderProgramDescriptor();
            var simpleModelProgram = GL.CreateProgram();

            var vert = GL.CreateShader(ShaderType.VertexShader);
            var vertText = File.ReadAllText(@"Content\Shaders\model.vert");
            GL.ShaderSource(vert, vertText);
            GL.CompileShader(vert);
            GL.AttachShader(simpleModelProgram, vert);


            int statusCode;
            GL.GetShader(vert, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vert, out info);
                throw new Exception("vertex shader: " + info);
            }

            var frag = GL.CreateShader(ShaderType.FragmentShader);
            var fragText = File.ReadAllText(@"Content\Shaders\model.frag");
            GL.ShaderSource(frag, fragText);
            GL.CompileShader(frag);
            GL.AttachShader(simpleModelProgram, frag);

            GL.LinkProgram(simpleModelProgram);

            GL.GetShader(frag, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(frag, out info);
                throw new Exception("fragment shader: " + info);
            }

            result.uniformLightPos = GL.GetUniformLocation(simpleModelProgram, "uLightPos");

            result.uniformMVP = GL.GetUniformLocation(simpleModelProgram, "uMVP");
            result.uniformMV = GL.GetUniformLocation(simpleModelProgram, "uMV");
            result.uniformProjection = GL.GetUniformLocation(simpleModelProgram, "uP");

            result.ProgramId = simpleModelProgram;

            result.AttribVerticesLocation = GL.GetAttribLocation(simpleModelProgram, "vPosition");
            result.AttribColorsLocation = GL.GetAttribLocation(simpleModelProgram, "vColor");
            result.AttribNormalsLocation = GL.GetAttribLocation(simpleModelProgram, "vNormal");

            result.AttribTextureCoordsLocation = GL.GetAttribLocation(simpleModelProgram, "a_TexCoordinate");
            result.TextureSampler = GL.GetUniformLocation(simpleModelProgram, "u_Texture");

            GL.GenBuffers(1, out result.verticesBuffer);
            GL.GenBuffers(1, out result.colorsBuffer);
            GL.GenBuffers(1, out result.normalsBuffer);
            GL.GenBuffers(1, out result.textureCoordsBuffer);

            result.ShaderKind = ShadersNeeded.SimpleModel;

            return result;
            
        }
    }
}
