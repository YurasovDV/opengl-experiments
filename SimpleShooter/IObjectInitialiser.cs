using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using SimpleShooter.Core;

namespace SimpleShooter
{
    interface IObjectInitialiser
    {
        IEnumerable<GameObject> CreateLevel();
        Camera InitCamera(Matrix4 projection);
    }
}
