using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;


using OpenTK;
using OpenTK.Graphics.OpenGL4;

using Particles.Classes;

namespace Particles.Graphics
{
    public class RenderEngine
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float PointSize { get; set; }
        public Matrix4[] ModelView { get; set; }

        public Vector3[] Vertices { get; set; }
        public Vector3[] Colors { get; set; }
        public Vector2[] TextureCoordinates { get; set; }

        public bool IsFirstDraw { get; set; }

        // параметры для шейдеров
        public int AttributeVPosition_Parameter_Address { get; set; }

        public int AttributeVColor_Parameter_Address { get; set; }

        public int UniformModelView_Parameter_Address { get; set; }

        public int UniformTexture_Parameter_Address { get; set; }

        public int AttributeTexcoord_Parameter_Address { get; set; }

        public int vbo_position_buffer_address;

        public int vbo_color_buffer_address;

        public int vbo_modelView_buffer_address;

        public int texcoord_buffer_address;

        public int TextureId { get; set; }

        public int ProgramId { get; set; }

        public RenderEngine(int viewWidth, int viewHeight)
        {
            Width = viewWidth;
            Height = viewHeight;
            PointSize = 1;
            IsFirstDraw = true;
        }

        public void InitGraphics()
        {
            ModelView = new Matrix4[] { Matrix4.Identity };
            GL.Viewport(0, 0, (int)Width, (int)Height);
            GL.PointSize(PointSize);

            // float aspect = (float)Width / (float)Height;
            //var m1 = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.01f, 500f);

            Vertices = new Vector3[] 
            { 
                new Vector3(-0.8f, -0.8f, 0f),
                new Vector3( 0.8f, -0.8f, 0f),
                new Vector3( 0f,  0.8f, 0f)
            };

            InitTextures();
            InitShaders();
        }

        private void InitTextures()
        {
            TextureId = new TextureManager().GetTexture(@"Assets\particle3.png");

            //Release texture
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void InitShaders()
        {
            ProgramId = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderText);
            GL.CompileShader(vertexShader);
            GL.AttachShader(ProgramId, vertexShader);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderText);
            GL.CompileShader(fragmentShader);
            GL.AttachShader(ProgramId, fragmentShader);

            GL.LinkProgram(ProgramId);

            AttributeVPosition_Parameter_Address = GL.GetAttribLocation(ProgramId, "vPosition");
            AttributeVColor_Parameter_Address = GL.GetAttribLocation(ProgramId, "vColor");
            UniformModelView_Parameter_Address = GL.GetUniformLocation(ProgramId, "modelview");


            AttributeTexcoord_Parameter_Address = GL.GetAttribLocation(ProgramId, "a_TexCoordinate");
            UniformTexture_Parameter_Address = GL.GetUniformLocation(ProgramId, "u_Texture");

            if (AttributeVPosition_Parameter_Address == -1
                || AttributeVColor_Parameter_Address == -1
                || UniformModelView_Parameter_Address == -1
                || AttributeTexcoord_Parameter_Address == -1
                || UniformModelView_Parameter_Address == -1
                || TextureId == -1)
            {

                throw new Exception("Error binding attributes");
            }

            GL.GenBuffers(1, out vbo_position_buffer_address);
            GL.GenBuffers(1, out vbo_color_buffer_address);
            GL.GenBuffers(1, out vbo_modelView_buffer_address);

            GL.GenBuffers(1, out texcoord_buffer_address);

        }

        internal void Draw()
        {
            float aspect = (float)Width / (float)Height;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.01f, 300);
            ModelView[0] = projection;

            Vector3 target = new Vector3(0, 11f, 1);
            Vector3 eye = new Vector3(0.0f, 0.6f, -55f);

            var viewMatrix = Matrix4.LookAt(eye, target, Vector3.UnitY);

            ModelView[0] = Matrix4.Mult(viewMatrix, projection);

            GL.UseProgram(ProgramId);


            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position_buffer_address);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                (IntPtr)(Vertices.Length * Vector3.SizeInBytes),
                Vertices,
                BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(AttributeVPosition_Parameter_Address, 3, VertexAttribPointerType.Float, false, 0, 0);



            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color_buffer_address);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                (IntPtr)(Colors.Length * Vector3.SizeInBytes),
                Colors,
                BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(AttributeVColor_Parameter_Address,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 0,
                offset: 0);




            if (IsFirstDraw)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, texcoord_buffer_address);
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(TextureCoordinates.Length * Vector2.SizeInBytes),
                  TextureCoordinates, BufferUsageHint.DynamicDraw);
                GL.VertexAttribPointer(AttributeTexcoord_Parameter_Address, 2, VertexAttribPointerType.Float, false, 0, 0);
            }

            GL.UniformMatrix4(UniformModelView_Parameter_Address,
                transpose: false,
                matrix: ref ModelView[0]);

            // активная текстура - т0
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(UniformTexture_Parameter_Address, 0);
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, texcoord_buffer_address);

            GL.EnableVertexAttribArray(AttributeVPosition_Parameter_Address);
            GL.EnableVertexAttribArray(AttributeVColor_Parameter_Address);
            GL.EnableVertexAttribArray(AttributeTexcoord_Parameter_Address);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
            GL.DrawArrays(PrimitiveType.Quads, 0, Vertices.Length);

            GL.DisableVertexAttribArray(AttributeVPosition_Parameter_Address);
            GL.DisableVertexAttribArray(AttributeVColor_Parameter_Address);
            GL.DisableVertexAttribArray(AttributeTexcoord_Parameter_Address);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.Flush();
            IsFirstDraw = false;
        }

        private const string vertexShaderText = @"#version 330
 
in vec3 vPosition;
in vec3 vColor;
out vec4 color;
uniform mat4 modelview;
 
// textures
attribute vec2 a_TexCoordinate; // Per-vertex texture coordinate information we will pass in.
varying vec2 v_TexCoordinate;   // This will be passed into the fragment shader.

void
main()
{
    gl_Position = modelview * vec4(vPosition, 1.0);
    // Pass through the texture coordinate.
    v_TexCoordinate = a_TexCoordinate;
    color = vec4(vColor, 0.0f);
}";


        private const string fragmentShaderText = @"#version 330
 
in vec4 color;
out vec4 outputColor;

//textures
uniform sampler2D u_Texture; // The input texture.
varying vec2 v_TexCoordinate; // Interpolated texture coordinate per fragment.

void
main()
{
   
    vec4 tex = texture2D(u_Texture, v_TexCoordinate);
    vec4 tempColor = vec4(color.rgb, 1);

    if(tex.a > 0.98f)
    {
        //tempColor = vec4(0, 1, 0, 1);

        //discard;
        /*float rg = max(color.r, color.g);
        float rgb = max(rg, color.b);
        tempColor = vec4(rgb, rgb, rgb, 1);*/
    }

    outputColor =  tempColor * tex;
    outputColor.a = tex.a;
    
 
    
}";



    }
}
