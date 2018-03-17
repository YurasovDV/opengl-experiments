using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using Common.Graphics;
using Common.Input;
using Common.Utils;
using DeferredRender.Graphics;
using OpenTK;

namespace DeferredRender
{
    class SimulationEngine
    {
        public const int LightRadius = 18;
        public const int LightNumber = 100;

        private Stopwatch Watch { get; set; }

        private KeyHandler _keyHandler;
        private Player _player;
        private GraphicsSystem _graphics;

        private List<SimpleModel> _models;
        private List<SimpleModel> _lightModels;
        private List<PointLight> _lights;

        public SimulationEngine(int width, int height, Stopwatch watch)
        {
            Watch = watch;
            _player = new Player();

            _player.Position = new Vector3(0, 0.5f, 0);
            _player.Target = _player.Position + _player.DefaultTarget;

            _graphics = new GraphicsSystem(width, height, _player);

            _keyHandler = new KeyHandler();
            _keyHandler.KeyPress += OnKeyPress;
            _models = new List<SimpleModel>();
            _lightModels = new List<SimpleModel>();
            InitObjects();

        }

        private void InitObjects()
        {
            int waferSize = 100;
            float axisWidth = 0.15f;

            var verticesPlane = new[]
            {
                new Vector3(waferSize, 0, -waferSize),
                new Vector3(-waferSize, 0, -waferSize),
                new Vector3(-waferSize, 0, waferSize),

                new Vector3(waferSize, 0, -waferSize),
                new Vector3(waferSize, 0, waferSize),
                new Vector3(-waferSize, 0, waferSize),
            };

            var verticesOx = new[]
            {
                new Vector3(waferSize, 0.2f, -axisWidth),
                new Vector3(-waferSize, 0.2f, axisWidth),
                new Vector3(-waferSize, 0.2f, -axisWidth),

                new Vector3(waferSize, 0.2f, -axisWidth),
                new Vector3(waferSize, 0.2f, axisWidth),
                new Vector3(-waferSize, 0.2f, axisWidth),
            };

            var verticesOZ = new[]
            {
                new Vector3(axisWidth, 0.2f, -waferSize),
                new Vector3(-axisWidth, 0.2f, waferSize),
                new Vector3(-axisWidth, 0.2f, -waferSize),

                new Vector3(axisWidth, 0.2f, -waferSize),
                new Vector3(axisWidth, 0.2f, waferSize),
                new Vector3(-axisWidth, 0.2f, waferSize),
            };

            var colorsCombined = new[]
            {
                 new Vector3(0, 0, 0.4f),
                 new Vector3(0, 0, 0.4f),
                 new Vector3(0, 0, 0.4f),

                 new Vector3(0, 0, 0.4f),
                 new Vector3(0, 0, 0.4f),
                 new Vector3(0, 0, 0.4f),

                 // 0x
                 new Vector3(0, 0.7f, 0.0f),
                 new Vector3(0, 0.1f, 0.0f),
                 new Vector3(0, 0.1f, 0.0f),

                 new Vector3(0, 0.7f, 0.0f),
                 new Vector3(0, 0.7f, 0.0f),
                 new Vector3(0, 0.1f, 0.0f), 
                 
                 // 0z
                 new Vector3(0.7f, 0.0f, 0.0f),
                 new Vector3(0.1f, 0.0f, 0.0f),
                 new Vector3(0.7f, 0.0f, 0.0f),

                 new Vector3(0.7f, 0.0f, 0.0f),
                 new Vector3(0.1f, 0.0f, 0.0f),
                 new Vector3(0.1f, 0.0f, 0.0f),
            };


            var verticesCombined = new List<Vector3>();
            verticesCombined.AddRange(verticesPlane);
            //verticesCombined.AddRange(verticesOx);
            //verticesCombined.AddRange(verticesOZ);

            var textManager = new TextureManager();
            int textureId = textManager.LoadTexture(@"Assets\Textures\Chess.png");

            var chessPlane = new SimpleModel()
            {
                Vertices = verticesCombined.ToArray(),
                Colors = colorsCombined,
                Normals = Enumerable.Repeat(Vector3.UnitY, verticesCombined.Count).ToArray(),
                TextureId = textureId,
            };

            chessPlane.TextureCoordinates = textManager.GetTextureCoordinates(chessPlane.Vertices);
            if (_models == null)
            {
                _models = new List<SimpleModel>();
            }
            _models.Add(chessPlane);

            SimpleModel lightVolume = new SimpleModel(@"Assets\simpleSphere.obj", null);
            _graphics.LightVolume = lightVolume;

            GenerateRandomLights();

            var cubeVerts = GeometryHelper.GetVerticesForCube(0.01f);
            var white = Enumerable.Repeat(Vector3.One, cubeVerts.Length).ToArray();
            var normals = cubeVerts.GetNormals();

            foreach (var light in _lights)
            {
                var lightModel = new SimpleModel();
                lightModel.Vertices = cubeVerts.Select(v => Vector3.Transform(v, Matrix4.CreateTranslation(0, -0.05f, 0) * light.Transform)).ToArray();

                lightModel.Colors = white;
                lightModel.Normals = normals;//  GetNormals(lightModel.Vertices);
                _lightModels.Add(lightModel);
            }

        }

        private void GenerateLights()
        {
            _lights = new List<PointLight>();

            var rand = new Random();

            for (int i = 0; i < LightNumber; i++)
            {
                var light = new PointLight();

                light.Center = new Vector3(0, 1f, 0);

                light.Radius = LightRadius;

                var translate = Matrix4.CreateTranslation(light.Center);

                var scale = Matrix4.CreateScale(light.Radius);

                light.Transform = scale * translate;
                light.Colors = new[] { new Vector3(0, 100, 0) };

                _lights.Add(light);
            }
        }

        private void GenerateRandomLights()
        {
            _lights = new List<PointLight>();

            var rand = new Random();

            for (int i = 0; i < LightNumber; i++)
            {
                var light = new PointLight();

                light.Center = new Vector3(rand.Next(200) - 100, 3, rand.Next(200) - 100);

                light.Radius = LightRadius;

                var translate = Matrix4.CreateTranslation(light.Center);

                var scale = Matrix4.CreateScale(light.Radius);

               // light.Vertices = _graphics.LightVolume.Vertices;//.Select(v => Vector3.Transform(v, scale * translate)).ToArray();
                light.Transform = scale * translate;

                light.Colors = new[] { new Vector3(rand.Next(250), rand.Next(250), rand.Next(250)) };

                _lights.Add(light);
            }
        }

        private void OnKeyPress(InputSignal signal)
        {
            _player.Handle(signal);
        }

        internal void Tick(long timeSlice, Vector2 dxdy)
        {
            _keyHandler.CheckKeys();
            _player.HandleMouseMove(dxdy);
            FullRender();
        }

        private void FullRender()
        {
            _graphics.Render(_models, _lightModels, _lights);
        }
    }
}
