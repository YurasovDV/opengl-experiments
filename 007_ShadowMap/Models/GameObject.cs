using OpenTK;

namespace ShadowMap
{
    public class GameObject : Common.SimpleModel
    {
        public static Vector3 CalcNormal(Vector3[] vrt)
        {
            var n = Vector3.Cross(vrt[0] - vrt[2], vrt[0] - vrt[1]);
            n.Normalize();
            return n;
        }

        public static void CalcNormal(Vector3[] vrt, Vector3[] normals)
        {
            var n = Vector3.Cross(vrt[0] - vrt[1], vrt[0] - vrt[2]);
            n.Normalize();
            normals[0] = n;
            normals[1] = n;
            normals[2] = n;
        }
    }
}
