using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.Force;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.Force
{
    [TestClass]
    public class FindTest
    {
        [TestMethod]
        public void FindOne()
        {
            var collide = new ForceCollide<Node>();
            var nodes = new List<Node>
            {
                new Node{X=5,Y=0 },
                 new Node{X=10,Y=16 },
                  new Node{X=-10,Y=-4 },
            };

            var f = new Simulation<Node>();
            f.Nodes = nodes;

            Assert.AreEqual(nodes[0],f.Find(0,0));
            Assert.AreEqual(nodes[1], f.Find(0, 20));
        }

        [TestMethod]
        public void FindWithRadius()
        {
            var collide = new ForceCollide<Node>();
            var nodes = new List<Node>
            {
                new Node{X=5,Y=0 },
                 new Node{X=10,Y=16 },
                  new Node{X=-10,Y=-4 },
            };

            var f = new Simulation<Node>();
            f.Nodes = nodes;

            Assert.AreEqual(nodes[0], f.Find(0, 0));
            Assert.AreEqual(null, f.Find(0, 0,1));
            Assert.AreEqual(nodes[1], f.Find(0, 20));
        }
    }
}
