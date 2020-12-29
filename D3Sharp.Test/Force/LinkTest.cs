using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.Force;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.Force
{
    [TestClass]
    public class LinkTest
    {
        [TestMethod]
        public void LinkNodes()
        {
            var mb = new ForceManyBody<Node>();
            var nodes = new List<Node>
            {
                new Node(),
                 new Node(),
                  new Node(),
            };
            var links = new List<Link> { 
                new Link{Source=0,Target=1},
                new Link{Source=0,Target=2},
                new Link{Source=2,Target=1}
            };
            var f = new Simulation<Node>(nodes)
                .AddForce("charge", new ForceLink<Node,Link>(links));
            f.Tick(10);

            Assert.IsTrue(NodeEquals.Equals(nodes[0], new Node
            {
                Index = 0,
                X = 16.09041099433763,
                Y = 3.5566118665783737,
                Vy = -0.0054287493843713204,
                Vx = -0.008594012984423784
            }));
            Assert.IsTrue(NodeEquals.Equals(nodes[1], new Node
            {
                Index = 1,
                X = -13.530127801425026,
                Y = 8.528827598527032,
                Vy = -0.0029644135266946848,
                Vx = 0.007059721513094188
            }));
            Assert.IsTrue(NodeEquals.Equals(nodes[2], new Node
            {
                Index = 2,
                X = -3.13778081756669,
                Y = -19.56325387055707,
                Vy = 0.008393162911065991,
                Vx = 0.00153429147132959
            }));

        }
    }
}
