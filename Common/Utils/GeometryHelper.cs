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
    }
}