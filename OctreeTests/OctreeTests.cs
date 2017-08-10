using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OctreeTests
{
    [TestClass]
    public class OctreeTests
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
            

        //    var centre = new Vector3(0, 0, 0);
        //    var halfSize = 50;

        //    var volume = BoundingVolume.CreateVolume(centre, halfSize);
        //    OcTree tree = new OcTree(volume);

        //    var ten = 10;

        //    var centre1 = new Vector3();
        //    var volume1 = BoundingVolume.CreateVolume(centre1, ten);

        //    var centre2 = new Vector3();
        //    var volume2 = BoundingVolume.CreateVolume(centre2, ten);

        //    tree.Insert(volume1);
        //    tree.Insert(volume2);

        //}

        //[TestMethod]
        //public void TestHighLevelBlocks()
        //{
        //    var centre = new Vector3(0, 0, 0);
        //    var halfSize = 80;

        //    var allVolume = BoundingVolume.CreateVolume(centre, halfSize);

        //    var MainTree = new OcTree(allVolume);

        //    var y = 20;

        //    var o = BoundingVolume.CreateVolume(new Vector3(-5, y, 5), 10, 20);
        //    o.Name = "n8";
        //    MainTree.Insert(o);

        //    o = BoundingVolume.CreateVolume(new Vector3(-47, y, 0), 5, 10);
        //    o.Name = "n7";
        //    MainTree.Insert(o);

        //    o = BoundingVolume.CreateVolume(new Vector3(-10, y, 40), 15, 10);
        //    o.Name = "n10";
        //    MainTree.Insert(o);

        //    o = BoundingVolume.CreateVolume(new Vector3(40, y, -5), 10, 10);
        //    o.Name = "n9";
        //    MainTree.Insert(o);

        //    o = BoundingVolume.CreateVolume(new Vector3(10, y, -25), 15, 10);
        //    o.Name = "n6";
        //    MainTree.Insert(o);

        //    o = BoundingVolume.CreateVolume(new Vector3(-2, y, -70), 10, 4);
        //    o.Name = "n2";
        //    MainTree.Insert(o);


        //    Assert.IsTrue(MainTree.Root.Objects.Any(ob => ob.Name == "n2"), "n2");
        //    Assert.IsTrue(MainTree.Root.Objects.Any(ob => ob.Name == "n6"), "n6");
        //    Assert.IsTrue(MainTree.Root.Objects.Any(ob => ob.Name == "n7"), "n7");
        //    Assert.IsTrue(MainTree.Root.Objects.Any(ob => ob.Name == "n8"), "n8");
        //    Assert.IsTrue(MainTree.Root.Objects.Any(ob => ob.Name == "n9"), "n9");
        //    Assert.IsTrue(MainTree.Root.Objects.Any(ob => ob.Name.Equals("n10", StringComparison.CurrentCulture)), "n10");


        //}

     /*   [TestMethod]
        public void CreateCube()
        {
            var y = 5;

            var speed = new Vector3(-0.1f, 0, 0);

            var o = BoundingVolume.CreateVolume(new Vector3(-5, y, 5), 10, 20, 3);
            var plane = new GameObject()
            {
                BoundingBox = o,
                Name = "n8",
                Speed = speed,
            };


            // result.Add(plane);


            o = BoundingVolume.CreateVolume(new Vector3(34f, y, -45), 3, 3, 1);
            var plane2 = new GameObject()
            {
                BoundingBox = o,
                Name = "n11",
                Speed = speed,
            };


            o = BoundingVolume.CreateVolume(new Vector3(34f, y, -55), 3, 3, 1);
            var plane3 = new GameObject()
            {
                BoundingBox = o,
                Name = "n12",
                Speed = speed,
            };

            var tree = new OcTree(BoundingVolume.CreateVolume(new Vector3(0, 0, 0), 160));

            tree.Insert(plane);
            tree.Insert(plane2);
            tree.Insert(plane3);

            var w1 = plane2.TreeSegment.Width;
            var w2 = plane3.TreeSegment.Width;
            Assert.IsTrue(w1 == 10, "plane2");
            Assert.IsTrue(w2 == 10, "plane3");
            //Assert.IsTrue(tree.Root.Objects.Any(ob => ob.Name == "n2"), "n2");

        }*/
    }
}
