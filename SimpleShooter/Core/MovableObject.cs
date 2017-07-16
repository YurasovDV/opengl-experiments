using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Geometry;
using Common.Utils;
using OpenTK;
using SimpleShooter.Graphics;

namespace SimpleShooter.Core
{
    public class MovableObject : GameObject, IMovableObject
    {
        public Vector3 Acceleration { get; set; }
        public Vector3 Speed { get; set; }
        protected BoundingVolume _updatedBox;

        public float Mass { get { return _mass; } }

        protected float _mass;

        public MovableObject(SimpleModel model, ShadersNeeded shadersNeeded, Vector3 speed, Vector3 acceleration, float mass = 0) : base(model, shadersNeeded)
        {
            _mass = mass;
            Speed = speed;
            Acceleration = acceleration;
            _updatedBox = BoundingVolume.InitBoundingBox(Model.Vertices);
        }

        public virtual Vector3 Tick(long delta)
        {
            // stop and wait for death
            if (TreeSegment == null)
            {
                return Vector3.Zero;
            }

            if (delta == 0)
            {
                delta = 1;
            }

            Speed += Acceleration * delta;
            var path = Speed / delta;

            _updatedBox.MoveBox(path);
            Move(path, _updatedBox);
            Acceleration = Vector3.Zero;
            return path;
        }

        protected void Move(Vector3 path, BoundingVolume updatedPosition)
        {
            bool outsideOfSegment = TreeSegment == null || !TreeSegment.Contains(updatedPosition);
            if (ReInsertImmediately || outsideOfSegment)
            {
                RaiseRemove();
                GeometryHelper.TranslateAll(Model.Vertices, path);
                BoundingBox.MoveBox(path);
                RaiseInsert();
            }
            else
            {
                Model.Vertices.TranslateAll(path);
                BoundingBox.MoveBox(path);
            }
        }

        public virtual void MoveAfterCollision(Vector3 rollback)
        {
            ReInsertImmediately = true;
            _updatedBox.MoveBox(rollback);
            Model.Vertices.TranslateAll(rollback);
            BoundingBox.MoveBox(rollback);
        }
    }
}
