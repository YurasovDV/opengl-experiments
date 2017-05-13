using Common.Input;
using Common.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class AbstractPlayer : IPlayer
    {
        /// <summary>
        /// тест пересечения
        /// </summary>
        public Predicate<Vector3> intersectionTest { get; set; }


        public const float Speed = 0.4f;
        protected readonly Vector3 STEP_FORWARD_VECTOR = new Vector3(0, 0, Speed);
        protected readonly Vector3 STEP_BACK_VECTOR = new Vector3(0, 0, -Speed);
        protected readonly Vector3 STEP_RIGHT_VECTOR = new Vector3(-Speed, 0, 0);
        protected readonly Vector3 STEP_LEFT_VECTOR = new Vector3(Speed, 0, 0);
        protected readonly Vector3 FLASHLIGHT_DIST = new Vector3(0, 0, 0);

        protected int MIN_CAMERA_MOVE = 1;

        protected int DEFAULT_ROTATION = 3;

        public virtual Vector3 DELTA_BETWEEN_POSITION_AND_TARGET { get; set; }
        public virtual Vector3 DEFAULT_POSITION { get; set; }

        public virtual Vector3 FlashlightPosition { get; set; }
        public virtual Vector3 Position { get; set; }
        public virtual Vector3 Target { get; set; }


        public AbstractPlayer(Predicate<Vector3> intersectionTest = null)
        {
            this.intersectionTest = intersectionTest;
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
        protected void UpdateTargetPointHorizontal()
        {

            Matrix4 rotation;
            Vector3 transformed = DELTA_BETWEEN_POSITION_AND_TARGET;

            rotation = Matrix4.CreateRotationX(AngleVerticalRad);
            transformed = Vector3.TransformPosition(transformed, rotation);

            rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            transformed = Vector3.TransformPosition(transformed, rotation);

            Target = Position + transformed;

            //Debug.WriteLine("{0} ======== {1} ======= {2}", transformed.Y, AngleVertical, AngleHorizontal);
        }

        #endregion

        public virtual void OnSignal(InputSignal sig)
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
                case InputSignal.SPACE:
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
                    Position = new Vector3(Position.X, Position.Y - Speed, Position.Z);
                    Target = new Vector3(Target.X, Target.Y - Speed, Target.Z);
                    break;
                case InputSignal.UP_PARALLEL:
                   Position = new Vector3(Position.X, Position.Y + Speed, Position.Z);
                    Target = new Vector3(Target.X, Target.Y + Speed, Target.Z);
                    break;
                default:
                    break;
            }
        }

        public virtual void Tick(long delta, Vector2 mouseDxDy)
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

        protected virtual void RotateAroundY(float mouseDx = 200)
        {
            int rotation = 0;

            if (mouseDx > 0)
            {
                rotation = ((int)mouseDx / 25 + MIN_CAMERA_MOVE);
            }
            else if (mouseDx < 0)
            {
                rotation = ((int)mouseDx / 25 - MIN_CAMERA_MOVE);
            }

            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, rotation);
            UpdateTargetPointHorizontal();
        }


        protected virtual void RotateAroundX(float mouseDy = 200)
        {
            int rotation = 0;

            if (mouseDy > 0)
            {
                rotation = ((int)mouseDy / 25 + MIN_CAMERA_MOVE);
            }
            else if (mouseDy < 0)
            {
                rotation = ((int)mouseDy / 25 - MIN_CAMERA_MOVE);
            }

            AngleVertical = MathHelperMINE.AddDegrees(AngleVertical, -rotation);
            if (AngleHorizontal > 80)
            {
                // Debug.WriteLine(AngleVertical);
            }
            UpdateTargetPointHorizontal();
        }

        protected virtual void RotateCounterClock(float mouseDx = 200)
        {
            int rotation = -DEFAULT_ROTATION;
            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, rotation);
            UpdateTargetPointHorizontal();
        }

        protected virtual void RotateClockWise(float mouseDx = 200)
        {
            int rotation = DEFAULT_ROTATION;
            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, rotation);
            UpdateTargetPointHorizontal();
        }

        protected virtual void MoveForward()
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var position1 = Position + Vector3.Transform(STEP_FORWARD_VECTOR, rotation);

            if (intersectionTest == null || intersectionTest(position1))
            {
                Position = position1;
            }
            UpdateTargetPointHorizontal();
            // target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }

        protected virtual void MoveBack()
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var position1 = Position + Vector3.Transform(STEP_BACK_VECTOR, rotation);
            if (intersectionTest == null || intersectionTest(position1))
            {
                Position = position1;
            }

            UpdateTargetPointHorizontal();

            // target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }

        protected virtual void MoveLeft()
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var position1 = Position + Vector3.Transform(STEP_LEFT_VECTOR, rotation);
            if (intersectionTest == null || intersectionTest(position1))
            {
                Position = position1;
            }
            UpdateTargetPointHorizontal();
            //target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }

        protected virtual void MoveRight()
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var position1 = Position + Vector3.Transform(STEP_RIGHT_VECTOR, rotation);
            if (intersectionTest == null || intersectionTest(position1))
            {
                Position = position1;
            }
            UpdateTargetPointHorizontal();
            //target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }


    }
}
