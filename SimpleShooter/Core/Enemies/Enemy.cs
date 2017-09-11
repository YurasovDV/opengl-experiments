using Common;
using Common.Geometry;
using OpenTK;
using SimpleShooter.Core.Events;
using SimpleShooter.Core.Weapons;

namespace SimpleShooter.Core.Enemies
{
    class Enemy : MovableObject
    {
        public float HitPoints { get; set; }

        public BaseWeapon Weapon { get; set; }

        public event PlayerActionHandler<ShotEventArgs> Shot;

        public Enemy(SimpleModel model, float mass, float hitPoints) : base(model, Graphics.ShadersNeeded.SimpleModel, Vector3.Zero, Vector3.Zero, mass)
        {
            HitPoints = hitPoints;
        }

        public override Vector3 Tick(long delta)
        {
            var b = base.Tick(delta);

            Weapon.Tick(delta);

            OnShot(new ShotEventArgs(BoundingBox.Centre));

            return b;
        }

        protected override void Move(Vector3 path, BoundingVolume updatedPosition)
        {
            base.Move(path, updatedPosition);
        }

        protected virtual ActionStatus OnShot(ShotEventArgs args)
        {
            var result = new ActionStatus()
            {
                Success = true
            };

            if (Shot != null && Weapon.IsReady)
            {
                Weapon.Shot(args);
                result = Shot(this, args);
                if (result.Success)
                {
                    Weapon.AfterShot();
                }
            }

            return result;
        }

    }
}
