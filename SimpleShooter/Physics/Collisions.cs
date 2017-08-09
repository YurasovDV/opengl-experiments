using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcTreeLibrary;
using OpenTK;
using SimpleShooter.Core;

namespace SimpleShooter.Physics
{
    public static class Collisions
    {
        public static void HandleCollision(IMovableObject obj1, IMovableObject obj2)
        {
            obj1.MoveAfterCollision(Vector3.Zero);
            obj2.MoveAfterCollision(Vector3.Zero);
        }

        public static void HandleCollision(IMovableObject obj1, IOctreeItem obj2)
        {
            obj1.MoveAfterCollision(Vector3.Zero);
            // obj2.MoveAfterCollision(Vector3.Zero);
        }

        public static void HandleCollision(IOctreeItem obj1, IMovableObject obj2)
        {
            var move = obj1.BoundingBox.GetCollisionResolution(obj2.BoundingBox);
            obj2.MoveAfterCollision(move);
        }

        // no one is movable
        public static void HandleCollision(IOctreeItem obj1, IOctreeItem obj2)
        {
            //obj1.MoveAfterCollision(Vector3.Zero);
            //obj2.MoveAfterCollision(Vector3.Zero);
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
