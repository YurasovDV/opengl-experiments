using System;
using SimpleShooter.Helpers;
using SimpleShooter.PlayerControl.Events;

namespace SimpleShooter.PlayerControl
{
    class PlayerController
    {
        private Engine _engine;

        public PlayerController(Engine engine)
        {
            _engine = engine;
        }

        public ActionStatus Player_Shot(object sender, ShotEventArgs args)
        {
            var res = new ActionStatus()
            {
                Success = false
            };

            var player = sender as IShooterPlayer;
            if (player != null)
            {
                res.Success = true;
                var projectile = ProjectilesHelper.CreateProjectile(player);
                _engine.AddObject(projectile);
            }

            return res;
        }
    }
}
