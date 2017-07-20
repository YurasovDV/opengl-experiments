using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SimpleShooter.PlayerControl.Events
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
