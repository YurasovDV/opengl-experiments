using Common.Input;
using OpenTK;
using SimpleShadows.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleShadows.Core.Models
{
    public class Player
    {
        private int twenty_five = 15;

        public const int MOUSE_SPEED = 4;

        private const float TARGET_DISTANCE = 100;
        private const int DEFAULT_ROTATION = 3;
        private const float Speed = 0.4f;

        private readonly Vector3 STEP_FORWARD_VECTOR = new Vector3(0, 0, Speed);
        private readonly Vector3 STEP_BACK_VECTOR = new Vector3(0, 0, -Speed);
        private readonly Vector3 STEP_RIGHT_VECTOR = new Vector3(-Speed, 0, 0);
        private readonly Vector3 STEP_LEFT_VECTOR = new Vector3(Speed, 0, 0);
        private readonly Vector3 FLASHLIGHT_DIST = new Vector3(0, 0, 0);


        private readonly Vector3 DEFAULT_POSITION = new Vector3(2f, 10f, 2f);
        private readonly Vector3 DEFAULT_TARGET = new Vector3(0, -1f, 50);

        private const int MIN_CAMERA_MOVE = 0;

        public Predicate<Vector3> intersectionTest { get; set; }


        private Vector3 position;
        private Vector3 target;
        private Vector3 flashlightPosition;



        public Player(Predicate<Vector3> intersectionTest)
        {
            this.intersectionTest = intersectionTest;
            Position = new Vector3(DEFAULT_POSITION);
            Target = new Vector3(DEFAULT_POSITION + DEFAULT_TARGET);
        }

        #region углы

        public virtual int AngleHorizontal { get; set; }
        public virtual int AngleVertical { get; set; }

        public virtual float AngleHorizontalRad
        {
            get
            {
                return MathHelper.DegreesToRadians(AngleHorizontal);
            }
            set
            {
                AngleHorizontal = (int)MathHelper.RadiansToDegrees(value);
            }
        }

        public virtual float AngleVerticalRad
        {
            get
            {
                return MathHelper.DegreesToRadians(AngleVertical);
            }
            set
            {
                AngleVertical = (int)MathHelper.RadiansToDegrees(value);
            }
        }

        /// <summary>
        /// вычислить точку, в которую смотрим, по углу и позиции
        /// </summary>
        private void UpdateTargetPointHorizontal()
        {

            Matrix4 rotation;
            Vector3 transformed = DEFAULT_TARGET;

            rotation = Matrix4.CreateRotationX(AngleVerticalRad);
            transformed = Vector3.TransformPosition(transformed, rotation);

            rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            transformed = Vector3.TransformPosition(transformed, rotation);

            

            target = position + transformed;

            //Debug.WriteLine("{0} ======== {1} ======= {2}", transformed.Y, AngleVertical, AngleHorizontal);
        }

        #endregion

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Target
        {
            get { return target; }
            set { target = value; }
        }

        public Vector3 FlashlightPosition
        {
            get { return flashlightPosition; }
            set { flashlightPosition = value; }
        }


        public void Tick(long delta, Vector2 mouseDxDy)
        {

           var dx = mouseDxDy.X * delta;

            if (dx > float.Epsilon || dx < -float.Epsilon)
            {
                RotateAroundY(mouseDxDy.X);
            }

            var dy = mouseDxDy.Y * delta;
            if (dy > float.Epsilon || dy < - float.Epsilon)
            {
                RotateAroundX(mouseDxDy.Y);
            }

            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var deltaFlash = Vector3.Transform(FLASHLIGHT_DIST, rotation);

            rotation = Matrix4.CreateRotationX(AngleVerticalRad);
            //deltaFlash = Vector3.Transform(deltaFlash, rotation);

            FlashlightPosition = new Vector3(Position) + deltaFlash;
        }

        public void OnSignal(InputSignal sig)
        {
            //float sin = MathHelperMINE.Sin[AngleHorizontal];
            //float cos = MathHelperMINE.Cos[AngleHorizontal];

            switch (sig)
            {
                case InputSignal.FORWARD:
                    MoveForward();
                    break;
                case InputSignal.BACK:
                    MoveBack();
                    break;
                case InputSignal.RIGHT:
                    MoveRight();
                    break;
                case InputSignal.LEFT:
                    MoveLeft();
                    break;
                case InputSignal.UP:
                    break;
                case InputSignal.DOWN:
                    break;
                case InputSignal.ROTATE_CLOCKWISE:
                    RotateClockWise();
                    break;
                case InputSignal.ROTATE_COUNTERCLOCKWISE:
                    RotateCounterClock();
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
                    position.Y -= Speed;
                    target.Y -= Speed;
                    break;
                case InputSignal.UP_PARALLEL:
                    position.Y += Speed;
                    target.Y += Speed;
                    break;
                default:
                    break;
            }
        }

        private void RotateAroundY(float mouseDx = 200)
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


        private void RotateAroundX(float mouseDy = 200)
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
         //   if (AngleHorizontal > 80)
         //   {
               // Debug.WriteLine(AngleVertical);
          //  }
            UpdateTargetPointHorizontal();
        }

        private void RotateCounterClock(float mouseDx = 200)
        {
            int rotation = -DEFAULT_ROTATION;
            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, rotation);
            UpdateTargetPointHorizontal();
        }

        private void RotateClockWise(float mouseDx = 200)
        {
            int rotation = DEFAULT_ROTATION;
            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, rotation);
            UpdateTargetPointHorizontal();
        }

        private void MoveForward()
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var position1 = position + Vector3.Transform(STEP_FORWARD_VECTOR, rotation);

            if (intersectionTest(position1))
            {
                position = position1;
            }
            UpdateTargetPointHorizontal();
            // target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }

        private void MoveBack()
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var position1 = position + Vector3.Transform(STEP_BACK_VECTOR, rotation);
            if (intersectionTest(position1))
            {
                position = position1;
            }

            UpdateTargetPointHorizontal(); 

            // target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }

        private void MoveLeft()
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var position1 = position + Vector3.Transform(STEP_LEFT_VECTOR, rotation);
            if (intersectionTest(position1))
            {
                position = position1;
            }
            UpdateTargetPointHorizontal();
            //target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }

        private void MoveRight()
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var position1 = position + Vector3.Transform(STEP_RIGHT_VECTOR, rotation);
            if (intersectionTest(position1))
            {
                position = position1;
            }
            UpdateTargetPointHorizontal();
            //target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }
    }
}
