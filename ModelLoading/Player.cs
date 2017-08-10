using System;
using Common;
using OpenTK;

namespace ModelLoading
{
    class Player : AbstractPlayer
    {
       // public Predicate<Vector3> intersectionTest { get; set; }


        private Vector3 position;
        private Vector3 target;
        private Vector3 flashlightPosition;

        public Player(Predicate<Vector3> intersectionTest = null)
        {
            DELTA_BETWEEN_POSITION_AND_TARGET = new Vector3(0, -1f, 50);
            DEFAULT_POSITION = new Vector3(2f, 5f, -2f);
            //this.intersectionTest = intersectionTest;
            Position = new Vector3(DEFAULT_POSITION);
            Target = new Vector3(DEFAULT_POSITION + DELTA_BETWEEN_POSITION_AND_TARGET);
            MIN_CAMERA_MOVE = 1;
        }

       

        public override Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public override Vector3 Target
        {
            get { return target; }
            set { target = value; }
        }

        public override Vector3 FlashlightPosition
        {
            get { return flashlightPosition; }
            set { flashlightPosition = value; }
        }


        public override void Tick(long delta, Vector2 mouseDxDy)
        {

            var dx = mouseDxDy.X * 0.01;

            if (dx > float.Epsilon || dx < -float.Epsilon)
            {
                RotateAroundY(mouseDxDy.X);
            }

            var dy = mouseDxDy.Y * 0.01;
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
    }
}
