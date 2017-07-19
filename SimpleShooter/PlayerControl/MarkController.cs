using System;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.Graphics.ShaderLoad;

namespace SimpleShooter.PlayerControl
{
    class MarkController
    {
        private Vector3[] _markForm;

        private Vector3[] _markColors;

        private Matrix4 _identity = Matrix4.Identity;

        public MarkController(int width, int height)
        {
            var h = 0.16f;
            var w = 0.09f;
            _markForm = new[]
            {
                new Vector3(0, 0.5f * h, 0),
                new Vector3(0, 0, 0),

                new Vector3(0, 0, 0),
                new Vector3(0, -0.5f * h, 0),

                new Vector3(-0.5f * w, 0f,0 ),
                new Vector3(0, 0f,0 ),

                new Vector3(0, 0f,0 ),
                new Vector3(0.5f * w, 0f, 0),
            };

            var red = Vector3.UnitX;
            var blue = Vector3.UnitZ;

            _markColors = new[]
            {
                red,
                blue,

                blue,
                red,

                red,
                blue,

                blue,
                red,
            };
        }

        public void Render(IShooterPlayer player)
        {
            ShaderProgramDescriptor descriptor;
            ShaderLoader.TryGet(ShadersNeeded.Line, out descriptor);

            GL.UseProgram(descriptor.ProgramId);

            GL.UniformMatrix4(descriptor.uniformMVP, false, ref _identity);

            RenderWrapper.BindVertices(descriptor, _markForm);
            RenderWrapper.BindColors(descriptor, _markColors);

            GL.LineWidth(5);
            GL.DrawArrays(PrimitiveType.Lines, 0, _markForm.Length);
            GL.LineWidth(1);
        }
    }
}
