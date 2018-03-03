using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics.FrameBuffer
{
    class FrameBufferInit
    {
        /// <summary>
        /// Main buffer for geometry, depth, normals, color
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public FrameBufferDesc CreateGBuffer(int width, int height)
        {
            var frameBufDesc = GetMainFrameBuffer(width, height);
            return frameBufDesc;
        }

        /// <summary>
        /// second buffer for diffuse light
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public FrameBufferDesc CreateSecondBuffer(int width, int height)
        {
            var frameBufDesc = GetMainFrameBuffer(width, height);
            return frameBufDesc;
        }


        private FrameBufferDesc GetMainFrameBuffer(int width, int height)
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

        private FrameBufferDesc GetSecondFrameBuffer(int width, int height)
        {
            int frameBufferObject = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);

         
            int diffuseBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, diffuseBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f,
                width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, diffuseBuffer, 0);


            int spectacularBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, spectacularBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f,
                width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, spectacularBuffer, 0);

           
            var enabledBuffers = new DrawBuffersEnum[] { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1 };

            GL.DrawBuffers(enabledBuffers.Length, enabledBuffers);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
            {
                return new FrameBufferDesc()
                {
                    FrameBufferObject = frameBufferObject,
                    DiffuseTextureId = diffuseBuffer,
                    SpectacularTextureId = spectacularBuffer,
                    PositionTextureId = -1,
                    NormalTextureId = -1,
                    ColorAndSpectacularTextureId = -1,
                    DepthTextureId = -1,
                };
            }

            throw new Exception("second frameBuffer fail " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
        }
    }
}
