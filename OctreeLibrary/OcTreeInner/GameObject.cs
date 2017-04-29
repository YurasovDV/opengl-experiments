using System;
using Common.Geometry;
using OpenTK;

namespace OcTreeLibrary
{
    internal class OctreeGameObject : IOctreeItem
    {
        public Vector3[] Points { get; set; }

        public OctreeGameObject(Vector3 centre, float angle = 3.1415f)
        {
            var points = Extensions.CreatePlane(centre, angle);
            Points = points;
            BoundingBox = BoundingVolume.InitBoundingBox(points);
        }

        public event EventHandler<ReinsertingEventArgs> NeedReinsert;

        public BoundingVolume BoundingBox { get; set; }

        public BoundingVolume TreeSegment { get; set; }

        public void Tick(long delta)
        {
            // change points positions
            BoundingVolume newBox = BoundingVolume.InitBoundingBox(Points);

            RaiseReinsert(newBox);
        }

        public void UpdateBoundingBox(BoundingVolume newBox)
        {
            BoundingBox = newBox;
        }

        public void RaiseReinsert(BoundingVolume newVolume)
        {
            if (NeedReinsert != null)
            {
                NeedReinsert(this, new ReinsertingEventArgs() { NewBox = newVolume });
            }
        }
    }
}
