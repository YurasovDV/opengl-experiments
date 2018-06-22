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
            var cubeMapTextureForReflections = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, cubeMapTextureForReflections);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)All.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);

            // GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureBorderColor, new float[] { 1, 1, 1, 1 });

            // FBO can't hold all of the textures so we will swap color attachment 0 at render stage
            for (int i = 0; i < 6; i++)
            {
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba,
                width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);


               /* GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f,
               width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.Float, IntPtr.Zero);*/
            }

            int frameBufferObject = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, 
                TextureTarget.TextureCubeMapPositiveX, cubeMapTextureForReflections, 0);

            var enabledBuffers = new DrawBuffersEnum[] { DrawBuffersEnum.ColorAttachment0 };

            GL.DrawBuffers(enabledBuffers.Length, enabledBuffers);

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);

            int depthBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, depthBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, width, height, 0, PixelFormat.DepthComponent, PixelType.UnsignedByte, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)All.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToBorder);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthBuffer, 0);

            //GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthBuffer, 0);


            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
            {
                return new FrameBufferDesc()
                {
                    FrameBufferObject = frameBufferObject,
                    DiffuseTextureId = cubeMapTextureForReflections,
                    // PositionTextureId = positionBuffer,
                };
            }

            throw new Exception("main frameBuffer fail " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
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
