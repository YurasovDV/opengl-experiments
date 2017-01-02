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
        private LightCube bulb;
        public RenderEngine MainRender { get; private set; }
        public FrameBufferManager FrameBuf { get; set; }
        public Stopwatch Watch { get; set; }


        public Matrix4 ModelViewStored { get; set; }
        public Matrix4 ProjectionStored { get; set; }
        public Matrix4 ModelViewProjectionStored { get; set; }

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
            bulb = bulb.Tick();
            AllObjects[1] = bulb;
            FullRender();
        }

        private void FullRender()
        {
            var model = GetMapAsModel();

            MainRender.LightPos = bulb.Center;
            MainRender.LightTarget = bulb.Target;

            PushModelViewAndProjection();
            MainRender.FormShadowMap = true;
            FrameBuf.EnableAuxillaryFrameBuffer();
            DrawToShadowMap(bulb.Center, model);

            lightMVP = MainRender.ModelViewProjection;
            lightMV = MainRender.ModelView;
            lightP = MainRender.Projection;

            FrameBuf.FlushAuxillaryFrameBuffer();
            PopModelViewAndProjection();
            MainRender.FormShadowMap = false;

            FrameBuf.EnableMainFrameBuffer();
            DrawUsingShadowMap(bulb.Center, lightMVP, model);
            FrameBuf.FlushMainFrameBuffer();
        }

        private void DrawToShadowMap(Vector3 light, SimpleModel model)
        {
            MainRender.PreRender();

            

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
            var obj = new Cube(center: new Vector3(60, 3, 60), color: Vector3.UnitX, scale: 2);

            bulb = new LightCube(center: new Vector3(50, 10, 50), color: new Vector3(1f, 1f, 1f), scale: 1f);
            bulb.InvertNormals();
            bulb.Target = new Vector3(60, 3, 60);

            GameObject towObj = GetTower();

            AllObjects = new List<GameObject>()
            {
                  //obj,
                  towObj,
                  bulb,
                 
            };
        }

        private static GameObject GetTower()
        {
            var tower = new SimpleModel(@"Assets\Tower\watchtower.obj", @"Assets\Tower\Wachturm_tex_x.png");

            var verticesTransformed = tower.Vertices;

            var move = Matrix4.CreateTranslation(60, -4, 60);

            var scale = Matrix4.CreateScale(5);

            for (int i = 0; i < verticesTransformed.Length; i++)
            {
                verticesTransformed[i] = Vector3.Transform(verticesTransformed[i], scale * move);
            }


            var towObj = new GameObject()
            {
                Colors = tower.Colors,
                Normals = tower.Normals,
                TextureId = tower.TextureId,
                TextureCoordinates = tower.TextureCoordinates,
                Vertices = verticesTransformed
            };

            return towObj;
        }
    }
}
