using Common.Input;
using OcTreeLibrary;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Core.Events;
using SimpleShooter.Core.Weapons;

namespace SimpleShooter.PlayerControl
{
    public interface IShooterPlayer : IMovableObject, IOctreeItem
    {
        Vector3 Position { get; set; }
        Vector3 Target { get; set; }

        BaseWeapon Weapon { get; set; }

        void HandleMouseMove(Vector2 mouseDxDy);
        void Handle(InputSignal signal);

        event PlayerActionHandler<ShotEventArgs> Shot;
    }
}