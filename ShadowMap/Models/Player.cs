using System;
using System.Diagnostics;
using Common;
using Common.Input;
using Common.Utils;
using OpenTK;

namespace ShadowMap
{
    public class Player : IPlayer
    {
        private int twenty_five = 15;

        private const int maxLeanY = 90;

        /// <summary>
        /// тест пересечения
        /// </summary>
        public Func<Vector3, Vector3> GetUpdatedLocation { get; set; }

        protected readonly Vector3 STEP_FORWARD_VECTOR = new Vector3(0, 0, Constants.Speed);
        protected readonly Vector3 STEP_BACK_VECTOR = new Vector3(0, 0, -Constants.Speed);
        protected readonly Vector3 STEP_RIGHT_VECTOR = new Vector3(-Constants.Speed, 0, 0);
        protected readonly Vector3 STEP_LEFT_VECTOR = new Vector3(Constants.Speed, 0, 0);
        protected readonly Vector3 FLASHLIGHT_DIST = new Vector3(0, 0, 0);

        protected int MinCameraMove = 1;

        protected int DEFAULT_ROTATION = 3;

        public virtual Vector3 DELTA_BETWEEN_POSITION_AND_TARGET { get; set; }
        public virtual Vector3 DEFAULT_POSITION { get; set; }

        public virtual Vector3 FlashlightPosition { get; set; }
        public virtual Vector3 Position { get; set; }
        public virtual Vector3 Target { get; set; }

        public Player(Func<Vector3, Vector3> getUpdatedPosition = null)
        {
            this.GetUpdatedLocation = getUpdatedPosition;


            DELTA_BETWEEN_POSITION_AND_TARGET = new Vector3(0, 0, 10f);
            DEFAULT_POSITION = new Vector3(0.0f, 3f, 20);

            Position = new Vector3(DEFAULT_POSITION);
            Target = new Vector3(DELTA_BETWEEN_POSITION_AND_TARGET);

            twenty_five = 15;

            MinCameraMove = 1;
            AngleVertical = 0;
            UpdateTargetPointHorizontal();
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
        }

        #endregion

        public virtual void OnSignal(InputSignal sig)
        {
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
                    Position = new Vector3(Position.X, Position.Y - Constants.Speed, Position.Z);
                    Target = new Vector3(Target.X, Target.Y - Constants.Speed, Target.Z);
                    break;
                case InputSignal.UP_PARALLEL:
                    Position = new Vector3(Position.X, Position.Y + Constants.Speed, Position.Z);
                    Target = new Vector3(Target.X, Target.Y + Constants.Speed, Target.Z);
                    break;
                default:
                    break;
            }
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

            if (GetUpdatedLocation == null)
            {
                Position = position1;
            }
            else
            {
                Position = GetUpdatedLocation(position1);
            }
            UpdateTargetPointHorizontal();
            // target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }

        protected virtual void MoveBack()
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var position1 = Position + Vector3.Transform(STEP_BACK_VECTOR, rotation);
            if (GetUpdatedLocation == null)
            {
                Position = position1;
            }
            else
            {
                Position = GetUpdatedLocation(position1);
            }

            UpdateTargetPointHorizontal();

            // target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }

        protected virtual void MoveLeft()
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var position1 = Position + Vector3.Transform(STEP_LEFT_VECTOR, rotation);
            if (GetUpdatedLocation == null)
            {
                Position = position1;
            }
            else
            {
                Position = GetUpdatedLocation(position1);
            }
            UpdateTargetPointHorizontal();
            //target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }

        protected virtual void MoveRight()
        {
            var rotation = Matrix4.CreateRotationY(AngleHorizontalRad);
            var position1 = Position + Vector3.Transform(STEP_RIGHT_VECTOR, rotation);
            if (GetUpdatedLocation == null)
            {
                Position = position1;
            }
            else
            {
                Position = GetUpdatedLocation(position1);
            }
            UpdateTargetPointHorizontal();
            //target = position + Vector3.TransformPosition(DEFAULT_TARGET, rotation);
        }

        public void Tick(long delta, Vector2 mouseDxDy)
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

        protected void RotateAroundY(float mouseDx = 200)
        {
            int rotation = 0;

            if (mouseDx > 0)
            {
                rotation = ((int)mouseDx / twenty_five + MinCameraMove);
            }
            else if (mouseDx < 0)
            {
                rotation = ((int)mouseDx / twenty_five - MinCameraMove);
            }

            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, rotation);
            UpdateTargetPointHorizontal();
        }


        protected void RotateAroundX(float mouseDy = 200)
        {
            int rotation = 0;

            if (mouseDy > 0)
            {
                rotation = ((int)mouseDy / twenty_five + MinCameraMove);
            }
            else if (mouseDy < 0)
            {
                rotation = ((int)mouseDy / twenty_five - MinCameraMove);
            }

            AngleVertical = MathHelperMINE.AddDegrees(AngleVertical, -rotation);


            var closerToHigherBound = AngleVertical - maxLeanY;
            var closerToLowerBound = 360 - maxLeanY -  AngleVertical;
            if (closerToHigherBound >= 0 && closerToLowerBound >= 0)
            {
                if (closerToHigherBound > closerToLowerBound)
                {
                    AngleVertical = 360 - maxLeanY + 1;
                }
                else
                {
                    AngleVertical = maxLeanY - 1;
                }
            }

            UpdateTargetPointHorizontal();
        }
    }
}
