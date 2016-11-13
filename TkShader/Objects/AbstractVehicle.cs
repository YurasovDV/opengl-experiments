using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ShaderOnForm
{
    public abstract class AbstractVehicle
    {
        #region позиция
        public virtual float[] Position { get; set; }
        public virtual float[] Target { get; set; }
        public virtual float X
        {
            get
            {
                return Position[0];
            }
            set
            {
                Position[0] = value;
            }
        }
        public virtual float Y
        {
            get
            {
                return Position[1];
            }
            set
            {
                Position[1] = value;
            }
        }
        public virtual float Z
        {
            get
            {
                return Position[2];
            }
            set
            {
                Position[2] = value;
            }
        }
        public virtual float EyeX
        {
            get
            {
                return Target[0];
            }
            set
            {
                Target[0] = value;
            }
        }
        public virtual float EyeY
        {
            get
            {
                return Target[1];
            }
            set
            {
                Target[1] = value;
            }
        }
        public virtual float EyeZ
        {
            get
            {
                return Target[2];
            }
            set
            {
                Target[2] = value;
            }
        }
        #endregion

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
        #endregion

        public virtual float Speed { get; set; }
        public virtual float Acceleration { get; set; }

        public abstract void Move(InputSignal signal);

        public virtual void Move()
        { 
        
        }

        public Func<float, float, bool> PointInField{get; set;}

        public abstract void Shot();
    }
}
