using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.Force;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.Force
{
    [TestClass]
    public class CollidTest
    {
        [TestMethod]
        public void RepositionNodes()
        {
            var collide = new ForceCollide<Node>(1);
            var nodes = new List<Node>
            {
                new Node(),
                 new Node(),
                  new Node(),
            };
            var f = new Simulation<Node>(null)
                .AddForce("collide", collide).Stop();
            f.Nodes = nodes;
            f.Tick(10);
            Assert.AreEqual(Math.Round(7.0710678118654755, 10),
                   Math.Round(nodes[0].X, 10));
            Assert.AreEqual(Math.Round(-9.03088751750192, 10),
                   Math.Round(nodes[1].X, 10));
            Assert.AreEqual(Math.Round(8.27303273571596, 10),
                   Math.Round(nodes[1].Y, 10));
            Assert.AreEqual(Math.Round(1.3823220809823638, 10),
                   Math.Round(nodes[2].X, 10));
            Assert.AreEqual(Math.Round(-15.750847141167634, 10),
                   Math.Round(nodes[2].Y, 10));
            collide.SetRadius(100);
            f.Tick(10);
            Assert.AreEqual(Math.Round(174.08616723117228, 10),
                   Math.Round(nodes[0].X, 10));
            Assert.AreEqual(Math.Round(66.51743051995625, 10),
                   Math.Round(nodes[0].Y, 10));
            Assert.AreEqual(Math.Round(0.26976816231064354, 10),
                   Math.Round(nodes[0].Vy, 10));
            Assert.AreEqual(Math.Round(0.677346615710878, 10),
                   Math.Round(nodes[0].Vx, 10));

            Assert.AreEqual(Math.Round(-139.73606544743998, 10),
                   Math.Round(nodes[1].X, 10));
            Assert.AreEqual(Math.Round(95.69860503079263, 10),
                   Math.Round(nodes[1].Y, 10));
            Assert.AreEqual(Math.Round(0.3545632444404687, 10),
                   Math.Round(nodes[1].Vy, 10));
            Assert.AreEqual(Math.Round(-0.5300880593105067, 10),
                   Math.Round(nodes[1].Vx, 10));

            Assert.AreEqual(Math.Round(-34.9275994083864, 10),
                   Math.Round(nodes[2].X, 10));
            Assert.AreEqual(Math.Round(-169.69384995620052, 10),
                   Math.Round(nodes[2].Y, 10));
            Assert.AreEqual(Math.Round(-0.6243314067511122, 10),
                   Math.Round(nodes[2].Vy, 10));
            Assert.AreEqual(Math.Round(-0.1472585564003713, 10),
                   Math.Round(nodes[2].Vx, 10));
        }

        [TestMethod]
        public void RespectsFixedPositions()
        {
            var collide = new ForceCollide<Node>(1);
            var nodes = new List<Node>
            {
                new Node{ Fx=0,Fy=0},
                 new Node(),
                  new Node(),
            };
            var f = new Simulation<Node>(null)
                .AddForce("collide", collide).Stop();
            f.Nodes = nodes;
            f.Tick(10);
            Assert.AreEqual(0, nodes[0].X);
            Assert.AreEqual(0, nodes[0].Y);
            Assert.AreEqual(0, nodes[0].Fx);
            Assert.AreEqual(0, nodes[0].Fy);
            Assert.AreEqual(0, nodes[0].Vx);
            Assert.AreEqual(0, nodes[0].Vy);
            collide.SetRadius(100);
            f.Tick(10);
            Assert.AreEqual(0, nodes[0].X);
            Assert.AreEqual(0, nodes[0].Y);
            Assert.AreEqual(0, nodes[0].Fx);
            Assert.AreEqual(0, nodes[0].Fy);
            Assert.AreEqual(0, nodes[0].Vx);
            Assert.AreEqual(0, nodes[0].Vy);
        }

        [TestMethod]
        public void JigglesEqualPositions()
        {
            var collide = new ForceCollide<Node>(1);
            var nodes = new List<Node>
            {
                new Node{ X=0,Y=0},
                 new Node{ X=0,Y=0},
            };
            var f = new Simulation<Node>(null)
                .AddForce("collide", collide).Stop();
            f.Nodes = nodes;
            f.Tick();
            Assert.AreNotEqual(nodes[0].X, nodes[1].X);
            Assert.AreNotEqual(nodes[0].Y, nodes[1].Y);

            Assert.AreEqual(nodes[0].Vx, -nodes[1].Vx);
            Assert.AreEqual(nodes[0].Vy, -nodes[1].Vy);
        }

        [TestMethod]
        public void JigglesReproducibleWay()
        {
            var collide = new ForceCollide<Node>();
            var nodes = new List<Node>();

            for (int i = 0; i < 10; i++)
                nodes.Add(new Node { X = 0, Y = 0 });

            var f = new Simulation<Node>();
            f.Nodes = nodes;
            f.AddForce("collide", collide).Stop().Tick(50);

            Assert.AreEqual(Math.Round(-5.371433857229194, 10),
                Math.Round(nodes[0].X, 10));
            Assert.AreEqual(Math.Round(-2.6644608278592576, 10),
                Math.Round(nodes[0].Y, 10));

            Assert.IsTrue(NodeEquals.Equals(nodes[0], new Node
            {
                X = -5.371433857229194,
                Y = -2.6644608278592576,
                Index = 0,
                Vx = 0,
                Vy = 0
            }));
        }
    }
}
