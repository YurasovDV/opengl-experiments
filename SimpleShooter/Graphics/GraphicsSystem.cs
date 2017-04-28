using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SimpleShooter.Graphics;
using SimpleShooter.LevelLoaders;
using SimpleShooter.Player;
using ClearBufferMask = OpenTK.Graphics.OpenGL4.ClearBufferMask;
using GL = OpenTK.Graphics.OpenGL4.GL;

namespace SimpleShooter.Graphics
{
    internal class GraphicsSystem
    {
        public Camera Camera { get; set; }

        public Vector3 LightPosition { get; set; }

        public GraphicsSystem(int width, int height, IObjectInitializer initializer)
        {
            InitGraphics(width, height, initializer);
        }

        private void InitGraphics(int width, int height, IObjectInitializer initializer)
        {
            float aspect = (float) width / height;
            GL.Viewport(0, 0, width, height);
            var projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 200);
            Camera = new Camera(projection);
        }

        internal void Render(IEnumerable<IRenderWrapper> objects, PlayerModel player)
        {
            Camera.RebuildMatrices(player);

            GL.ClearColor(0, 0, 0.0f, 1);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DepthTest);
           
            foreach (var model in objects)
            {
                Render(model);
            }

            GL.Flush();
        }


        internal void Render(IRenderWrapper obj)
        {
            obj.Bind(Camera, LightPosition);
            GL.DrawArrays(obj.RenderType, 0, obj.VerticesCount);
        }
    }
}