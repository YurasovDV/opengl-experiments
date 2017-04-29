using System;
using OpenTK;
using System.Collections.Generic;
using Common.Geometry;
using OcTreeLibrary;

namespace SimpleShooter.Core
{
    class OctreeGameObject : IOctreeItem
    {
        public Vector3[] Points { get; set; }

        public OctreeGameObject()
        {
            //BoundingBox = BoundingVolume.InitBoundingBox(points);
        }

        public event EventHandler<ReinsertingEventArgs> NeedReinsert;

        public BoundingVolume BoundingBox { get; set; }

        public BoundingVolume TreeSegment { get; set; }

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
