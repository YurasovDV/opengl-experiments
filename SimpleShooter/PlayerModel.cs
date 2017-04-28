using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Input;
using OpenTK;

namespace SimpleShooter
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

        private float AngleHorizontal = 0;
        private float AngleVertical = 0;


        public void Handle(Vector2 mouseDxDy, Camera camera)
        {
            var dx = mouseDxDy.X;
            if (dx > 1 || dx < -1)
            {
                RotateAroundY(camera, mouseDxDy.X);
            }

            var dy = mouseDxDy.Y;
            if (dy > 1 || dy < -1)
            {
                RotateAroundX(camera, mouseDxDy.Y);
            }
        }


        protected void RotateAroundY(Camera camera, float mouseDx)
        {
            float rotation = mouseDx / mouseHandicap;
            AngleHorizontal += rotation;
            Rotate(camera, camera.Position);
        }

        protected void RotateAroundX(Camera camera, float mouseDy)
        {
            float rotation = mouseDy / mouseHandicap;
            AngleVertical += rotation;
            Rotate(camera, camera.Position);
        }

        public void Handle(InputSignal signal, Camera camera)
        {
            var oldPosition = camera.Position;
            var oldTarget = camera.Target;

            Matrix4 translation;
            Matrix4 rot;
            Matrix4 rot2;

            Vector3 targetTransformed;
            Vector3 dTarget;
            Vector3 dPosition;

            switch (signal)
            {
                case InputSignal.FORWARD:
                    StepXZ(StepForward, camera);

                    break;
                case InputSignal.BACK:
                    StepXZ(StepBack, camera);
                    break;
                case InputSignal.RIGHT:
                    StepXZ(StepRight, camera);
                    break;
                case InputSignal.LEFT:
                    StepXZ(StepLeft, camera);
                    break;
                case InputSignal.UP:

                    AngleVertical -= 0.01f;
                    Rotate(camera, oldPosition);

                    break;
                case InputSignal.DOWN:
                    AngleVertical += 0.01f;
                    Rotate(camera, oldPosition);

                    break;
                case InputSignal.ROTATE_CLOCKWISE:
                    AngleHorizontal += 0.01f;
                    Rotate(camera, oldPosition);
                    break;
                case InputSignal.ROTATE_COUNTERCLOCKWISE:
                    AngleHorizontal -= 0.01f;
                    Rotate(camera, oldPosition);
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
                    StepYZ(StepDown, camera);
                    break;
                case InputSignal.UP_PARALLEL:
                    StepYZ(StepUp, camera);
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

        private void Rotate(Camera camera, Vector3 oldPosition)
        {
            Matrix4 rotVertical = Matrix4.CreateRotationZ(AngleVertical);
            Matrix4 rotHorizontal = Matrix4.CreateRotationY(AngleHorizontal);
            var targetTransformed = Vector3.Transform(DefaultTarget, rotVertical * rotHorizontal);
            camera.Target = oldPosition + targetTransformed;
        }

        private void StepYZ(Vector3 stepDirection, Camera camera)
        {
            camera.Position += stepDirection;
            camera.Target += stepDirection;
        }

        private void StepXZ(Vector3 stepDirection, Camera camera)
        {
            var rot = Matrix4.CreateRotationY(AngleHorizontal);

            Vector3 dPosition = Vector3.Transform(stepDirection, rot);
            Vector3 dTarget = Vector3.Transform(stepDirection, rot);

            camera.Position += dPosition;
            camera.Target += dTarget;
        }
    }
}
