using System;
using Common.Input;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Player.Events;

namespace SimpleShooter.Player
{
    public interface IShooterPlayer : IMovableObject
    {
        Vector3 Position { get; set; }
        Vector3 Target { get; set; }

        void HandleMouseMove(Vector2 mouseDxDy);
        void Handle(InputSignal signal);

        event PlayerActionHandler<ShotEventArgs> Shot;
        event PlayerActionHandler<JumpEventArgs> Jump;
        event PlayerActionHandler<MoveEventArgs> Move;
    }
}