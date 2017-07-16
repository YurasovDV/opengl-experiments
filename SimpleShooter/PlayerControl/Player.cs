using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Geometry;
using Common.Input;
using Common.Utils;
using OcTreeLibrary;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.PlayerControl.Events;

namespace SimpleShooter.PlayerControl
{
    public abstract class Player : MovableObject, IShooterPlayer
    {

        public Player(SimpleModel model, float mass) : base(model, Graphics.ShadersNeeded.Line, Vector3.Zero, Vector3.Zero, mass)
        {

        }

        const float stepLen = 0.4f;

        public Vector3 DefaultTarget = new Vector3(100, 0, 0);
        protected Vector3 StepForward = new Vector3(stepLen, 0, 0);
        protected Vector3 StepBack = new Vector3(-stepLen, 0, 0);
        protected Vector3 StepRight = new Vector3(0, 0, stepLen);
        protected Vector3 StepLeft = new Vector3(0, 0, -stepLen);
        protected Vector3 StepUp = new Vector3(0, stepLen, 0);
        protected Vector3 StepDown = new Vector3(0, -stepLen, 0);

        protected float mouseHandicap = 2400;

        #region state
        public GameObject Mark { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
        public float AngleHorizontalRadians = 0;
        public float AngleVerticalRadians = 0;

        protected long shotCoolDown = 0;
        protected long shotCoolDownDefault = 1000;

        #endregion

        #region engine callbacks

        public event PlayerActionHandler<ShotEventArgs> Shot;

        protected virtual ActionStatus OnShot(ShotEventArgs args)
        {
            var result = new ActionStatus()
            {
                Success = true
            };

            if (Shot != null && shotCoolDown <= 0)
            {
                shotCoolDown = shotCoolDownDefault;
                result = Shot(this, args);
            }

            return result;
        }

        #endregion

        #region moves

        public abstract void Handle(InputSignal signal);

        public void HandleMouseMove(Vector2 mouseDxDy)
        {
            var dx = mouseDxDy.X;
            if (dx > 1 || dx < -1)
            {
                RotateAroundY(mouseDxDy.X);
            }

            var dy = mouseDxDy.Y;
            if (dy > 1 || dy < -1)
            {
                RotateAroundX(mouseDxDy.Y);
            }
        }

        public override Vector3 Tick(long delta)
        {
            if (delta == 0)
            {
                delta = 1;
            }

            shotCoolDown -= delta;

            var path = base.Tick(delta);

            Mark.Model.Vertices.TranslateAll(path);
            Position += path;
            Target += path;

            Acceleration = Vector3.Zero;

            if (Position.Y < 1)
            {
                Speed = Vector3.Zero;
            }
            return path;
        }

        #endregion

        protected virtual void Rotate()
        {
            Matrix4 rotVertical = Matrix4.CreateRotationZ(AngleVerticalRadians);
            Matrix4 rotHorizontal = Matrix4.CreateRotationY(AngleHorizontalRadians);

            var rotationResulting = rotVertical * rotHorizontal;

            var targetTransformed = Vector3.Transform(DefaultTarget, rotationResulting);
            Target = Position + targetTransformed;

            if (Mark != null)
            {
                MarkController.SetTo(this, Mark, rotationResulting);
            }
        }

        protected virtual void StepYZ(Vector3 stepDirection)
        {
            Position += stepDirection;
            Target += stepDirection;

            _updatedBox.MoveBox(stepDirection);
            Move(stepDirection, _updatedBox);

            Mark.Model.Vertices.TranslateAll(stepDirection);
        }

        protected virtual void StepXZ(Vector3 stepDirection)
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRadians);
            Vector3 dPosition = Vector3.Transform(stepDirection, rotation);

            Position += dPosition;
            Target += dPosition;

            _updatedBox.MoveBox(dPosition);
            Move(dPosition, _updatedBox);

            Mark.Model.Vertices.TranslateAll(dPosition);
        }

        protected virtual void RotateAroundY(float mouseDx)
        {
            float rotation = mouseDx / mouseHandicap;
            AngleHorizontalRadians += rotation;
            Rotate();
        }

        protected virtual void RotateAroundX(float mouseDy)
        {
            float rotation = mouseDy / mouseHandicap;
            AngleVerticalRadians += rotation;
            Rotate();
        }
    }
}
