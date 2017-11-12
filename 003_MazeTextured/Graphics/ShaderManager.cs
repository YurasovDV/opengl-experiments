using System;
using System.IO;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SimpleShadows.Core.Models;

namespace SimpleShadows.Graphics
{
    class ShaderManager
    {
        private RenderEngine renderEngine;

        public int VertexAttribLocation { get; set; }
        public int ColorAttribLocation { get; set; }
        public int NormalAttribLocation { get; set; }

        public int AttributeTexcoord_Parameter_Address { get; set; }

        public int Uniform_ModelViewProjection { get; set; }
        public int Uniform_Projection { get; set; }
        public int Uniform_ModelView { get; set; }

        public int Uniform_LightPos { get; set; }


        public int UniformTexture_Parameter_Address { get; set; }


        public int UniformViewSkybox { get; set; }
        public int UniformProjectionSkybox { get; set; }
        public int AttribPositionSkybox { get; set; }
        public int UniformSkySampler { get; set; }


        public int vertex_buffer_address;
        public int color_buffer_address;
        public int normal_buffer_address;
        public int texcoord_buffer_address;
        public int sky_texcoord_buffer_address;

        /// <summary>
        /// Основная программа
        /// </summary>
        public int ProgramId { get; set; }

        /// <summary>
        /// облегченные шейдеры
        /// </summary>
        public int ProgramIdForLight { get; set; }

        /// <summary>
        /// Скайбокс
        /// </summary>
        public int ProgramIdForSky { get; set; }

        private bool firstDraw = true;

        public ShaderManager(RenderEngine renderEngine)
        {
            this.renderEngine = renderEngine;

            CreateMainProgram();
            CreatePointProgram();
            CreateSkyboxProgram();



            GL.GenBuffers(1, out vertex_buffer_address);
            GL.GenBuffers(1, out color_buffer_address);
            GL.GenBuffers(1, out normal_buffer_address);

            GL.GenBuffers(1, out texcoord_buffer_address);

            GL.GenBuffers(1, out sky_texcoord_buffer_address);


        }

        private void CreatePointProgram()
        {
            ProgramIdForLight = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            // Define a simple shader program for our point.
            String pointVertexShader = null;

            using (StreamReader rd = new StreamReader(@"Assets\Shaders\lightVertex.glsl"))
            {
                pointVertexShader = rd.ReadToEnd();
            }


            GL.ShaderSource(vertexShader, pointVertexShader);
            GL.CompileShader(vertexShader);
            GL.AttachShader(ProgramIdForLight, vertexShader);

            String pointFragmentShader = null;
            using (StreamReader rd = new StreamReader(@"Assets\Shaders\lightFragment.glsl"))
            {
                pointFragmentShader = rd.ReadToEnd();
            }

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, pointFragmentShader);
            GL.CompileShader(fragmentShader);
            GL.AttachShader(ProgramIdForLight, fragmentShader);
            GL.LinkProgram(ProgramIdForLight);

        }



        private void CreateSkyboxProgram()
        {
            ProgramIdForSky = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            string pointVertexShader = null;

            using (StreamReader rd = new StreamReader(@"Assets\Shaders\skyboxVertex.glsl"))
            {
                pointVertexShader = rd.ReadToEnd();
            }


            GL.ShaderSource(vertexShader, pointVertexShader);
            GL.CompileShader(vertexShader);
            GL.AttachShader(ProgramIdForSky, vertexShader);

            String pointFragmentShader = null;
            using (StreamReader rd = new StreamReader(@"Assets\Shaders\skyboxFragment.glsl"))
            {
                pointFragmentShader = rd.ReadToEnd();
            }

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, pointFragmentShader);
            GL.CompileShader(fragmentShader);
            GL.AttachShader(ProgramIdForSky, fragmentShader);
            GL.LinkProgram(ProgramIdForSky);

            GL.UseProgram(ProgramIdForSky);

            AttribPositionSkybox = GL.GetAttribLocation(ProgramIdForSky, "position");
            UniformProjectionSkybox = GL.GetUniformLocation(ProgramIdForSky, "projection");
            UniformViewSkybox = GL.GetUniformLocation(ProgramIdForSky, "view");

            UniformSkySampler = GL.GetUniformLocation(ProgramIdForSky, "cubemap");
        }

        private void CreateMainProgram()
        {
            ProgramId = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            string vertexShaderText = string.Empty;
            using (StreamReader rd = new StreamReader(@"Assets\Shaders\vertex.glsl"))
            {
                vertexShaderText = rd.ReadToEnd();
            }
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

            var fragmentShaderText = string.Empty;

            using (StreamReader rd = new StreamReader(@"Assets\Shaders\fragment.glsl"))
            {
                fragmentShaderText = rd.ReadToEnd();
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

            AttributeTexcoord_Parameter_Address = GL.GetAttribLocation(ProgramId, "a_TexCoordinate");
            UniformTexture_Parameter_Address = GL.GetUniformLocation(ProgramId, "u_Texture");

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
    }
}
