using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Common;
using Common.Input;
using OpenTK;
using Common.Utils;

namespace OcTreeExample
{
    class Player : AbstractPlayer
    {
        private int twenty_five = 15;

        public Player()
            : base(intersectionTest: null)
        {
            DEFAULT_TARGET = new Vector3(0, 0f, -30);
            DEFAULT_POSITION = new Vector3(0.0f, 300f, 0);

            Position = new Vector3(DEFAULT_POSITION);
            Target = new Vector3(DEFAULT_TARGET);

            MIN_CAMERA_MOVE = 1;
            AngleVertical = 90;
            //UpdateTargetPointHorizontal();

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

            rotation = Matrix4.CreateRotationX(AngleVerticalRad);

            //deltaFlash = Vector3.Transform(deltaFlash, rotation);

            FlashlightPosition = new Vector3(Position) + deltaFlash;

        }

        protected override void RotateAroundY(float mouseDx = 200)
        {
            int rotation = 0;

            if (mouseDx > 0)
            {
                rotation = ((int)mouseDx / twenty_five + MIN_CAMERA_MOVE);
            }
            else if (mouseDx < 0)
            {
                rotation = ((int)mouseDx / twenty_five - MIN_CAMERA_MOVE);
            }

            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, rotation);
            UpdateTargetPointHorizontal();
        }


        protected override void RotateAroundX(float mouseDy = 200)
        {
            int rotation = 0;

            if (mouseDy > 0)
            {
                rotation = ((int)mouseDy / twenty_five + MIN_CAMERA_MOVE);
            }
            else if (mouseDy < 0)
            {
                rotation = ((int)mouseDy / twenty_five - MIN_CAMERA_MOVE);
            }

            AngleVertical = MathHelperMINE.AddDegrees(AngleVertical, -rotation);
            UpdateTargetPointHorizontal();
        }
    }
}
