using System.Collections.Generic;
using OpenTK;
using SimpleShooter.Player;

namespace SimpleShooter.Core
{
    public class Level
    {
        public Vector3 LightPosition { get; set; }
        public IEnumerable<GameObject> Objects { get; set; }
        public IPlayer Player { get; set; }
    }
}
