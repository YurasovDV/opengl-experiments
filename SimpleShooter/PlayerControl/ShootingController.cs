using System;
using SimpleShooter.Core;
using SimpleShooter.Core.Enemies;
using SimpleShooter.Core.Events;
using SimpleShooter.Helpers;

namespace SimpleShooter.PlayerControl
{
    class ShootingController
    {
        private Engine _engine;

        public ShootingController(Engine engine)
        {
            _engine = engine;
        }

        public ActionStatus Player_Shot(GameObject sender, ShotEventArgs args)
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
                _engine.SoundManager.Shot(args);
            }

            return res;
        }

        public ActionStatus Enemy_Shot(GameObject sender, ShotEventArgs args)
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
                _engine.SoundManager.Shot(args);
            }

            return res;
        }
    }
}
