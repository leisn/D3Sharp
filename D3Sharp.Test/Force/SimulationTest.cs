using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.Force;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.Force
{
    [TestClass]
    public class SimulationTest
    {
        [TestMethod]
        public void InitializesWidthIndicesAndPhyllotaxisPositions()
        {
            var collide = new ForceCollide<Node>();
            var nodes = new List<Node>
            {
                new Node(),
                 new Node(),
                  new Node(),
            };

            var f = new Simulation<Node>();
            f.Nodes = nodes;

            Assert.IsTrue(NodeEquals.Equals(nodes[0], new Node
            {
                X = 7.0710678118654755,
                Y = 0,
                Index = 0,
                Vx = 0,
                Vy = 0
            }));

            Assert.IsTrue(NodeEquals.Equals(nodes[1], new Node
            {
                X = -9.03088751750192,
                Y = 8.27303273571596,
                Index = 1,
                Vx = 0,
                Vy = 0
            }));

            Assert.IsTrue(NodeEquals.Equals(nodes[2], new Node
            {
                X = 1.3823220809823638,
                Y = -15.750847141167634,
                Index = 2,
                Vx = 0,
                Vy = 0
            }));
        }
    }
}
