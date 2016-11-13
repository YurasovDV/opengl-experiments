using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OcTreeExample.OcTree
{
    public class OcTreeItem
    {

        public OcTreeItem(BoundingVolume volume)
        {
            Volume = volume;
            Objects = new List<BoundingVolume>();

            var minX = volume.Bottom.Min(v => v.X);
            var minY = volume.Bottom.Min(v => v.Y);
            var minZ = volume.Bottom.Min(v => v.Z);

            var maxX = volume.Bottom.Max(v => v.X);
            var maxY = volume.Bottom.Max(v => v.Y);
            var maxZ = volume.Bottom.Max(v => v.Z);

            Children = new OcTreeItem[8];

            HalfSize = (maxX - minX) * 0.5f;

        }

        public int Number { get; set; }

        public float HalfSize { get; set; }

        public List<BoundingVolume> Objects { get; set; }

        public OcTreeItem[] Children { get; set; }

        public BoundingVolume Volume { get; set; }

        public int Level { get; internal set; }


        public bool IsLeaf => Children[0] == null && !Objects.Any();

        public void AddObject(BoundingVolume obj)
        {
            Objects.Add(obj);
            //InitChildren();
        }

        public void Insert(BoundingVolume obj)
        {
            if (IsLeaf)
            {
                AddObject(obj);
                return;
            }

            InitChildren();

            var child = Children.FindVolume(obj);
            if (child != null)
            {
                child.Insert(obj);
                /*var old = new List<BoundingVolume>(Objects);

                foreach (var o in old)
                {
                    Objects.Remove(o);
                    Insert(o);
                }*/
            }
            else
            {
                AddObject(obj);
            }
        }


        public void InitChildren()
        {
            if (Children[0] == null)
            {
                var centre = Volume.Centre;

                var h = HalfSize;

                CreateChild0(centre, h);
                CreateChild1(centre, h);
                CreateChild2(centre, h);
                CreateChild3(centre, h);

                CreateChild4(centre, h);
                CreateChild5(centre, h);
                CreateChild6(centre, h);
                CreateChild7(centre, h);

                foreach (var child in Children)
                {
                    child.Level = Level + 1;
                }
            }
        }


        public override string ToString()
        {
            string res = string.Format("L {0}, N {1}, Hs {2}, (", Level, Number, HalfSize);

            foreach (var o in Objects)
            {
                res += o.Name + ", ";
            }

            res = res.Trim(',');

            res += ")";

            return res;
        }

        private void CreateChild0(Vector3 centre, float HalfSize)
        {
            var top = new Vector3[]
            {
                new Vector3(centre.X - HalfSize, centre.Y + HalfSize, centre.Z - HalfSize),
                new Vector3(centre.X, centre.Y + HalfSize, centre.Z - HalfSize),
                new Vector3(centre.X, centre.Y + HalfSize, centre.Z),
                new Vector3(centre.X - HalfSize, centre.Y + HalfSize, centre.Z),
            };

            var bottom = new Vector3[]
            {
                new Vector3(centre.X - HalfSize, centre.Y, centre.Z - HalfSize),
                new Vector3(centre.X, centre.Y, centre.Z - HalfSize),
                new Vector3(centre.X, centre.Y, centre.Z),
                new Vector3(centre.X -HalfSize, centre.Y, centre.Z),
            };

            var b0 = new BoundingVolume(top, bottom);
            Children[0] = new OcTreeItem(b0) { Number = 0 };
        }

        private void CreateChild1(Vector3 centre, float HalfSize)
        {
            var top = new Vector3[]
            {
                new Vector3(centre.X, centre.Y + HalfSize, centre.Z - HalfSize),
                new Vector3(centre.X + HalfSize, centre.Y + HalfSize, centre.Z - HalfSize),
                new Vector3(centre.X + HalfSize, centre.Y + HalfSize, centre.Z),
                new Vector3(centre.X, centre.Y + HalfSize, centre.Z),
            };

            var bottom = new Vector3[]
            {
                new Vector3(centre.X, centre.Y, centre.Z - HalfSize),
                new Vector3(centre.X + HalfSize, centre.Y, centre.Z - HalfSize),
                new Vector3(centre.X + HalfSize, centre.Y, centre.Z),
                new Vector3(centre.X, centre.Y, centre.Z),
            };

            var b0 = new BoundingVolume(top, bottom);
            Children[1] = new OcTreeItem(b0) { Number = 1 };
        }

        private void CreateChild2(Vector3 centre, float HalfSize)
        {
            var top0 = new Vector3[]
            {
                new Vector3(centre.X, centre.Y + HalfSize, centre.Z),
                new Vector3(centre.X + HalfSize, centre.Y + HalfSize, centre.Z),
                new Vector3(centre.X + HalfSize, centre.Y + HalfSize, centre.Z + HalfSize),
                new Vector3(centre.X, centre.Y + HalfSize, centre.Z + HalfSize),
            };

            var bottom = new Vector3[]
            {
                new Vector3(centre.X, centre.Y, centre.Z),
                new Vector3(centre.X + HalfSize, centre.Y, centre.Z),
                new Vector3(centre.X + HalfSize, centre.Y, centre.Z + HalfSize),
                new Vector3(centre.X, centre.Y, centre.Z + HalfSize),
            };

            var b0 = new BoundingVolume(top0, bottom);
            Children[2] = new OcTreeItem(b0) { Number = 2 };
        }

        private void CreateChild3(Vector3 centre, float HalfSize)
        {
            var top0 = new Vector3[]
            {
                new Vector3(centre.X - HalfSize, centre.Y + HalfSize, centre.Z),
                new Vector3(centre.X, centre.Y + HalfSize, centre.Z),
                new Vector3(centre.X, centre.Y + HalfSize, centre.Z + HalfSize),
                new Vector3(centre.X - HalfSize, centre.Y + HalfSize, centre.Z + HalfSize),
            };

            var bottom = new Vector3[]
            {
                 new Vector3(centre.X - HalfSize, centre.Y, centre.Z),
                new Vector3(centre.X, centre.Y, centre.Z),
                new Vector3(centre.X, centre.Y, centre.Z + HalfSize),
                new Vector3(centre.X - HalfSize, centre.Y, centre.Z + HalfSize),
            };

            var b0 = new BoundingVolume(top0, bottom);
            Children[3] = new OcTreeItem(b0) { Number = 3 };
        }

        private void CreateChild4(Vector3 centre, float HalfSize)
        {
            var top4 = new Vector3[]
            {
                new Vector3(centre.X - HalfSize, centre.Y, centre.Z - HalfSize),
                new Vector3(centre.X, centre.Y, centre.Z - HalfSize),
                new Vector3(centre.X, centre.Y, centre.Z),
                new Vector3(centre.X - HalfSize, centre.Y, centre.Z),
            };

            var bot4 = new Vector3[]
            {
                new Vector3(centre.X - HalfSize, centre.Y - HalfSize, centre.Z - HalfSize),
                new Vector3(centre.X, centre.Y - HalfSize, centre.Z - HalfSize),
                new Vector3(centre.X, centre.Y - HalfSize, centre.Z),
                new Vector3(centre.X - HalfSize, centre.Y - HalfSize, centre.Z),
            };

            var b4 = new BoundingVolume(top4, bot4);
            Children[4] = new OcTreeItem(b4) { Number = 4 };
        }

        private void CreateChild5(Vector3 centre, float HalfSize)
        {
            var top0 = new Vector3[]
            {
                new Vector3(centre.X, centre.Y, centre.Z - HalfSize),
                new Vector3(centre.X + HalfSize, centre.Y, centre.Z - HalfSize),
                new Vector3(centre.X + HalfSize, centre.Y, centre.Z),
                new Vector3(centre.X, centre.Y, centre.Z),
            };

            var bottom = new Vector3[]
            {
                new Vector3(centre.X, centre.Y - HalfSize, centre.Z - HalfSize),
                new Vector3(centre.X + HalfSize, centre.Y - HalfSize, centre.Z - HalfSize),
                new Vector3(centre.X + HalfSize, centre.Y - HalfSize, centre.Z),
                new Vector3(centre.X, centre.Y - HalfSize, centre.Z),
            };

            var b0 = new BoundingVolume(top0, bottom);
            Children[5] = new OcTreeItem(b0) { Number = 5 };
        }

        private void CreateChild6(Vector3 centre, float HalfSize)
        {
            var top0 = new Vector3[]
            {
                new Vector3(centre.X, centre.Y, centre.Z),
                new Vector3(centre.X + HalfSize, centre.Y, centre.Z),
                new Vector3(centre.X + HalfSize, centre.Y, centre.Z + HalfSize),
                new Vector3(centre.X, centre.Y, centre.Z + HalfSize),
            };

            var bottom = new Vector3[]
            {
                new Vector3(centre.X, centre.Y - HalfSize, centre.Z),
                new Vector3(centre.X + HalfSize, centre.Y - HalfSize, centre.Z),
                new Vector3(centre.X + HalfSize, centre.Y - HalfSize, centre.Z + HalfSize),
                new Vector3(centre.X, centre.Y - HalfSize, centre.Z + HalfSize)
            };

            var b0 = new BoundingVolume(top0, bottom);
            Children[6] = new OcTreeItem(b0) { Number = 6 };
        }

        private void CreateChild7(Vector3 centre, float HalfSize)
        {
            var top0 = new Vector3[]
            {

                 new Vector3(centre.X - HalfSize, centre.Y, centre.Z),
                new Vector3(centre.X, centre.Y, centre.Z),
                new Vector3(centre.X, centre.Y, centre.Z + HalfSize),
                new Vector3(centre.X - HalfSize, centre.Y, centre.Z + HalfSize),


            };

            var bottom = new Vector3[]
            {
                 new Vector3(centre.X - HalfSize, centre.Y - HalfSize, centre.Z),
                new Vector3(centre.X, centre.Y - HalfSize, centre.Z),
                new Vector3(centre.X, centre.Y - HalfSize, centre.Z + HalfSize),
                new Vector3(centre.X - HalfSize, centre.Y - HalfSize, centre.Z + HalfSize),
            };

            var b0 = new BoundingVolume(top0, bottom);
            Children[7] = new OcTreeItem(b0) { Number = 7 };
        }



    }
}
