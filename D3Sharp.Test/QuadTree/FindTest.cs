using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.QuadTree;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.QuadTree
{
    [TestClass]
    public class FindTest
    {
        [TestMethod]
        public void ClosetPoint()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();

            var datas = new List<CustomData>();
            for (int i = 0; i < 17 * 17; i++)
            {
                datas.Add(new CustomData
                {
                    X = i % 17d,
                    Y = i / 17 | 0
                });
            }
            q.AddAll(datas);

            CustomData data = q.Find(0.1, 0.1);
            Assert.AreEqual(data.X, 0); Assert.AreEqual(data.Y, 0);
            data = q.Find(7.1, 7.1);
            Assert.AreEqual(data.X, 7); Assert.AreEqual(data.Y, 7);
            data = q.Find(0.1, 15.9);
            Assert.AreEqual(data.X, 0); Assert.AreEqual(data.Y, 16);
            data = q.Find(15.9, 15.9);
            Assert.AreEqual(data.X, 16); Assert.AreEqual(data.Y, 16);
        }

        [TestMethod]
        public void ClosetPointWithinRadius()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();

            var datas = new List<CustomData>() {
                new CustomData{X=0,Y=0},
                new CustomData{X=100,Y=0},
                new CustomData{X=0,Y=100},
                new CustomData{X=100,Y=100},
            };
            q.AddAll(datas);

            CustomData data = q.Find(20, 20, double.PositiveInfinity);
            Assert.AreEqual(data.X, 0); Assert.AreEqual(data.Y, 0);
            data = q.Find(20, 20, 20 * Math.Sqrt(2) + 1e-6);
            Assert.AreEqual(data.X, 0); Assert.AreEqual(data.Y, 0);
            data = q.Find(20, 20, 20 * Math.Sqrt(2) - 1e-6);
            Assert.IsNull(data);
            data = q.Find(0, 20, 20 + 1e-6);
            Assert.AreEqual(data.X, 0); Assert.AreEqual(data.Y, 0);
            data = q.Find(0, 20, 20 - 1e-6);
            Assert.IsNull(data);
            data = q.Find(20, 0, 20 + 1e-6);
            Assert.AreEqual(data.X, 0); Assert.AreEqual(data.Y, 0);
            data = q.Find(20, 0, 20 - 1e-6);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void ClosetPointWithinRadiusasInfinity()
        {
            var q = new QuadTree<CustomData,QuadNode<CustomData>>();

            var datas = new List<CustomData>() {
                new CustomData{X=0,Y=0},
                new CustomData{X=100,Y=0},
                new CustomData{X=0,Y=100},
                new CustomData{X=100,Y=100},
            };
            q.AddAll(datas);

            CustomData data = q.Find(20, 20, double.NaN);
            Assert.AreEqual(data.X, 0); Assert.AreEqual(data.Y, 0);
            data = q.Find(20, 20, double.PositiveInfinity);
            Assert.AreEqual(data.X, 0); Assert.AreEqual(data.Y, 0);
            data = q.Find(20, 20, double.NegativeInfinity);
            Assert.IsNull(data);
        }
    }
}
