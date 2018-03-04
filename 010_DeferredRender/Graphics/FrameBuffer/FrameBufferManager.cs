using System;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics.FrameBuffer
{
    public class FrameBufferManager
    {
        #region main framebuffer

        public FrameBufferDesc GeometryFrameBufferDescriptorDescriptor { get; set; }

        #endregion       
        
        #region second framebuffer

        public FrameBufferDesc LightingFrameBufferDescriptorDescriptor { get; set; }

        #endregion

        public int Width { get; set; }
        public int Height { get; set; }

        public int FrameBufferProgramId { get; private set; }

        public FrameBufferManager(int width, int height)
        {
            Width = width;
            Height = height;
            var init = new FrameBufferInit();

            GeometryFrameBufferDescriptorDescriptor = init.CreateGBuffer(width, height);

            LightingFrameBufferDescriptorDescriptor = init.CreateSecondBuffer(width, height);
        }
        
        // draw geometry
        public void EnableMainFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, GeometryFrameBufferDescriptorDescriptor.FrameBufferObject);
            GL.Viewport(0, 0, Width, Height);
        }

        public void DisableMainFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void EnableSecondFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, LightingFrameBufferDescriptorDescriptor.FrameBufferObject);
            GL.Viewport(0, 0, Width, Height);
        }

        public void DisableSecondFrameBuffer()
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



        #region bottom panel for buffer parts


        private readonly Vector2[] QuadVerticesColor0 = new[] {
        new Vector2(-1.0f,  -0.5f),
        new Vector2(-1.0f, -0.9f),
        new Vector2(-0.5f, -0.9f),

        new Vector2(-1.0f,  -0.5f),
        new Vector2(-0.5f, -0.9f),
        new Vector2(-0.5f,  -0.5f),
        };

        private readonly Vector2[] QuadVerticesColor1 = new[] {
         new Vector2(-0.49f,  -0.5f),
        new Vector2(-0.49f, -0.9f),
        new Vector2(-0.01f, -0.9f),

        new Vector2(-0.49f,  -0.5f),
        new Vector2(-0.01f, -0.9f),
        new Vector2(-0.01f,  -0.5f),
        };

        private readonly Vector2[] QuadVerticesColor2 = new[] {
         new Vector2(0f,  -0.5f),
        new Vector2(0.0f, -0.9f),
        new Vector2(0.5f, -0.9f),

        new Vector2(0f,  -0.5f),
        new Vector2(0.5f, -0.9f),
        new Vector2(0.5f,  -0.5f),
        };

        private readonly Vector2[] QuadVerticesDepth = new[] {
         new Vector2(0.51f,  -0.5f),
        new Vector2(0.51f, -0.9f),
        new Vector2(1f, -0.9f),

        new Vector2(0.51f,  -0.5f),
        new Vector2(1f, -0.9f),
        new Vector2(1f,  -0.5f),
        };

        #endregion


    }
}
