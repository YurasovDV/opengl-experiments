﻿using System;
using Common.Geometry;
using OpenTK;

namespace OcTreeLibrary
{
    public interface IOctreeItem
    {
        BoundingVolume BoundingBox { get; set; }
        BoundingVolume TreeSegment { get; set; }

        event EventHandler<ReinsertingEventArgs> NeedsRemoval;
        event EventHandler<ReinsertingEventArgs> NeedsInsert;


        void RaiseRemove();
        void RaiseInsert();
    }
}