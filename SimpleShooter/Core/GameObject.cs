using Common;
using OpenTK;
using SimpleShooter.Graphics;

namespace SimpleShooter.Core
{
    public class GameObject
    {

        public Matrix4 Transform { get; set; }

        public SimpleModel Model { get; set; }
        public RenderWrapper Wrapper { get; set; }

        public ShadersNeeded ShaderKind { get; set; }

        public GameObject(SimpleModel model, ShadersNeeded shadersNeeded)
        {
            ShaderKind = shadersNeeded;
            Model = model;
            Wrapper = new RenderWrapper(this);
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
            var up = Vector3.UnitY;
            var normals = new Vector3[points.Length];
            var tempVertices = new Vector3[3];

            for (int i = 0; i < points.Length; i += 6)
            {
                //int k = i / 6;
                Vector3 norm = up;

                tempVertices[0] = points[i];
                tempVertices[1] = points[i + 1];
                tempVertices[2] = points[i + 2];

                norm = CalcNormal(tempVertices);

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