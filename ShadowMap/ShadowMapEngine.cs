using System;
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
        public FrameBufferManager FrameBuf { get; set; }
        public Stopwatch Watch { get; set; }


        public Matrix4 ModelViewStored { get; set; }
        public Matrix4 ProjectionStored { get; set; }
        public Matrix4 ModelViewProjectionStored { get; set; }
        public Vector3 PlayerFlashlightPositionStored { get; set; }

        private Vector3 Light { get; set; } = new Vector3(50, 30, 50);

        private HeightMap Map { get; set; }

        private List<GameObject> AllObjects { get; set; }


        public ShadowMapEngine(int Width, int Height, Stopwatch watch)
            : base(Width, Height)
        {
            Map = HeightMap.Create(256);
            Watch = watch;

            AllObjects = new List<GameObject>()
            {
                new Cube(center:new Vector3(50, 3, 50) , color: Vector3.UnitX, scale:1)
            };

            Player = new Player(TryMove);
            KeyHandler.KeyPress += HandleKeyPress;
            MainRender = new RenderEngine(Width, Height, Player);
            FrameBuf = new FrameBufferManager(MainRender);
            previousChange = 0;
        }


        public override void Click(Vector2 point)
        {

        }

        public override void Tick(long timeSlice, Vector2 dxdy)
        {
            KeyHandler.CheckKeys();
            Player.Tick(timeSlice, dxdy);
            FullRender();
        }

        private void FullRender()
        {
            var lightMVP = MainRender.ModelViewProjection;
            var model = GetMapAsModel();

            PushModelViewAndProjection();
            MainRender.FormShadowMap = true;
            FrameBuf.EnableAuxillaryFrameBuffer();
            DrawToShadowMap(Light, model);
            FrameBuf.FlushAuxillaryFrameBuffer();
            PopModelViewAndProjection();
            MainRender.FormShadowMap = false;

            FrameBuf.EnableMainFrameBuffer();
            DrawUsingShadowMap(Light, lightMVP, model);
            FrameBuf.FlushMainFrameBuffer();
        }

        private void DrawToShadowMap(Vector3 light, SimpleModel model)
        {
            MainRender.PreRender();
            MainRender.Draw(model, light, null);
            foreach (var someobj in AllObjects)
            {
                MainRender.Draw(someobj, light, null);
            }
            MainRender.PostRender();
        }

        private void DrawUsingShadowMap(Vector3 light, Matrix4 lightMVP, SimpleModel model)
        {
            MainRender.PreRender();

            MainRender.Draw(model, light, lightMVP, FrameBuf.SecondDepthMapBufferTextureId);
            foreach (var someobj in AllObjects)
            {
                MainRender.Draw(someobj, light, lightMVP, FrameBuf.SecondDepthMapBufferTextureId);
            }
            MainRender.PostRender();
        }

        private void PushModelViewAndProjection()
        {
            ModelViewStored = MainRender.ModelView;
            ProjectionStored = MainRender.Projection;
            ModelViewProjectionStored = MainRender.ModelViewProjection;
            PlayerFlashlightPositionStored = Player.FlashlightPosition;
        }

        private void PopModelViewAndProjection()
        {
            MainRender.ModelView = ModelViewStored;
            MainRender.Projection = ProjectionStored;
            MainRender.ModelViewProjection = ModelViewProjectionStored;
            Player.FlashlightPosition = PlayerFlashlightPositionStored;
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
