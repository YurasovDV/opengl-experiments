using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OcTreeRevisited
{
    class LineRender : AbstractShaderManager
    {

        public int Uniform_ModelViewProjection { get; set; }
        public int AttrPosition { get; set; }
        public int AttrColor { get; set; }

        public RenderEngine RenderMain { get; set; }

        public LineRender(int Width, int Height, AbstractPlayer Player, RenderEngine MainRender) : base()
        {
            this.RenderMain = MainRender;
            InitShaders();
        }

        private void InitShaders()
        {
            CreateMainProgram(@"Assets\Shaders\vertLine.glsl", @"Assets\Shaders\fragLine.glsl");

            AttrPosition = GL.GetAttribLocation(ProgramId, "vPosition");
            AttrColor = GL.GetAttribLocation(ProgramId, "vColor");
            Uniform_ModelViewProjection = GL.GetUniformLocation(ProgramId, "u_modelViewprojection");


        }

        public void Render(SimpleModel model)
        {
            

            BindBuffers(model, Vector3.One);

            GL.EnableVertexAttribArray(AttrPosition);
            GL.EnableVertexAttribArray(AttrColor);

            GL.DrawArrays(PrimitiveType.Lines, 0, model.Vertices.Length);

            GL.DisableVertexAttribArray(AttrPosition);
            GL.DisableVertexAttribArray(AttrColor);
        }



        public override void BindBuffers(SimpleModel model, Vector3 light, bool refreshVertices = false, bool refreshColors = false)
        {
            GL.UseProgram(ProgramId);

            RenderMain.SetupVieport();

            var m = new Matrix4(RenderMain.ModelViewProjection.Row0,
                RenderMain.ModelViewProjection.Row1,
                RenderMain.ModelViewProjection.Row2,
                RenderMain.ModelViewProjection.Row3);

            GL.UniformMatrix4(Uniform_ModelViewProjection, false, ref m);


            GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_address);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(model.Vertices.Length * Vector3.SizeInBytes),
                model.Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrPosition, 3, VertexAttribPointerType.Float, false, 0, 0);



            GL.BindBuffer(BufferTarget.ArrayBuffer, color_buffer_address);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(model.Colors.Length * Vector3.SizeInBytes),
                model.Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrColor, 3, VertexAttribPointerType.Float, false, 0, 0);

        }
    }
}
