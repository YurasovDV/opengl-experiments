using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Particles.Graphics
{
    class TextureManager
    {
        public int GetTexture(string path)
        {
            var TextureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            var png = new Bitmap(path);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                png.Width, png.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);

            //Lock pixel data to memory and prepare for pass through
            var rect = new Rectangle(0, 0, png.Width, png.Height);

            BitmapData bitmap_data = png.LockBits(rect,
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //Tell gl to write the data from are bitmap image/data to the bound texture
            GL.TexSubImage2D(TextureTarget.Texture2D, level: 0, xoffset: 0, yoffset: 0,
                width: png.Width, height: png.Height, format: OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                type: PixelType.UnsignedByte,
                pixels: bitmap_data.Scan0);

            //Release from memory
            png.UnlockBits(bitmap_data);

            return TextureId;
        }

        public Vector2[] GetTextureCoordinates(Array particles)
        {
            Vector2[] texCoordsPoints = new Vector2[]
            {
                new Vector2(1.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(0.0f, 0.0f), 	
			   /* 
                new Vector2(1.0f, 0.0f),
		    	new Vector2(0.0f, 1.0f),*/
		    	
            };

            var texCoordsPointsCurrent = new Vector2[particles.Length];
            for (int i = 0; i < texCoordsPointsCurrent.Length; i++)
            {
                texCoordsPointsCurrent[i] = texCoordsPoints[i % 4];
            }

            return texCoordsPointsCurrent;
        }
    }
}
