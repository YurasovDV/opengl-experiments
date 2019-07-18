using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using gl4 = OpenTK.Graphics.OpenGL4;

namespace ModelLoading
{
    class ModelForm : GameWindow
    {

        private Engine engine;
        private Stopwatch watch;
        private long start = 0;


        public ModelForm()
            : base(1920, 1080, GraphicsMode.Default, "Model Loading", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {
            engine = new Engine(Width, Height);
            watch = new Stopwatch();
            CursorVisible = false;
            Location = new System.Drawing.Point()
            {
                X = 0,
                Y = 0,
            };
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
                        Exit();
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnMouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            engine.Click();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if (!watch.IsRunning)
            {
                watch.Start();
            }
            long end = watch.ElapsedMilliseconds;
            Vector2 dxdy = GetChanges();
            engine.Tick(end - start, dxdy);
            ResetMouse();

            SwapBuffers();
            start = end;
        }

        private Vector2 GetChanges()
        {

            var mouseState = System.Windows.Forms.Cursor.Position;

            Vector2 res = new Vector2()
            {
                X = (this.Location.X + Width / 2) - mouseState.X,
                Y = (this.Location.Y + Height / 2) - mouseState.Y
            };

            return res;
        }

        private void ResetMouse()
        {
            OpenTK.Input.Mouse.SetPosition(this.Location.X + Width / 2, this.Location.Y + Height / 2);
        }
    }
}
