using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Geometry;
using OpenTK;

namespace OcTreeLibrary
{
    public class GameObject
    {
        public static Vector3[] ColorsForCube;
        public static Vector3 blue;

        public float AngleHorizontal { get; set; }

        public Vector3[] Points { get; set; }

        static GameObject()
        {
            blue = new Vector3(0, 0, 1);
            ColorsForCube = Enumerable.Repeat(blue, 24).ToArray();
        }

        public GameObject(Vector3 centre, float angle = 3.1415f)
        {
            var points = CreatePlane(centre, angle);
            Points = points;
            AngleHorizontal = angle;
            Speed = new Vector3(0, 0, 0);

            BoundingBox = InitBoundingBox(points);
        }

        public event Action<object, ReinsertingEventArgs> NeedReinsert;

        public Vector3 Speed { get; set; }

        public BoundingVolume BoundingBox { get; private set; }

        public BoundingVolume TreeSegment { get; set; }

        public string Name { get; set; }

        public void Tick(long delta)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = Points[i] + Speed;
            }

            BoundingVolume newBox = InitBoundingBox(Points);

            if (NeedReinsert != null)
            {
                NeedReinsert(this, new ReinsertingEventArgs() { NewBox = newBox });
            }
        }

        public SimpleModel GetAsModel()
        {
            var vertices = GetVertices();

            Vector3[] colors = null;
            if (vertices.Length == 24)
            {
                colors = ColorsForCube;
            }
            else
            {
                colors = Enumerable.Repeat(blue, vertices.Length).ToArray();
            }

            return new SimpleModel()
            {
                Vertices = vertices,
                Colors = colors
            };
        }

        internal void UpdateBoundingBox(BoundingVolume newBox)
        {
            BoundingBox = newBox;
        }

        private Vector3[] GetVertices()
        {
            var v1 = BoundingBox.GetLines();
            List<Vector3> list = new List<Vector3>();

            for (int i = 0; i < Points.Length; i++)
            {
                for (int j = i + 1; j < Points.Length; j++)
                {
                    list.Add(Points[i]);
                    list.Add(Points[j]);
                }
            }

            return list.ToArray();
        }

        public override string ToString()
        {
            return Name;
        }

        public Vector3[] GetLines()
        {
            return BoundingBox.GetLines();
        }

        private static Vector3[] CreatePlane(Vector3 centre, float angle = 0)
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
    }
}
