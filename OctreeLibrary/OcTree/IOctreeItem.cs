using System;
using Common;
using Common.Geometry;
using OpenTK;

namespace OcTreeLibrary
{
    public interface IOctreeItem
    {
        BoundingVolume BoundingBox { get; }
        BoundingVolume TreeSegment { get; set; }
        event Action<object, ReinsertingEventArgs> NeedReinsert;

        void UpdateBoundingBox(BoundingVolume newBox);
    }
}