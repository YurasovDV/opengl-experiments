using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.LevelLoaders;
using SimpleShooter.Physics;

namespace OctreeTests
{
    [TestClass]
    public class CollisionStaticVsDynamic
    {
        Vector3 _green;
        ShadersNeeded _shaders;

        public CollisionStaticVsDynamic()
        {
            _green = new Vector3(0, 1, 0);
            _shaders = ShadersNeeded.TextureLess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="size">actual cube will have dimension = size * 2</param>
        /// <returns></returns>
        public GameObject CreateCube(Matrix4 center, float size)
        {
            return ObjectInitializer.CreateCube(center, _green, size, _shaders);
        }


        [TestMethod]
        public void StaticDynamic_OneVertexInsideX()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject @static = CreateCube(centre, size: 10);



            centre = Matrix4.CreateTranslation(19, 19, 19);
            GameObject obj2 = CreateCube(centre, size: 10f);
            MovableObject movable = new MovableObject(obj2.Model, obj2.ShaderKind, Vector3.Zero, Vector3.Zero, 0);

            bool isCollided = Collisions.CheckAndHandle(@static, movable);

            Assert.IsTrue(isCollided);

            Assert.IsTrue(@static.BoundingBox.Centre == Vector3.Zero);

            Assert.AreEqual(20, movable.BoundingBox.Centre.X, "x should be used for resolving");
            Assert.AreEqual(19, movable.BoundingBox.Centre.Y, "y should be used for resolving");
            Assert.AreEqual(19, movable.BoundingBox.Centre.Z, "z should be used for resolving");



        }

    }
}
