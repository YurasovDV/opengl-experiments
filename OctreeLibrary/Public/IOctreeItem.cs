using System;
using Common.Geometry;

namespace OcTreeLibrary
{
    public interface IOctreeItem
    {
        BoundingVolume BoundingBox { get; set; }
        BoundingVolume TreeSegment { get; set; }
        event EventHandler<ReinsertingEventArgs> NeedReinsert;
        void UpdateBoundingBox(BoundingVolume newBox);
    }
}