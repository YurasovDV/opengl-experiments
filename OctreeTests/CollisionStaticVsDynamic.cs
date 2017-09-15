using System;
using System.Collections.Generic;
using GLTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.LevelLoaders;
using SimpleShooter.Physics;

namespace CollisionTests
{
    [TestClass]
    public class CollisionStaticVsDynamic
    {

        [TestMethod]
        public void StaticDynamic_OneVertexInsideEqualDist()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject @static = Utils.CreateCube(centre, size: 10);



            centre = Matrix4.CreateTranslation(19, 19, 19);
            GameObject obj2 = Utils.CreateCube(centre, size: 10f);
            MovableObject movable = new MovableObject(obj2.Model, obj2.ShaderKind, Vector3.Zero, Vector3.Zero);

            bool isCollided = Collisions.CheckAndHandle(@static, movable);

            Assert.IsTrue(isCollided);

            Assert.IsTrue(@static.BoundingBox.Centre == Vector3.Zero);

            Assert.AreEqual(20, movable.BoundingBox.Centre.X, "x should be used for resolving");
            Assert.AreEqual(19, movable.BoundingBox.Centre.Y, "y should not be used for resolving");
            Assert.AreEqual(19, movable.BoundingBox.Centre.Z, "z should not be used for resolving");
        }

        [TestMethod]
        public void StaticDynamic_OneVertexInsideX()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject @static = Utils.CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(19.1f, 19, 19);
            GameObject obj2 = Utils.CreateCube(centre, size: 10f);
            MovableObject movable = new MovableObject(obj2.Model, obj2.ShaderKind, Vector3.Zero, Vector3.Zero);

            bool isCollided = Collisions.CheckAndHandle(@static, movable);

            Assert.IsTrue(isCollided);

            Assert.IsTrue(@static.BoundingBox.Centre == Vector3.Zero);

            Assert.AreEqual(20, movable.BoundingBox.Centre.X, "x should be used for resolving");
            Assert.AreEqual(19, movable.BoundingBox.Centre.Y, "y should not be used for resolving");
            Assert.AreEqual(19, movable.BoundingBox.Centre.Z, "z should not be used for resolving");
        }

        [TestMethod]
        public void StaticDynamic_OneVertexInsideY()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject @static = Utils.CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(19f, 19.1f, 19);
            GameObject obj2 = Utils.CreateCube(centre, size: 10f);
            MovableObject movable = new MovableObject(obj2.Model, obj2.ShaderKind, Vector3.Zero, Vector3.Zero);

            bool isCollided = Collisions.CheckAndHandle(@static, movable);

            Assert.IsTrue(isCollided);

            Assert.IsTrue(@static.BoundingBox.Centre == Vector3.Zero);

            Assert.AreEqual(19, movable.BoundingBox.Centre.X, "x should not be used for resolving");
            Assert.AreEqual(20, movable.BoundingBox.Centre.Y, "y should be used for resolving");
            Assert.AreEqual(19, movable.BoundingBox.Centre.Z, "z should not be used for resolving");
        }

        [TestMethod]
        public void StaticDynamic_OneVertexInsideZ()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject @static = Utils.CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(19f, 19f, 19.1f);
            GameObject obj2 = Utils.CreateCube(centre, size: 10f);
            MovableObject movable = new MovableObject(obj2.Model, obj2.ShaderKind, Vector3.Zero, Vector3.Zero);

            bool isCollided = Collisions.CheckAndHandle(@static, movable);

            Assert.IsTrue(isCollided);

            Assert.IsTrue(@static.BoundingBox.Centre == Vector3.Zero);

            Assert.AreEqual(19, movable.BoundingBox.Centre.X, "x should not be used for resolving");
            Assert.AreEqual(19, movable.BoundingBox.Centre.Y, "y should not be used for resolving");
            Assert.AreEqual(20, movable.BoundingBox.Centre.Z, "z should be used for resolving");
        }

        [TestMethod]
        public void StaticDynamic_OneVertexInsideYZ()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject @static = Utils.CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(19f, 19.1f, 19.1f);
            GameObject obj2 = Utils.CreateCube(centre, size: 10f);
            MovableObject movable = new MovableObject(obj2.Model, obj2.ShaderKind, Vector3.Zero, Vector3.Zero);

            bool isCollided = Collisions.CheckAndHandle(@static, movable);

            Assert.IsTrue(isCollided);

            Assert.IsTrue(@static.BoundingBox.Centre == Vector3.Zero);

            Assert.AreEqual(19, movable.BoundingBox.Centre.X, "x should not be used for resolving");
            Assert.AreEqual(20, movable.BoundingBox.Centre.Y, "y should be used for resolving");
            Assert.AreEqual(19.1, Math.Round(movable.BoundingBox.Centre.Z, 1, MidpointRounding.ToEven), "z should not be used for resolving");
        }

        [TestMethod]
        public void PlayerVsWafer()
        {
            var level = new Level();
            var objectList = new List<GameObject>();
            objectList.Add(ObjectInitializer.CreateWafer());

        }

    }
}
