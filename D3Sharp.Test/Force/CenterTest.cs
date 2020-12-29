using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.Force;

using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace D3Sharp.Test.Force
{
    [TestClass]
    public class CenterTest
    {
        [TestMethod]
        public void RepositionNodes()
        {
            var center = new ForceCenter<Node>(0, 0);
            var nodes = new List<Node>
            {
                new Node{X=100,Y=0},
                 new Node{X=200,Y=0},
                  new Node{X=300,Y=0},
            };
            var s = new Simulation<Node>(null)
                .AddForce("center", center).Stop();
            s.Nodes = nodes;
            s.Tick();
            Assert.AreEqual(-100, nodes[0].X);
            Assert.AreEqual(0, nodes[1].X);
            Assert.AreEqual(100, nodes[2].X);
        }

        [TestMethod]
        public void RespectFixedPositions()
        {
            var center = new ForceCenter<Node>();
            var nodes = new List<Node>
            {
                new Node{Fx=0,Fy=0},
                 new Node(),
                  new Node(),
            };
            var s = new Simulation<Node>(null)
                .AddForce("center", center).Stop();
            s.Nodes = nodes;
            s.Tick();
            Assert.AreEqual(0, nodes[0].X);
            Assert.AreEqual(0, nodes[0].Y);
            Assert.AreEqual(0, nodes[0].Fx);
            Assert.AreEqual(0, nodes[0].Fy);
            Assert.AreEqual(0, nodes[0].Vx);
            Assert.AreEqual(0, nodes[0].Vy);
        }
    }
}
