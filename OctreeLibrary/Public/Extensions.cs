using System.Collections.Generic;
using OpenTK;

namespace OcTreeLibrary
{
    public static class Extensions
    {
        public static OcTreeItem FindVolume(this OcTreeItem[] items, IOctreeItem obj)
        {
            OcTreeItem res = null;
            foreach (var vol in items)
            {
                if (vol != null)
                {
                    var contains = vol.Volume.Contains(obj.BoundingBox);
                    if (contains)
                    {
                        res = vol;
                        break;
                    }
                }
            }

            return res;
        }

        internal static Vector3[] CreatePlane(Vector3 centre, float angle = 0)
        {
            var rotation = Matrix4.CreateRotationY(angle);

            var nose = centre + Vector3.Transform(new Vector3(3, 0, 0), rotation);

            var left = centre + Vector3.Transform(new Vector3(-2, -1, -3), rotation);

            var right = centre + Vector3.Transform(new Vector3(-2, -1, 3), rotation);

            var top = centre + Vector3.Transform(new Vector3(-3, 1, 0), rotation);

            var result = new List<Vector3>()
            {
                nose, left, top, right
            };

            return result.ToArray();
        }
    }
}
