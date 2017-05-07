using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Geometry;
using OpenTK;
using SimpleShooter.Graphics;

namespace SimpleShooter.Core
{
    class MovableObject : GameObject, IMovableObject
    {
        private Vector3 Acceleration { get; set; }
        private Vector3 Speed { get; set; }
        private Vector3 Path { get; set; }

        public MovableObject(SimpleModel model, ShadersNeeded shadersNeeded, Vector3 speed, Vector3 acceleration) : base(model, shadersNeeded)
        {
            Speed = speed;
            Acceleration = acceleration;
        }

        public void Tick(long delta)
        {
            // stop and wait for death
            if (OctreeItem.TreeSegment == null)
            {
                return;
            }

            if (delta == 0)
            {
                delta = 1;
            }

            Speed += Acceleration * delta;
            Path = Speed / delta;

            var updatedBox = new BoundingVolume(OctreeItem.BoundingBox.BottomLeftBack + Path, OctreeItem.BoundingBox.TopRightFront + Path);
            if (OctreeItem.ReinsertImmediately || !OctreeItem.TreeSegment.Contains(updatedBox))
            {
                OctreeItem.RaiseRemove();
                Move();
                OctreeItem.RaiseInsert();
            }
            else
            {
                Move();
            }
        }

        private void TranslateAll(Vector3[] vertices, Vector3 transformed)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += transformed;
            }
        }

        public void Move()
        {
            TranslateAll(Model.Vertices, Path);
            TranslateAll(OctreeItem.BoundingBox.VerticesBottom, Path);
            TranslateAll(OctreeItem.BoundingBox.VerticesTop, Path);
            OctreeItem.BoundingBox.BottomLeftBack += Path;
            OctreeItem.BoundingBox.TopRightFront += Path;
            OctreeItem.BoundingBox.Centre += Path;
        }
    }
}
