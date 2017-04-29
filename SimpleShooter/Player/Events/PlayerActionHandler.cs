using System;

namespace SimpleShooter.Player.Events
{
    public delegate ActionStatus PlayerActionHandler<T>(object sender, T args) where T : EventArgs;
}
