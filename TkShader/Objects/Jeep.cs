using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderOnForm
{
    public class Jeep : AbstractVehicle
    {
        public Jeep()
        {
            AngleVertical = 45;
            AngleHorizontal = 90;
            Position = new float[] { 0, 2f, 0 };
            Target = new float[] { 0, 0, 0 };
            UpdateTargetPointHorizontal();
            Speed = 0;
        }

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


        public override void Move(InputSignal signal)
        {
            
        }

        public override void Shot()
        {
           //пока без стрельбы
        }
    }
}
