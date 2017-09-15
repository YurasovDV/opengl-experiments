using System;
using OcTreeLibrary;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.PlayerControl;

namespace SimpleShooter.Physics
{
    class AccelerationUpdater
    {
        public static Vector3 GetGravityAcceleration(IMovableObject movable)
        {
            var res = Vector3.Zero;

            if (movable.Mass != 0)
            {
                res = new Vector3(0, -0.01f * movable.Mass, 0);
            }

            return res;
        }

        public static Vector3 GetGravityAcceleration(IShooterPlayer player)
        {
            var res = Vector3.Zero;

            if (player.Position.Y >= 2)
            {
                res = GetGravityAcceleration(player as IMovableObject);
            }
            return res;
        }

        public static void UpdateSpeed(Vector3 move, IMovableObject obj1, IMovableObject obj2)
        {
            var massFirst = obj1.Mass > 0 ? obj1.Mass : 1;
            var massSecond = obj2.Mass > 0 ? obj2.Mass : 1;

            var energyFirst = massFirst * 0.5f * obj1.Speed.LengthSquared * obj1.Rigidness;
            var energySecond = massSecond * 0.5f * obj2.Speed.LengthSquared * obj2.Rigidness;

            obj1.Speed = Vector3.Zero;
            obj2.Speed = Vector3.Zero;

            var momentumFirst = (energyFirst + energySecond) * (massFirst / (massFirst + massSecond));
            var momentumSecond = (energyFirst + energySecond) * (massSecond / (massFirst + massSecond));

            move.NormalizeFast();

            obj1.Acceleration += move * -momentumFirst;
            obj2.Acceleration += move * momentumSecond;
        }

        internal static void UpdateSpeed(Vector3 move, IOctreeItem obj1, IMovableObject obj2)
        {
            var massSecond = obj2.Mass > 0 ? obj2.Mass : 1;
            var energySecond = massSecond * 0.5f * obj2.Speed.LengthSquared * obj2.Rigidness;

            var momentumSecond = energySecond;

            obj2.Speed = Vector3.Zero;
            move.NormalizeFast();
            obj2.Acceleration += move * momentumSecond;
        }

        internal static void Friction(MovableObject movableObject)
        {
            if (movableObject.Speed.LengthFast > 0)
            {
                movableObject.Speed *= 0.98f;
            }
        }
    }
}
