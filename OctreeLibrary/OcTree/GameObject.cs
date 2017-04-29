using System;
using System.Collections.Generic;
using System.Linq;
using Common.Geometry;
using OpenTK;

namespace OcTreeLibrary
{
    public class OctreeGameObject : IOctreeItem
    {
        public Vector3[] Points { get; set; }

        public OctreeGameObject(Vector3 centre, float angle = 3.1415f)
        {
            var points = Extensions.CreatePlane(centre, angle);
            Points = points;
            BoundingBox = BoundingVolume.InitBoundingBox(points);
        }

        public event Action<object, ReinsertingEventArgs> NeedReinsert;

        public BoundingVolume BoundingBox { get; private set; }

        public BoundingVolume TreeSegment { get; set; }

        public void Tick(long delta)
        {
            // change points positions
            BoundingVolume newBox = BoundingVolume.InitBoundingBox(Points);

            if (NeedReinsert != null)
            {
                NeedReinsert(this, new ReinsertingEventArgs() { NewBox = newBox });
            }
        }

        public void UpdateBoundingBox(BoundingVolume newBox)
        {
            BoundingBox = newBox;
        }
    }
}
