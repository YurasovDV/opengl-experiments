using Common.Geometry;
using OpenTK;

namespace OcTreeLibrary
{
    public class ReinsertingEventArgs
    {
        public BoundingVolume NewBox { get; set; }
        public BoundingVolume OldBox { get; set; }
    }
}
