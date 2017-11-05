using System;
using OpenTK;

namespace SimpleShooter.Core.Events
{
    public class ShotEventArgs : EventArgs
    {
        public Vector3 BarrelEnd { get; set; }

        public ShotEventArgs(Vector3 point)
        {
            BarrelEnd = point;
        }
    }
}
