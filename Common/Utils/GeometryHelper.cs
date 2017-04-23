using OpenTK;

namespace Common.Utils
{
    public class GeometryHelper
    {
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