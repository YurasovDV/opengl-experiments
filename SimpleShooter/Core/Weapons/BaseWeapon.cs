using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleShooter.Core.Events;

namespace SimpleShooter.Core.Weapons
{
    public class BaseWeapon
    {
        private long CoolDown;
        private long CoolDownDefault;

        public BaseWeapon()
        {
            CoolDown = 0;
            CoolDownDefault = 1000;
        }

        public BaseWeapon(long coolDown)
        {
            CoolDownDefault = coolDown;
        }


        public bool IsReady
        {
            get
            {
                return CoolDown <= 0;
            }
        }

        public virtual void Shot(ShotEventArgs args)
        {

        }

        internal virtual void AfterShot()
        {
            CoolDown = CoolDownDefault;
        }

        internal virtual void Tick(long delta)
        {
            if (CoolDown >= 0)
            {
                CoolDown -= delta;
            }
        }
    }
}
