using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.LevelLoaders;
using SimpleShooter.Physics;

namespace OctreeTests
{
    [TestClass]
    public class CollisionsTestsStaticCheckContact
    {
        Vector3 _green;
        ShadersNeeded _shaders;

        public CollisionsTestsStaticCheckContact()
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

        #region static vs static

        [TestMethod]
        public void NoContact()
        {
            Matrix4 centre = Matrix4.CreateTranslation(30, 4, 0);
            GameObject obj1 = CreateCube(centre, size: 1);

            centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject obj2 = CreateCube(centre, size: 0.5f);


            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);
            Assert.IsFalse(isCollided);
        }

        [TestMethod]
        public void FullyInside()
        {
            Matrix4 centre = Matrix4.CreateTranslation(30, 4, 0);
            GameObject obj1 = CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(30, 4, 0);
            GameObject obj2 = CreateCube(centre, size: 0.5f);

            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);
            Assert.IsTrue(isCollided);
        }

        [TestMethod]
        public void ExactMatch()
        {
            Matrix4 centre = Matrix4.CreateTranslation(30, 4, 0);
            GameObject obj1 = CreateCube(centre, size: 0.5f);

            centre = Matrix4.CreateTranslation(30, 4, 0);
            GameObject obj2 = CreateCube(centre, size: 0.5f);

            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);
            Assert.IsTrue(isCollided);
        }

        [TestMethod]
        public void OneVertexInside()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject obj1 = CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(19, 19, 19);
            GameObject obj2 = CreateCube(centre, size: 10);

            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);
            Assert.IsTrue(isCollided);
        }


        [TestMethod]
        public void VertexVsVertex()
        {
            Matrix4 centre = Matrix4.CreateTranslation(0, 0, 0);
            GameObject obj1 = CreateCube(centre, size: 10);

            centre = Matrix4.CreateTranslation(20, 20, 20);
            GameObject obj2 = CreateCube(centre, size: 10);

            bool isCollided = Collisions.CheckAndHandle(obj1, obj2);
            Assert.IsTrue(isCollided);
        }

        #endregion
    }
}
