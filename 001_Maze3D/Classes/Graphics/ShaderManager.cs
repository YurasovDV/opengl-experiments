using System;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Maze3D.Classes.Graphics
{
    class ShaderManager
    {
        private RenderEngine renderEngine;

        public int VertexAttribLocation { get; set; }
        public int ColorAttribLocation { get; set; }
        public int NormalAttribLocation { get; set; }

        public int Uniform_ModelViewProjection { get; set; }
        public int Uniform_Projection { get; set; }
        public int Uniform_ModelView { get; set; }

        public int Uniform_LightPos { get; set; }

        public int vertex_buffer_address;
        public int color_buffer_address;
        public int normal_buffer_address;

        public int ProgramId { get; set; }


        private bool firstDraw = true;

        public ShaderManager(RenderEngine renderEngine)
        {
            this.renderEngine = renderEngine;

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
                GL.GetShaderInfoLog(fragmentShader, out info);
                throw new Exception("fragment shader: " + info);
            }


            GL.LinkProgram(ProgramId);

            GL.UseProgram(ProgramId);

            VertexAttribLocation = GL.GetAttribLocation(ProgramId, "vPosition");
            ColorAttribLocation = GL.GetAttribLocation(ProgramId, "vColor");
            NormalAttribLocation = GL.GetAttribLocation(ProgramId, "vNormal");

            Uniform_ModelViewProjection = GL.GetUniformLocation(ProgramId, "u_modelViewprojection");
            Uniform_Projection = GL.GetUniformLocation(ProgramId, "u_projection");
            Uniform_ModelView = GL.GetUniformLocation(ProgramId, "u_modelview");

            Uniform_LightPos = GL.GetUniformLocation(ProgramId, "vLightPosition");

            GL.GenBuffers(1, out vertex_buffer_address);
            GL.GenBuffers(1, out color_buffer_address);
            GL.GenBuffers(1, out normal_buffer_address);

        }

        internal void BindBuffers(SimpleModel model, Vector3 light, bool refreshVertices = false, bool refreshColors = false)
        {
            GL.UseProgram(ProgramId);

            GL.UniformMatrix4(Uniform_ModelViewProjection, false, ref renderEngine.ModelViewProjection);
            GL.UniformMatrix4(Uniform_ModelView, false, ref renderEngine.ModelView);
            GL.UniformMatrix4(Uniform_Projection, false, ref renderEngine.Projection);

            GL.Uniform3(Uniform_LightPos, ref light);

            if (firstDraw || refreshVertices || refreshColors)
            {
                if (firstDraw || refreshVertices)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_address);
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(model.Vertices.Length * Vector3.SizeInBytes),
                        model.Vertices, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(VertexAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
                }


                if (firstDraw || refreshColors)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, color_buffer_address);
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(model.Colors.Length * Vector3.SizeInBytes),
                        model.Colors, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(ColorAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
                }

                firstDraw = false;

            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, normal_buffer_address);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(model.Normals.Length * Vector3.SizeInBytes),
                model.Normals, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(NormalAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(VertexAttribLocation);
            GL.EnableVertexAttribArray(ColorAttribLocation);
            GL.EnableVertexAttribArray(NormalAttribLocation);
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

    diffuse = 200 * diffuse * (1.0f / (1 +  0.25 * distance * distance));

    //diffuse = max(diffuse, 0.1f);

    outputColor = color * diffuse;
}";


    }
}
