using OpenTK;
using System.Collections.Generic;
using Common.Geometry;
using System;

namespace OcTreeLibrary
{
    public class OcTreeItem
    {
        public OcTreeItem(BoundingVolume volume)
        {
            Volume = volume;
            Objects = new List<IOctreeItem>();

            Children = new OcTreeItem[8];

            HalfSize = volume.Width * 0.5f;
        }

        public int Number { get; set; }

        public float HalfSize { get; set; }

        public List<IOctreeItem> Objects { get; set; }

        public OcTreeItem[] Children { get; set; }

        public BoundingVolume Volume { get; set; }

        public int Level { get; internal set; }

        public int InsertedObjectsCount { get; set; }

        public bool IsLeaf => Children[0] == null;

        public void AddObject(IOctreeItem obj, bool reinsertIfMoves = false)
        {
            Objects.Add(obj);
            obj.ReInsertImmediately = reinsertIfMoves;
        }

        public BoundingVolume Insert(IOctreeItem dataToInsert)
        {
            BoundingVolume insertedWhere = null;
            if (Volume.Contains(dataToInsert.BoundingBox))
            {
                var maxDimensionObject = dataToInsert.BoundingBox.MaxDimension;
                bool forceSetToChildren = Volume.MaxDimension > maxDimensionObject * 10;

                if (IsLeaf && !forceSetToChildren)
                {
                    AddObject(dataToInsert, reinsertIfMoves:false);
                    insertedWhere = Volume;
                }
                else
                {
                    AssureChildrenInitialized();
                    foreach (var c in Children)
                    {
                        insertedWhere = c.Insert(dataToInsert);
                        if (insertedWhere != null)
                        {
                            InsertedObjectsCount++;
                            break;
                        }
                    }

                    if (insertedWhere == null)
                    {
                        AddObject(dataToInsert, reinsertIfMoves: true);
                        insertedWhere = Volume;
                    }
                }
            }

            dataToInsert.TreeSegment = insertedWhere;
            return insertedWhere;
        }

        /// <summary>
        /// returns count of deleted
        /// </summary>
        /// <param name="dataToRemove"></param>
        public int Remove(IOctreeItem dataToRemove)
        {
            int result = 0;
            var inside = Volume.Contains(dataToRemove.BoundingBox);
            if (inside)
            {
                if (Objects.Remove(dataToRemove))
                {
                    dataToRemove.TreeSegment = null;
                    result = 1;
                }
                else
                {
                    var child = Children.FindVolume(dataToRemove);
                    if (child != null)
                    {
                        result = child.Remove(dataToRemove);
                        InsertedObjectsCount -= result;
                    }
                    //TryClearChildren();
                }
            }

            return result;
        }

        public void EnumeratePossibleCollision(IOctreeItem dataToCheck, List<IOctreeItem> result)
        {
            if (Volume.Contains(dataToCheck.BoundingBox))
            {
                result.AddRange(Objects);
                if (!IsLeaf)
                {
                    foreach (var c in Children)
                    {
                        c.EnumeratePossibleCollision(dataToCheck, result);
                    }
                }
            }
        }

        private void TryClearChildren()
        {
            if (InsertedObjectsCount == 0 && Level > 20)
            {
                Children = new OcTreeItem[8];
            }
        }

        private void AssureChildrenInitialized()
        {
            if (Children[0] == null)
            {
                var parentCentre = Volume.Centre;

                var h = HalfSize;

                CreateChild0(parentCentre, h);
                CreateChild1(parentCentre, h);
                CreateChild2(parentCentre, h);
                CreateChild3(parentCentre, h);

                CreateChild4(parentCentre, h);
                CreateChild5(parentCentre, h);
                CreateChild6(parentCentre, h);
                CreateChild7(parentCentre, h);

                foreach (var child in Children)
                {
                    child.Level = Level + 1;
                }
            }
        }

        #region init of children

        private void CreateChild0(Vector3 parentCentre, float halfSize)
        {
            var bottom = new Vector3(parentCentre.X - halfSize, parentCentre.Y, parentCentre.Z - halfSize);
            var top = new Vector3(parentCentre.X, parentCentre.Y + halfSize, parentCentre.Z);
            var b0 = new BoundingVolume(bottom, top);
            Children[0] = new OcTreeItem(b0) { Number = 0 };
        }

        private void CreateChild1(Vector3 centre, float halfSize)
        {
            var bottom = new Vector3(centre.X, centre.Y, centre.Z - halfSize);
            var top = new Vector3(centre.X + halfSize, centre.Y + halfSize, centre.Z);
            var b0 = new BoundingVolume(bottom, top);
            Children[1] = new OcTreeItem(b0) { Number = 1 };
        }

        private void CreateChild2(Vector3 centre, float halfSize)
        {
            var bottom = new Vector3(centre.X, centre.Y, centre.Z);
            var top = new Vector3(centre.X + halfSize, centre.Y + halfSize, centre.Z + halfSize);
            var b0 = new BoundingVolume(bottom, top);
            Children[2] = new OcTreeItem(b0) { Number = 2 };
        }

        private void CreateChild3(Vector3 centre, float halfSize)
        {
            var bottom = new Vector3(centre.X - halfSize, centre.Y, centre.Z);
            var top = new Vector3(centre.X, centre.Y + halfSize, centre.Z + halfSize);
            var b0 = new BoundingVolume(bottom, top);
            Children[3] = new OcTreeItem(b0) { Number = 3 };
        }

        private void CreateChild4(Vector3 centre, float HalfSize)
        {
            var bottom = new Vector3(centre.X - HalfSize, centre.Y - HalfSize, centre.Z - HalfSize);
            var top = new Vector3(centre.X, centre.Y, centre.Z);
            var b4 = new BoundingVolume(bottom, top);
            Children[4] = new OcTreeItem(b4) { Number = 4 };
        }

        private void CreateChild5(Vector3 centre, float HalfSize)
        {
            var bottom = new Vector3(centre.X, centre.Y - HalfSize, centre.Z - HalfSize);
            var top = new Vector3(centre.X + HalfSize, centre.Y, centre.Z);
            var b0 = new BoundingVolume(bottom, top);
            Children[5] = new OcTreeItem(b0) { Number = 5 };
        }

        private void CreateChild6(Vector3 centre, float HalfSize)
        {
            var bottom = new Vector3(centre.X, centre.Y - HalfSize, centre.Z);
            var top = new Vector3(centre.X + HalfSize, centre.Y, centre.Z + HalfSize);
            var b0 = new BoundingVolume(bottom, top);
            Children[6] = new OcTreeItem(b0) { Number = 6 };
        }

        private void CreateChild7(Vector3 centre, float HalfSize)
        {
            var bottom = new Vector3(centre.X - HalfSize, centre.Y - HalfSize, centre.Z);
            var top = new Vector3(centre.X, centre.Y, centre.Z + HalfSize);
            var b0 = new BoundingVolume(bottom, top);
            Children[7] = new OcTreeItem(b0) { Number = 7 };
        }

        #endregion


    }
}
