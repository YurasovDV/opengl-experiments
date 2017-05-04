using System;
using OpenTK;
using System.Collections.Generic;
using Common.Geometry;
using OcTreeLibrary;

namespace SimpleShooter.Core
{
    class OctreeGameObject : IOctreeItem
    {

        public OctreeGameObject()
        {

        }

        public BoundingVolume BoundingBox { get; set; }

        public BoundingVolume TreeSegment { get; set; }

        public event EventHandler<ReinsertingEventArgs> NeedsRemoval;
        public event EventHandler<ReinsertingEventArgs> NeedsInsert;


        public void RaiseRemove()
        {
            if (NeedsRemoval != null)
            {
                NeedsRemoval(this, new ReinsertingEventArgs());
            }
        }

        public void RaiseInsert()
        {
            if (NeedsInsert != null)
            {
                NeedsInsert(this, new ReinsertingEventArgs());
            }
        }
    }
}
