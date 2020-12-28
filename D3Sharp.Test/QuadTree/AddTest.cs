using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.QuadTree;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.QuadTree
{
    [TestClass]
    public class AddTest
    {
        [TestMethod]
        public void AddPoint()
        {
            var q = new QuadTree<CustomData,CustomNode<CustomData>>();
            var val1 = new CustomData { X = 0.0, Y = 0.0 };
            var root = q.Add(val1).Root;
            Assert.AreSame(root.Data, val1);
            var val2 = new CustomData { X = 0.9, Y = 0.9 };
            root = q.Add(val2).Root;
            Assert.AreSame(root[0].Data, val1);
            Assert.AreSame(root[3].Data, val2);
            var val3 = new CustomData { X = 0.9, Y = 0 };
            root = q.Add(val3).Root;
            Assert.AreSame(root[1].Data, val3);
            var val4 = new CustomData { X = 0, Y = 0.9 };
            root = q.Add(val4).Root;
            Assert.AreSame(root[2].Data, val4);
            var val5 = new CustomData { X = 0.4, Y = 0.4 };
            root = q.Add(val5).Root;
            Assert.AreSame(root[0][0].Data, val1);
            Assert.AreSame(root[0][3].Data, val5);
            Assert.AreSame(root[1].Data, val3);
            Assert.AreSame(root[2].Data, val4);
            Assert.AreSame(root[3].Data, val2);
        }

        [TestMethod]
        public void PointsOnPerimeterOfBounds()
        {
            var q = new QuadTree<CustomData,CustomNode<CustomData>>()
                .Extent(new double[,] { { 0, 0 }, { 1, 1 } });

            var val1 = new CustomData { X = 0, Y = 0 };
            var root = q.Add(val1).Root;
            Assert.AreSame(root.Data, val1);
            var val2 = new CustomData { X = 1, Y = 1 };
            root = q.Add(val2).Root;
            Assert.AreSame(root[0].Data, val1);
            Assert.AreSame(root[3].Data, val2);
            var val3 = new CustomData { X = 1, Y = 0 };
            root = q.Add(val3).Root;
            Assert.AreSame(root[1].Data, val3);
            var val4 = new CustomData { X = 0, Y = 1 };
            root = q.Add(val4).Root;
            Assert.AreSame(root[2].Data, val4);
            Assert.AreSame(root[0].Data, val1);
            Assert.AreSame(root[3].Data, val2);
            Assert.AreSame(root[1].Data, val3);
        }

        [TestMethod]
        public void PointsAtTopOfBounds()
        {
            var q = new QuadTree<CustomData,CustomNode<CustomData>>()
                .Extent(new double[,] { { 0, 0 }, { 2, 2 } });
            var bound = q.Add(new CustomData { X = 1, Y = -1 }).Extents;
            Tests.AreValuesEqual(new double[,] { { 0, -4 }, { 8, 4 } }, bound);
        }

        [TestMethod]
        public void PointsAtRightOfBounds()
        {
            var q = new QuadTree<CustomData,CustomNode<CustomData>>()
                .Extent(new double[,] { { 0, 0 }, { 2, 2 } });

            var bound = q.Add(new CustomData { X = 3, Y = 1 }).Extents;
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 4, 4 } }, bound);
        }

        [TestMethod]
        public void PointsAtBottomOfBounds()
        {
            var q = new QuadTree<CustomData,CustomNode<CustomData>>()
                .Extent(new double[,] { { 0, 0 }, { 2, 2 } });

            var bound = q.Add(new CustomData { X = 1, Y = 3 }).Extents;
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 4, 4 } }, bound);
        }

        [TestMethod]
        public void PointsAtLeftOfBounds()
        {
            var q = new QuadTree<CustomData,CustomNode<CustomData>>()
                .Extent(new double[,] { { 0, 0 }, { 2, 2 } });

            var bound = q.Add(new CustomData { X = -1, Y = 1 }).Extents;
            Tests.AreValuesEqual(new double[,] { { -4, 0 }, { 4, 8 } }, bound);
        }

        [TestMethod]
        public void CoincidentPointsByCreateLinkedList()
        {
            var q = new QuadTree<CustomData,CustomNode<CustomData>>()
                .Extent(new double[,] { { 0, 0 }, { 1, 1 } });

            var val1 = new CustomData { X = 0, Y = 0 };
            var root = q.Add(val1).Root;
            Assert.AreSame(root.Data, val1);
            var val2 = new CustomData { X = 1, Y = 0 };
            root = q.Add(val2).Root;
            Assert.AreSame(root[0].Data, val1);
            Assert.AreSame(root[1].Data, val2);
            var val3 = new CustomData { X = 0, Y = 1 };
            root = q.Add(val3).Root;
            Assert.AreSame(root[2].Data, val3);
            var val4 = new CustomData { X = 0, Y = 1 };
            root = q.Add(val4).Root;
            Assert.AreSame(root[2].Next.Data, val3);
            Assert.AreSame(root[2].Data, val4);
        }

        [TestMethod]
        public void ImplicitlyDeinesTrivialBoundsForFistPoint()
        {
            var q = new QuadTree<CustomData,CustomNode<CustomData>>();
            var val1 = new CustomData { X = 1, Y = 2 };
            var bound = q.Add(val1).Extents;
            Tests.AreValuesEqual(new double[,] { { 1, 2 }, { 2, 3 } }, bound);
            var root = q.Root;
            Assert.AreSame(root.Data, val1);
        }


        [TestMethod]
        public void AddAllPointsIgnoreInvalid()
        {
            var q = new QuadTree<CustomData,CustomNode<CustomData>>();

            var datas = new List<CustomData> {
                new CustomData{X=double.NaN,Y=0},
                new CustomData{X=0,Y=double.NaN},
            };

            var root = q.AddAll(datas).Root;
            Assert.AreEqual(root, null);
            Assert.AreEqual(q.Extents, null);

            var datas2 = new List<CustomData> {
                new CustomData{X=0,Y=0},
                new CustomData{X=0.9,Y=0.9},
            };
            root = q.AddAll(datas2).Root;
            Assert.AreEqual(datas2[0], root[0].Data);
            Assert.AreEqual(datas2[1], root[3].Data);

            root = q.AddAll(datas).Root;
            Assert.AreEqual(datas2[0], root[0].Data);
            Assert.AreEqual(datas2[1], root[3].Data);
            Assert.AreEqual(null, root[1]);
            Assert.AreEqual(null, root[2]);

            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 1, 1 } }, q.Extents);
        }

        [TestMethod]
        public void AddEmptyArray()
        {
            var q = new QuadTree<CustomData,CustomNode<CustomData>>();

            var datas = new List<CustomData>();

            var root = q.AddAll(datas).Root;
            Assert.AreEqual(root, null);
            Assert.AreEqual(q.Extents, null);

            var datas2 = new List<CustomData> {
                new CustomData{X=0,Y=0},
                new CustomData{X=1,Y=1},
            };
            root = q.AddAll(datas2).Root;
            Assert.AreEqual(datas2[0], root[0].Data);
            Assert.AreEqual(datas2[1], root[3].Data);

            root = q.AddAll(datas).Root;
            Assert.AreEqual(datas2[0], root[0].Data);
            Assert.AreEqual(datas2[1], root[3].Data);
            Assert.AreEqual(null, root[1]);
            Assert.AreEqual(null, root[2]);

            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 2, 2 } }, q.Extents);
        }


        [TestMethod]
        public void DataInTree()
        {
            var q = new QuadTree<CustomData,CustomNode<CustomData>>();

            Assert.AreEqual(q.Data.Count, 0);

            var datas = new List<CustomData>(){
                new CustomData{X=0,Y=0},
                new CustomData{X=1,Y=2},
            };

            q.AddAll(datas);
            var ds = q.Data;
            Assert.AreEqual(ds[0], datas[0]);
            Assert.AreEqual(ds[1], datas[1]);
        }

    }
}
