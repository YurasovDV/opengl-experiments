using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics.FrameBuffer
{
    class FrameBufferInit
    {
        public FrameBufferDesc CreateGBuffer(int width, int height)
        {
            var frameBufDesc = GetMainFrameBuffer(width, height);
            return frameBufDesc;
        }


        public FrameBufferDesc GetMainFrameBuffer(int width, int height)
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
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32f, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthBuffer, 0);

            //int renderBufferObject = GL.GenRenderbuffer();
            //GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderBufferObject);
            //GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
            //GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

            //GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,
            //    FramebufferAttachment.DepthStencilAttachment,
            //    RenderbufferTarget.Renderbuffer, renderBufferObject);


            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
            {
                return new FrameBufferDesc()
                {
                    FramBufferObject = frameBufferObject,

                    PositionTextureId = positionBuffer,
                    NormalTextureId = normalBuffer,
                    ColorAndSpectacularTextureId = colorAndSpectacularBuffer,
                    DepthTextureId = depthBuffer,
                };
            }

            throw new Exception("frameBuffer fail");
        }
    }
}
