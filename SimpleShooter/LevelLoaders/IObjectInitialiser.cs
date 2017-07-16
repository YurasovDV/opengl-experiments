using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.PlayerControl;

namespace SimpleShooter.LevelLoaders
{
    interface IObjectInitializer
    {
        Level CreateLevel();
    }
}
