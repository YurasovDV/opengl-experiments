using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Common.Utils;
using SimpleShooter.Graphics.ShaderLoad;
using SimpleShooter.PlayerControl;

namespace SimpleShooter.Graphics
{
    class SkyBoxRenderer
    {
        public int SkyBoxTextureId { get; set; }

        private Vector3[] _verticesForCube = null;
        private float _size;

        public SkyBoxRenderer(float size)
        {
            _size = size;
            _verticesForCube = GeometryHelper.GetVerticesForCube(size);
            SkyBoxTextureId = LoadTextures(size);
        }


        public void Render(IShooterPlayer player, Camera camera)
        {
            ShaderProgramDescriptor descriptor;
            ShaderLoader.TryGet(ShadersNeeded.SkyBox, out descriptor);

            GL.UseProgram(descriptor.ProgramId);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(descriptor.TextureSampler, 0);

            GL.BindTexture(TextureTarget.TextureCubeMap, SkyBoxTextureId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_verticesForCube.Length * Vector3.SizeInBytes), _verticesForCube, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribVerticesLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindVertexArray(descriptor.AttribVerticesLocation);
            GL.EnableVertexAttribArray(descriptor.AttribVerticesLocation);

            var translation = Matrix4.CreateTranslation(player.Position.X, player.Position.Y - 0, player.Position.Z);

            var view = translation * camera.ModelView;

            GL.UniformMatrix4(descriptor.uniformMV, false, ref view);
            GL.UniformMatrix4(descriptor.uniformProjection, false, ref camera.Projection);

            GL.DrawArrays(PrimitiveType.Triangles, 0, _verticesForCube.Length);
        }


        private int LoadTextures(float size)
        {
            var textureId = 0;
            GL.ActiveTexture(TextureUnit.Texture0);
            textureId = GL.GenTexture();

            for (int i = 0; i < 6; i++)
            {
                var png = new Bitmap(@"Content\Textures\Skybox\" + skyboxPaths[i]);
                var width = png.Width;
                var height = png.Height;

                var bitmapData = png.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.BindTexture(TextureTarget.TextureCubeMap, textureId);

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
             "back.png",
             "front.png"
        };

    }
}
