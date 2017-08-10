using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ShaderOnForm
{
    public class ShaderManager
    {
        #region поля
        private int mainProgramID;

        private int vertexShaderID;

        /*       
            in vec3 vPosition;
            in  vec3 vColor;
            out vec4 color;
            uniform mat4 modelview;
         */

        private int fragmentShaderID;

        /*
         in vec4 color;
         out vec4 outputColor;
         */

        //private int attribute_vcol;
        //private int attribute_vpos;
        //private int uniform_mview;

        // Vertex Buffer Object 
        private int vbo_position;
        private int vbo_color;
        private int vbo_mview;

        private int vbo_normal;

        #endregion
        #region свойства

        //public GraphicsManager GraphicsManager { get; set; }

        public int AttributeVColor { get; set; }

        public int AttributeVPosition { get; set; }

        public int UniformModelView { get; set; }



        public int texcoord_buffer_address;
        public int TextureId { get; set; }
        public int UniformTexture_Parameter_Address { get; set; }


        public int AttributeTexcoord_Parameter_Address { get; set; }
        public Vector2[] TextureCoordinates { get; set; }


        public int AttributeNormal_Parameter_Address  { get; set; }
        public int UniformLightPosition_Parameter_Address { get; set; }

        public int VboPosition
        {
            get { return vbo_position; }
            set { vbo_position = value; }
        }

        public int VboColor
        {
            get { return vbo_color; }
            set { vbo_color = value; }
        }


        public int VboModelView
        {
            get { return vbo_mview; }
            set { vbo_mview = value; }
        }

        #endregion
        #region Методы

        /// <summary>
        /// Загрузить шейдеры
        /// </summary>
        public void InitProgram()
        {
            mainProgramID = GL.CreateProgram();
            vertexShaderID = LoadAndAttachShader("Shaders\\vs.glsl", ShaderType.VertexShader, mainProgramID);
            fragmentShaderID = LoadAndAttachShader("Shaders\\fs.glsl", ShaderType.FragmentShader, mainProgramID);
            GL.LinkProgram(mainProgramID);


            GetTexture(@"Assets\grass.jpg");

            AttributeVPosition = GL.GetAttribLocation(mainProgramID, "vPosition");
            AttributeVColor = GL.GetAttribLocation(mainProgramID, "vColor");
            UniformModelView = GL.GetUniformLocation(mainProgramID, "modelview");

            AttributeTexcoord_Parameter_Address = GL.GetAttribLocation(mainProgramID, "a_TexCoordinate");
            UniformTexture_Parameter_Address = GL.GetUniformLocation(mainProgramID, "u_Texture");

            AttributeNormal_Parameter_Address = GL.GetAttribLocation(mainProgramID, "a_Normal");
            UniformLightPosition_Parameter_Address = GL.GetUniformLocation(mainProgramID, "u_LightPos");
            

            if (AttributeVPosition == -1 || AttributeVColor == -1 || UniformModelView == -1)
            {
                throw new Exception("Error binding attributes");
            }
            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);

            GL.GenBuffers(1, out texcoord_buffer_address);


            GL.GenBuffers(1, out vbo_normal);
        }

        private int LoadAndAttachShader(String filename, ShaderType type, int program)
        {
            int address = GL.CreateShader(type);


            switch (filename)
            {
                case "Shaders\\vs.glsl":
                    GL.ShaderSource(address,
                        @"#version 330
 
in vec3 vPosition;
in  vec3 vColor;
out vec4 color;
out vec3 v_Normal;
uniform mat4 modelview;
// lighting
attribute vec3 a_Normal;

// textures
attribute vec2 a_TexCoordinate; // Per-vertex texture coordinate information we will pass in.
varying vec2 v_TexCoordinate;   // This will be passed into the fragment shader.

void
main()
{
    gl_Position = modelview * vec4(vPosition, 1.0);

 // Pass through the texture coordinate.
    v_TexCoordinate = a_TexCoordinate;

     v_Normal = vec3(modelview * vec4(a_Normal, 0.0));

    color = vec4( vColor, 1.0);
}");
                    break;





                case "Shaders\\fs.glsl":
                    GL.ShaderSource(address,
                        @"#version 330
 
precision mediump float; 

in vec4 color;
out vec4 outputColor;
 
//textures
uniform sampler2D u_Texture; // The input texture.
varying vec2 v_TexCoordinate; // Interpolated texture coordinate per fragment.


// lighting
uniform vec3 u_LightPos;      // The position of the light in eye space.
vec3 vPosition;
varying vec3 v_Normal;         // Interpolated normal for this fragment.

void
main()
{

    
    float distance = length(u_LightPos - vPosition);
 
    // Get a lighting direction vector from the light to the vertex.
    vec3 lightVector = normalize(u_LightPos - vPosition);
 
    // Calculate the dot product of the light vector and vertex normal. If the normal and light vector are
    // pointing in the same direction then it will get max illumination.
    float diffuse = max(dot(v_Normal, lightVector), 0.1);
 
    // Add attenuation.
    diffuse = diffuse * (1.0 / (1.0 + (0.25 * distance * distance)));
 
    // Multiply the color by the diffuse illumination level to get final output color.
    //gl_FragColor = v_Color * diffuse;

    diffuse = diffuse * 20;
    outputColor = texture2D(u_Texture, v_TexCoordinate) * (color * 1);

    if(diffuse < 0.01)
    {
        outputColor = vec4(0);
    }

}");
                    break;

                default:
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        GL.ShaderSource(address, sr.ReadToEnd());
                    }

                    break;
            }

            GL.CompileShader(address);
            GL.AttachShader(program, address);
            return address;
        }


        public void GetTexture(string path)
        {
            TextureId = GL.GenTexture();
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
        }

        public Vector2[] GetTextureCoordinates(Array particles)
        {
            Vector2[] texCoordsPoints = new Vector2[]
            {
                new Vector2(0.0f, 0.0f), 				
		    	new Vector2(0.0f, 1.0f),
		    	new Vector2(1.0f, 0.0f),

                new Vector2(0.0f, 1.0f),
		    	new Vector2(1.0f, 1.0f),
		    	new Vector2(1.0f, 0.0f),	
            };

            var texCoordsPointsCurrent = new Vector2[particles.Length];
            for (int i = 0; i < texCoordsPointsCurrent.Length; i++)
            {
                texCoordsPointsCurrent[i] = texCoordsPoints[i % 6];
            }

            return texCoordsPointsCurrent;
        }


        public void BindShaderParams(Vector3[] vertices, Vector3[] colors, Vector3[] Normals, ref Matrix4 modelView)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttributeVPosition, 3, VertexAttribPointerType.Float, false, 0, 0);// -> in v3 vposition

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);// связать буфер vbo_color c colors -> in v3 vColor
            GL.BufferData<Vector3>(
                BufferTarget.ArrayBuffer,
                size: (IntPtr)(colors.Length * Vector3.SizeInBytes),//чтобы цвет был у каждой точки, а не у первых трех, например
                data: colors,
                usage: BufferUsageHint.StaticDraw);//выделив под него память и взяв массив colors

            GL.VertexAttribPointer(
                index: AttributeVColor,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: true,
                stride: 0,
                offset: 0);

            GL.UniformMatrix4(
                location: UniformModelView,
                transpose: false,
                matrix: ref modelView);

            GL.UseProgram(mainProgramID);

               // активная текстура - т0
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(UniformTexture_Parameter_Address, 0);
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            TextureCoordinates = GetTextureCoordinates(vertices);


            GL.BindBuffer(BufferTarget.ArrayBuffer, texcoord_buffer_address);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(TextureCoordinates.Length * Vector2.SizeInBytes),
              TextureCoordinates, BufferUsageHint.StaticCopy);
            GL.VertexAttribPointer(AttributeTexcoord_Parameter_Address, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_normal);

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(Normals.Length * Vector3.SizeInBytes),
              Normals, BufferUsageHint.StaticCopy);
            GL.VertexAttribPointer(AttributeNormal_Parameter_Address, 3, VertexAttribPointerType.Float, false, 0, 0);


        }
        #endregion
    }
}
