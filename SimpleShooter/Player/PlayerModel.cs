using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Input;
using OpenTK;

namespace SimpleShooter.Player
{
    public class PlayerModel
    {
        static readonly Vector3 DefaultTarget = new Vector3(100, 0, 0);
        static readonly Vector3 StepForward = new Vector3(0.1f, 0, 0);
        static readonly Vector3 StepBack = new Vector3(-0.1f, 0, 0);
        static readonly Vector3 StepRight = new Vector3(0, 0, 0.1f);
        static readonly Vector3 StepLeft = new Vector3(0, 0, -0.1f);
        static readonly Vector3 StepUp = new Vector3(0, 0.1f, 0);
        static readonly Vector3 StepDown = new Vector3(0, -0.1f, 0);
        static float mouseHandicap = 2400;


        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }

        private float AngleHorizontal = 0;
        private float AngleVertical = 0;


        public PlayerModel(Vector3 position, Vector3 target)
        {
            Position = position;
            target = Target;
        }

        public void Handle(Vector2 mouseDxDy)
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


        protected void RotateAroundY(float mouseDx)
        {
            float rotation = mouseDx / mouseHandicap;
            AngleHorizontal += rotation;
            Rotate();
        }

        protected void RotateAroundX(float mouseDy)
        {
            float rotation = mouseDy / mouseHandicap;
            AngleVertical += rotation;
            Rotate();
        }

        public void Handle(InputSignal signal)
        {
            var oldPosition = Position;
            var oldTarget = Target;

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
                case InputSignal.UP:

                    AngleVertical -= 0.01f;
                    Rotate();

                    break;
                case InputSignal.DOWN:
                    AngleVertical += 0.01f;
                    Rotate();

                    break;
                case InputSignal.ROTATE_CLOCKWISE:
                    AngleHorizontal += 0.01f;
                    Rotate();
                    break;
                case InputSignal.ROTATE_COUNTERCLOCKWISE:
                    AngleHorizontal -= 0.01f;
                    Rotate();
                    break;
                case InputSignal.SHOT:
                    break;
                case InputSignal.LOOK_UP:
                    break;
                case InputSignal.LOOK_DOWN:
                    break;
                case InputSignal.FLY_BY_CLOCKWISE:
                    break;
                case InputSignal.FLY_BY_COUNTERCLOCKWISE:
                    break;
                case InputSignal.NONE:
                    break;
                case InputSignal.DOWN_PARALLEL:
                    StepYZ(StepDown);
                    break;
                case InputSignal.UP_PARALLEL:
                    StepYZ(StepUp);
                    break;
                case InputSignal.MOUSE_CLICK:
                    break;
                case InputSignal.RENDER_MODE:
                    break;
                case InputSignal.CHANGE_CAMERA:
                    break;
                default:
                    break;
            }
        }

        private void Rotate()
        {
            Matrix4 rotVertical = Matrix4.CreateRotationZ(AngleVertical);
            Matrix4 rotHorizontal = Matrix4.CreateRotationY(AngleHorizontal);
            var targetTransformed = Vector3.Transform(DefaultTarget, rotVertical * rotHorizontal);
            Target = Position + targetTransformed;
        }

        private void StepYZ(Vector3 stepDirection)
        {
            Position += stepDirection;
            Target += stepDirection;
        }

        private void StepXZ(Vector3 stepDirection)
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontal);

            Vector3 dPosition = Vector3.Transform(stepDirection, rotation);
            Vector3 dTarget = Vector3.Transform(stepDirection, rotation);

            Position += dPosition;
            Target += dTarget;
        }
    }
}
