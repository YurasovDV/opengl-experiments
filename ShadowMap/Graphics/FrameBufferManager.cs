using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ShadowMap
{
    public class FrameBufferManager
    {
        public int depthMapBuffer;
        public int depthMapBufferTextureId;
        public int renderBuffer;


        public int AttrVertexFrameLocation { get; set; }
        public int AttrTexcoordFrameLocation { get; set; }
        public int uniformTextureFrame;


        public int vertexBufferForFrameAddress;
        public int texcoordsForFrameAddress;

        public int FrameBufferProgramId { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }


        public FrameBufferManager(RenderEngine eng)
        {
            Width = eng.Width;
            Height = eng.Height;
            FrameBufferProgramId = eng.Shaders.FrameBufferProgramId;

            var frameBufDesc = new TextureManager().GetFrameBuffer(Width, Height);
            depthMapBuffer = frameBufDesc.FramBufferObject;
            depthMapBufferTextureId = frameBufDesc.TextureId;
            renderBuffer = frameBufDesc.RenderBufferObject;

            GL.UseProgram(FrameBufferProgramId);
            AttrVertexFrameLocation = GL.GetAttribLocation(FrameBufferProgramId, "vPosition");
            AttrTexcoordFrameLocation = GL.GetAttribLocation(FrameBufferProgramId, "vTexCoordinate");
            uniformTextureFrame = GL.GetUniformLocation(FrameBufferProgramId, "uTexture");

            GL.GenBuffers(1, out texcoordsForFrameAddress);
            GL.GenBuffers(1, out vertexBufferForFrameAddress);
        }

        public void EnableFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapBuffer);
            GL.Viewport(0, 0, Width, Height);
        }

        public void FlushFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(1, 0f, 0f, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(FrameBufferProgramId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferForFrameAddress);
            var points = GetFrameBufferVertices();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), points, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrVertexFrameLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(AttrVertexFrameLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, texcoordsForFrameAddress);
            var texCoords = GetFrameBufferTextureCoords();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrTexcoordFrameLocation, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(AttrTexcoordFrameLocation);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(uniformTextureFrame, 0);
            GL.BindTexture(TextureTarget.Texture2D, depthMapBufferTextureId);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);

            GL.Flush();
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
