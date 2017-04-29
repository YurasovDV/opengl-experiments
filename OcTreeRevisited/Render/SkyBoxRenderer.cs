using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OcTreeRevisited
{
    class SkyBoxRenderer
    {
        private const float SIZE = 580f;

        public static Vector3[] VERTICES = {
       new Vector3(-SIZE,  SIZE, -SIZE),
       new Vector3(-SIZE, -SIZE, -SIZE),
       new Vector3(SIZE, -SIZE, -SIZE),
       new Vector3(SIZE, -SIZE, -SIZE),
       new Vector3(SIZE,  SIZE, -SIZE),
       new Vector3(-SIZE,  SIZE, -SIZE),

        new Vector3(-SIZE, -SIZE,  SIZE),
        new Vector3(-SIZE, -SIZE, -SIZE),
        new Vector3(-SIZE,  SIZE, -SIZE),
        new Vector3(-SIZE,  SIZE, -SIZE),
        new Vector3(-SIZE,  SIZE,  SIZE),
        new Vector3(-SIZE, -SIZE,  SIZE),

         new Vector3(SIZE, -SIZE, -SIZE),
         new Vector3(SIZE, -SIZE,  SIZE),
         new Vector3(SIZE,  SIZE,  SIZE),
         new Vector3(SIZE,  SIZE,  SIZE),
         new Vector3(SIZE,  SIZE, -SIZE),
         new Vector3(SIZE, -SIZE, -SIZE),

        new Vector3(-SIZE, -SIZE,  SIZE),
        new Vector3(-SIZE,  SIZE,  SIZE),
        new Vector3( SIZE,  SIZE,  SIZE),
        new Vector3( SIZE,  SIZE,  SIZE),
        new Vector3( SIZE, -SIZE,  SIZE),
        new Vector3(-SIZE, -SIZE,  SIZE),

        new Vector3(-SIZE,  SIZE, -SIZE),
        new Vector3( SIZE,  SIZE, -SIZE),
        new Vector3( SIZE,  SIZE,  SIZE),
        new Vector3( SIZE,  SIZE,  SIZE),
        new Vector3(-SIZE,  SIZE,  SIZE),
        new Vector3(-SIZE,  SIZE, -SIZE),

        new Vector3(-SIZE, -SIZE, -SIZE),
        new Vector3(-SIZE, -SIZE,  SIZE),
        new Vector3( SIZE, -SIZE, -SIZE),
        new Vector3( SIZE, -SIZE, -SIZE),
        new Vector3(-SIZE, -SIZE,  SIZE),
        new Vector3( SIZE, -SIZE,  SIZE)
    };

        public static string[] skyboxPaths = new string[] {
                                                    @"Assets\\Skybox\\right.png",
                                                    @"Assets\\Skybox\\left.png",
                                                    @"Assets\\Skybox\\top.png",
                                                    @"Assets\\Skybox\\bottom.png",
                                                    @"Assets\\Skybox\\back.png",
                                                    @"Assets\\Skybox\\front.png"};


        public int TextureId { get; set; }

        public int UniformSkySampler { get; set; }

        public int ShaderProgramForSky { get; set; }

        public int sky_buffer;

        public int AttribPositionSkybox { get; set; }

        public int UniformViewSkybox { get; set; }

        public int UniformProjectionSkybox { get; set; }


        public SkyBoxRenderer()
        {
            TextureId = LoadCubeMapForSkyBox(skyboxPaths);
            GL.GenBuffers(1, out sky_buffer);


            ShaderProgramForSky = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            // Define a simple shader program for our point.
            string pointVertexShader = null;

            using (StreamReader rd = new StreamReader(@"Assets\Shaders\skyboxVertex.glsl"))
            {
                pointVertexShader = rd.ReadToEnd();
            }


            GL.ShaderSource(vertexShader, pointVertexShader);
            GL.CompileShader(vertexShader);
            GL.AttachShader(ShaderProgramForSky, vertexShader);

            string pointFragmentShader = null;
            using (StreamReader rd = new StreamReader(@"Assets\Shaders\skyboxFragment.glsl"))
            {
                pointFragmentShader = rd.ReadToEnd();
            }

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, pointFragmentShader);
            GL.CompileShader(fragmentShader);
            GL.AttachShader(ShaderProgramForSky, fragmentShader);
            GL.LinkProgram(ShaderProgramForSky);

            GL.UseProgram(ShaderProgramForSky);

            AttribPositionSkybox = GL.GetAttribLocation(ShaderProgramForSky, "position");
            UniformProjectionSkybox = GL.GetUniformLocation(ShaderProgramForSky, "projection");
            UniformViewSkybox = GL.GetUniformLocation(ShaderProgramForSky, "view");
            UniformSkySampler = GL.GetUniformLocation(ShaderProgramForSky, "cubemap");
        }

        private int LoadCubeMapForSkyBox(string[] paths)
        {
            int textureId;

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.GenTextures(1, out textureId);

            for (int i = 0; i < paths.Length; i++)
            {
                var path = paths[i];

                var png = new Bitmap(path);

                var width = png.Width;

                var height = png.Height;

                var rect = new Rectangle(0, 0, width, height);

                var bitmap_data = png.LockBits(rect, ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                GL.BindTexture(TextureTarget.TextureCubeMap, textureId);

                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0,
                    PixelInternalFormat.Rgba,
                    width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                    PixelType.UnsignedByte, IntPtr.Zero);


                GL.TexSubImage2D(TextureTarget.TextureCubeMapPositiveX + i,
                    level: 0, xoffset: 0, yoffset: 0,
                 width: width, height: height,
                 format: OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                 type: PixelType.UnsignedByte,
                 pixels: bitmap_data.Scan0);

                png.UnlockBits(bitmap_data);

                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)All.Linear);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)All.Linear);

                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)All.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);

            }

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);

            return textureId;

        }

        public void Render(Vector3 viewPoint, Matrix4 modelView, Matrix4 projection)
        {
            GL.UseProgram(ShaderProgramForSky);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(UniformSkySampler, 0);
            GL.BindTexture(TextureTarget.TextureCubeMap, TextureId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, sky_buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VERTICES.Length * Vector3.SizeInBytes),
                VERTICES, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttribPositionSkybox, 3, VertexAttribPointerType.Float,
                                false, 0, 0);
            GL.BindVertexArray(AttribPositionSkybox);
            GL.EnableVertexAttribArray(AttribPositionSkybox);

            var translation = Matrix4.CreateTranslation(viewPoint.X, 0, viewPoint.Z);
            var view = translation * modelView;

            GL.UniformMatrix4(UniformViewSkybox, false, ref view);
            GL.UniformMatrix4(UniformProjectionSkybox, false, ref projection);


            GL.DrawArrays(PrimitiveType.Triangles, 0, VERTICES.Length);
        }
    }
}
