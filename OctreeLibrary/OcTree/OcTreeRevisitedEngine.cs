using System.Collections.Generic;
using Common.Geometry;
using OpenTK;

namespace OcTreeLibrary
{
    internal class OcTreeRevisitedEngine
    {
        private OcTree Tree { get; set; }
        private List<IOctreeItem> Objects { get; set; }

        public OcTreeRevisitedEngine()
        {
            Tree = new OcTree(BoundingVolume.CreateVolume(new Vector3(0, 0, 0), 80));
            var plane2 = new OctreeGameObject(new Vector3(47f, 5, -45));
            plane2.NeedReinsert += HandleLeavingOctant;
        }

        public void Tick()
        {
            Objects.ForEach(o => (o as OctreeGameObject).Tick(0));
        }

        public void HandleLeavingOctant(object sender, ReinsertingEventArgs args)
        {
            var gameObj = sender as IOctreeItem;
            Tree.Remove(gameObj);
            gameObj.UpdateBoundingBox(args.NewBox);
            Tree.Insert(gameObj);
        }
    }
}