using OpenTK;
using System;

namespace OcTreeExample.OcTree
{
    /// <summary>
    /// AABB
    /// </summary>
    public class BoundingVolume
    {

        public string Path { get; set; }

        public Vector3[] Top { get; set; }
        public Vector3[] Bottom { get; set; }

        public Vector3 Centre { get; set; }

        public string Name { get; set; }

        private float sumSquaresForAllVertices = -1;

        public float SumSquaresForAllVertices
        {
            get { return sumSquaresForAllVertices; }
            set { sumSquaresForAllVertices = value; }
        }



        public BoundingVolume(Vector3[] top, Vector3[] bottom)
        {
            Top = new Vector3[top.Length];
            top.CopyTo(Top, 0);

            Bottom = new Vector3[bottom.Length];
            bottom.CopyTo(Bottom, 0);

           

            Centre = new Vector3(

               (top[0].X + top[2].X) * 0.5f,
                (top[0].Y + bottom[2].Y) * 0.5f,
                (top[0].Z + top[2].Z) * 0.5f);

        }

       

        /// <summary>
        /// очень приблизительная реализация, смотрим только на центр
        /// </summary>
        /// <param name="another"></param>
        /// <returns></returns>
        public bool Contains(BoundingVolume another)
        {
            if (another == null)
            {
                throw new ArgumentNullException("BoundingVolume.Contains: BoundingVolume another == null");
            }

            return false;
        }


        public bool Intersects(BoundingVolume another)
        {
            if (another == null)
            {
                throw new ArgumentNullException("BoundingVolume.Intersects: BoundingVolume another == null");
            }

            bool res = false;

            return res;
        }

        public static BoundingVolume CreateVolume(Vector3 centre, int halfSize)
        {
            var top = new Vector3[]
            {
                new Vector3(centre.X - halfSize, centre.Y + halfSize, centre.Z - halfSize),
                new Vector3(centre.X + halfSize, centre.Y + halfSize, centre.Z - halfSize),
                new Vector3(centre.X + halfSize, centre.Y + halfSize, centre.Z + halfSize),
                new Vector3(centre.X - halfSize, centre.Y + halfSize, centre.Z + halfSize),
            };

            var bottom = new Vector3[]
            {
                new Vector3(centre.X - halfSize, centre.Y - halfSize, centre.Z - halfSize),
                new Vector3(centre.X + halfSize, centre.Y - halfSize, centre.Z - halfSize),
                new Vector3(centre.X + halfSize, centre.Y - halfSize, centre.Z + halfSize),
                new Vector3(centre.X - halfSize, centre.Y - halfSize, centre.Z + halfSize),
            };

            return new BoundingVolume(top, bottom);
        }

        public static BoundingVolume CreateVolume(Vector3 centre, int halfSizeX, int halfSizeZ, int halfSizeY = 4)
        {
            var top = new Vector3[]
            {
                new Vector3(centre.X - halfSizeX, centre.Y + halfSizeY, centre.Z - halfSizeZ),
                new Vector3(centre.X + halfSizeX, centre.Y + halfSizeY, centre.Z - halfSizeZ),
                new Vector3(centre.X + halfSizeX, centre.Y + halfSizeY, centre.Z + halfSizeZ),
                new Vector3(centre.X - halfSizeX, centre.Y + halfSizeY, centre.Z + halfSizeZ),
            };

            var bottom = new Vector3[]
            {
                new Vector3(centre.X - halfSizeX, centre.Y - halfSizeY, centre.Z - halfSizeZ),
                new Vector3(centre.X + halfSizeX, centre.Y - halfSizeY, centre.Z - halfSizeZ),
                new Vector3(centre.X + halfSizeX, centre.Y - halfSizeY, centre.Z + halfSizeZ),
                new Vector3(centre.X - halfSizeX, centre.Y - halfSizeY, centre.Z + halfSizeZ),
            };

            return new BoundingVolume(top, bottom);
        }

    }
}
