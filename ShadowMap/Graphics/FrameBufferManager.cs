using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ShadowMap
{
    public class FrameBufferManager
    {
        #region основной фреймбуфер
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

        #region вспомогательный фреймбуфер
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


        public FrameBufferManager(RenderEngine mainRender)
        {
            Width = mainRender.Width;
            Height = mainRender.Height;
            var textureMgr = new TextureManager();

            CreateMainFrameBuffer(mainRender, textureMgr);
            CreateSecondFrameBuffer(mainRender, textureMgr);

        }

        private void CreateMainFrameBuffer(RenderEngine mainRender, TextureManager textureMgr)
        {
            MainFrameBufferProgramId = mainRender.Shaders.FrameBufferProgramId;

            var frameBufDesc = textureMgr.GetMainFrameBuffer(Width, Height);
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

        private FrameBufferDesc CreateSecondFrameBuffer(RenderEngine mainRender, TextureManager textureMgr)
        {
            FrameBufferDesc frameBufDesc = textureMgr.GetFrameBuffer(Width, Height);
            SecondDepthMapBufferObject = frameBufDesc.FramBufferObject;
            SecondDepthMapBufferTextureId = frameBufDesc.TextureId;

            SecondFrameBufferProgramId = mainRender.Shaders.CreateFrameBufferProgram();

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
            /*GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(1, 0f, 0f, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(SecondFrameBufferProgramId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferForFrameSecondAddress);
            var points = GetFrameBufferVertices();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), points, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrVertexFrameSecondLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(AttrVertexFrameSecondLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, texCoordsForFrameSecondAddress);
            var texCoords = GetFrameBufferTextureCoords();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrTexcoordFrameSecondLocation, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(AttrTexcoordFrameSecondLocation);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(UniformTextureFrame, 0);
            GL.BindTexture(TextureTarget.Texture2D, SecondDepthMapBufferTextureId);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);*/
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
            GL.BindTexture(TextureTarget.Texture2D, MainDepthMapBufferTextureId);

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
