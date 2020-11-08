using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Common;
using Common.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Glass.Graphics
{
    class SkyBoxRenderer
    {
        public int SkyBoxTextureId { get; set; }

        private Vector3[] _verticesForCube = null;
        private float _size;

        public SkyBoxRenderer(float size)
        {
            _size = size;
            _verticesForCube = GeometryHelper.GetVerticesForSkyBoxCube(size);
            SkyBoxTextureId = LoadTextures(size);
        }


        public void Render(Vector3 playerPos, Matrix4 modelView, Matrix4 projection)
        {
            Shaders.BindSkybox(_verticesForCube, playerPos, modelView, projection, SkyBoxTextureId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _verticesForCube.Length);
        }


        private int LoadTextures(float size)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            var textureId = GL.GenTexture();

            GL.BindTexture(TextureTarget.TextureCubeMap, textureId);

            for (int i = 0; i < 6; i++)
            {
                var png = new Bitmap(@"Assets\Textures_p\Skybox\" + skyboxPaths[i]);
                var width = png.Width;
                var height = png.Height;

                var bitmapData = png.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);


                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i,
                    0,
                    PixelInternalFormat.Rgba,
                    width,
                    height,
                    0,
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    IntPtr.Zero);

                GL.TexSubImage2D(TextureTarget.TextureCubeMapPositiveX + i,
                    0,
                    0,
                    0,
                    width,
                    height,
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    bitmapData.Scan0);


                png.UnlockBits(bitmapData);

                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)All.Linear);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)All.Linear);

                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)All.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);

            }

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);

            return textureId;
        }

        private static string[] skyboxPaths = new string[]
        {
             "right.png",
             "left.png",
             "top.png",
             "bottom.png",
             "front.png",
             "back.png",
        };

    }
}
