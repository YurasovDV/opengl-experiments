using System;
using OpenTK;

namespace Common.Utils
{
    public static class GeometryHelper
    {
        public static bool BetweenX(this Vector3 point, Vector3 left, Vector3 right)
        {
            return left.X <= point.X && right.X >= point.X;
        }

        public static bool BetweenY(this Vector3 point, Vector3 bottom, Vector3 top)
        {
            return bottom.Y <= point.Y && top.Y >= point.Y;
        }

        public static bool BetweenZ(this Vector3 point, Vector3 far, Vector3 near)
        {
            return far.Z <= point.Z && near.Z >= point.Z;
        }

        public static bool Inside(this Vector3 point, Vector3 bottomLeftBack, Vector3 topRightFront)
        {
            return point.BetweenX(bottomLeftBack, topRightFront)
                && point.BetweenY(bottomLeftBack, topRightFront)
                && point.BetweenZ(bottomLeftBack, topRightFront);
        }

        public static void TranslateAll(this Vector3[] vertices, Vector3 transformed)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException(nameof(vertices));
            }
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += transformed;
            }
        }

        public static Vector3[] GetVerticesForCube(float size)
        {
            return new[]
            {
                new Vector3(-size, size, -size),
                new Vector3(-size, -size, -size),
                new Vector3(size, -size, -size),
                new Vector3(size, -size, -size),
                new Vector3(size, size, -size),
                new Vector3(-size, size, -size),

                new Vector3(-size, -size, size),
                new Vector3(-size, -size, -size),
                new Vector3(-size, size, -size),
                new Vector3(-size, size, -size),
                new Vector3(-size, size, size),
                new Vector3(-size, -size, size),

                new Vector3(size, -size, -size),
                new Vector3(size, -size, size),
                new Vector3(size, size, size),
                new Vector3(size, size, size),
                new Vector3(size, size, -size),
                new Vector3(size, -size, -size),

                new Vector3(-size, -size, size),
                new Vector3(-size, size, size),
                new Vector3(size, size, size),
                new Vector3(size, size, size),
                new Vector3(size, -size, size),
                new Vector3(-size, -size, size),

               // up
                new Vector3(-size, size, -size),
                new Vector3(size, size, -size),
                new Vector3(size, size, size),
                new Vector3(size, size, size),
                new Vector3(-size, size, size),
                new Vector3(-size, size, -size),


                // down
                new Vector3(-size, -size, -size),
                new Vector3(-size, -size, size),
                new Vector3(size, -size, -size),
                new Vector3(size, -size, -size),
                new Vector3(-size, -size, size),
                new Vector3(size, -size, size)
            };
        }

        public static Vector3[] GetNormals(this Vector3[] points)
        {
            var normals = new Vector3[points.Length];
            var tempVertices = new Vector3[3];

            for (int i = 0; i < points.Length; i += 6)
            {
                tempVertices[0] = points[i];
                tempVertices[1] = points[i + 1];
                tempVertices[2] = points[i + 2];

                var norm = CalcNormalForTriangle(tempVertices);

                for (int j = i; j < i + 6; j++)
                {
                    normals[j] = norm;
                }
            }

            return normals;
        }

        public static Vector3 CalcNormalForTriangle(Vector3[] vrt)
        {
            var n = Vector3.Cross(vrt[0] - vrt[2], vrt[0] - vrt[1]);
            n.Normalize();
            return n;
        }
    }
}