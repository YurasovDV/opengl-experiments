using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleShooter.Core
{
    interface IMovableObject
    {
        void Move(long delta);
    }
}
