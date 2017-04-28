using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Utils;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.Player;

namespace SimpleShooter.LevelLoaders
{
    class ObjectInitializer : IObjectInitializer
    {
        private Vector3 _lightPos = new Vector3(30, 10, 0);

        private float edge = 50;

        private float tapeWidth =0.05f;

        public Level CreateLevel()
        {
            var level = new Level();

            var objectList = new List<GameObject>();
            Matrix4 translate = Matrix4.CreateTranslation(30, 4, 0);
            var green = new Vector3(0, 1, 0);
            GameObject obj = CreateCube(translate, green, 1, ShadersNeeded.TextureLess);
            objectList.Add(obj);

            translate = Matrix4.CreateTranslation(_lightPos);
            obj = CreateCube(translate, new Vector3(100, 100, 100), 0.5f, ShadersNeeded.TextureLessNoLight);
            objectList.Add(obj);

            obj = CreateWafer();
            objectList.Add(obj);


            translate = Matrix4.CreateTranslation(_lightPos);
            obj = CreateCube(translate, new Vector3(1, 0, 0), 2f, ShadersNeeded.TextureLessNoLight);
            var movableObj = new MovableObject(obj.Model, ShadersNeeded.TextureLessNoLight, new Vector3(1, 0, 0));
            objectList.Add(movableObj);

            level.Objects = objectList;

            var p = new PlayerModel(new Vector3(0, 0.5f, 0), new Vector3(100, 0.5f, 0));
            level.Player = p;

            return level;
        }

        private GameObject CreateWafer()
        {
            var verticesPlane = new[]
            {
                new Vector3(edge, 0, -edge),
                new Vector3(-edge, 0, edge),
                new Vector3(-edge, 0, -edge),

                new Vector3(edge, 0, -edge),
                new Vector3(edge, 0, edge),
                new Vector3(-edge, 0, edge),
            };

            var verticesOx = new[]
            {
                new Vector3(edge, 0.2f, -tapeWidth),
                new Vector3(-edge, 0.2f, tapeWidth),
                new Vector3(-edge, 0.2f, -tapeWidth),

                new Vector3(edge, 0.2f, -tapeWidth),
                new Vector3(edge, 0.2f, tapeWidth),
                new Vector3(-edge, 0.2f, tapeWidth),
            };

            var verticesOZ = new[]
            {
                new Vector3(tapeWidth, 0.2f, -edge),
                new Vector3(-tapeWidth, 0.2f, edge),
                new Vector3(-tapeWidth, 0.2f, -edge),

                new Vector3(tapeWidth, 0.2f, -edge),
                new Vector3(tapeWidth, 0.2f, edge),
                new Vector3(-tapeWidth, 0.2f, edge),
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

            var model = new SimpleModel()
            {
                Vertices = verticesCombined.ToArray(),
                Colors = colorsCombined
            };
            var obj = new GameObject(model, ShadersNeeded.TextureLessNoLight);
            obj.Id = IdService.GetNext();
            return obj;
        }

        private static GameObject CreateCube(Matrix4 translate, Vector3 color, float size, ShadersNeeded shadersKind)
        {
            var model = new SimpleModel();
            var vertices = GeometryHelper.GetVerticesForCube(size);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Transform(vertices[i], translate);
            }

            model.Vertices = vertices;
            model.Colors = Enumerable.Repeat(color, vertices.Length).ToArray();

            var obj = new GameObject(model, shadersKind);
            obj.CalcNormals();
            obj.Id = IdService.GetNext();
            return obj;
        }
    }
}
