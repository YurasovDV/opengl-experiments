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

        private long cameraChange;
        private long previousCameraChange;



        private Matrix4 lightMVP;
        private Matrix4 lightMV;
        private Matrix4 lightP;

        public Player Player { get; private set; }
        public RenderEngine MainRender { get; private set; }
        public FrameBufferManager FrameBuf { get; set; }
        public Stopwatch Watch { get; set; }


        public Matrix4 ModelViewStored { get; set; }
        public Matrix4 ProjectionStored { get; set; }
        public Matrix4 ModelViewProjectionStored { get; set; }

        private Vector3 Light { get; set; }
        private Vector3 LightTarget { get; set; }

        private HeightMap Map { get; set; }

        private List<GameObject> AllObjects { get; set; }

        /// <summary>
        /// switch to light camera
        /// </summary>
        public bool CameraChange { get; private set; }

        public ShadowMapEngine(int Width, int Height, Stopwatch watch)
            : base(Width, Height)
        {
            Map = HeightMap.Create(256);
            Watch = watch;

            InitObjects();

            Player = new Player(TryMove);
            KeyHandler.KeyPress += HandleKeyPress;
            MainRender = new RenderEngine(Width, Height, Player);
            FrameBuf = new FrameBufferManager(MainRender);

            MainRender.LightPos = Light;
            MainRender.LightTarget = LightTarget;

            previousChange = 0;
            previousCameraChange = 0;
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
            var model = GetMapAsModel();

            PushModelViewAndProjection();
            MainRender.FormShadowMap = true;
            FrameBuf.EnableAuxillaryFrameBuffer();
            DrawToShadowMap(Light, model);

            lightMVP = MainRender.ModelViewProjection;
            lightMV = MainRender.ModelView;
            lightP = MainRender.Projection;

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

            MainRender.Draw(AllObjects[0], light, lightMVP, FrameBuf.SecondDepthMapBufferTextureId);

            /*foreach (var someobj in AllObjects)
            {
                MainRender.Draw(someobj, light, null);
            }*/
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
        }

        private void PopModelViewAndProjection()
        {
            MainRender.ModelView = ModelViewStored;
            MainRender.Projection = ProjectionStored;
            MainRender.ModelViewProjection = ModelViewProjectionStored;
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

            if (signal == InputSignal.CHANGE_CAMERA)
            {
                cameraChange = Watch.ElapsedMilliseconds;
                if (cameraChange - previousCameraChange > Constants.RenderChangeCooldown)
                {
                    CameraChange = !CameraChange;
                    previousCameraChange = cameraChange;

                    FrameBuf.DebugDepth = CameraChange;
                }
            }

        }

        private SimpleModel GetMapAsModel()
        {
            var model = Map.GetAsModel();
            return model;
        }

        private void InitObjects()
        {
            Light = new Vector3(50, 10, 50);
            LightTarget = new Vector3(60, 3, 60);

            var obj = new Cube(center: new Vector3(60, 3, 60), color: Vector3.UnitX , scale: 1);

            //var Oxy = new Cube(center: new Vector3(0, 1, 0), color: Vector3.UnitZ, scale: 1);
            //var bulb = new Cube(center: new Vector3(Light), color: new Vector3(1, 1, 1), scale: 0.1f);
            //bulb.InvertNormals();
            //var targ = new Cube(center: new Vector3(LightTarget), color: new Vector3(0.85f, 0.3f, 0), scale: 0.1f);
            //var targ2 = new Cube(center: new Vector3(64.3f, 0, 64.3f), color: new Vector3(0.85f, 0.3f, 0), scale: 1f);

            AllObjects = new List<GameObject>()
            {
                  obj,
            };
        }

    }
}
