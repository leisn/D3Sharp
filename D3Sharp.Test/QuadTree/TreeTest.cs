using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.QuadTree;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.QuadTree
{
    [TestClass]
    public class TreeTest
    {
        [TestMethod]
        public void EmptyTree()
        {
            var q = new QuadTree<QuadTreeData>();
            Assert.AreEqual(0, q.Size);
            Assert.IsNull(q.Extents);
            Assert.IsNull(q.Root);
            Assert.IsNotNull(q.Data);
            Assert.AreEqual(0, q.Data.Count);
        }

        [TestMethod]
        public void Size()
        {
            var q = new QuadTree<QuadTreeData>();
            Assert.AreEqual(0, q.Size);
            q.Add(new QuadTreeData { X = 0, Y = 0 });
            Assert.AreEqual(1, q.Size);
            q.Add(new QuadTreeData { X = 1, Y = 1 })
               .Add(new QuadTreeData { X = 3, Y = 3 });
            Assert.AreEqual(3, q.Size);
        }
    }
}
