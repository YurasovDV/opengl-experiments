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
            //obj1.MoveAfterCollision(Vector3.Zero);
            //obj1.BoundingBox.
            obj2.MoveAfterCollision(Vector3.Zero);
        }

        // no one is movable
        public static void HandleCollision(IOctreeItem obj1, IOctreeItem obj2)
        {
            //obj1.MoveAfterCollision(Vector3.Zero);
            //obj2.MoveAfterCollision(Vector3.Zero);
        }
    }
}
