using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utils;
using OpenTK;

namespace Common.Geometry
{
    public class BoundingVolume
    {
        private float _maxDimension;

        public Vector3 BottomLeftBack { get; set; }
        public Vector3 TopRightFront { get; set; }

        public Vector3 Centre { get; set; }

       

        public Vector3[] VerticesTop { get; set; }

        public Vector3[] VerticesBottom { get; set; }


        public float MaxDimension
        {
            get
            {
                return _maxDimension;
            }
        }

        public float Width { get; private set; }

        public BoundingVolume(Vector3 bottomLeftBack, Vector3 topRightFront)
        {
            BottomLeftBack = new Vector3(bottomLeftBack);
            TopRightFront = new Vector3(topRightFront);

            Width = topRightFront.X - bottomLeftBack.X;
            var Height = topRightFront.Y - bottomLeftBack.Y;
            var Depth = topRightFront.Z - bottomLeftBack.Z;

            VerticesTop = new Vector3[]
                {
                    new Vector3(bottomLeftBack) + new Vector3(0, Height, 0),
                    new Vector3(bottomLeftBack) + new Vector3(Width, Height, 0),
                    new Vector3(TopRightFront),
                    new Vector3(bottomLeftBack) + new Vector3(0, Height, Depth)
                };

            VerticesBottom = new Vector3[]
                {
                    new Vector3(bottomLeftBack),
                    new Vector3(bottomLeftBack) + new Vector3(Width, 0, 0),
                    new Vector3(bottomLeftBack) + new Vector3(Width, 0, Depth),
                    new Vector3(bottomLeftBack) + new Vector3(0, 0, Depth)
                };

            Centre = new Vector3(
               (bottomLeftBack.X + topRightFront.X) * 0.5f,
                (bottomLeftBack.Y + topRightFront.Y) * 0.5f,
                (bottomLeftBack.Z + topRightFront.Z) * 0.5f);

            _maxDimension = Math.Max(Width, (Math.Max(Height, Depth)));

        }

        public static BoundingVolume CreateVolume(Vector3 centre, float halfSize)
        {
            var shift = new Vector3(halfSize, halfSize, halfSize);

            return new BoundingVolume(
               centre - shift,
                centre + shift
                );
        }

        public static BoundingVolume CreateVolume(Vector3 centre, int halfSizeX, int halfSizeZ, int halfSizeY = 4)
        {
            var shift = new Vector3(halfSizeX, halfSizeY, halfSizeZ);

            return new BoundingVolume(
              centre - shift,
               centre + shift);
        }

        public static BoundingVolume InitBoundingBox(Vector3[] points)
        {
            var minX = points.Min(p => p.X);
            var maxX = points.Max(p => p.X);

            var minY = points.Min(p => p.Y);
            var maxY = points.Max(p => p.Y);

            var minZ = points.Min(p => p.Z);
            var maxZ = points.Max(p => p.Z);

            var min = new Vector3(minX, minY, minZ);
            var max = new Vector3(maxX, maxY, maxZ);

            var volume = new BoundingVolume(min, max);

            return volume;
        }

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

        public Vector3[] GetLines()
        {
            return new Vector3[]
           {
            VerticesTop[0],
            VerticesBottom[0],

            VerticesTop[1],
            VerticesBottom[1],

            VerticesTop[2],
            VerticesBottom[2],

            VerticesTop[3],
            VerticesBottom[3],

            VerticesTop[0],
            VerticesTop[1],

            VerticesTop[1],
            VerticesTop[2],

            VerticesTop[2],
            VerticesTop[3],

            VerticesTop[3],
            VerticesTop[0],

            VerticesBottom[0],
            VerticesBottom[1],

            VerticesBottom[1],
            VerticesBottom[2],

            VerticesBottom[2],
            VerticesBottom[3],

            VerticesBottom[3],
            VerticesBottom[0],

             };
        }

    }

}
