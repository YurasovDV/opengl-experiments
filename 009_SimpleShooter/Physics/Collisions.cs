using OcTreeLibrary;
using OpenTK;
using SimpleShooter.Core;

namespace SimpleShooter.Physics
{
    public static class Collisions
    {
        public static void HandleCollision(IMovableObject movable, IMovableObject movable2)
        {
            Vector3 rollback = movable.BoundingBox.GetCollisionResolution(movable2.BoundingBox);
            movable.MoveAfterCollision(rollback * -1);
            movable2.MoveAfterCollision(rollback);
            AccelerationUpdater.UpdateSpeed(rollback, movable, movable2);
        }

        public static void HandleCollision(IMovableObject movable, IOctreeItem @static)
        {
            HandleCollision(@static, movable);
        }

        public static void HandleCollision(IOctreeItem @static, IMovableObject movable)
        {
            Vector3 rollback = @static.BoundingBox.GetCollisionResolution(movable.BoundingBox);
            AccelerationUpdater.UpdateSpeed(rollback, @static, movable);
            movable.MoveAfterCollision(rollback);
        }

        // no one is movable
        public static void HandleCollision(IOctreeItem static1, IOctreeItem static2)
        {         
        }

        public static bool CheckAndHandle(GameObject entityWorkWith, IOctreeItem possibleCollider)
        {
            var result = false;
            if (entityWorkWith.BoundingBox.Intersects(possibleCollider.BoundingBox))
            {
                if (entityWorkWith is IMovableObject)
                {
                    if (possibleCollider is IMovableObject)
                    {
                        HandleCollision(entityWorkWith as IMovableObject, possibleCollider as IMovableObject);
                    }
                    else
                    {
                        HandleCollision(entityWorkWith as IMovableObject, possibleCollider);
                    }
                }
                else
                {
                    if (possibleCollider is IMovableObject)
                    {
                        HandleCollision(entityWorkWith, possibleCollider as IMovableObject);
                    }
                    else
                    {
                        HandleCollision(entityWorkWith, possibleCollider);
                    }
                }

                result = true;
            }
            return result;
        }
    }
}
