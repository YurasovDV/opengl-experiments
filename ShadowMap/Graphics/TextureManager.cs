using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ShadowMap
{
    public class TextureManager
    {
        public TextureManager()
        {

        }

        public int LoadTexture(string path, int width = -1, int height = -1)
        {
            var TextureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            Bitmap png;
            var bitmap_data = GetBitmapData(path, TextureTarget.Texture2D, ref width, ref height, out png);
            LoadToOpenGL(width, height, png, bitmap_data);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            return TextureId;
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
                    TextureId= depthMapTextureId, 
                };
            }

            throw new Exception("frameBuffer fail");           
        }

        public FrameBufferDesc GetFrameBuffer(int width, int height)
        {
            int frameBufferObject = GL.GenFramebuffer();
            int depthMapTextureId = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, depthMapTextureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent,
                width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.DepthComponent, PixelType.UnsignedByte, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)All.ClampToEdge);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthMapTextureId, 0);

            // no need for color attachment
            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status == FramebufferErrorCode.FramebufferComplete)
            {
                return new FrameBufferDesc()
                {
                    FramBufferObject = frameBufferObject,
                    RenderBufferObject = 0,
                    TextureId = depthMapTextureId,
                };
            }

            throw new Exception("frameBuffer second fail" + status.ToString());
        }

        private BitmapData GetBitmapData(string path, TextureTarget kind, ref int width, ref int height, out Bitmap png)
        {
            png = new Bitmap(path);

            if (width == -1)
            {
                width = png.Width;
            }

            if (height == -1)
            {
                height = png.Height;
            }

            GL.TexImage2D(kind, 0, PixelInternalFormat.Rgba,
                width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

            var rect = new Rectangle(0, 0, width, height);

            var bitmap_data = png.LockBits(rect,
            ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            return bitmap_data;

        }

        private void LoadToOpenGL(int width, int height, Bitmap png, BitmapData bitmap_data)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);

            GL.TexSubImage2D(TextureTarget.Texture2D, level: 0, xoffset: 0, yoffset: 0,
                             width: width, height: height, format: OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                             type: PixelType.UnsignedByte,
                             pixels: bitmap_data.Scan0);

            png.UnlockBits(bitmap_data);
        }

        public Vector2[] GetTextureCoordinates(Array vertices)
        {
            Vector2[] texCoordsPoints = new Vector2[]
            {

                new Vector2(0.0f, 1.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 0.0f),


                new Vector2(0.0f, 1.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                
            };

            var texCoordsPointsCurrent = new Vector2[vertices.Length];
            for (int i = 0; i < texCoordsPointsCurrent.Length; i++)
            {
                texCoordsPointsCurrent[i] = texCoordsPoints[i % 6];
            }

            return texCoordsPointsCurrent;
        }
    }
}
