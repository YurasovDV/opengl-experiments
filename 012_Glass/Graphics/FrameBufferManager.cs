using System;
using Common.Graphics.FrameBuffer;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Glass.Graphics
{
    public class FrameBufferManager
    {
        #region main framebuffer

        public FrameBufferDesc ReflectionsMapFrameBufferDescriptor { get; set; }

        #endregion

        public int Width { get; set; }
        public int Height { get; set; }

        // public int FrameBufferProgramId { get; private set; }

        public FrameBufferManager(int width, int height)
        {
            Width = width;
            Height = height;

            ReflectionsMapFrameBufferDescriptor = CreateFrameBufferWithCubeMap(width, height);
        }

        private FrameBufferDesc CreateFrameBufferWithCubeMap(int width, int height)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            var cubeMapTextureForReflections = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, cubeMapTextureForReflections);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)All.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);

            // FBO can't hold all of the textures so we will swap color attachment 0 at render stage
            for (int i = 0; i < 6; i++)
            {
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba,
                1024, 1024, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            }

            int frameBufferObject = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);

            GL.FramebufferTextureLayer(FramebufferTarget.Framebuffer, 
                FramebufferAttachment.ColorAttachment0,
                cubeMapTextureForReflections, 
                0,
                0);

            var enabledBuffers = new DrawBuffersEnum[] { DrawBuffersEnum.ColorAttachment0 };

            GL.DrawBuffers(enabledBuffers.Length, enabledBuffers);


            //var depthBuffer = GL.GenRenderbuffer();
            //GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
            //GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent16, width, height);
            //GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            //GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
            {
                return new FrameBufferDesc()
                {
                    FrameBufferObject = frameBufferObject,
                    DiffuseTextureId = cubeMapTextureForReflections,
                    // PositionTextureId = positionBuffer,
                };
            }

            throw new Exception("main frameBuffer fail: " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
        }

        public void EnableReflectionsFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, ReflectionsMapFrameBufferDescriptor.FrameBufferObject);
            GL.Viewport(0, 0, Width, Height);
        }

        public void DisableReflectionsFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}
