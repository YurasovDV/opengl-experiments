using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utils;
using OpenTK;


namespace Common.Geometry
{
    public partial class BoundingVolume
    {
        public bool Contains(BoundingVolume another)
        {
            if (another == null)
            {
                throw new ArgumentNullException("BoundingVolume.Contains: BoundingVolume another == null");
            }

            var cnts = CheckPointsInside(another);

            var res = cnts.All(inside => inside);
            return res;
        }


        public bool Intersects(BoundingVolume another)
        {
            if (another == null)
            {
                throw new ArgumentNullException("BoundingVolume.Intersects: BoundingVolume another == null");
            }

            var cnts = CheckPointsInside(another);

            var res = cnts.Any(inside => inside);

            return res;
        }

        public bool[] CheckPointsInside(BoundingVolume another)
        {
            if (another == null)
            {
                throw new ArgumentNullException("BoundingVolume.CheckPointsInside: BoundingVolume another == null");
            }


            int i = 0;
            bool[] cnts = new bool[8];

            var anotherTop = another.VerticesTop;
            foreach (var vert in anotherTop)
            {
                cnts[i] = vert.Inside(BottomLeftBack, TopRightFront);
                i++;
            }

            var anotherBottom = another.VerticesBottom;
            foreach (var vert in anotherBottom)
            {
                cnts[i] = vert.Inside(BottomLeftBack, TopRightFront);
                i++;
            }

            return cnts;

        }
    }
}
