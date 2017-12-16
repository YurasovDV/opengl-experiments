using System;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics.FrameBuffer
{
    public class FrameBufferManager
    {
        #region main framebuffer

        public FrameBufferDesc FrameBufDesc { get; set; }

        #endregion

        public int Width { get; set; }
        public int Height { get; set; }

        public bool DebugDepth { get; set; }
        public int FrameBufferProgramId { get; private set; }

        public FrameBufferManager(int width, int height)
        {
            Width = width;
            Height = height;
            var init = new FrameBufferInit();

            FrameBufDesc = init.CreateMainFrameBuffer(width, height);
        }
        
        public void EnableMainFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufDesc.FramBufferObject);
            GL.Viewport(0, 0, Width, Height);
        }

        public void DisableMainFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public Vector2[] GetFrameBufferVertices()
        {
            return QuadVertices;
        }

        public Vector2[] GetFrameBufferTextureCoords()
        {
            return TextQuadVertices;
        }

        private readonly Vector2[] TextQuadVertices = new[] {
              new Vector2(0.0f, 1.0f),
              new Vector2(0.0f, 0.0f),
              new Vector2(1.0f, 0.0f),

              new Vector2(0.0f, 1.0f),
              new Vector2(1.0f, 0.0f),
              new Vector2(1.0f, 1.0f)
        };

        private readonly Vector2[] QuadVertices = new[] {
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
