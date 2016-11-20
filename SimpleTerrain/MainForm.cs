using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
namespace SimpleTerrain
{
    class MainForm : GameWindow
    {
        public TerrainEngine Engine { get; set; }
        public Stopwatch Watch { get; private set; }
        public long Start { get; private set; }

        public MainForm() : base(1920, 900, GraphicsMode.Default, "terrain", GameWindowFlags.Default,
            DisplayDevice.Default, major: 4, minor: 0, flags: GraphicsContextFlags.ForwardCompatible)
        {
            CursorVisible = false;
            Location = new System.Drawing.Point()
            {
                X = 0,
                Y = 0
            };
            Watch = new Stopwatch();
            Engine = new TerrainEngine(Width, Height, Watch);
            ResetMouse();
            Watch.Start();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //base.OnRenderFrame(e);

            if (!Watch.IsRunning)
            {
                Watch.Start();
            }

            long end = Watch.ElapsedMilliseconds;
            Vector2 dxdy = GetChanges();
            Engine.Tick(end - Start, dxdy);
            ResetMouse();

            SwapBuffers();
            Start = end;

        }

        private Vector2 GetChanges()
        {
            var mouseState = System.Windows.Forms.Cursor.Position;

            var res = new Vector2()
            {
                X = (Location.X + Width / 2 - mouseState.X),
                Y = (Location.Y + Height / 2 - mouseState.Y)
            };

            return res;
        }

        private void ResetMouse()
        {
            OpenTK.Input.Mouse.SetPosition(Location.X + Width / 2, Location.Y + Height / 2);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            OpenTK.Graphics.OpenGL4.GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F4:
                    if (e.Alt)
                    {
                        Exit();
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
