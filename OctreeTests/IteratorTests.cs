using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleShooter;

namespace GLTests
{
    [TestClass]
    public class IteratorTests
    {
        [TestMethod]
        public void EmptyContainer()
        {
            ObjectsGrouped o = new ObjectsGrouped();
            List<GameObjectDescriptor> descs = new List<GameObjectDescriptor>();
            foreach (var item in o)
            {
                descs.Add(item);
            }

            Assert.IsTrue(descs.Count == 0);
        }

        [TestMethod]
        public void ContainerWithOnlyLines()
        {
            ObjectsGrouped o = new ObjectsGrouped();
            // o.GetList()
            List<GameObjectDescriptor> descs = new List<GameObjectDescriptor>();
            foreach (var item in o)
            {
                descs.Add(item);
            }
        }
    }
}
