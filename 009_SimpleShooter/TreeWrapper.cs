using System;
using System.Collections.Generic;
using Common.Geometry;
using OcTreeLibrary;
using SimpleShooter.Core;

namespace SimpleShooter
{
    class TreeWrapper
    {
        private OcTree _tree;

        public TreeWrapper(Level level)
        {
            _tree = new OcTree(level.Volume);
        }


        public void OctreeItem_Remove(object sender, ReinsertingEventArgs args)
        {
            var gameObj = sender as IOctreeItem;
            if (gameObj == null)
                throw new ArgumentException();
            _tree.Remove(gameObj);
        }

        public void OctreeItem_Insert(object sender, ReinsertingEventArgs args)
        {
            var gameObj = sender as IOctreeItem;
            if (gameObj == null)
                throw new ArgumentException();
            var v = _tree.Insert(gameObj);
            if (v == null)
            {
                //_objects.Remove(gameObj);
            }
        }

        internal List<IOctreeItem> GetPossibleCollisions(GameObject entity)
        {
            return _tree.GetPossibleCollisions(entity);
        }

        internal BoundingVolume Insert(IOctreeItem entity)
        {
           return _tree.Insert(entity);
        }
    }
}
