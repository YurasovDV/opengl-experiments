using Common;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Geometry;

namespace OcTreeLibrary
{
    public class OcTree
    {
        /*
        e g, 
            child:	0 1 2 3 4 5 6 7
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
            return Root.Insert(obj);
        }

        public void Remove(IOctreeItem obj)
        {
            Root.Remove(obj);
        }
    }
}
