using System;
using Common.Graphics.FrameBuffer;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Glass.Graphics
{
    public class FrameBufferManager
    {
        #region main framebuffer

        public FrameBufferDesc GeometryFrameBufferDescriptorDescriptor { get; set; }

        #endregion

        public int Width { get; set; }
        public int Height { get; set; }

        public int FrameBufferProgramId { get; private set; }

        public FrameBufferManager(int width, int height)
        {
            Width = width;
            Height = height;

            GeometryFrameBufferDescriptorDescriptor = CreateFrameBuffer(width, height);
        }

        private FrameBufferDesc CreateFrameBuffer(int width, int height)
        {
            int frameBufferObject = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);


            int positionBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, positionBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f,
                width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, positionBuffer, 0);


            int normalBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, normalBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f,
                width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, normalBuffer, 0);

            int colorAndSpectacularBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, colorAndSpectacularBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, colorAndSpectacularBuffer, 0);


            var enabledBuffers = new DrawBuffersEnum[] { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1, DrawBuffersEnum.ColorAttachment2 };

            GL.DrawBuffers(enabledBuffers.Length, enabledBuffers);

            int depthBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, depthBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, width, height, 0, PixelFormat.DepthComponent, PixelType.UnsignedByte, IntPtr.Zero);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)All.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToBorder);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new float[] { 1, 1, 1, 1 });

            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthBuffer, 0);


            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
            {
                return new FrameBufferDesc()
                {
                    FrameBufferObject = frameBufferObject,

                    PositionTextureId = positionBuffer,
                    NormalTextureId = normalBuffer,
                    ColorAndSpectacularTextureId = colorAndSpectacularBuffer,
                    DepthTextureId = depthBuffer,
                };
            }

            throw new Exception("main frameBuffer fail " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
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

      /*  public void EnableSecondFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, LightingFrameBufferDescriptorDescriptor.FrameBufferObject);
            GL.Viewport(0, 0, Width, Height);
        }

        public void DisableSecondFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }*/




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

        /*private readonly Vector2[] QuadVerticesColor2 = new[] {
         new Vector2(-1f,  1f),
        new Vector2(-1f, -1f),
        new Vector2(1f, -1f),

        new Vector2(-1f, 1f),
        new Vector2(1f, -1f),
        new Vector2(1f,  1f),
        };*/

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
