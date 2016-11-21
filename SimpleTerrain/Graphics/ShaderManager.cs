using System;
using System.IO;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace SimpleTerrain
{
    public class ShaderManager
    {
        private RenderEngine renderEngine;


        public int AttrVertexLocation { get; set; }

        public int AttrColorLocation { get; set; }

        public int AttrNormalLocation { get; set; }

        public int AttrTexcoordLocation { get; set; }

        public int vertexBufferAddress;

        public int colorBufferAddress;

        public int normalBufferAddress;

        public int texCoordBufferAddress;


        public int uniformTextureFirst;

        public int uniformMVP;
        public int uniformMV;
        public int uniformProjection;

        public int uniformLightPos;

        public int MainProgramId { get; set; }

        public ShaderManager(RenderEngine renderEngine)
        {
            this.renderEngine = renderEngine;

            CreateMainProgram();

            GL.GenBuffers(1, out vertexBufferAddress);
            GL.GenBuffers(1, out colorBufferAddress);
            GL.GenBuffers(1, out normalBufferAddress);
            GL.GenBuffers(1, out texCoordBufferAddress);
        }

        private void CreateMainProgram()
        {
            MainProgramId = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            using (var rd = new StreamReader(@"Assets\Shaders\main_vertex.glsl"))
            {
                GL.ShaderSource(vertexShader, rd.ReadToEnd());
            }
            GL.CompileShader(vertexShader);
            GL.AttachShader(MainProgramId, vertexShader);

            int statusCode;

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vertexShader, out info);
                throw new Exception("vertex shader" + info);
            }

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            using (var rd = new StreamReader(@"Assets\Shaders\main_fragment.glsl"))
            {
                string text = rd.ReadToEnd();
                GL.ShaderSource(fragmentShader, text);
            }
            GL.CompileShader(fragmentShader);
            GL.AttachShader(MainProgramId, fragmentShader);

            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(fragmentShader, out info);
                throw new Exception("fragment shader: " + info);
            }

            GL.LinkProgram(MainProgramId);

            GL.UseProgram(MainProgramId);

            AttrVertexLocation = GL.GetAttribLocation(MainProgramId, "vPosition");
            AttrColorLocation = GL.GetAttribLocation(MainProgramId, "vColor");
            AttrNormalLocation = GL.GetAttribLocation(MainProgramId, "vNormal");

            uniformLightPos = GL.GetUniformLocation(MainProgramId, "uLightPos");

            uniformMVP = GL.GetUniformLocation(MainProgramId, "uMVP");
            uniformMV = GL.GetUniformLocation(MainProgramId, "uMV");
            uniformProjection = GL.GetUniformLocation(MainProgramId, "uProjection");

            AttrTexcoordLocation = GL.GetAttribLocation(MainProgramId, "vTexCoordinate");
            uniformTextureFirst = GL.GetUniformLocation(MainProgramId, "uTexture");

        }


        public void BindBuffers(SimpleModel model, Vector3 lightPosition)
        {
            GL.UseProgram(MainProgramId);

            GL.UniformMatrix4(uniformMVP, false, ref renderEngine.ModelViewProjection);
            GL.UniformMatrix4(uniformMV, false, ref renderEngine.ModelView);
            GL.UniformMatrix4(uniformProjection, false, ref renderEngine.Projection);

            GL.Uniform3(uniformLightPos, lightPosition);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferAddress);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Vertices.Length * Vector3.SizeInBytes), model.Vertices, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(AttrVertexLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferAddress);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Colors.Length * Vector3.SizeInBytes), model.Colors, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(AttrColorLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferAddress);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Normals.Length * Vector3.SizeInBytes), model.Normals, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(AttrNormalLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

            if (model.TextureId != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, texCoordBufferAddress);
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(model.TextureCoordinates.Length * Vector2.SizeInBytes),
                  model.TextureCoordinates, BufferUsageHint.DynamicDraw);
                GL.VertexAttribPointer(AttrTexcoordLocation, 2, VertexAttribPointerType.Float, false, 0, 0);

                GL.EnableVertexAttribArray(AttrTexcoordLocation);

                // активная текстура - т0
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.Uniform1(uniformTextureFirst, 0);
                GL.BindTexture(TextureTarget.Texture2D, model.TextureId);
            }
            else
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, texCoordBufferAddress);
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(0 * Vector2.SizeInBytes),
                  model.TextureCoordinates, BufferUsageHint.DynamicDraw);
                GL.VertexAttribPointer(AttrTexcoordLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            }

            GL.EnableVertexAttribArray(AttrVertexLocation);
            GL.EnableVertexAttribArray(AttrColorLocation);
            GL.EnableVertexAttribArray(AttrNormalLocation);

        }

        public void Release()
        {
            GL.DisableVertexAttribArray(AttrVertexLocation);
            GL.DisableVertexAttribArray(AttrColorLocation);
            GL.DisableVertexAttribArray(AttrNormalLocation);
            GL.DisableVertexAttribArray(AttrTexcoordLocation);
        }
    }
}