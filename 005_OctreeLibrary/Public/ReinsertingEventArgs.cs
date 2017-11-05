using Common.Geometry;
using System;

namespace OcTreeLibrary
{
    public class ReinsertingEventArgs : EventArgs
    {
        public BoundingVolume NewBox { get; set; }
        public BoundingVolume OldBox { get; set; }
    }
}
