using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleShooter.Core;

namespace SimpleShooter
{
    interface IObjectInitialiser
    {
        IEnumerable<GameObject> CreateLevel();
    }
}
