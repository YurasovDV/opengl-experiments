using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleShooter.Core
{
    public interface IMovableObject
    {
        void Tick(long delta);
    }
}
