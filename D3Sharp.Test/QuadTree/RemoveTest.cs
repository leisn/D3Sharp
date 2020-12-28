using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.QuadTree;

using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace D3Sharp.Test.QuadTree
{
    [TestClass]
    public class RemoveTest
    {
        [TestMethod]
        public void RemoveOne()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();
            var p0 = new CustomData { X = 1, Y = 1 };
            q.Add(p0);
            Assert.AreEqual(1, q.Size);
            q.Remove(p0);

            Assert.IsNull(q.Root);
            Assert.AreEqual(0, q.Size);
            Tests.AreValuesEqual(new double[,] { { 1, 1 }, { 2, 2 } }, q.Extents);
        }

        [TestMethod]
        public void RemoveFirst()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();
            var p0 = new CustomData { X = 1, Y = 1 };
            var p1 = new CustomData { X = 1, Y = 1 };
            q.Add(p0).Add(p1);
            Assert.AreEqual(2, q.Size);
            q.Remove(p0);
            Tests.AreValuesEqual(new double[,] { { 1, 1 }, { 2, 2 } }, q.Extents);

            Assert.AreEqual(p1, q.Root.Data);
        }

        [TestMethod]
        public void RemoveAnother()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();
            var p0 = new CustomData { X = 1, Y = 1 };
            var p1 = new CustomData { X = 1, Y = 1 };
            q.Add(p0).Add(p1);
            Assert.AreEqual(2, q.Size);
            q.Remove(p1);
            Tests.AreValuesEqual(new double[,] { { 1, 1 }, { 2, 2 } }, q.Extents);

            Assert.AreEqual(p0, q.Root.Data);
        }

        [TestMethod]
        public void RemoveNonRoot()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();
            var p0 = new CustomData { X = 0, Y = 0 };
            var p1 = new CustomData { X = 1, Y = 1 };
            q.Add(p0).Add(p1);
            Assert.AreEqual(2, q.Size);
            q.Remove(p0);
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 2, 2 } }, q.Extents);

            Assert.AreEqual(p1, q.Root.Data);
        }

        [TestMethod]
        public void RemoveNonRootAnother()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();
            var p0 = new CustomData { X = 0, Y = 0 };
            var p1 = new CustomData { X = 1, Y = 1 };
            q.Add(p0).Add(p1);
            Assert.AreEqual(2, q.Size);
            q.Remove(p1);
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 2, 2 } }, q.Extents);

            Assert.AreEqual(p0, q.Root.Data);
        }

        [TestMethod]
        public void IgnoresPointNotInTree()
        {
            var q0 = new QuadTree<CustomData,QuadNode<CustomData>>();
            var q1 = new QuadTree<CustomData,QuadNode<CustomData>>();
            var p0 = new CustomData { X = 0, Y = 0 };
            var p1 = new CustomData { X = 0, Y = 0 };
            q0.Add(p0);
            q1.Add(p1);

            q0.Remove(p1);
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 1, 1 } }, q0.Extents);

            Assert.AreEqual(p0, q0.Root.Data);
            Assert.AreEqual(p1, q1.Root.Data);
        }

        [TestMethod]
        public void RemoveAnotheFindInTreer()
        {
            var ds = new List<CustomData>
                {
                    new CustomData { X = 630, Y = 438 },
                    new CustomData { X = 715, Y = 464 },
                    new CustomData { X = 523, Y = 516 },
                    new CustomData { X = 646, Y = 318 },
                    new CustomData { X = 434, Y = 620 },
                    new CustomData { X = 570, Y = 489 },
                    new CustomData { X = 520, Y = 345 },
                    new CustomData { X = 459, Y = 443 },
                    new CustomData { X = 346, Y = 405 },
                    new CustomData { X = 529, Y = 444 },
                };
            var q = new QuadTree<CustomData,QuadNode<CustomData>>().Extent(0, 0, 959, 959).AddAll(ds);
            q.Remove(q.Find(546,440));

            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 1024, 1024 } }, q.Extents);
            Assert.AreEqual(ds[8], q.Root[0][3][2].Data);
            Assert.AreEqual(ds[7], q.Root[0][3][3].Data);
        }
    }
}
