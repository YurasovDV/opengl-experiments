using System;
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

        // program for screen-quad render
        private static FullScreenQuadProgram _fullScreenQuadProgram;

        // special program for drawing auxillary buffers
        private static OneQuadProgram _auxillaryProgram;

        private static LightVolumeProgram _lightDescriptor;

        internal static TexturelessNoLight InitTexturelessNoLight()
        {
            var textureLessProgId = CreateProgramFrom(@"Assets\Shaders\1GeometryPass\texturelessNoLight.vert", @"Assets\Shaders\1GeometryPass\texturelessNoLight.frag");

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
            int texturedProgId = CreateProgramFrom(@"Assets\Shaders\1GeometryPass\texturedNoLight.vert", @"Assets\Shaders\1GeometryPass\texturedNoLight.frag");

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

        public static FullScreenQuadProgram InitFinalPassProgram()
        {
            var vertexPath = @"Assets\Shaders\3CombinePass\frameBuffer.vert";
            var fragmentPath = @"Assets\Shaders\3CombinePass\frameBuffer.frag";

            var programId = CreateProgramFrom(vertexPath, fragmentPath);

            GL.UseProgram(programId);
            _fullScreenQuadProgram = new FullScreenQuadProgram();
            _fullScreenQuadProgram.AttribVerticesLocation = GL.GetAttribLocation(programId, "vPosition");
            _fullScreenQuadProgram.TexCoordsLocation = GL.GetAttribLocation(programId, "vTexCoordinate");

            _fullScreenQuadProgram.uniformTextureColor = GL.GetUniformLocation(programId, "gAlbedoSpecSampler");

            _fullScreenQuadProgram.uniformTextureDiffuse = GL.GetUniformLocation(programId, "gDiffuseSampler");

            GL.GenBuffers(1, out _fullScreenQuadProgram.texCoordsBuffer);
            GL.GenBuffers(1, out _fullScreenQuadProgram.verticesBuffer);

            _fullScreenQuadProgram.ProgramId = programId;
            return _fullScreenQuadProgram;
        }

        public static OneQuadProgram InitOneQuadProgramProgram()
        {
            var vertexPath = @"Assets\Shaders\3CombinePass\frameBuffer.vert";
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

            _auxillaryProgram.IsDepth = GL.GetUniformLocation(programId, "isDepth");

            GL.GenBuffers(1, out _auxillaryProgram.texCoordsBuffer);
            GL.GenBuffers(1, out _auxillaryProgram.verticesBuffer);


            _auxillaryProgram.ProgramId = programId;
            return _auxillaryProgram;
        }


        public static LightVolumeProgram InitLightVolumeProgram()
        {
            var vertexPath = @"Assets\Shaders\2LightPass\lightVolume.vert";
            var fragmentPath = @"Assets\Shaders\2LightPass\lightVolume.frag";

            int programId = CreateProgramFrom(vertexPath, fragmentPath);

            GL.UseProgram(programId);
            _lightDescriptor = new LightVolumeProgram();

            _lightDescriptor.uniformMVP = GL.GetUniformLocation(programId, "uMVP");
            _lightDescriptor.uniformMV = GL.GetUniformLocation(programId, "uMV");


            _lightDescriptor.uniformColor = GL.GetUniformLocation(programId, "lightColor");
            _lightDescriptor.uniformPosition = GL.GetUniformLocation(programId, "lightPos");

            _lightDescriptor.uniformTexturePos = GL.GetUniformLocation(programId, "gPositionSampler");
            _lightDescriptor.uniformTextureNormal = GL.GetUniformLocation(programId, "gNormalSampler");
            _lightDescriptor.uniformTextureColor = GL.GetUniformLocation(programId, "gAlbedoSpecSampler");

            _lightDescriptor.AttribVerticesLocation = GL.GetAttribLocation(programId, "vPosition");

            GL.GenBuffers(1, out _lightDescriptor.verticesBuffer);

            _lightDescriptor.ProgramId = programId;
            return _lightDescriptor;
        }

        private static int CreateProgramFrom(string vertexPath, string fragmentPath)
        {
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
            return programId;
        }
    }
}
