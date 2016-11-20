using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Input;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace SimpleTerrain
{
    class TerrainEngine : AbstractEngine
    {
        private long modeChange;
        private long previousChange;

        public Player Player { get; private set; }
        public RenderEngine MainRender { get; private set; }
        public Stopwatch Watch { get; set; }

        private HeightMap Map { get; set; }

        public TerrainEngine(int Width, int Height, Stopwatch watch)
            : base(Width, Height)
        {
            Map = HeightMap.Create(256);
            Watch = watch;

            Player = new Player(TryMove);
            KeyHandler.KeyPress += HandleKeyPress;
            MainRender = new RenderEngine(Width, Height, Player);

            previousChange = 0;
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

        private void HandleKeyPress(InputSignal signal)
        {
            Player.OnSignal(signal);
            if (signal == InputSignal.RENDER_MODE)
            {
                modeChange = Watch.ElapsedMilliseconds;
                if (modeChange - previousChange > Constants.RenderChangeCooldown)
                {
                    MainRender.ChangeRenderMode();
                    Map.RebuildVerticesAccordingly(MainRender.RenderMode);
                    previousChange = modeChange;
                }
            }
        }
        

        private SimpleModel GetSimpleModel()
        {
            var model = Map.GetAsModel();
            return model;
        }


        protected Vector3 TryMove(Vector3 desiredPosition)
        {
            var oldPosition = Player.Position;

            var updated = new Vector3(desiredPosition);

            var closestX = (int)Math.Round(desiredPosition.X);
            var closestZ = (int)Math.Round(desiredPosition.Z);

            var previousClosestX = (int)Math.Round(oldPosition.X);
            var previousClosestZ = (int)Math.Round(oldPosition.X);

            float height;
            bool inside = Map.TryGetValue(closestX, closestZ, out height);

            float previousHeight;
            bool prevInside = Map.TryGetValue(previousClosestX, previousClosestZ, out previousHeight);

            if (inside && prevInside)
            {
                var y = 3.0f;
                y += height * 0.75f + previousHeight * 0.25f;
                updated.Y = y;
            }
            else
            {
                updated = oldPosition;
            }

            return updated;
        }
    }
}
