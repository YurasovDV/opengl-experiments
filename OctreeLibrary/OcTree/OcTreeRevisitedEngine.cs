using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OcTreeLibrary
{
    internal class OcTreeRevisitedEngine
    {
        OcTree Tree { get; set; }

        public List<IOctreeItem> Objects { get; set; }

        public OcTreeRevisitedEngine()
        {
            Tree = CreateTree();
        }

        public void HandleLeavingOctant(object sender, ReinsertingEventArgs args)
        {
            if (sender == null)
            {
                throw new ArgumentNullException();
            }
            var gameObj = sender as IOctreeItem;
            if (gameObj == null)
            {
                throw new ArgumentException($"{gameObj.GetType()}", nameof(sender));
            }

            Tree.Remove(gameObj);

            if (args.NewBox == null)
            {
                throw new ArgumentException("args.NewBox is null");
            }

            gameObj.UpdateBoundingBox(args.NewBox);
            Tree.Insert(gameObj);
        }    

        private OcTree CreateTree()
        {
            var tree = new OcTree(BoundingVolume.CreateVolume(new Vector3(0, 0, 0), 80));

            Objects = CreateListObjects();
            Objects.ForEach(
                o =>
                tree.Insert(o));

            return tree;
        }

        private List<IOctreeItem> CreateListObjects()
        {
            var result = new List<IOctreeItem>();

            Random rand = new Random(300);
            AddRandomPlane(result, new Vector3(47f, 5, -45), rand);

            AddRandomPlane(result, new Vector3(46f, 17, -45), rand);

            AddRandomPlane(result, new Vector3(40f, 35, -45), rand);

            AddRandomPlane(result, new Vector3(55f, -35, -15), rand);

            AddRandomPlane(result, new Vector3(60f, 45, -35), rand);

            AddRandomPlane(result, new Vector3(54f, 12, -5), rand);

            return result;
        }

        private void AddRandomPlane(List<IOctreeItem> result, Vector3 centre, Random rand)
        {
            var dx = (float)rand.NextDouble() * -0.5f;
            var plane2 = new OctreeGameObject(centre);
            plane2.NeedReinsert += HandleLeavingOctant;
            result.Add(plane2);
        }
    }
}