using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace FabricSimulation
{
    public class FabricForm : GameWindow
    {
        public FabricSimulationEngine Engine { get; set; }
        public long Start { get; set; }
        public Stopwatch Watch { get; set; }


        public FabricForm()
            : base(1920, 900, GraphicsMode.Default, "fabric", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Location = new System.Drawing.Point(0, 0);
            OpenTK.Input.Mouse.SetPosition(this.Location.X + Width / 2, this.Location.Y + Height / 2);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Engine = new FabricSimulationEngine(Width, Height);
            Start = 0;
            Watch = new Stopwatch();
            Watch.Start();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            var time = Watch.ElapsedMilliseconds;
            Engine.Tick(time - Start);
            SwapBuffers();
            Start = time;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            OpenTK.Graphics.OpenGL4.GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Engine.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Engine.OnMouseUp(e);
        }
    }
}
