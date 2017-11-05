using System.Collections.Generic;

using Common.Input;

namespace Common
{
    public abstract class AbstractEngine
    {
        protected KeyHandler KeyHandler { get; set; }

        protected AbstractRenderEngine ParticleRenderer { get; set; }

        protected int Width { get; set; }
        protected int Height { get; set; }

        protected List<SimpleModel> Models { get; set; }

        public AbstractEngine(int width, int height)
        {
            Width = width;
            Height = height;
            KeyHandler = new KeyHandler();
            Models = new List<SimpleModel>();
        }

        public abstract void Click(OpenTK.Vector2 point);

        public abstract void Tick(long timeSlice, OpenTK.Vector2 dxdy);
    }
}
