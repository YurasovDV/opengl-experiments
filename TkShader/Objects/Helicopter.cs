using System;

namespace ShaderOnForm
{
    public class Helicopter : AbstractVehicle
    {

        public Helicopter()
        {
            AngleVertical = 45;
            AngleHorizontal = 90;
            Position = new float[] { 0, 6f, 0 };
            Target = new float[] { 0, 0, 0 };
            UpdateTargetPointHorizontal();
            Speed = Constants.DEFAULT_SPEED;
        }

        public override void Move(InputSignal signal)
        {
            float sin = MathHelperMINE.Sin[AngleHorizontal];
            float cos = MathHelperMINE.Cos[AngleHorizontal];
            if (PointInField == null)
            {
                throw new NullReferenceException("Func Helicopter.PointInField() не задана");
            }
            switch (signal)
            {
                case InputSignal.FORWARD: MoveForward(sin, cos); break;

                case InputSignal.BACK: MoveBack(sin, cos); break;

                case InputSignal.RIGHT: MoveRight(sin, cos); break;

                case InputSignal.LEFT: MoveLeft(sin, cos); break;

                case InputSignal.UP:
                    Y += Speed;
                    //EyeY += Speed;
                    break;

                case InputSignal.DOWN:
                    Y -= Speed;
                    // EyeY -= Speed;
                    break;

                case InputSignal.ROTATE_CLOCKWISE: RotateClockWise(); break;

                case InputSignal.ROTATE_COUNTERCLOCKWISE: RotateCounterClock(); break;

                case InputSignal.SHOT: Shot(); break;

                case InputSignal.LOOK_UP: UpdateTargetPointVertical(); break;

                case InputSignal.LOOK_DOWN:

                    UpdateTargetPointVertical();
                    break;
                case InputSignal.FLY_BY_CLOCKWISE:
                    //AngleHorizontal = (AngleHorizontal + Constants.DEFAULT_ROTATION) % 360;
                    SteerClockwise();
                    break;
                case InputSignal.FLY_BY_COUNTERCLOCKWISE:
                    SteerCounterClockwise();
                    break;
                case InputSignal.DOWN_PARALLEL:
                    Y -= Speed;
                    EyeY -= Speed;
                    break;
                case InputSignal.UP_PARALLEL:
                    Y += Speed;
                    EyeY += Speed;
                    break;
                case InputSignal.NONE:

                default:
                    break;
            }
        }

        public override void Shot()
        {
            //пока без стрельбы
        }

        #region повороты

        /// <summary>
        /// вычислить точку, в которую смотрим, по углу и позиции
        /// </summary>
        private void UpdateTargetPointHorizontal()
        {
            float sin = MathHelperMINE.Sin[AngleHorizontal];
            float cos = MathHelperMINE.Cos[AngleHorizontal];

            EyeX = X + Constants.TARGET_DISTANCE * sin;
            EyeZ = Z + Constants.TARGET_DISTANCE * cos;
        }

        private void UpdateTargetPointVertical()
        {

        }


        private void RotateCounterClock()
        {
            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, -Constants.DEFAULT_ROTATION);
            UpdateTargetPointHorizontal();
        }

        private void RotateClockWise()
        {
            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, Constants.DEFAULT_ROTATION);
            UpdateTargetPointHorizontal();
        }

        private void SteerClockwise()
        {
            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, -10);
            float xNext = EyeX - Constants.TARGET_DISTANCE * MathHelperMINE.Sin[AngleHorizontal];
            float zNext = EyeZ - Constants.TARGET_DISTANCE * MathHelperMINE.Cos[AngleHorizontal];
            if (PointInField(xNext, zNext))
            {
                X = xNext;
                Z = zNext;
            }
            //UpdateTargetPointHorizontal();
        }

        private void SteerCounterClockwise()
        {
            AngleHorizontal = MathHelperMINE.AddDegrees(AngleHorizontal, 10);
            float xNext = EyeX - Constants.TARGET_DISTANCE * MathHelperMINE.Sin[AngleHorizontal];
            float zNext = EyeZ - Constants.TARGET_DISTANCE * MathHelperMINE.Cos[AngleHorizontal];
            if (PointInField(xNext, zNext))
            {
                X = xNext;
                Z = zNext;
            }

            //int temp = 10;// MathHelperMINE.AddDegrees(AngleHorizontal, 5);
            //float dxz = 2*Constants.TARGET_DISTANCE * MathHelperMINE.Sin[5];

            //float dx = dxz * MathHelperMINE.Sin[temp];
            //float dz = dxz * MathHelperMINE.Cos[temp];

            //int t = MathHelperMINE.AddDegrees(AngleHorizontal, -90);

            //float diffEyeTargX = Constants.TARGET_DISTANCE;
            //float diffEyeTargZ = Constants.TARGET_DISTANCE;
            //X = X + dx * MathHelperMINE.Sin[t];
            //Z = Z + dz * MathHelperMINE.Cos[t];

            /*  int t = MathHelperMINE.AddDegrees(AngleHorizontal, -90);
              if (AngleHorizontal >= 180)
              {
                  X += dx * MathHelperMINE.Sin[t];
                  Z -= dz * MathHelperMINE.Cos[t];
              }
              else
              {
                  X += dx * MathHelperMINE.Sin[t];
                  Z += dz * MathHelperMINE.Cos[t];
              }*/
            /* 
             float sin = MathHelperMINE.Sin[temp];
             float cos = MathHelperMINE.Cos[temp];

             //X = EyeX + Constants.TARGET_DISTANCE * cos;
             //Z = EyeZ + Constants.TARGET_DISTANCE * sin;


             X += Constants.TARGET_DISTANCE * cos;
             Z -= Constants.TARGET_DISTANCE * sin;*/



            //float angle = MathHelperMINE.GetAngle(EyeX - X, EyeZ - Z, 0, 1);
            // AngleHorizontalRad = angle;

            //UpdateTargetPointHorizontal();
        }

        #endregion

        #region движение по кресту

        private void MoveLeft(float sin, float cos)
        {
            float xNext = cos * Speed;
            float zNext = sin * Speed;
            if (PointInField(X + xNext, Z - zNext))
            {
                X += xNext;
                Z -= zNext;

                EyeX += xNext;
                EyeZ -= zNext;
            }
        }

        private void MoveRight(float sin, float cos)
        {
            float xNext = cos * Speed;
            float zNext = sin * Speed;
            if (PointInField(X - xNext, Z + zNext))
            {
                X -= xNext;
                Z += zNext;

                EyeX -= xNext;
                EyeZ += zNext;
            }
        }

        private void MoveForward(float sin, float cos)
        {
            float xNext = sin * Speed;
            float zNext = cos * Speed;
            if (PointInField(X + xNext, Z + zNext))
            {
                X += xNext;
                Z += zNext;

                EyeX += xNext;
                EyeZ += zNext;
            }
        }

        private void MoveBack(float sin, float cos)
        {
            float xNext = sin * Speed;
            float zNext = cos * Speed;
            if (PointInField(X - xNext, Z + zNext))
            {
                X -= xNext;
                Z -= zNext;

                EyeX -= xNext;
                EyeZ -= zNext;
            }
        }

        #endregion
    }
}
