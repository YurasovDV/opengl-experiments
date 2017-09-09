using Common;
using OpenTK;
using SimpleShooter.Core.Events;

namespace SimpleShooter.Core.Enemies
{
    class Enemy : MovableObject
    {
        protected long shotCoolDown = 0;
        protected long shotCoolDownDefault = 1000;

        public float HitPoints { get; set; }

        public event PlayerActionHandler<ShotEventArgs> Shot;

        public Enemy(SimpleModel model, float mass, float hitPoints) : base(model, Graphics.ShadersNeeded.SimpleModel, Vector3.Zero, Vector3.Zero, mass)
        {
            HitPoints = hitPoints;
        }

        public override Vector3 Tick(long delta)
        {
            var b = base.Tick(delta);

            OnShot(new ShotEventArgs(BoundingBox.Centre));

            return b;
        }

        protected virtual ActionStatus OnShot(ShotEventArgs args)
        {
            var result = new ActionStatus()
            {
                Success = true
            };

            if (Shot != null && shotCoolDown <= 0)
            {
                shotCoolDown = shotCoolDownDefault;
                result = Shot(this, args);
            }

            return result;
        }

    }
}
