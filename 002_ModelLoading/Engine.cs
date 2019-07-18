using System.Collections.Generic;

using Common;
using Common.Input;
using OpenTK;

namespace ModelLoading
{
    class Engine
    {

        public KeyHandler KeyHandler { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public List<SimpleModel> Models { get; set; }

        public RenderEngine renderEngine { get; set; }

        public Player Player { get; set; }


        public Engine(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;

            Player = new Player();

            renderEngine = new RenderEngine(Width, Height, Player);
            KeyHandler = new KeyHandler();
            KeyHandler.KeyPress += Player.OnSignal;



            var tower = new SimpleModel(@"Assets\Tower\watchtower.obj", @"Assets\Tower\Wachturm_tex_x.png");

            var verticesTransformed = tower.Vertices;
            var scale = Matrix4.CreateScale(5);

            for (int i = 0; i < verticesTransformed.Length; i++)
            {
                verticesTransformed[i] = Vector3.Transform(verticesTransformed[i], scale);
            }
            tower.Vertices = verticesTransformed;

            Models = new List<SimpleModel>() { tower };
        }

        internal void Click()
        {
        }

        internal void Tick(long delta, OpenTK.Vector2 mouseDxDy)
        {
            KeyHandler.CheckKeys();

            Player.Tick(delta, mouseDxDy);

            renderEngine.Render(Models); 
        }
    }
}
