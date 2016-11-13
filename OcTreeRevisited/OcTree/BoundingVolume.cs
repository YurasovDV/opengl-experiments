using OpenTK;
using System;
using System.Linq;

namespace OcTreeRevisited.OcTree
{
    /// <summary>
    /// AABB
    /// </summary>
    public class BoundingVolume
    {
        private float _maxDimension;
        private float _space = -1;

        public string Path { get; set; }

        public Vector3 BottomLeftBack { get; set; }
        public Vector3 TopRightFront { get; set; }

        public Vector3 Centre { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Z
        /// </summary>
        public float Depth { get; set; }

        public Vector3[] VerticesTop { get; set; }

        public Vector3[] VerticesBottom { get; set; }

        public float Space
        {
            get
            {
                if (_space == -1)
                {
                    _space = Width * Height * Depth;
                }
                return _space;
            }
        }

        public float MaxDimension
        {
            get
            {
                return _maxDimension;
            }
        }

        public BoundingVolume(Vector3 bottomLeftBack, Vector3 topRightFront)
        {
            BottomLeftBack = new Vector3(bottomLeftBack);
            TopRightFront = new Vector3(topRightFront);

            Width = topRightFront.X - bottomLeftBack.X;
            Height = topRightFront.Y - bottomLeftBack.Y;
            Depth = topRightFront.Z - bottomLeftBack.Z;

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

        public void Reinit(BoundingVolume newBox)
        {
            var bottomLeftBack = newBox.BottomLeftBack;
            var topRightFront = newBox.TopRightFront;

            BottomLeftBack = new Vector3(bottomLeftBack);
            TopRightFront = new Vector3(topRightFront);

            Width = topRightFront.X - bottomLeftBack.X;
            Height = topRightFront.Y - bottomLeftBack.Y;
            Depth = topRightFront.Z - bottomLeftBack.Z;

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
