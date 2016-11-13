using Common;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace OcTreeExample.OcTree
{
    public class OcTree
    {

        public OcTreeItem Root { get; set; }

        public OcTree(BoundingVolume world)
        {

            Root = new OcTreeItem(world);
            //Root.InitChildren();

            Root.Number = 0;
            Root.Level = 0;
        }


        public void Insert(BoundingVolume obj)
        {
            if (Root.IsLeaf)
            {
                Root.AddObject(obj);
            }

            else
            {
                Root.Insert(obj);
            }
        }

        public SimpleModel GetModel()
        {
            var model = new SimpleModel();

            var green = new Vector3(0, 1, 0);
            var red = new Vector3(1, 0, 0);

            var array = VisitVertices(Root, objColor: red, volumeColor: green);

            model.Vertices = array.Item1.ToArray();

            model.Colors = array.Item2.ToArray();

            return model;
        }

        public IEnumerable<OcTreeItem> Visit()
        {
            return VisitAll(Root).ToList();
        }

        private IEnumerable<OcTreeItem> VisitAll(OcTreeItem item)
        {
            var result = new List<OcTreeItem>();

            if (item != null)
            {
                result.Add(item);
                if (item.Children!=null)
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

        internal void Serialize(string path)
        {
            using (StreamWriter wr = new StreamWriter(path))
            {
                var vertices = Visit();
                int currentLevel = vertices.First().Level;

                while (vertices.Any())
                {
                    var verticesOnCurrentLevel = vertices.Where(i => i.Level == currentLevel);
                    foreach (var v in verticesOnCurrentLevel)
                    {
                        wr.Write(v);
                        wr.Write(" NEXT ");
                    }

                    vertices = vertices.Where(i => i.Level != currentLevel).ToList();

                    currentLevel++;
                    wr.WriteLine();
                }
            }
        }

        private Tuple<List<Vector3>, List<Vector3>> GetCubeLines(OcTreeItem item, Vector3 objColor, Vector3 volumeColor)
        {

            List<Vector3> resultVertices = new List<Vector3>(50);
            var resultColors = new List<Vector3>();

            Vector3[] a = GetLines(item.Volume);
            resultVertices.AddRange(a);
            resultColors.AddRange(Enumerable.Repeat(volumeColor, a.Length));

            if (item.Objects != null)
            {
                var realObjects = item.Objects.Where(o => o != null);
                //float i = 0;
                foreach (var obj in realObjects)
                {
                    a = GetLines(obj);
                    resultVertices.AddRange(a);
                    resultColors.AddRange(Enumerable.Repeat(objColor, a.Length));
                }
            }

            resultVertices.TrimExcess();

            return new Tuple<List<Vector3>, List<Vector3>>(resultVertices, resultColors);
        }

        private static Vector3[] GetLines(BoundingVolume item)
        {
            return new Vector3[]
           {
             item.Top[0],
             item.Bottom[0],

             item.Top[1],
             item.Bottom[1],

             item.Top[2],
             item.Bottom[2],

             item.Top[3],
             item.Bottom[3],

             item.Top[0],
             item.Top[1],

             item.Top[1],
             item.Top[2],

             item.Top[2],
             item.Top[3],

             item.Top[3],
             item.Top[0],

             item.Bottom[0],
             item.Bottom[1],

             item.Bottom[1],
             item.Bottom[2],

             item.Bottom[2],
             item.Bottom[3],

             item.Bottom[3],
             item.Bottom[0],

             };
        }
    }
}
