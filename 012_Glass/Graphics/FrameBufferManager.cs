using System;
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

        public const int FrameBufWidth = 2048;
        public const int FrameBufHeight = 2048;

        public FrameBufferManager(int width, int height)
        {
            Width = width;
            Height = height;

            ReflectionsMapFrameBufferDescriptor = CreateFrameBufferWithCubeMap(FrameBufWidth, FrameBufHeight);
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
            int frameBufferObject = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);

            for (int i = 0; i < 6; i++)
            {
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba,
                width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            }


            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment0,
                TextureTarget.TextureCubeMapPositiveX,
                cubeMapTextureForReflections,
                0);

            var enabledBuffers = new DrawBuffersEnum[] 
            { 
                DrawBuffersEnum.ColorAttachment0,
            };

            GL.DrawBuffers(enabledBuffers.Length, enabledBuffers);

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
            {
                return new FrameBufferDesc()
                {
                    FrameBufferObject = frameBufferObject,
                    DiffuseTextureId = cubeMapTextureForReflections,
                };
            }

            throw new Exception($"main frameBuffer fail: {GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer)}");
        }

        public void EnableReflectionsFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, ReflectionsMapFrameBufferDescriptor.FrameBufferObject);
            GL.Viewport(0, 0, FrameBufWidth, FrameBufHeight);
        }

        public void DisableReflectionsFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, Width, Height);
        }
    }
}
