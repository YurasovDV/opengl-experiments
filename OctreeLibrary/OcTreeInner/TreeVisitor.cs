using Common;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using OcTreeLibrary;

namespace OctreeLibrary
{
    internal class TreeVisitor
    {
        OcTree tree;

        public TreeVisitor(OcTree tree)
        {
            this.tree = tree;
        }

        public SimpleModel GetModel()
        {
            var model = new SimpleModel();

            var green = new Vector3(0, 1, 0);
            var red = new Vector3(1, 0, 0);

            var array = VisitVertices(tree.Root, objColor: red, volumeColor: green);

            model.Vertices = array.Item1.ToArray();

            model.Colors = array.Item2.ToArray();

            return model;
        }

        public IEnumerable<OcTreeItem> Visit()
        {
            return VisitAll(tree.Root).ToList();
        }

        private IEnumerable<OcTreeItem> VisitAll(OcTreeItem item)
        {
            var result = new List<OcTreeItem>();

            if (item != null)
            {
                result.Add(item);
                if (item.Children != null)
                {
                    foreach (var child in item.Children.Where(i => i != null))
                    {
                        result.AddRange(VisitAll(child));
                    }
                }
            }

            return result;
        }


        private Tuple<List<Vector3>, List<Vector3>> VisitVertices(OcTreeItem item, Vector3 objColor, Vector3 volumeColor)
        {
            var result = new Tuple<List<Vector3>, List<Vector3>>(new List<Vector3>(), new List<Vector3>());

            if (item == null)
            {
                return result;
            }

            var x = GetCubeLines(item, objColor, volumeColor);

            result.Item1.AddRange(x.Item1);
            result.Item2.AddRange(x.Item2);

            if (item.Children == null)
            {
                return result;
            }

            foreach (var child in item.Children)
            {
                x = VisitVertices(child, objColor, volumeColor);
                result.Item1.AddRange(x.Item1);
                result.Item2.AddRange(x.Item2);
            }

            return result;
        }

        private Tuple<List<Vector3>, List<Vector3>> GetCubeLines(OcTreeItem item, Vector3 objColor, Vector3 volumeColor)
        {
            List<Vector3> resultVertices = new List<Vector3>(50);
            var resultColors = new List<Vector3>();

            Vector3[] a = item.Volume.GetLines();
            resultVertices.AddRange(a);
            resultColors.AddRange(Enumerable.Repeat(volumeColor, a.Length));

            resultVertices.TrimExcess();

            return new Tuple<List<Vector3>, List<Vector3>>(resultVertices, resultColors);
        }
    }
}
