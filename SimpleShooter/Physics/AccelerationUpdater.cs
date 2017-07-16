using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.PlayerControl;

namespace SimpleShooter.Physics
{
    class AccelerationUpdater
    {
        public static Vector3 GetGravityAcceleration(IMovableObject movable)
        {
            Vector3 res = Vector3.Zero;

            if (movable.Mass != 0)
            {
                res = new Vector3(0, -0.01f * movable.Mass, 0);
            }

            return res;
        }

        public static Vector3 GetGravityAcceleration(IShooterPlayer player)
        {
            var res = Vector3.Zero;

            if (player.Position.Y >= 2)
            {
                res = GetGravityAcceleration(player as IMovableObject);
            }
            return res;
        }
    }
}
