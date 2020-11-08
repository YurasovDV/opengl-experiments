using System;
using System.Drawing;
using System.IO;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Glass.Graphics
{
    class Shaders
    {
        private static TexturelessNoLight _texturelessNoLightDescriptor;
        private static EnvironmentMapNoLight _environmentMapNoLightDescriptor;
        private static Skybox _skybox;

        class TexturelessNoLight
        {
            public int uniformMVP = 0;
            public int uniformMV = 0;
            public int uniformProjection = 0;
            public int ProgramId = 0;
            public int AttribVerticesLocation = 0;
            public int AttribNormalsLocation = 0;
            public int AttribColorsLocation = 0;
            public int verticesBuffer = 0;
            public int colorsBuffer = 0;
            public int normalsBuffer = 0;
        }

        class EnvironmentMapNoLight
        {
            public int cube_texture = 0;
            public int uniformMVP = 0;
            public int uniformMV = 0;
            public int uniformProjection = 0;
            public int uniformCameraPos = 0;

            public int ProgramId = 0;
            public int AttribVerticesLocation = 0;
            public int AttribNormalsLocation = 0;
            public int AttribColorsLocation = 0;
            public int verticesBuffer = 0;
            public int colorsBuffer = 0;
            public int normalsBuffer = 0;
  
        }

        class Skybox
        {
            public int verticesBuffer;

            public int uniformMV;
            public int uniformProjection;

            public int ProgramId { get; set; }
            public int TextureSampler { get; set; }
            public int AttribVerticesLocation { get; set; }
        }

        public static void InitTexturelessNoLight()
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
        }

        internal static void BindTexturelessNoLight(SimpleModel model, Matrix4 modelView, Matrix4 modelViewProjection, Matrix4 projection)
        {
            var descriptor = _texturelessNoLightDescriptor;

            GL.UseProgram(descriptor.ProgramId);

            GL.UniformMatrix4(descriptor.uniformMV, false, ref modelView);
            GL.UniformMatrix4(descriptor.uniformMVP, false, ref modelViewProjection);
            GL.UniformMatrix4(descriptor.uniformProjection, false, ref projection);


            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Vertices.Length * Vector3.SizeInBytes),
                model.Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribVerticesLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribVerticesLocation);


            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.colorsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Colors.Length * Vector3.SizeInBytes),
                model.Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribColorsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribColorsLocation);
        }


        internal static void InitRenderWithEnvironmentMap()
        {
           
            var environmentMapProgramId = GL.CreateProgram();

            var vert = GL.CreateShader(ShaderType.VertexShader);
            var vertText = File.ReadAllText(@"Assets\Shaders\reflectiveNoLight.vert");
            GL.ShaderSource(vert, vertText);
            GL.CompileShader(vert);
            GL.AttachShader(environmentMapProgramId, vert);

            int statusCode;
            GL.GetShader(vert, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vert, out info);
                throw new Exception("vertex shader: " + info);
            }

            var frag = GL.CreateShader(ShaderType.FragmentShader);
            var fragText = File.ReadAllText(@"Assets\Shaders\reflectiveNoLight.frag");
            GL.ShaderSource(frag, fragText);
            GL.CompileShader(frag);
            GL.AttachShader(environmentMapProgramId, frag);

            GL.LinkProgram(environmentMapProgramId);

            GL.GetShader(frag, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(frag, out info);
                throw new Exception("fragment shader: " + info);
            }

            _environmentMapNoLightDescriptor = new EnvironmentMapNoLight();

            _environmentMapNoLightDescriptor.cube_texture = GL.GetUniformLocation(environmentMapProgramId, "cube_texture");

            _environmentMapNoLightDescriptor.uniformMVP = GL.GetUniformLocation(environmentMapProgramId, "uMVP");
            _environmentMapNoLightDescriptor.uniformMV = GL.GetUniformLocation(environmentMapProgramId, "uMV");
            _environmentMapNoLightDescriptor.uniformProjection = GL.GetUniformLocation(environmentMapProgramId, "uP");
            _environmentMapNoLightDescriptor.uniformCameraPos = GL.GetUniformLocation(environmentMapProgramId, "cameraPos");

            _environmentMapNoLightDescriptor.ProgramId = environmentMapProgramId;

            _environmentMapNoLightDescriptor.AttribVerticesLocation = GL.GetAttribLocation(environmentMapProgramId, "vPosition");
            _environmentMapNoLightDescriptor.AttribNormalsLocation = GL.GetAttribLocation(environmentMapProgramId, "vNormal");
            _environmentMapNoLightDescriptor.AttribColorsLocation = GL.GetAttribLocation(environmentMapProgramId, "vColor");

            GL.GenBuffers(1, out _environmentMapNoLightDescriptor.verticesBuffer);
            GL.GenBuffers(1, out _environmentMapNoLightDescriptor.colorsBuffer);
            GL.GenBuffers(1, out _environmentMapNoLightDescriptor.normalsBuffer);
        }

        internal static void BindWithEnvironmentMap(SimpleModel model, Vector3 pos, int cubeMapTextureId, Matrix4 modelView, Matrix4 modelViewProjection, Matrix4 projection)
        {
            var descriptor = _environmentMapNoLightDescriptor;

            GL.UseProgram(descriptor.ProgramId);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, cubeMapTextureId);

            GL.UniformMatrix4(descriptor.uniformMV, false, ref modelView);
            GL.UniformMatrix4(descriptor.uniformMVP, false, ref modelViewProjection);
            GL.UniformMatrix4(descriptor.uniformProjection, false, ref projection);
            GL.Uniform1(descriptor.cube_texture, 0);
            GL.Uniform3(descriptor.uniformCameraPos, pos);

            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Vertices.Length * Vector3.SizeInBytes),
                model.Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribVerticesLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribVerticesLocation);


            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.colorsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Colors.Length * Vector3.SizeInBytes),
                model.Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribColorsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribColorsLocation);
        }


        public static void InitSkybox()
        {
            var result = new Skybox();

            var ProgramIdForSky = GL.CreateProgram();

            var vert = GL.CreateShader(ShaderType.VertexShader);
            var vertText = File.ReadAllText(@"Assets\Shaders\skybox.vert");
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
            var fragText = File.ReadAllText(@"Assets\Shaders\skybox.frag");
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

            _skybox = result;
        }

        internal static void BindSkybox(Vector3[] verticesForCube, Vector3 playerPos, Matrix4 modelView, Matrix4 projection, int skyBoxTextureId)
        {
            var descriptor = _skybox;
            GL.UseProgram(descriptor.ProgramId);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(descriptor.TextureSampler, 0);

            GL.BindTexture(TextureTarget.TextureCubeMap, skyBoxTextureId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verticesForCube.Length * Vector3.SizeInBytes), verticesForCube, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(descriptor.AttribVerticesLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindVertexArray(descriptor.AttribVerticesLocation);
            GL.EnableVertexAttribArray(descriptor.AttribVerticesLocation);

            var translation = Matrix4.CreateTranslation(playerPos.X, playerPos.Y, playerPos.Z);

            var view = translation * modelView;

            GL.UniformMatrix4(descriptor.uniformMV, false, ref view);
            GL.UniformMatrix4(descriptor.uniformProjection, false, ref projection);
        }
    }
}
