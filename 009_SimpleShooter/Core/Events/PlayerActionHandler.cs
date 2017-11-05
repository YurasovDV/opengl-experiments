using System;

namespace SimpleShooter.Core.Events
{
    public delegate ActionStatus PlayerActionHandler<T>(GameObject sender, T args) where T : EventArgs;
}
