using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using OpenTK;
using SimpleShooter.Graphics;

namespace SimpleShooter.Core
{
    class MovableObject : GameObject, IMovableObject
    {
        public Vector3 Acceleration { get; set; }
        public Vector3 Speed { get; set; }


        public MovableObject(SimpleModel model, ShadersNeeded shadersNeeded, Vector3 speed) : base(model, shadersNeeded)
        {
            Speed = speed;
        }

        public void Move(long delta)
        {
            if (delta == 0)
            {
                delta = 1;
            }
            var s = Speed / delta;
            for (int i = 0; i < Model.Vertices.Length; i++)
            {
                Model.Vertices[i] = Model.Vertices[i] + s;
            }

        }
    }
}
