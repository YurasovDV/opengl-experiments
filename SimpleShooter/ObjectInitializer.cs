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

namespace SimpleShooter
{
    class ObjectInitializer : IObjectInitialiser
    {
        public IEnumerable<GameObject> CreateLevel()
        {
            var result = new List<GameObject>();
            var model = new SimpleModel();
            var vertices = GeometryHelper.GetVerticesForCube(5);

            Matrix4 translate = Matrix4.CreateTranslation(30, 0, 0);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Transform(vertices[i], translate);
            }

            var green = new Vector3(0, 1, 0);
            model.Vertices = vertices;
            model.Colors = Enumerable.Repeat(green, vertices.Length).ToArray();

            var obj = new GameObject(model, ShadersNeeded.TextureLess);
            obj.CalcNormals();
            result.Add(obj);
            return result;
        }
    }
}
