using System;

namespace SimpleShooter.PlayerControl.Events
{
    public delegate ActionStatus PlayerActionHandler<T>(object sender, T args) where T : EventArgs;
}
