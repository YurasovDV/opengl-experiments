using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using Common.Input;
using Common.Utils;
using Glass.Graphics;
using OpenTK;

namespace Glass
{
    class SimulationEngine
    {
        private Stopwatch Watch { get; set; }

        private KeyHandler _keyHandler;
        private Player _player;
        private GraphicsSystem _graphics;

        private List<SimpleModel> _models;
        private List<SimpleModel> _reflectiveModels;

        public SimulationEngine(int width, int height, Stopwatch watch)
        {
            Watch = watch;
            _player = new Player();

            _player.Position = new Vector3(0, 0.5f, 0);
            _player.Target = new Vector3(100, 0.5f, 0);
            var mirrorCenter = new Vector3(0, 10, 0);
            _graphics = new GraphicsSystem(width, height, _player, mirrorCenter);

            _keyHandler = new KeyHandler();
            _keyHandler.KeyPress += OnKeyPress;

            InitObjects();

        }

        private void InitObjects()
        {
            int waferSize = 100;
            float axisWidth = 0.15f;

            var verticesPlane = new[]
            {
                new Vector3(waferSize, 0, -waferSize),
                new Vector3(-waferSize, 0, waferSize),
                new Vector3(-waferSize, 0, -waferSize),

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

            const float ozLift = 0.25f;

            var verticesOZ = new[]
            {
                new Vector3(axisWidth, ozLift, -waferSize),
                new Vector3(-axisWidth, ozLift, waferSize),
                new Vector3(-axisWidth, ozLift, -waferSize),

                new Vector3(axisWidth, ozLift, -waferSize),
                new Vector3(axisWidth, ozLift, waferSize),
                new Vector3(-axisWidth, ozLift, waferSize),
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
            verticesCombined.AddRange(verticesOx);
            verticesCombined.AddRange(verticesOZ);

            var wafer = new SimpleModel()
            {
                Vertices = verticesCombined.ToArray(),
                Colors = colorsCombined,
                Normals = Enumerable.Repeat(Vector3.UnitY, verticesCombined.Count).ToArray()
            };

            _models = new List<SimpleModel>() { wafer };

            //var sphere = new SimpleModel(@"Assets\simpleSphere.obj", null);
            //sphere.Vertices.TranslateAll(new Vector3(0, 2, 0));
            //_reflectiveModels = new List<SimpleModel>() { sphere };

            var cube = new SimpleModel();
            cube.Vertices = GeometryHelper.GetVerticesForOrdinaryCube(1);
            cube.Vertices.TranslateAll(new Vector3(0, 10, 0));
            // cube.Colors = Enumerable.Repeat(Vector3.UnitX, cube.Vertices.Length).ToArray();
            cube.Colors =
               new[]
               {
                    Enumerable.Repeat(Vector3.UnitX, 6),
                    Enumerable.Repeat(Vector3.UnitX* 0.5f, 6),
                    Enumerable.Repeat(Vector3.UnitY, 6),
                    Enumerable.Repeat(Vector3.UnitY* 0.5f, 6),
                    Enumerable.Repeat(Vector3.UnitZ, 6),
                    Enumerable.Repeat(Vector3.UnitZ * 0.5f, 6)
               }
               .SelectMany(a => a.ToArray())
               .ToArray();
            cube.Normals =
                new[]
               {
                    Enumerable.Repeat(Vector3.UnitX, 6),
                    Enumerable.Repeat(-Vector3.UnitX, 6),
                    Enumerable.Repeat(Vector3.UnitY, 6),
                    Enumerable.Repeat(-Vector3.UnitY, 6),
                    Enumerable.Repeat(Vector3.UnitZ, 6),
                    Enumerable.Repeat(-Vector3.UnitZ, 6)
               }
               .SelectMany(a => a.ToArray())
               .ToArray();

            _reflectiveModels = new List<SimpleModel>() { cube };
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
            _graphics.Render(_models, _reflectiveModels);
        }
    }
}
