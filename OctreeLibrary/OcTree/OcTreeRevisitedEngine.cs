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

        public SimpleModel Model { get; set; }

        OcTree Tree { get; set; }

        public AbstractRenderEngine MainRender { get; set; }

        public List<GameObject> Objects { get; set; }

        public OcTreeRevisitedEngine(int Width, int Height)
        {
            Tree = CreateTree();
        }

        public void HandleLeavingOctant(object sender, ReinsertingEventArgs args)
        {
            if (sender == null)
            {
                throw new ArgumentNullException();
            }
            var gameObj = sender as GameObject;
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

        private List<GameObject> CreateListObjects()
        {
            var result = new List<GameObject>();

            Random rand = new Random(300);
            AddRandomPlane(result, new Vector3(47f, 5, -45), rand, "n11");

            AddRandomPlane(result, new Vector3(46f, 17, -45), rand, "n12");

            AddRandomPlane(result, new Vector3(40f, 35, -45), rand, "n13");

            AddRandomPlane(result, new Vector3(55f, -35, -15), rand, "n14");

            AddRandomPlane(result, new Vector3(60f, 45, -35), rand, "n15");

            AddRandomPlane(result, new Vector3(54f, 12, -5), rand, "n16");

            return result;
        }

        private void AddRandomPlane(List<GameObject> result, Vector3 centre, Random rand, string name)
        {
            var dx = (float)rand.NextDouble() * -0.5f;
            var speed = new Vector3(dx, 0, 0);
            var plane2 = new GameObject(centre)
            {
                Name = name,
                Speed = speed
            };
            plane2.NeedReinsert += HandleLeavingOctant;
            result.Add(plane2);
        }
    }
}