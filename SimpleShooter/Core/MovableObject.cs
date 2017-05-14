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
        private BoundingVolume _updatedBox;

        public MovableObject(SimpleModel model, ShadersNeeded shadersNeeded, Vector3 speed, Vector3 acceleration) : base(model, shadersNeeded)
        {
            Speed = speed;
            Acceleration = acceleration;
            _updatedBox = BoundingVolume.InitBoundingBox(Model.Vertices);
        }

        public virtual void Tick(long delta)
        {
            // stop and wait for death
            if (TreeSegment == null)
            {
                return;
            }

            if (delta == 0)
            {
                delta = 1;
            }

            Speed += Acceleration * delta;
            var path = Speed / delta;

            _updatedBox.MoveBox(path);
            if (ReinsertImmediately || !TreeSegment.Contains(_updatedBox))
            {
                RaiseRemove();
                Move(path);
                RaiseInsert();
            }
            else
            {
                Move(path);
            }
        }

        protected virtual void Move(Vector3 Path)
        {
            GeometryHelper.TranslateAll(Model.Vertices, Path);
            BoundingBox.MoveBox(Path);
        }

        public virtual void MoveAfterCollision(Vector3 rollback)
        {
            ReinsertImmediately = true;
            _updatedBox.MoveBox(rollback);
            Move(rollback);
        }
    }
}
