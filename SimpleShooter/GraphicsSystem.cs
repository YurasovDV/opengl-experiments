using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using ClearBufferMask = OpenTK.Graphics.OpenGL4.ClearBufferMask;
using GL = OpenTK.Graphics.OpenGL4.GL;

namespace SimpleShooter
{
    internal class GraphicsSystem
    {

        public Camera Camera { get; set; }


        public GraphicsSystem(int width, int height, IObjectInitialiser initialiser)
        {
            InitGraphics(width, height, initialiser);
        }

        private void InitGraphics(int width, int height, IObjectInitialiser initialiser)
        {
            float aspect = (float) width / height;
            GL.Viewport(0, 0, width, height);
            var projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 200);
            Camera = initialiser.InitCamera(projection);
        }

        internal void Render(IEnumerable<GameObject> objects)
        {
            Camera.RebuildMatrices();

            GL.ClearColor(0, 0, 0.0f, 1);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DepthTest);
           
            foreach (var model in objects)
            {
                Render(model);
            }

            GL.Flush();
        }


        internal void Render(GameObject obj)
        {
            obj.Wrapper.Bind(Camera);
            GL.DrawArrays(PrimitiveType.Triangles, 0, obj.Model.Vertices.Length);
        }
    }
}