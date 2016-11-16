using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace SimpleTerrain
{
    class TerrainEngine : AbstractEngine
    {
        public Player Player { get; private set; }
        public RenderEngine MainRender { get; private set; }

        private HeightMap Map { get; set; }

        public TerrainEngine(int Width, int Height)
            : base(Width, Height)
        {
            Player = new Player();
            KeyHandler.KeyPress += HandleKeyPress;
            MainRender = new RenderEngine(Width, Height, Player);

            Map = HeightMap.Create(256);

        }


        public override void Click(Vector2 point)
        {

        }

        public override void Tick(long timeSlice, Vector2 dxdy)
        {
            KeyHandler.CheckKeys();

            Player.Tick(timeSlice, dxdy);

            var model = GetSimpleModel();

            MainRender.Draw(model, Player.FlashlightPosition);

        }

        private void HandleKeyPress(Common.Input.InputSignal signal)
        {
            Player.OnSignal(signal);
        }

        private SimpleModel GetSimpleModel()
        {
            var model = Map.GetAsModel();

            return model;
        }
    }
}
