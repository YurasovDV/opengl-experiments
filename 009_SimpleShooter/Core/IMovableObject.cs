using OcTreeLibrary;
using OpenTK;

namespace SimpleShooter.Core
{
    public interface IMovableObject : IOctreeItem
    {

        float Mass { get; }
        Vector3 Acceleration { get; set; }
        Vector3 Speed { get; set; }
        float Rigidness { get; }

        Vector3 Tick(long delta);
        void MoveAfterCollision(Vector3 rollback);
    }
}
