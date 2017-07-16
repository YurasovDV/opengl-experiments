using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utils;


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

        private void CheckPointsInside(BoundingVolume another)
        {
            if (another == null)
            {
                throw new ArgumentNullException("BoundingVolume.CheckPointsInside: BoundingVolume another == null");
            }

            for (int j = 0; j < contains.Length; j++)
            {
                contains[j] = false;
            }

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
    }
}
