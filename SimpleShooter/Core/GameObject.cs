using System;
using Common;
using Common.Geometry;
using OcTreeLibrary;
using OpenTK;
using SimpleShooter.Graphics;

namespace SimpleShooter.Core
{
    public class GameObject : IOctreeItem
    {
        public long Id { get; set; }

        public SimpleModel Model { get; set; }

        public ShadersNeeded ShaderKind { get; set; }


        public GameObject(SimpleModel model, ShadersNeeded shadersNeeded)
        {
            ShaderKind = shadersNeeded;
            Model = model;
            BoundingBox = BoundingVolume.InitBoundingBox(Model.Vertices);
        }

        public void CalcNormals()
        {
            Model.Normals = GetNormals(Model.Vertices);
        }

        private Vector3[] GetNormals(Vector3[] points)
        {
            var normals = new Vector3[points.Length];
            var tempVertices = new Vector3[3];

            for (int i = 0; i < points.Length; i += 6)
            {
                tempVertices[0] = points[i];
                tempVertices[1] = points[i + 1];
                tempVertices[2] = points[i + 2];

                var norm = CalcNormal(tempVertices);

                for (int j = i; j < i + 6; j++)
                {
                    normals[j] = norm;
                }
            }

            return normals;
        }

        public static Vector3 CalcNormal(Vector3[] vrt)
        {
            var n = Vector3.Cross(vrt[0] - vrt[2], vrt[0] - vrt[1]);
            n.Normalize();
            return n;
        }

        #region IOctreeItem

        public BoundingVolume BoundingBox { get; set; }

        public BoundingVolume TreeSegment { get; set; }

        public bool ReinsertImmediately { get; set; }

        public event EventHandler<ReinsertingEventArgs> NeedsRemoval;
        public event EventHandler<ReinsertingEventArgs> NeedsInsert;


        public void RaiseRemove()
        {
            if (NeedsRemoval != null)
            {
                NeedsRemoval(this, new ReinsertingEventArgs());
            }
        }

        public void RaiseInsert()
        {
            if (NeedsInsert != null)
            {
                NeedsInsert(this, new ReinsertingEventArgs());
            }
        }

        #endregion
    }
}