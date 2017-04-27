using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using gl4 = OpenTK.Graphics.OpenGL4;

namespace SimpleShooter
{
    class MainWindow : GameWindow
    {

        private readonly Engine _engine;
        private GraphicsSystem _graphics;
        private Stopwatch _watch;
        private long _start = 0;


        public MainWindow() : base(1920, 1000, GraphicsMode.Default, "Simple Shooter", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {
            var initializer = new ObjectInitializer();
            _graphics = new GraphicsSystem(Width, Height, initializer);
            _engine = new Engine(_graphics, initializer);
            _watch = new Stopwatch();
            CursorVisible = false;

        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            gl4.GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case OpenTK.Input.Key.F4:
                    if (e.Alt)
                    {
                         Close();
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if (!_watch.IsRunning)
            {
                _watch.Start();
            }
            long end = _watch.ElapsedMilliseconds;
            Vector2 dxdy = GetChanges();
            _engine.Tick(end - _start, dxdy);
            ResetMouse();

            SwapBuffers();
            _start = end;
        }

        private Vector2 GetChanges()
        {
            var mouseState = OpenTK.Input.Mouse.GetCursorState();

            Vector2 res = new Vector2()
            {
                X = ((this.Location.X + Width / 2) - mouseState.X) * 1,
                Y = ((this.Location.Y + Height / 2) - mouseState.Y) * 1
            };

            return res;
        }

        private void ResetMouse()
        {
            OpenTK.Input.Mouse.SetPosition(this.Location.X + Width / 2, this.Location.Y + Height / 2);
        }
    }
}
