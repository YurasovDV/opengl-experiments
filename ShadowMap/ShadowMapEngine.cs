using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using Common.Input;
using Common.Utils;
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
        private LightSphere bulb;
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
            MainRender = new RenderEngine(Width, Height, Player, 300);
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

            MainRender.Draw(AllObjects[0], light, bulb.Target, lightMVP, FrameBuf.SecondDepthMapBufferTextureId);

            MainRender.PostRender();
        }

        private void DrawUsingShadowMap(Vector3 light, Matrix4 lightMVP, SimpleModel model)
        {
            MainRender.PreRender();

            MainRender.Draw(model, light, bulb.Target, lightMVP, FrameBuf.SecondDepthMapBufferTextureId);
            foreach (var someobj in AllObjects)
            {
                MainRender.Draw(someobj, light, bulb.Target, lightMVP, FrameBuf.SecondDepthMapBufferTextureId);
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
            bulb = GetSphere();
            bulb.Target = new Vector3(80, 3, 80);

            GameObject towObj = GetTower();

            AllObjects = new List<GameObject>()
            {
                  towObj,
                  bulb,

            };
        }

        private LightSphere GetSphere()
        {
            var sphere = new SimpleModel(@"Assets\Models\sphere.obj", null);

            var verticesTransformed = sphere.Vertices;

            var move = Matrix4.CreateTranslation(65, 10, 65);

            var scale = Matrix4.CreateScale(1.5f);

            for (int i = 0; i < verticesTransformed.Length; i++)
            {
                verticesTransformed[i] = Vector3.Transform(verticesTransformed[i], scale * move);
            }


            var result = new LightSphere(new Vector3(65, 10, 65))
            {
                Colors = Enumerable.Repeat(new Vector3(1, 1, 1), verticesTransformed.Length).ToArray(),
                Normals = sphere.Normals,
                TextureId = sphere.TextureId,
                TextureCoordinates = sphere.TextureCoordinates,
                Vertices = verticesTransformed
            };

            return result;
        }

        private GameObject GetTower()
        {
            var tower = new SimpleModel(@"Assets\Models\Tower\watchtower.obj", @"Assets\Models\Tower\Wachturm_tex_x.png");

            var verticesTransformed = tower.Vertices;

            float h = GetMinHeightForTower();

            var move = Matrix4.CreateTranslation(80, h, 80);
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

        private float GetMinHeightForTower()
        {
            float h = 10;


            float h1;
            if (!Map.TryGetValue(58, 58, out h1))
            {
                h1 = 0;
            }

            float h2;
            if (!Map.TryGetValue(58, 61, out h2))
            {
                h2 = 0;
            }

            float h3;
            if (!Map.TryGetValue(61, 58, out h3))
            {
                h3 = 0;
            }

            float h4;
            if (!Map.TryGetValue(61, 61, out h4))
            {
                h4 = 0;
            }

            h = (float)MathHelperMINE.Max(-h1, -h2, -h3, -h4) * -1;

            return h;
        }
    }
}
