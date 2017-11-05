using System;
using Common;
using Common.Input;
using OpenTK;

namespace DeferredRender
{
    public class Player
    {

        public Player()
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

        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }

        public float AngleHorizontalRadians = 0;
        public float AngleVerticalRadians = 0;

        #endregion

        #region moves

        public void Handle(InputSignal signal)
        {
            switch (signal)
            {
                case InputSignal.FORWARD:
                    StepXZ(StepForward);
                    break;
                case InputSignal.BACK:
                    StepXZ(StepBack);
                    break;
                case InputSignal.RIGHT:
                    StepXZ(StepRight);
                    break;
                case InputSignal.LEFT:
                    StepXZ(StepLeft);
                    break;
                default:
                    break;
            }
        }

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

        #endregion

        protected virtual void Rotate()
        {
            Matrix4 rotVertical = Matrix4.CreateRotationZ(AngleVerticalRadians);
            Matrix4 rotHorizontal = Matrix4.CreateRotationY(AngleHorizontalRadians);

            var rotationResulting = rotVertical * rotHorizontal;

            var targetTransformed = Vector3.Transform(DefaultTarget, rotationResulting);
            Target = Position + targetTransformed;
        }

        protected virtual void StepYZ(Vector3 stepDirection)
        {
            Position += stepDirection;
            Target += stepDirection;
        }

        protected virtual void StepXZ(Vector3 stepDirection)
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRadians);
            Vector3 dPosition = Vector3.Transform(stepDirection, rotation);

            Position += dPosition;
            Target += dPosition;
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
