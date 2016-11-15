using Common;
using Common.Utils;
using OpenTK;

namespace SimpleTerrain
{
    class Player : AbstractPlayer
    {
        private int twenty_five = 15;

        public Player()
            : base(intersectionTest: null)
        {
            DELTA_BETWEEN_POSITION_AND_TARGET = new Vector3(0, 0, 10f);
            DEFAULT_POSITION = new Vector3(0.0f, 40f, -20);

            Position = new Vector3(DEFAULT_POSITION);
            Target = new Vector3(DELTA_BETWEEN_POSITION_AND_TARGET);

            twenty_five = 15;

            MIN_CAMERA_MOVE = 1;
            AngleVertical = 0;
            UpdateTargetPointHorizontal();

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
