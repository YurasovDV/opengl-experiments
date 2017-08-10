using GLTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Physics;

namespace CollisionTests
{
    [TestClass]
    public class CollisionsTestsStatic
    {
        #region check contact static vs static

        [TestMethod]
        public void StaticStatic_NoContact()
        {
            Matrix4 centre = Matrix4.CreateTranslation(30, 4, 0);
            GameObject obj1 = Utils.CreateCube(centre, size: 1);

            centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject obj2 = Utils.CreateCube(centre, size: 0.5f);


            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);
            Assert.IsFalse(isCollided);
        }

        [TestMethod]
        public void StaticStatic_FullyInside_Contact()
        {
            Matrix4 centre = Matrix4.CreateTranslation(30, 4, 0);
            GameObject obj1 = Utils.CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(30, 4, 0);
            GameObject obj2 = Utils.CreateCube(centre, size: 0.5f);

            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);
            Assert.IsTrue(isCollided);
        }

        [TestMethod]
        public void StaticStatic_ExactMatch_Contact()
        {
            Matrix4 centre = Matrix4.CreateTranslation(30, 4, 0);
            GameObject obj1 = Utils.CreateCube(centre, size: 0.5f);

            centre = Matrix4.CreateTranslation(30, 4, 0);
            GameObject obj2 = Utils.CreateCube(centre, size: 0.5f);

            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);
            Assert.IsTrue(isCollided);
        }

        [TestMethod]
        public void StaticStatic_OneVertexInsideXYZ_Contact()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject obj1 = Utils.CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(19, 19, 19);
            GameObject obj2 = Utils.CreateCube(centre, size: 10);

            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);
            Assert.IsTrue(isCollided);
        }

        [TestMethod]
        public void StaticStatic_OneVertexInsideX_Contact()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject obj1 = Utils.CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(19.5f, 19, 19);
            GameObject obj2 = Utils.CreateCube(centre, size: 10);

            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);
            Assert.IsTrue(isCollided);
        }


        [TestMethod]
        public void StaticStatic_VertexVsVertex_Contact()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject obj1 = Utils.CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(20, 20, 20);
            GameObject obj2 = Utils.CreateCube(centre, size: 10);

            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);
            Assert.IsTrue(isCollided);
        }

        #endregion

        #region resolve collision static vs static

        [TestMethod]
        public void StaticStatic_ResolveOneVertexInside()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject obj1 = Utils.CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(19.5f, 19, 19);
            GameObject obj2 = Utils.CreateCube(centre, size: 10);

            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);

            Assert.IsTrue(obj2.BoundingBox.Centre.X == 19.5, "obj2 should remain unchanged");
            Assert.IsTrue(obj2.BoundingBox.Centre.Y == 19, "obj2 should remain unchanged");
            Assert.IsTrue(obj2.BoundingBox.Centre.Z == 19, "obj2 should remain unchanged");


            Assert.IsTrue(obj1.BoundingBox.Centre.X == 0, "obj1 should remain unchanged");
            Assert.IsTrue(obj1.BoundingBox.Centre.Y == 0, "obj1 should remain unchanged");
            Assert.IsTrue(obj1.BoundingBox.Centre.Z == 0, "obj1 should remain unchanged");

        }


        #endregion


    }
}
