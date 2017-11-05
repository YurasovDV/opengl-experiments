using System;
using System.Linq;

namespace SimpleShadows.Core.Utils
{
    public class MathHelperMINE
    {
        public static float[] Sin;
        public static float[] Cos;

        static MathHelperMINE()
        {
            Sin = new float[360];
            Cos = new float[360];
            //float rad;
            /*for (int i = 0; i < 360; i++)
            {
                rad = DegreeToRadians(i);
                Sin[i] = (float)Math.Sin(rad);
                Cos[i] = (float)Math.Cos(rad);
            }*/
        }


        public static double Average(double a, double b, double c, double d)
        {
            return (a + b + c + d) * 0.25;
        }

        public static double Average(double a, double b, double c)
        {
            return (a + b + c) * 0.333;
        }

        public static double Average(params double[] numbers)
        {
            return numbers.Average();
        }

        public static float DegreeToRadians(int degree)
        {
            return (float)(degree / 180.0 * Math.PI);
        }

        public static int RadiansToDegrees(float rad)
        {
            return (int)(rad * 180 / Math.PI);
        }

        public static double Max(params double[] differences)
        {
            double temp;
            double max = 0;
            for (int i = 0; i < differences.Length; i++)
            {
                temp = differences[i];
                if (temp < 0 && -temp > max)
                {
                    max = -temp;
                }
                else if (temp >= 0 && temp > max)
                {
                    max = temp;
                }
            }
            return max;
        }
        //скалярное
        public static float GetAngle(float eyeX, float eyeZ, float x, float z)
        {
            float lenEye = (float)Math.Sqrt(eyeX * eyeX + eyeZ * eyeZ);
            float lenPos = (float)Math.Sqrt(x * x + z * z);
            float cos = (eyeX * x + eyeZ * z) / (lenEye * lenPos);
            var res = Math.Acos(cos);
            return (float)res;
        }

        public static int AddDegrees(int angle, int delta)
        {
            if (delta == 0) return angle;
            if (delta > 0)
            {
                angle = (angle + delta) % 360;
            }
            else
            {
                angle = angle + delta;
                if (angle < 0)
                {
                    angle = 360 + angle;
                }
            }
            return angle;
        }
    }
}
