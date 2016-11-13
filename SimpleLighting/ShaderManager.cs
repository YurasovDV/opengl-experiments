using System;

using OpenTK;
using OpenTK.Graphics.OpenGL4;


namespace SimpleLighting
{
    class ShaderManager
    {
        public int Attrib_Vertex_Address { get; set; }
        public int Attrib_Normal_Address { get; set; }
        public int Attrib_Color_Address { get; set; }

        public int Uniform_LightPos { get; set; }


        public int Uniform_ModelViewProjection { get; set; }
        public int Uniform_Projection { get; set; }
        public int Uniform_ModelView { get; set; }
          

        public int ProgramId { get; set; }

        public GraphicSystem Graphics { get; set; }


        private int vbo_vertex_buffer;
        private int vbo_normal_buffer;
        private int vbo_color_buffer;

        public ShaderManager(GraphicSystem graphContext)
        {
            Graphics = graphContext;
            InitShaders();
        }


        public void InitShaders()
        {
            ProgramId = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderText);
            GL.CompileShader(vertexShader);
            GL.AttachShader(ProgramId, vertexShader);

            int status_code;
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
            {
                string info;
                GL.GetShaderInfoLog(vertexShader, out info);
                throw new Exception("vertex shader: " + info);
            }

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderText);
            GL.CompileShader(fragmentShader);
            GL.AttachShader(ProgramId, fragmentShader);

            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out status_code);
            if (status_code != 1)
            {
                string info;
                GL.GetShaderInfoLog(vertexShader, out info);
                throw new Exception("fragment shader: " + info);
            }


            GL.LinkProgram(ProgramId);


            GL.UseProgram(ProgramId);

            Attrib_Vertex_Address = GL.GetAttribLocation(ProgramId, "vPosition");
            Attrib_Normal_Address = GL.GetAttribLocation(ProgramId, "vNormal");
            Attrib_Color_Address = GL.GetAttribLocation(ProgramId, "vColor");


            Uniform_LightPos = GL.GetUniformLocation(ProgramId, "vLightPosition");

            Uniform_ModelViewProjection = GL.GetUniformLocation(ProgramId, "u_modelViewprojection");
            Uniform_ModelView = GL.GetUniformLocation(ProgramId, "u_modelview");
            Uniform_Projection = GL.GetUniformLocation(ProgramId, "u_projection");


            GL.GenBuffers(1, out vbo_vertex_buffer);
            GL.GenBuffers(1, out vbo_normal_buffer);
            GL.GenBuffers(1, out vbo_color_buffer);
        }


        public void BindParams(SimpleModel model)
        {

            GL.UseProgram(ProgramId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_vertex_buffer);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                (IntPtr)(model.Vertices.Length * Vector3.SizeInBytes),
                model.Vertices
                , BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Attrib_Vertex_Address, 3, VertexAttribPointerType.Float, false, 0, 0);


            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color_buffer);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                (IntPtr)(model.Color.Length * Vector3.SizeInBytes),
                model.Color
                , BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Attrib_Color_Address, 3, VertexAttribPointerType.Float, false, 0, 0);



            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_normal_buffer);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                (IntPtr)(model.Normals.Length * Vector3.SizeInBytes),
                model.Normals
                , BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(Attrib_Normal_Address, 3, VertexAttribPointerType.Float, false, 0, 0);

            

            GL.Uniform3(Uniform_LightPos, Vector3.UnitZ);


            GL.EnableVertexAttribArray(Attrib_Vertex_Address);
            GL.EnableVertexAttribArray(Attrib_Color_Address);
            GL.EnableVertexAttribArray(Attrib_Normal_Address);

        }

        internal void BindMatrices(Matrix4 ViewMatrix, Matrix4 Projection)
        {
            GL.UniformMatrix4(Uniform_ModelViewProjection, false, ref Graphics.ModelView[0]);
            GL.UniformMatrix4(Uniform_ModelView, false, ref ViewMatrix);
            GL.UniformMatrix4(Uniform_Projection, false, ref Projection);
        }


        public void Unbind()
        {
            GL.DisableVertexAttribArray(Attrib_Vertex_Address);
            GL.DisableVertexAttribArray(Attrib_Color_Address);
            GL.DisableVertexAttribArray(Attrib_Normal_Address);

        }

        private string vertexShaderText = @"#version 330

uniform mat4 u_modelview;
uniform mat4 u_modelViewprojection;
uniform mat4 u_projection;

attribute vec3 vPosition;
attribute vec3 vNormal;
attribute vec3 vColor;


out vec4 color;
out vec3 normal;
out vec3 position;

void 
main()
{
    gl_Position = u_modelViewprojection * /*u_projection * u_modelview **/ vec4(vPosition, 1.0);
    color = vec4(vColor, 1.0);
    position = vec3(u_modelview * vec4(vPosition, 0.0)); 
    normal = vec3(u_modelview * vec4(vNormal, 0.0));
}";


        private string fragmentShaderText = @"#version 330

varying vec4 color;
varying vec3 normal;
varying vec3 position;
uniform vec3 vLightPosition;

out vec4 outputColor;

void 
main()
{
    vec3 toLight = vLightPosition - position; 
    float distance = length(toLight);

    vec3 lightVector = normalize(toLight);

    float diffuse = dot(lightVector, normalize(normal));
    diffuse = max(diffuse, 0.1f);

    diffuse = diffuse * (1.0f / (1 +  0.75 * distance * distance));

    outputColor = color * diffuse;
}";


    }
}
