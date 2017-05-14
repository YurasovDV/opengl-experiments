using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SimpleShooter.Core
{
    public interface IMovableObject
    {
        Vector3 Acceleration { get; set; }
        Vector3 Speed { get; set; }

        void Tick(long delta);
        void MoveAfterCollision(Vector3 rollback);
    }
}
