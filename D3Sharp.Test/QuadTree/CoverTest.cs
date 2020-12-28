using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.QuadTree;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.QuadTree
{
    [TestClass]
    public class CoverTest
    {

        [TestMethod]
        public void TrivialExtentIfNull()
        {
            Tests.AreValuesEqual(new double[,] { { 1, 2 }, { 2, 3 } },
                new QuadTree<CustomData,QuadNode<CustomData>>().Cover(1, 2).Extents);
        }

        [TestMethod]
        public void NonTrivialSquarifiedNCenteredExtentIfTrivial()
        {
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 4, 4 } },
                new QuadTree<CustomData,QuadNode<CustomData>>().Cover(0, 0).Cover(1, 2).Extents);
        }


        [TestMethod]
        public void IgnoreInvalidPoints()
        {
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 1, 1 } },
                new QuadTree<CustomData,QuadNode<CustomData>>().Cover(0, 0).Cover(double.NaN, 2).Extents);
        }

        [TestMethod]
        public void RepeatesExistExtentIfNonTrivial()
        {
            Tests.AreValuesEqual(new double[,] { { -4, -4 }, { 4, 4 } },
                new QuadTree<CustomData,QuadNode<CustomData>>()
                .Cover(0, 0).Cover(2, 2).Cover(-1, -1).Extents);
            Tests.AreValuesEqual(new double[,] { { 0, -4 }, { 8, 4 } },
                new QuadTree<CustomData,QuadNode<CustomData>>()
                .Cover(0, 0).Cover(2, 2).Cover(1, -1).Extents);

            Tests.AreValuesEqual(new double[,] { { 0, -4 }, { 8, 4 } },
                new QuadTree<CustomData,QuadNode<CustomData>>()
                .Cover(0, 0).Cover(2, 2).Cover(3, -1).Extents);

            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 4, 4 } },
                new QuadTree<CustomData,QuadNode<CustomData>>()
                .Cover(0, 0).Cover(2, 2).Cover(3, 1).Extents);

            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 4, 4 } },
                new QuadTree<CustomData,QuadNode<CustomData>>()
                .Cover(0, 0).Cover(2, 2).Cover(3, 3).Extents);

            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 4, 4 } },
                new QuadTree<CustomData,QuadNode<CustomData>>()
                .Cover(0, 0).Cover(2, 2).Cover(1, 3).Extents);

            Tests.AreValuesEqual(new double[,] { { -4, 0 }, { 4, 8 } },
                new QuadTree<CustomData,QuadNode<CustomData>>()
                .Cover(0, 0).Cover(2, 2).Cover(-1, 3).Extents);

            Tests.AreValuesEqual(new double[,] { { -4, 0 }, { 4, 8 } },
               new QuadTree<CustomData,QuadNode<CustomData>>()
               .Cover(0, 0).Cover(2, 2).Cover(-1, 1).Extents);

            Tests.AreValuesEqual(new double[,] { { -4, -4 }, { 4, 4 } },
               new QuadTree<CustomData,QuadNode<CustomData>>()
               .Cover(0, 0).Cover(2, 2).Cover(-3, -3).Extents);

            Tests.AreValuesEqual(new double[,] { { 0, -4 }, { 8, 4 } },
               new QuadTree<CustomData,QuadNode<CustomData>>()
               .Cover(0, 0).Cover(2, 2).Cover(3, -3).Extents);

            Tests.AreValuesEqual(new double[,] { { 0, -4 }, { 8, 4 } },
              new QuadTree<CustomData,QuadNode<CustomData>>()
              .Cover(0, 0).Cover(2, 2).Cover(5, -3).Extents);

            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 8, 8 } },
             new QuadTree<CustomData,QuadNode<CustomData>>()
             .Cover(0, 0).Cover(2, 2).Cover(5, 3).Extents);

            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 8, 8 } },
             new QuadTree<CustomData,QuadNode<CustomData>>()
             .Cover(0, 0).Cover(2, 2).Cover(5, 5).Extents);

            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 8, 8 } },
             new QuadTree<CustomData,QuadNode<CustomData>>()
             .Cover(0, 0).Cover(2, 2).Cover(3, 5).Extents);

            Tests.AreValuesEqual(new double[,] { { -4, 0 }, { 4, 8 } },
             new QuadTree<CustomData,QuadNode<CustomData>>()
             .Cover(0, 0).Cover(2, 2).Cover(-3, 5).Extents);

            Tests.AreValuesEqual(new double[,] { { -4, 0 }, { 4, 8 } },
             new QuadTree<CustomData,QuadNode<CustomData>>()
             .Cover(0, 0).Cover(2, 2).Cover(-3, 3).Extents);
        }

        [TestMethod]
        public void RepeatWrapRootIfChildren()
        {
            var datas = new List<CustomData> {
                new CustomData{X=0,Y=0},
                new CustomData{X=2,Y=2},
            };
            var q = new QuadTree<CustomData,QuadNode<CustomData>>(datas);
            Assert.AreEqual(datas[0], q.Root[0].Data);
            Assert.AreEqual(datas[1], q.Root[3].Data);

            var root = q.Copy().Cover(3, 3).Root;
            Assert.AreEqual(datas[0], root[0].Data);
            Assert.AreEqual(datas[1], root[3].Data);

            root = q.Copy().Cover(-1, 3).Root;
            Assert.AreEqual(datas[0], root[1][0].Data);
            Assert.AreEqual(datas[1], root[1][3].Data);

            root = q.Copy().Cover(3, -1).Root;
            Assert.AreEqual(datas[0], root[2][0].Data);
            Assert.AreEqual(datas[1], root[2][3].Data);

            root = q.Copy().Cover(-1, -1).Root;
            Assert.AreEqual(datas[0], root[3][0].Data);
            Assert.AreEqual(datas[1], root[3][3].Data);

            root = q.Copy().Cover(5, 5).Root;
            Assert.AreEqual(datas[0], root[0][0].Data);
            Assert.AreEqual(datas[1], root[0][3].Data);

            root = q.Copy().Cover(-3, 5).Root;
            Assert.AreEqual(datas[0], root[1][0].Data);
            Assert.AreEqual(datas[1], root[1][3].Data);

            root = q.Copy().Cover(5, -3).Root;
            Assert.AreEqual(datas[0], root[2][0].Data);
            Assert.AreEqual(datas[1], root[2][3].Data);

            root = q.Copy().Cover(-3, -3).Root;
            Assert.AreEqual(datas[0], root[3][0].Data);
            Assert.AreEqual(datas[1], root[3][3].Data);
        }


        [TestMethod]
        public void DonotWrapRootIfLeaf()
        {
            var data = new CustomData { X = 2, Y = 2 };
            var q = new QuadTree<CustomData,QuadNode<CustomData>>().Cover(0, 0).Add(data);

            Assert.AreEqual(data, q.Root.Data);
            Assert.AreEqual(data, q.Copy().Cover(3, 3).Root.Data);
            Assert.AreEqual(data, q.Copy().Cover(-1, 3).Root.Data);
            Assert.AreEqual(data, q.Copy().Cover(3, -1).Root.Data);
            Assert.AreEqual(data, q.Copy().Cover(-1, -1).Root.Data);
            Assert.AreEqual(data, q.Copy().Cover(5, 5).Root.Data);
            Assert.AreEqual(data, q.Copy().Cover(-3, 5).Root.Data);
            Assert.AreEqual(data, q.Copy().Cover(5, -3).Root.Data);
            Assert.AreEqual(data, q.Copy().Cover(-3, -3).Root.Data);
        }

        [TestMethod]
        public void DonotWrapRootIfUndefined()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>().Cover(0, 0).Cover(2, 2);

            Assert.AreEqual(null, q.Root);
            Assert.AreEqual(null, q.Copy().Cover(3, 3).Root);
            Assert.AreEqual(null, q.Copy().Cover(-1, 3).Root);
            Assert.AreEqual(null, q.Copy().Cover(3, -1).Root);
            Assert.AreEqual(null, q.Copy().Cover(-1, -1).Root);
            Assert.AreEqual(null, q.Copy().Cover(5, 5).Root);
            Assert.AreEqual(null, q.Copy().Cover(-3, 5).Root);
            Assert.AreEqual(null, q.Copy().Cover(5, -3).Root);
            Assert.AreEqual(null, q.Copy().Cover(-3, -3).Root);
        }
    }
}
