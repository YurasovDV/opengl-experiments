using System.Collections.Generic;
using Common.Geometry;

namespace OcTreeLibrary
{
    public class OcTree
    {
        /*
        e g, 
            child:	    0 1 2 3 4 5 6 7
				x:      - - - - + + + +
				y:      - - + + - - + +
				z:      - + - + - + - +
            */
        public OcTreeItem Root { get; set; }

        public OcTree(BoundingVolume world)
        {
            Root = new OcTreeItem(world);

            Root.Number = 0;
            Root.Level = 0;
        }

        public BoundingVolume Insert(IOctreeItem obj)
        {
            obj.ReinsertImmediately = false;
            return Root.Insert(obj);
        }

        public void Remove(IOctreeItem obj)
        {
            obj.ReinsertImmediately = false;
            Root.Remove(obj);
        }

        public List<IOctreeItem> GetPossibleCollisions(IOctreeItem obj)
        {
            var result = new List<IOctreeItem>();
            Root.EnumeratePossibleCollision(obj, result);
            return result;
        }

    }
}
