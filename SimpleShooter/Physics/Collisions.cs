using System;
using OcTreeLibrary;
using OpenTK;
using SimpleShooter.Core;

namespace SimpleShooter.Physics
{
    public static class Collisions
    {
        public static void HandleCollision(IMovableObject obj1, IMovableObject obj2)
        {
            Vector3 move = obj1.BoundingBox.GetCollisionResolution(obj2.BoundingBox);
            obj2.MoveAfterCollision(move);
            obj1.MoveAfterCollision(move * -1);
            AccelerationUpdater.UpdateSpeed(move, obj1, obj2);
        }

        public static void HandleCollision(IMovableObject obj1, IOctreeItem obj2)
        {
            HandleCollision(obj2, obj1);
        }

        public static void HandleCollision(IOctreeItem obj1, IMovableObject obj2)
        {
            var move = obj1.BoundingBox.GetCollisionResolution(obj2.BoundingBox);

            AccelerationUpdater.UpdateSpeed(move, obj1, obj2);

            obj2.MoveAfterCollision(move);
        }

        // no one is movable
        public static void HandleCollision(IOctreeItem obj1, IOctreeItem obj2)
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
