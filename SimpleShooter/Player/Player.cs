using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Input;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Player.Events;

namespace SimpleShooter.Player
{
    public abstract class Player : IShooterPlayer
    {
        public Vector3 DefaultTarget = new Vector3(100, 0, 0);
        protected Vector3 StepForward = new Vector3(0.1f, 0, 0);
        protected Vector3 StepBack = new Vector3(-0.1f, 0, 0);
        protected Vector3 StepRight = new Vector3(0, 0, 0.1f);
        protected Vector3 StepLeft = new Vector3(0, 0, -0.1f);
        protected Vector3 StepUp = new Vector3(0, 0.1f, 0);
        protected Vector3 StepDown = new Vector3(0, -0.1f, 0);

        protected float mouseHandicap = 2400;

        #region state
        public GameObject Mark { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
        public float AngleHorizontalRadians = 0;
        public float AngleVerticalRadians = 0;

        #endregion

        #region engine callbacks



        public event PlayerActionHandler<ShotEventArgs> Shot;
        public event PlayerActionHandler<JumpEventArgs> Jump;
        public event PlayerActionHandler<MoveEventArgs> Move;

        protected virtual ActionStatus OnShot(ShotEventArgs args)
        {
            var result = new ActionStatus()
            {
                Success = true
            };

            if (Shot != null)
            {
                result = Shot(this, args);
            }

            return result;
        }

        protected virtual ActionStatus OnJump(JumpEventArgs args)
        {
            var result = new ActionStatus()
            {
                Success = true
            };

            if (Jump != null)
            {
                result = Jump(this, args);
            }

            return result;
        }

        protected virtual ActionStatus OnMove(MoveEventArgs args)
        {
            var result = new ActionStatus()
            {
                Success = true
            };

            if (Move != null)
            {
                result = Move(this, args);
            }

            return result;
        }

        #endregion

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

            for (int i = 0; i < Mark.Model.Vertices.Length; i++)
            {
                Mark.Model.Vertices[i] += stepDirection;
            }

        }

        protected virtual void StepXZ(Vector3 stepDirection)
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRadians);
            Vector3 dPosition = Vector3.Transform(stepDirection, rotation);

            var args = new MoveEventArgs()
            {
                Position = Position + dPosition,
                Target = Target + dPosition
            };

            var actionResult = OnMove(args);
            if (actionResult.Success)
            {
                Position += dPosition;
                Target += dPosition;

                for (int i = 0; i < Mark.Model.Vertices.Length; i++)
                {
                    Mark.Model.Vertices[i] += dPosition;
                }
            }
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
