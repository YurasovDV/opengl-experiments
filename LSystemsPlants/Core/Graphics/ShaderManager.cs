using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace LSystemsPlants.Core.Graphics
{
    class ShaderManager
    {
        public int MainProgramId { get; set; }

        public int AttrVertexLocation { get; set; }

        public int AttrColorLocation { get; set; }

        public int AttrNormalLocation { get; set; }

        public int UMvpLocation { get; set; }

        public int UMvLocation { get; set; }

        public int UPLocation { get; set; }




        public ShaderManager()
        {
            MainProgramId = GL.CreateProgram();

            var vertex = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertex, File.ReadAllText(""));
            GL.CompileShader(vertex);
            GL.AttachShader(MainProgramId, vertex);



            GL.LinkProgram(MainProgramId);

            AttrColorLocation = GL.GetAttribLocation(MainProgramId, "");
            AttrVertexLocation = GL.GetAttribLocation(MainProgramId, "");
            AttrNormalLocation = GL.GetAttribLocation(MainProgramId, "");

            UMvpLocation = GL.GetUniformLocation(MainProgramId, "");

        }

    }
}
