using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.QuadTree;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.QuadTree
{
    [TestClass]
    public class ExtentTest
    {
        [TestMethod]
        public void ExtendsTheExtent()
        {
            Tests.AreValuesEqual(new double[,] { { 0, 1 }, { 8, 9 } },
                new QuadTree<CustomData,QuadNode<CustomData>>().Extent(new double[,] {
                {0,1 },{2,6}
                }).Extents);
        }

        [TestMethod]
        public void InferredByCover()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 1, 1 } },
              q.Cover(0, 0).Extents);
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 8, 8 } },
              q.Cover(2, 4).Extents);
        }

        [TestMethod]
        public void SquarfiesAndCentersSpecified()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();
            Tests.AreValuesEqual(new double[,] { { 0, 1 }, { 8, 9 } },
              q.Extent(new double[,] { { 0, 1 }, { 2, 6 } }).Extents);
        }

        [TestMethod]
        public void InvalidExtentIgnores()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();
            Tests.AreValuesEqual(null,
              q.Extent(new double[,] { { 0, double.NaN }, { double.NaN, 0 } }).Extents);
            Tests.AreValuesEqual(null,
             q.Extent(new double[,] { { 1, double.NaN }, { double.NaN, double.NaN } }).Extents);
            Tests.AreValuesEqual(null,
             q.Extent(new double[,] { { double.NaN, double.NaN }, { double.NaN, double.NaN } }).Extents);
        }

        [TestMethod]
        public void FlipsInvertedExtents()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 2, 2 } },
              q.Extent(new double[,] { { 1, 1 }, { 0, 0 } }).Extents);
        }


        [TestMethod]
        public void ToleratesPartiallyValidExtents()
        {
            Tests.AreValuesEqual(new double[,] { { 1, 1 }, { 2, 2 } },
                new QuadTree<CustomData,QuadNode<CustomData>>().Extent(new double[,] {
                {double.NaN,0 },{1,1}
                }).Extents);
            Tests.AreValuesEqual(new double[,] { { 1, 1 }, { 2, 2 } },
                new QuadTree<CustomData,QuadNode<CustomData>>().Extent(new double[,] {
                {0 ,double.NaN},{1,1}
                }).Extents);
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 1, 1 } },
                new QuadTree<CustomData,QuadNode<CustomData>>().Extent(new double[,] {
                {0 ,0},{double.NaN,1}
                }).Extents);
            Tests.AreValuesEqual(new double[,] { { 0, 0 }, { 1, 1 } },
                new QuadTree<CustomData,QuadNode<CustomData>>().Extent(new double[,] {
                {0 ,0},{double.NaN,1}
                }).Extents);
        }
    }
}
