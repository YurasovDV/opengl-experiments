using System;
using Common.Geometry;

namespace OcTreeLibrary
{
    public interface IOctreeItem
    {
        BoundingVolume BoundingBox { get; }
        BoundingVolume TreeSegment { get; set; }
        event EventHandler<ReinsertingEventArgs> NeedReinsert;
        void UpdateBoundingBox(BoundingVolume newBox);
    }
}