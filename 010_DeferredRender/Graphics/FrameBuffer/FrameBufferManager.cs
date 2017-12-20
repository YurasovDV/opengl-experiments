using System;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics.FrameBuffer
{
    public class FrameBufferManager
    {
        #region main framebuffer

        public FrameBufferDesc GBuferDescriptor { get; set; }

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

            GBuferDescriptor = init.CreateGBuffer(width, height);
        }
        
        public void EnableMainFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, GBuferDescriptor.FramBufferObject);
            GL.Viewport(0, 0, Width, Height);
        }

        public void DisableMainFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public Vector2[] GetFrameBufferVertices()
        {
            return QuadVerticesFullScreen;
        }

        public Vector2[] GetFrameBufferVertices(FramebufferAttachment? kind)
        {
            if (kind != null)
            {
                switch (kind)
                {
                    case FramebufferAttachment.ColorAttachment0:
                        return QuadVerticesColor0;

                    case FramebufferAttachment.ColorAttachment1:
                        return QuadVerticesColor1;

                    case FramebufferAttachment.ColorAttachment2:
                        return QuadVerticesColor2;

                    case FramebufferAttachment.DepthAttachment:
                        return QuadVerticesDepth;

                    default:
                        break;
                }
            }

            return QuadVerticesFullScreen;
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


        private readonly Vector2[] QuadVerticesFullScreen = new[] {
        new Vector2(-1.0f,  1.0f),
        new Vector2(-1.0f, -1.0f),
        new Vector2(1.0f, -1.0f),

        new Vector2(-1.0f,  1.0f),
        new Vector2( 1.0f, -1.0f),
        new Vector2(1.0f,  1.0f),
        };

        private readonly Vector2[] QuadVerticesColor0 = new[] {
        new Vector2(-1.0f,  1.0f),
        new Vector2(-1.0f, -1.0f),
        new Vector2(1.0f, -1.0f),

        new Vector2(-1.0f,  1.0f),
        new Vector2( 1.0f, -1.0f),
        new Vector2(1.0f,  1.0f),
        };

        private readonly Vector2[] QuadVerticesColor1 = new[] {
        new Vector2(-1.0f,  1.0f),
        new Vector2(-1.0f, -1.0f),
        new Vector2(1.0f, -1.0f),

        new Vector2(-1.0f,  1.0f),
        new Vector2( 1.0f, -1.0f),
        new Vector2(1.0f,  1.0f),
        };

        private readonly Vector2[] QuadVerticesColor2 = new[] {
        new Vector2(-1.0f,  1.0f),
        new Vector2(-1.0f, -1.0f),
        new Vector2(1.0f, -1.0f),

        new Vector2(-1.0f,  1.0f),
        new Vector2( 1.0f, -1.0f),
        new Vector2(1.0f,  1.0f),
        };

        private readonly Vector2[] QuadVerticesDepth = new[] {
        new Vector2(-1.0f,  1.0f),
        new Vector2(-1.0f, -1.0f),
        new Vector2(1.0f, -1.0f),

        new Vector2(-1.0f,  1.0f),
        new Vector2( 1.0f, -1.0f),
        new Vector2(1.0f,  1.0f),
        };



    }
}
