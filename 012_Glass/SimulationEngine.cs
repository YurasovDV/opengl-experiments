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
        private Vector3 _mirrorCenter;
        private GraphicsSystem _graphics;

        private List<SimpleModel> _models;
        private List<SimpleModel> _reflectiveModels;

        float angle1 = 0;
        float angle2 = 0;
        float angle3 = 0;

        Vector3 defaultVec1 = new Vector3(-30, 0, 0);
        Vector3 defaultVec2 = new Vector3(-30, 0, 30);
        Vector3 defaultVec3 = new Vector3(30, 0, 30);

        public SimulationEngine(int width, int height, Stopwatch watch)
        {
            Watch = watch;
            _player = new Player();

            _player.Position = new Vector3(0, 0.5f, 0);
            _player.Target = new Vector3(100, 0.5f, 0);
            _mirrorCenter = new Vector3(0, 0, 0);
            _graphics = new GraphicsSystem(width, height, _player, _mirrorCenter);

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


            var cubeNormals = new[]
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


            var nonReflectiveCube = new SimpleModel();
            var nonReflectiveCube2 = new SimpleModel();
            var nonReflectiveCube3 = new SimpleModel();

            nonReflectiveCube.Vertices = GeometryHelper.GetVerticesForOrdinaryCube(1);
            nonReflectiveCube2.Vertices = GeometryHelper.GetVerticesForOrdinaryCube(1);
            nonReflectiveCube3.Vertices = GeometryHelper.GetVerticesForOrdinaryCube(1);
            nonReflectiveCube.Vertices.TranslateAll(defaultVec1);
            nonReflectiveCube2.Vertices.TranslateAll(defaultVec2);
            nonReflectiveCube3.Vertices.TranslateAll(defaultVec3);

            nonReflectiveCube.Normals = cubeNormals;
            nonReflectiveCube2.Normals = cubeNormals;
            nonReflectiveCube3.Normals = cubeNormals;

            nonReflectiveCube.Colors =
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
            
            nonReflectiveCube2.Colors =
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
            
            nonReflectiveCube3.Colors =
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

            _models = new List<SimpleModel>() { nonReflectiveCube,/* nonReflectiveCube2, nonReflectiveCube3,  /*, wafer*/ };

            //var sphere = new SimpleModel(@"Assets\simpleSphere.obj", null);
            //sphere.Vertices.TranslateAll(new Vector3(0, 2, 0));
            //_reflectiveModels = new List<SimpleModel>() { sphere };

            var cube = new SimpleModel();
            cube.Vertices = GeometryHelper.GetVerticesForOrdinaryCube(5);
            cube.Vertices.TranslateAll(_mirrorCenter);
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
            cube.Normals = cubeNormals;

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
            UpdatePositions();
            FullRender();
        }

        private void UpdatePositions()
        {
            angle1 += MathHelper.TwoPi / 270.0f;
            //angle2 += MathHelper.TwoPi / 270.0f;
            //angle3 += MathHelper.TwoPi / 270.0f;

            _models[0].Vertices = GeometryHelper.GetVerticesForOrdinaryCube(1);
            var rot = Matrix4.CreateRotationY(angle1);
            _models[0].Vertices.TranslateAll(Vector3.TransformVector(defaultVec1, rot));            
            
            
            //_models[1].Vertices = GeometryHelper.GetVerticesForOrdinaryCube(1);
            //rot = Matrix4.CreateRotationY(angle2);
            //_models[1].Vertices.TranslateAll(Vector3.TransformVector(defaultVec2, rot));            
            
            //_models[2].Vertices = GeometryHelper.GetVerticesForOrdinaryCube(1);
            //rot = Matrix4.CreateRotationY(angle3);
            //_models[2].Vertices.TranslateAll(Vector3.TransformVector(defaultVec3, rot));
        }

        private void FullRender()
        {
            _graphics.Render(_models, _reflectiveModels);
        }
    }
}
