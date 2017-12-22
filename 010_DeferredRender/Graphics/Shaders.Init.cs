using System;
using System.Collections.Generic;
using System.IO;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics
{
    partial class Shaders
    {
        private static TexturelessNoLight _texturelessNoLightDescriptor;
        private static TexturedNoLight _texturedNoLightDescriptor;
        private static GBufferSecondPass _secondGBufferPassDescriptor;
        private static OneQuadProgram _auxillaryProgram;

        internal static TexturelessNoLight InitTexturelessNoLight()
        {
            var textureLessProgId = GL.CreateProgram();

            var vert = GL.CreateShader(ShaderType.VertexShader);
            var vertText = File.ReadAllText(@"Assets\Shaders\texturelessNoLight.vert");
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
            var fragText = File.ReadAllText(@"Assets\Shaders\texturelessNoLight.frag");
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

            _texturelessNoLightDescriptor = new TexturelessNoLight();

            _texturelessNoLightDescriptor.uniformMVP = GL.GetUniformLocation(textureLessProgId, "uMVP");
            _texturelessNoLightDescriptor.uniformMV = GL.GetUniformLocation(textureLessProgId, "uMV");
            _texturelessNoLightDescriptor.uniformProjection = GL.GetUniformLocation(textureLessProgId, "uP");

            _texturelessNoLightDescriptor.ProgramId = textureLessProgId;

            _texturelessNoLightDescriptor.AttribVerticesLocation = GL.GetAttribLocation(textureLessProgId, "vPosition");
            _texturelessNoLightDescriptor.AttribNormalsLocation = GL.GetAttribLocation(textureLessProgId, "vNormal");
            _texturelessNoLightDescriptor.AttribColorsLocation = GL.GetAttribLocation(textureLessProgId, "vColor");

            GL.GenBuffers(1, out _texturelessNoLightDescriptor.verticesBuffer);
            GL.GenBuffers(1, out _texturelessNoLightDescriptor.colorsBuffer);
            GL.GenBuffers(1, out _texturelessNoLightDescriptor.normalsBuffer);

            return _texturelessNoLightDescriptor;
        }

        public static TexturedNoLight InitTexturedNoLight()
        {
            var texturedProgId = GL.CreateProgram();

            var vert = GL.CreateShader(ShaderType.VertexShader);

            var vertText = File.ReadAllText(@"Assets\Shaders\texturedNoLight.vert");
            GL.ShaderSource(vert, vertText);
            GL.CompileShader(vert);
            GL.AttachShader(texturedProgId, vert);


            int statusCode;
            GL.GetShader(vert, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vert, out info);
                throw new Exception("vertex shader: " + info);
            }

            var frag = GL.CreateShader(ShaderType.FragmentShader);
            var fragText = File.ReadAllText(@"Assets\Shaders\texturedNoLight.frag");
            GL.ShaderSource(frag, fragText);
            GL.CompileShader(frag);
            GL.AttachShader(texturedProgId, frag);

            GL.LinkProgram(texturedProgId);

            GL.GetShader(frag, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(frag, out info);
                throw new Exception("fragment shader: " + info);
            }

            _texturedNoLightDescriptor = new TexturedNoLight();

            _texturedNoLightDescriptor.uniformMVP = GL.GetUniformLocation(texturedProgId, "uMVP");
            _texturedNoLightDescriptor.uniformMV = GL.GetUniformLocation(texturedProgId, "uMV");
            _texturedNoLightDescriptor.uniformProjection = GL.GetUniformLocation(texturedProgId, "uP");
            _texturedNoLightDescriptor.uniformTexture1 = GL.GetUniformLocation(texturedProgId, "u_Texture");

            _texturedNoLightDescriptor.ProgramId = texturedProgId;

            _texturedNoLightDescriptor.AttribVerticesLocation = GL.GetAttribLocation(texturedProgId, "vPosition");
            _texturedNoLightDescriptor.AttribNormalsLocation = GL.GetAttribLocation(texturedProgId, "vNormal");
            _texturedNoLightDescriptor.AttribColorsLocation = GL.GetAttribLocation(texturedProgId, "vColor");
            _texturedNoLightDescriptor.TexCoordsLocation = GL.GetAttribLocation(texturedProgId, "a_TexCoordinate");

            GL.GenBuffers(1, out _texturedNoLightDescriptor.verticesBuffer);
            GL.GenBuffers(1, out _texturedNoLightDescriptor.colorsBuffer);
            GL.GenBuffers(1, out _texturedNoLightDescriptor.normalsBuffer);
            GL.GenBuffers(1, out _texturedNoLightDescriptor.texCoordsBuffer);

            return _texturedNoLightDescriptor;
        }

        public static GBufferSecondPass InitSecondGBufferPassProgram()
        {
            var vertexPath = @"Assets\Shaders\frameBufferVertex.glsl";
            var fragmentPath = @"Assets\Shaders\frameBufferFragment.glsl";

            var programId = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            using (var rd = new StreamReader(vertexPath))
            {
                string text = rd.ReadToEnd();
                GL.ShaderSource(vertexShader, text);
            }
            GL.CompileShader(vertexShader);
            GL.AttachShader(programId, vertexShader);

            int statusCode;

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vertexShader, out info);
                throw new Exception("vertex shader" + info);
            }

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            using (var rd = new StreamReader(fragmentPath))
            {
                string text = rd.ReadToEnd();
                GL.ShaderSource(fragmentShader, text);
            }
            GL.CompileShader(fragmentShader);
            GL.AttachShader(programId, fragmentShader);

            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(fragmentShader, out info);
                throw new Exception("fragment shader: " + info);
            }

            GL.LinkProgram(programId);

            GL.UseProgram(programId);
            _secondGBufferPassDescriptor = new GBufferSecondPass();
            _secondGBufferPassDescriptor.AttribVerticesLocation = GL.GetAttribLocation(programId, "vPosition");
            _secondGBufferPassDescriptor.TexCoordsLocation = GL.GetAttribLocation(programId, "vTexCoordinate");

            _secondGBufferPassDescriptor.uniformTexturePos = GL.GetUniformLocation(programId, "gPositionSampler");
            _secondGBufferPassDescriptor.uniformTextureNormal = GL.GetUniformLocation(programId, "gNormalSampler");
            _secondGBufferPassDescriptor.uniformTextureColor = GL.GetUniformLocation(programId, "gAlbedoSpecSampler");

            _secondGBufferPassDescriptor.UniformLightLocation = GL.GetUniformLocation(programId, "uLightPos");
            _secondGBufferPassDescriptor.UniformCameraPosition = GL.GetUniformLocation(programId, "uCameraPos");

            GL.GenBuffers(1, out _secondGBufferPassDescriptor.texCoordsBuffer);
            GL.GenBuffers(1, out _secondGBufferPassDescriptor.verticesBuffer);


            _secondGBufferPassDescriptor.ProgramId = programId;
            return _secondGBufferPassDescriptor;
        }



        public static OneQuadProgram InitOneQuadProgramProgram()
        {
            var vertexPath = @"Assets\Shaders\frameBufferVertex.glsl";
            var fragmentPath = @"Assets\Shaders\oneQuadShader.frag";

            var programId = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            using (var rd = new StreamReader(vertexPath))
            {
                string text = rd.ReadToEnd();
                GL.ShaderSource(vertexShader, text);
            }
            GL.CompileShader(vertexShader);
            GL.AttachShader(programId, vertexShader);

            int statusCode;

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vertexShader, out info);
                throw new Exception("vertex shader" + info);
            }

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            using (var rd = new StreamReader(fragmentPath))
            {
                string text = rd.ReadToEnd();
                GL.ShaderSource(fragmentShader, text);
            }
            GL.CompileShader(fragmentShader);
            GL.AttachShader(programId, fragmentShader);

            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(fragmentShader, out info);
                throw new Exception("fragment shader: " + info);
            }

            GL.LinkProgram(programId);

            GL.UseProgram(programId);
            _auxillaryProgram = new OneQuadProgram();
            _auxillaryProgram.AttribVerticesLocation = GL.GetAttribLocation(programId, "vPosition");
            _auxillaryProgram.TexCoordsLocation = GL.GetAttribLocation(programId, "vTexCoordinate");

            _auxillaryProgram.uniformTexture0 = GL.GetUniformLocation(programId, "uTexture");


            GL.GenBuffers(1, out _auxillaryProgram.texCoordsBuffer);
            GL.GenBuffers(1, out _auxillaryProgram.verticesBuffer);


            _auxillaryProgram.ProgramId = programId;
            return _auxillaryProgram;
        }




    }
}
