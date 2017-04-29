using Common;
using OcTreeLibrary;
using OpenTK;
using SimpleShooter.Graphics;

namespace SimpleShooter.Core
{
    public class GameObject
    {
        public long Id { get; set; }

        public SimpleModel Model { get; set; }

        public IOctreeItem OctreeItem { get; set; }

        public ShadersNeeded ShaderKind { get; set; }

        public GameObject(SimpleModel model, ShadersNeeded shadersNeeded)
        {
            ShaderKind = shadersNeeded;
            Model = model;
            OctreeItem = new OctreeGameObject();
        }

        public void CalcNormals()
        {
            Model.Normals = GetNormals(Model.Vertices);
        }

        public void InvertNormals()
        {
            for (int i = 0; i < Model.Normals.Length; i++)
            {
                Model.Normals[i] *= -1;
            }
        }

        private Vector3[] GetNormals(Vector3[] points)
        {
            var normals = new Vector3[points.Length];
            var tempVertices = new Vector3[3];

            for (int i = 0; i < points.Length; i += 6)
            {
                tempVertices[0] = points[i];
                tempVertices[1] = points[i + 1];
                tempVertices[2] = points[i + 2];

                var norm = CalcNormal(tempVertices);

                for (int j = i; j < i + 6; j++)
                {
                    normals[j] = norm;
                }
            }

            return normals;
        }

        public static Vector3 CalcNormal(Vector3[] vrt)
        {
            var n = Vector3.Cross(vrt[0] - vrt[2], vrt[0] - vrt[1]);
            n.Normalize();
            return n;
        }
    }
}