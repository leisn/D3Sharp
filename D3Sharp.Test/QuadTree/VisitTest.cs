using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.QuadTree;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.QuadTree
{
    [TestClass]
    public class VisitTest
    {
        [TestMethod]
        public void VisitsEach()
        {
            var ds = new List<CustomData>
                {
                    new CustomData { X = 0, Y = 0 },
                    new CustomData { X = 1, Y = 0 },
                    new CustomData { X = 0, Y = 1 },
                    new CustomData { X = 1, Y = 1 },
                };
            var q = new QuadTree<CustomData,QuadNode<CustomData>>(ds);

            List<double> result = new List<double>();

            q.Visit((node, x0, y0, x1, y1) =>
            {
                result.Push(x0);
                result.Push(y0);
                result.Push(x1);
                result.Push(y1);
                return false;
            });

            Tests.AreValuesEqual(new double[]{
                0,0,2,2,
                0,0,1,1,
                1,0,2,1,
                0,1,1,2,
                1,1,2,2
            }, result.ToArray());
        }

        [TestMethod]
        public void VisitsApplyPreOrderTraversal()
        {
            var ds = new List<CustomData>
                {
                    new CustomData { X = 100, Y = 100 },
                    new CustomData { X = 200, Y = 200 },
                    new CustomData { X = 300, Y = 300 },
                };
            var q = new QuadTree<CustomData,QuadNode<CustomData>>().Extent(0, 0, 960, 960).AddAll(ds);

            List<double> result = new List<double>();

            q.Visit((node, x0, y0, x1, y1) =>
            {
                result.Push(x0);
                result.Push(y0);
                result.Push(x1);
                result.Push(y1);
                return false;
            });

            Tests.AreValuesEqual(new double[]{
                0,0,1024,1024,
                0,0,512,512,
                0,0,256,256,
                0,0,128,128,
                128,128,256,256,
                 256,256,512,512
            }, result.ToArray());
        }

        [TestMethod]
        public void VisitsNotRecureseIfCallbackReutnTruthy()
        {
            var ds = new List<CustomData>
                {
                    new CustomData { X = 100, Y = 100 },
                    new CustomData { X = 700, Y = 700 },
                    new CustomData { X = 800, Y = 800 },
                };
            var q = new QuadTree<CustomData,QuadNode<CustomData>>().Extent(0, 0, 960, 960).AddAll(ds);

            List<double> result = new List<double>();

            q.Visit((node, x0, y0, x1, y1) =>
            {
                result.Push(x0);
                result.Push(y0);
                result.Push(x1);
                result.Push(y1);
                return x0 > 0;
            });

            Tests.AreValuesEqual(new double[]{
                0,0,1024,1024,
                0,0,512,512,
                512,512,1024,1024,
            }, result.ToArray());
        }

        [TestMethod]
        public void VisitsEmptyTreeNoBounds()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();

            List<double> result = new List<double>();

            q.Visit((node, x0, y0, x1, y1) =>
            {
                result.Push(x0);
                result.Push(y0);
                result.Push(x1);
                result.Push(y1);
                return false;
            });

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void VisitsEmptyTreeWithBounds()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>().Extent(0, 0, 960, 960);

            List<double> result = new List<double>();

            q.Visit((node, x0, y0, x1, y1) =>
            {
                result.Push(x0);
                result.Push(y0);
                result.Push(x1);
                result.Push(y1);
                return false;
            });

            Assert.AreEqual(0, result.Count);
        }
    }
}
