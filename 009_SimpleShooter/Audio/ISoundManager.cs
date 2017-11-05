using System;
using SimpleShooter.Core.Events;

namespace SimpleShooter.Audio
{
    internal interface ISoundManager : IDisposable
    {
        void Shot(ShotEventArgs args);
    }
}