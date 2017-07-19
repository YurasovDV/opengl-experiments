using System;
using SimpleShooter.Core.Enemies;
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

        public ActionStatus Enemy_Shot(object sender, ShotEventArgs args)
        {
            var res = new ActionStatus()
            {
                Success = false
            };

            var enemy = sender as Enemy;
            if (enemy != null)
            {
                res.Success = true;
                var projectile = ProjectilesHelper.CreateProjectile(enemy);
                _engine.AddObjectAfterTick(projectile);
            }

            return res;
        }
    }
}
