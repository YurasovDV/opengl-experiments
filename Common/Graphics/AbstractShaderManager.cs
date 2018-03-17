using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Common.Graphics
{
    public abstract class AbstractShaderManager
    {

        /// <summary>
        /// main program
        /// </summary>
        public int ProgramId { get; set; }


        /// <summary>
        /// skybox
        /// </summary>
        public int ProgramIdForSky { get; set; }


        protected bool firstDraw = true;


        public bool IsFirstDrawCall
        {
            get
            {
                return firstDraw;
            }
        }

        public int vertex_buffer_address;
        public int color_buffer_address;
        public int normal_buffer_address;
        public int texcoord_buffer_address;
        public int sky_texcoord_buffer_address;


        public AbstractShaderManager()
        {

            GL.GenBuffers(1, out vertex_buffer_address);
            GL.GenBuffers(1, out color_buffer_address);
            GL.GenBuffers(1, out normal_buffer_address);

            GL.GenBuffers(1, out texcoord_buffer_address);

            GL.GenBuffers(1, out sky_texcoord_buffer_address);
        }

        protected void CreateMainProgram(string vertexPath, string fragmentPath)
        {
            ProgramId = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            string vertexShaderText = string.Empty;
            using (StreamReader rd = new StreamReader(vertexPath))
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

            using (StreamReader rd = new StreamReader(fragmentPath))
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

        }

        public abstract void BindBuffers(SimpleModel model, Vector3 light, bool refreshVertices = false, bool refreshColors = false);

        public virtual void BindBuffers(SimpleModel model, Vector3 light, RefreshKind refresh)
        { 
            var v = (refresh & RefreshKind.Vertices) != RefreshKind.None;
            var c = (refresh & RefreshKind.Color) != RefreshKind.None;
            BindBuffers(model, light, v, c);
        }
    }
}
