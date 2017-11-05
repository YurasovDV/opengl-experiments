using System.Collections.Generic;
using Common.Geometry;
using OpenTK;
using SimpleShooter.PlayerControl;

namespace SimpleShooter.Core
{
    public class Level
    {
        public BoundingVolume Volume { get; set; }
        public Vector3 LightPosition { get; set; }
        public List<GameObject> Objects { get; set; }
        public IShooterPlayer Player { get; set; }
    }
}
