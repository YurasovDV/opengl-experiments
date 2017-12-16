using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Common.Graphics.FrameBuffer
{
    public class FrameBufferManager
    {
        #region main framebuffer
        public int MainDepthMapBufferObject { get; set; }
        public int MainDepthMapBufferTextureId { get; set; }
        public int MainRenderBufferObject { get; set; }

        public int AttrVertexFrameLocation { get; set; }
        public int AttrTexcoordFrameLocation { get; set; }
        public int UniformTextureFrame { get; set; }

        public int vertexBufferForFrameAddress;
        public int texCoordsForFrameAddress;

        public int MainFrameBufferProgramId { get; set; }

        #endregion

        #region auxillary framebuffer
        public int SecondDepthMapBufferObject { get; set; }
        public int SecondDepthMapBufferTextureId { get; set; }
        public int SecondRenderBufferObject { get; set; }

        public int AttrVertexFrameSecondLocation { get; set; }
        public int AttrTexcoordFrameSecondLocation { get; set; }
        public int UniformTextureFrameSecond { get; set; }

        public int vertexBufferForFrameSecondAddress;
        public int texCoordsForFrameSecondAddress;

        public int SecondFrameBufferProgramId { get; set; }

        #endregion

        public int Width { get; set; }
        public int Height { get; set; }

        public bool DebugDepth { get; set; }
        public int FrameBufferProgramId { get; private set; }

        public FrameBufferManager(int width, int height)
        {
            Width = width;
            Height = height;
            CreateFrameBufferProgram();
            CreateMainFrameBuffer();
            CreateSecondFrameBuffer();
        }
        
        private void CreateMainFrameBuffer()
        {
            MainFrameBufferProgramId = FrameBufferProgramId;

            var frameBufDesc = FrameBufferDesc.GetMainFrameBuffer(Width, Height);
            MainDepthMapBufferObject = frameBufDesc.FramBufferObject;
            MainDepthMapBufferTextureId = frameBufDesc.TextureId;
            MainRenderBufferObject = frameBufDesc.RenderBufferObject;

            GL.UseProgram(MainFrameBufferProgramId);
            AttrVertexFrameLocation = GL.GetAttribLocation(MainFrameBufferProgramId, "vPosition");
            AttrTexcoordFrameLocation = GL.GetAttribLocation(MainFrameBufferProgramId, "vTexCoordinate");
            UniformTextureFrame = GL.GetUniformLocation(MainFrameBufferProgramId, "uTexture");

            GL.GenBuffers(1, out texCoordsForFrameAddress);
            GL.GenBuffers(1, out vertexBufferForFrameAddress);
        }
        
        private FrameBufferDesc CreateSecondFrameBuffer()
        {
            FrameBufferDesc frameBufDesc = FrameBufferDesc.GetFrameBuffer(Width, Height);
            SecondDepthMapBufferObject = frameBufDesc.FramBufferObject;
            SecondDepthMapBufferTextureId = frameBufDesc.TextureId;

            CreateFrameBufferProgram();

            GL.UseProgram(SecondFrameBufferProgramId);
            AttrVertexFrameLocation = GL.GetAttribLocation(SecondFrameBufferProgramId, "vPosition");
            AttrTexcoordFrameLocation = GL.GetAttribLocation(SecondFrameBufferProgramId, "vTexCoordinate");
            UniformTextureFrame = GL.GetUniformLocation(SecondFrameBufferProgramId, "uTexture");

            GL.GenBuffers(1, out texCoordsForFrameSecondAddress);
            GL.GenBuffers(1, out vertexBufferForFrameSecondAddress);
            return frameBufDesc;
        }

        public void EnableAuxillaryFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, SecondDepthMapBufferObject);
            GL.Viewport(0, 0, Width, Height);
        }

        public void FlushAuxillaryFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void EnableMainFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, MainDepthMapBufferObject);
            GL.Viewport(0, 0, Width, Height);
        }

        public void FlushMainFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(1, 0f, 0f, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(MainFrameBufferProgramId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferForFrameAddress);
            var points = GetFrameBufferVertices();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), points, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrVertexFrameLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(AttrVertexFrameLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, texCoordsForFrameAddress);
            var texCoords = GetFrameBufferTextureCoords();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrTexcoordFrameLocation, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(AttrTexcoordFrameLocation);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(UniformTextureFrame, 0);

            if (DebugDepth)
            {
                GL.BindTexture(TextureTarget.Texture2D, SecondDepthMapBufferTextureId);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, MainDepthMapBufferTextureId);
            }
            

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);

            GL.Flush();
        }


        public void CreateFrameBufferProgram()
        {
            var programId = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            using (var rd = new StreamReader(@"Assets\Shaders\frameBufferVertex.glsl"))
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
            using (var rd = new StreamReader(@"Assets\Shaders\farameBufferFragment.glsl"))
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

            FrameBufferProgramId = programId;
        }


        private Vector2[] GetFrameBufferVertices()
        {
            return quadVertices;
        }

        private Vector2[] GetFrameBufferTextureCoords()
        {
            return textQuadIndices;
        }

        private readonly Vector2[] textQuadIndices = new[] {
              new Vector2(0.0f, 1.0f),
              new Vector2(0.0f, 0.0f),
              new Vector2(1.0f, 0.0f),

              new Vector2(0.0f, 1.0f),
              new Vector2(1.0f, 0.0f),
              new Vector2(1.0f, 1.0f)
        };

        private readonly Vector2[] quadVertices = new[] {
        // Positions  
        new Vector2(-1.0f,  1.0f),
        new Vector2(-1.0f, -1.0f),
        new Vector2(1.0f, -1.0f),

        new Vector2(-1.0f,  1.0f),
        new Vector2( 1.0f, -1.0f),
        new Vector2(1.0f,  1.0f),
        };
    }
}
