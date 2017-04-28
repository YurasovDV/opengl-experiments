using System.Collections.Generic;
using OpenTK;
using SimpleShooter.Player;

namespace SimpleShooter.Core
{
    public class Level
    {
        public Vector3 LightPosition { get; internal set; }
        public IEnumerable<GameObject> Objects { get; set; }
        public PlayerModel Player { get; set; }
    }
}
