using System;
using OpenTK;

namespace SimpleShooter.Core.Events
{
    public class MoveEventArgs : EventArgs
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
    }
}
