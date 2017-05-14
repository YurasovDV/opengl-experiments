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

        public MovableObject(SimpleModel model, ShadersNeeded shadersNeeded, Vector3 speed, Vector3 acceleration) : base(model, shadersNeeded)
        {
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
            return path;
        }

        protected void Move(Vector3 path, BoundingVolume updatedPosition)
        {
            if (ReinsertImmediately || !TreeSegment.Contains(updatedPosition))
            {
                RaiseRemove();
                Model.Vertices.TranslateAll(path);
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
            ReinsertImmediately = true;
            _updatedBox.MoveBox(rollback);
            Model.Vertices.TranslateAll(rollback);
            BoundingBox.MoveBox(rollback);
        }
    }
}
