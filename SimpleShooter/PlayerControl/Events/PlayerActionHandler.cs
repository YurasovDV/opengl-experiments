using System;
using SimpleShooter.Core;

namespace SimpleShooter.PlayerControl.Events
{
    public delegate ActionStatus PlayerActionHandler<T>(GameObject sender, T args) where T : EventArgs;
}
