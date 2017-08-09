using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utils;
using OpenTK;

namespace Common.Geometry
{
    public partial class BoundingVolume
    {
        private static bool[] contains = new bool[8];


        public bool Contains(BoundingVolume another)
        {
            if (another == null)
            {
                throw new ArgumentNullException("BoundingVolume.Contains: BoundingVolume another == null");
            }

            CheckPointsInside(another);

            var res = contains.All(inside => inside);
            return res;
        }


        public bool Intersects(BoundingVolume another)
        {
            if (another == null)
            {
                throw new ArgumentNullException("BoundingVolume.Intersects: BoundingVolume another == null");
            }

            CheckPointsInside(another);

            var res = contains.Any(inside => inside);

            return res;
        }


        /// <summary>
        /// returns the shortest vector which can be used for collision resolution
        /// </summary>
        /// <param name="another"></param>
        /// <returns></returns>
        public Vector3 GetCollisionResolution(BoundingVolume another)
        {
            if (another == null)
            {
                throw new ArgumentNullException("BoundingVolume.Intersects: BoundingVolume another == null");
            }

            var anotherCentre = another.Centre;

            var anotherdimensions = another.TopRightFront - another.BottomLeftBack;

            var myDimensions = TopRightFront - BottomLeftBack;

            var dx = Math.Abs(Centre.X - anotherCentre.X) - (anotherdimensions.X + myDimensions.X) * 0.5f;
            var dy = Math.Abs(Centre.Y - anotherCentre.Y) - (anotherdimensions.Y + myDimensions.Y) * 0.5f;
            var dz = Math.Abs(Centre.Z - anotherCentre.Z) - (anotherdimensions.Z + myDimensions.Z) * 0.5f;

            dx = (dx >= 0) ? 0 : (Centre.X < anotherCentre.X ? -dx : dx);
            dy = (dy >= 0) ? 0 : (Centre.Y < anotherCentre.Y ? -dy : dy);
            dz = (dz >= 0) ? 0 : (Centre.Z < anotherCentre.Z ? -dz : dz);

            if (dx == 0 || dy == 0 || dz == 0)
            {
                return Vector3.Zero;
            }


            var absDx = Math.Abs(dx);
            var absDy = Math.Abs(dy);
            var absDz = Math.Abs(dz);

            if (absDz < Math.Min(absDx, absDy))
            {
                return new Vector3(0, 0, dz);
            }
            else if (absDy < Math.Min(absDx, absDz))
            {
                return new Vector3(0, dy, 0);
            }
            return new Vector3(dx, 0, 0);
        }


        private void CheckPointsInside(BoundingVolume another)
        {
            if (another == null)
            {
                throw new ArgumentNullException("BoundingVolume.CheckPointsInside: BoundingVolume another == null");
            }

            ReInitState();

            int i = 0;

            var anotherTop = another.VerticesTop;
            foreach (var vert in anotherTop)
            {
                contains[i] = vert.Inside(BottomLeftBack, TopRightFront);
                i++;
            }

            var anotherBottom = another.VerticesBottom;
            foreach (var vert in anotherBottom)
            {
                contains[i] = vert.Inside(BottomLeftBack, TopRightFront);
                i++;
            }

        }

        private static void ReInitState()
        {
            for (int j = 0; j < contains.Length; j++)
            {
                contains[j] = false;
            }
        }
    }
}
