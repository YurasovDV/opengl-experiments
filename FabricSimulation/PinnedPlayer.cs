using System;
using System.Diagnostics;
using Common;
using Common.Input;
using Common.Utils;
using OpenTK;

namespace FabricSimulation
{
    class PinnedPlayer : AbstractPlayer
    {
        private readonly int _twentyFive;

        public PinnedPlayer()
          : base(intersectionTest: null)
        {
            //DEFAULT_TARGET = new Vector3(0, 0, 10f);
            //DEFAULT_POSITION = new Vector3(0.0f, 0f, -40);
            DELTA_BETWEEN_POSITION_AND_TARGET = new Vector3(0, 0, -30f);
            DEFAULT_POSITION = new Vector3(0.0f, 0f, 30);

            Position = new Vector3(DEFAULT_POSITION);

            _twentyFive = 15;
            MIN_CAMERA_MOVE = 1;
            UpdateTargetPointHorizontal();

        }

        public override void OnSignal(InputSignal sig)
        {
            //base.OnSignal(sig);
            if (sig == InputSignal.MOUSE_CLICK)
            {
                //Debug.WriteLine("click");
            }
        }

        public override void Tick(long delta, Vector2 mouseDxDy)
        {
            var dx = mouseDxDy.X * delta;

            if (dx > float.Epsilon || dx < -float.Epsilon)
            {
                RotateAroundY(mouseDxDy.X);
            }

            var dy = mouseDxDy.Y * delta;
            if (dy > float.Epsilon || dy < -float.Epsilon)
            {
                RotateAroundX(mouseDxDy.Y);
            }

            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var deltaFlash = Vector3.Transform(FLASHLIGHT_DIST, rotation);

            // rotation = Matrix4.CreateRotationX(AngleVerticalRad);

            //deltaFlash = Vector3.Transform(deltaFlash, rotation);

            FlashlightPosition = new Vector3(Position) + deltaFlash;

        }

        protected override void RotateAroundY(float mouseDx = 200)
        {
            var rotation = 0;

            if (mouseDx > 0)
            {
                rotation = ((int)mouseDx / _twentyFive + MIN_CAMERA_MOVE);
            }
            else if (mouseDx < 0)
            {
                rotation = ((int)mouseDx / _twentyFive - MIN_CAMERA_MOVE);
            }

            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, rotation);
            UpdateTargetPointHorizontal();
        }


        protected override void RotateAroundX(float mouseDy = 200)
        {
            var rotation = 0;

            if (mouseDy > 0)
            {
                rotation = ((int)mouseDy / _twentyFive + MIN_CAMERA_MOVE);
            }
            else if (mouseDy < 0)
            {
                rotation = ((int)mouseDy / _twentyFive - MIN_CAMERA_MOVE);
            }

            AngleVertical = MathHelperMINE.AddDegrees(AngleVertical, -rotation);
            UpdateTargetPointHorizontal();
        }
    }

}