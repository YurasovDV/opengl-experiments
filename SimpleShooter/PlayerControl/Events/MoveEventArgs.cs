using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SimpleShooter.PlayerControl.Events
{
    public class MoveEventArgs : EventArgs
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
    }
}
