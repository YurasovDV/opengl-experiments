using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL4;


namespace SimpleShadows.Graphics
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
           
                new Vector2(1.0f, 0.0f), 	
                new Vector2(1.0f, 1.0f),
                new Vector2(0.0f, 1.0f),
                
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
