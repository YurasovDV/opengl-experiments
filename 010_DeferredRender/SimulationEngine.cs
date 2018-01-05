using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Common;
using Common.Graphics;
using Common.Input;
using DeferredRender.Graphics;
using OpenTK;

namespace DeferredRender
{
    class SimulationEngine
    {
        private Stopwatch Watch { get; set; }

        private KeyHandler _keyHandler;
        private Player _player;
        private GraphicsSystem _graphics;

        private List<SimpleModel> _models;
        private List<SimpleModel> _lights;

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

            var model = new SimpleModel()
            {
                Vertices = verticesCombined.ToArray(),
                Colors = colorsCombined,
                Normals = Enumerable.Repeat(Vector3.UnitY, verticesCombined.Count).ToArray(),
                TextureId = textureId,
            };

            model.TextureCoordinates = textManager.GetTextureCoordinates(model.Vertices);
            if (_models == null)
            {
                _models = new List<SimpleModel>();
            }
            _models.Add(model);

            SimpleModel tree = new SimpleModel(@"Assets\tree.obj", null);
            Vector3 green = Vector3.UnitY;
            var colors = Enumerable.Repeat<Vector3>(green, tree.Vertices.Length).ToArray();
            tree.Colors = colors;
            Matrix4 move = Matrix4.CreateScale(4) * Matrix4.CreateTranslation(15, 0, 0);
            for (int i = 0; i < tree.Vertices.Length; i++)
            {
                var v = tree.Vertices[i];
                tree.Vertices[i] = Vector3.Transform(v, move);
            }

             //_models.Add(tree);

            GenerateRandomLights();
        }

        private void GenerateLights()
        {
            _lights = new List<SimpleModel>();

            var rand = new Random();

            for (int i = 0; i < 10; i++)
            {
                var light = new SimpleModel();

                light.Vertices = new[] { new Vector3(0, 1f, 0) };

                light.Colors = new[] { new Vector3(0, 100, 0) };

                // radius
                light.Normals = new[] { new Vector3(50, 0, 0) };

                _lights.Add(light);
            }
        }

        private void GenerateRandomLights()
        {
            _lights = new List<SimpleModel>();

            var rand = new Random();

            for (int i = 0; i < 10; i++)
            {
                var light = new SimpleModel();

                light.Vertices = new[] { new Vector3(rand.Next(200) - 100, 10, rand.Next(200) - 100) };

                light.Colors = new[] { new Vector3(rand.Next(250), rand.Next(250), rand.Next(250)) };

                // radius
                light.Normals = new[] { new Vector3(500, 0, 0) };

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
            _graphics.Render(_models, _lights);
        }
    }
}
