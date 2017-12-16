using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics.FrameBuffer
{
    class FrameBufferInit
    {
        public FrameBufferDesc CreateMainFrameBuffer(int width, int height)
        {
            var frameBufDesc = GetMainFrameBuffer(width, height);
            return frameBufDesc;
        }


        public FrameBufferDesc GetMainFrameBuffer(int width, int height)
        {
            int frameBufferObject = GL.GenFramebuffer();
            int depthMapTextureId = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, depthMapTextureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, depthMapTextureId, 0);

            int renderBufferObject = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderBufferObject);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,
                FramebufferAttachment.DepthStencilAttachment,
                RenderbufferTarget.Renderbuffer, renderBufferObject);


            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
            {
                return new FrameBufferDesc()
                {
                    FramBufferObject = frameBufferObject,
                    RenderBufferObject = renderBufferObject,
                    TextureId = depthMapTextureId,
                };
            }

            throw new Exception("frameBuffer fail");
        }
    }
}
