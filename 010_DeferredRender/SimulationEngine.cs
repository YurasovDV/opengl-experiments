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
        public const int LightRadius = 30;
        public const int LightNumber = 100;

        public static readonly Vector3[] CubeVerts = GeometryHelper.GetVerticesForCube(0.01f);

        private Stopwatch Watch { get; set; }

        private KeyHandler _keyHandler;
        private Player _player;
        private GraphicsSystem _graphics;

        private List<SimpleModel> _models;
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

            var white = Enumerable.Repeat(Vector3.One, CubeVerts.Length).ToArray();
            var normals = CubeVerts.GetNormals();

            foreach (var light in _lights)
            {
                light.Tick(0);
                var lightModel = new SimpleModel();
                lightModel.Vertices = CubeVerts;

                lightModel.Colors = white;
                lightModel.Normals = normals;

                light.DenotationCube = lightModel;
            }
        }

       

        private void GenerateRandomLights()
        {
            _lights = new List<PointLight>();

            var rand = new Random();

            for (int i = 0; i < LightNumber; i++)
            {
                var pos = new Vector3(rand.Next(-75, 75), 3 + (float)rand.NextDouble() * 3, rand.Next(-75, 75));

                var light = new PointLight(0.15f * (50.0f / pos.Length))
                {
                    Radius = LightRadius
                };

                light.CurrentPosition = pos;

                var top = 250;
                var minLevel = 70;

                light.Colors = new[] { new Vector3(rand.Next(minLevel, top), rand.Next(minLevel, top), rand.Next(minLevel, top)) };

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

            ObjectsTick(timeSlice);

            FullRender();
        }

        private void ObjectsTick(long timeSlice)
        {
            foreach (var light in _lights)
            {
                light.Tick(timeSlice);
            }
        }

        private void FullRender()
        {
            _graphics.Render(_models, _lights);
        }
    }
}
