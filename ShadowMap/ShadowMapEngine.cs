using System.Collections.Generic;
using System.Diagnostics;
using Common;
using Common.Input;
using OpenTK;

namespace ShadowMap
{
    public partial class ShadowMapEngine : AbstractEngine
    {
        private long modeChange;
        private long previousChange;

        public Player Player { get; private set; }
        public RenderEngine MainRender { get; private set; }

        public Stopwatch Watch { get; set; }

        private HeightMap Map { get; set; }

        private List<GameObject> AllObjects { get; set; }

        public ShadowMapEngine(int Width, int Height, Stopwatch watch)
            : base(Width, Height)
        {
            Map = HeightMap.Create(256);
            Watch = watch;

            var red = new Vector3(10, 0, 0);
            var center = new Vector3(50, 0, 50);

            AllObjects = new List<GameObject>()
            {
                new Cube(center, red)
            };

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

            RenderToDefaulTarget();
        }

        private void RenderToDefaulTarget()
        {
            MainRender.EnableFrameBuffer();

            MainRender.PreRender();
            var model = GetMapAsModel();
            MainRender.Draw(model, Player.FlashlightPosition);
            foreach (var someobj in AllObjects)
            {
                MainRender.Draw(someobj, Player.FlashlightPosition);
            }
            MainRender.PostRender();

            MainRender.FlushFrameBuffer();
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


        private SimpleModel GetMapAsModel()
        {
            var model = Map.GetAsModel();
            return model;
        }
    }
}
