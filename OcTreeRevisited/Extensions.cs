using OcTreeRevisited.OcTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OcTreeRevisited
{
    public static class Extensions
    {
        public static OcTreeItem FindVolume(this OcTreeItem[] items, GameObject obj)
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

    }
}
