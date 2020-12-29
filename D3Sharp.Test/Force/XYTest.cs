using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.Force;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.Force
{
    [TestClass]
    public class XYTest
    {
        [TestMethod]
        public void ForceXCenterNodes()
        {
            var collide = new ForceCollide<Node>();
            var nodes = new List<Node>
            {
                new Node{ X=100,Y=0},
                 new Node{ X=200,Y=0},
                  new Node{ X=300,Y=0},
            };

            var f = new Simulation<Node>().AddForce("x", new ForceX<Node>(200));
            f.Nodes = nodes;
            f.Tick(30);

            Assert.IsTrue(nodes[0].X > 190);
            Assert.IsTrue(nodes[0].Vx > 0);
            Assert.AreEqual(nodes[1].X, 200);
            Assert.AreEqual(nodes[1].Vx, 0);
            Assert.IsTrue(nodes[2].X < 210);
            Assert.IsTrue(nodes[2].Vx < 0);
        }

        [TestMethod]
        public void ForceYCenterNodes()
        {
            var collide = new ForceCollide<Node>();
            var nodes = new List<Node>
            {
                new Node{ Y=100,X=0},
                 new Node{ Y=200,X=0},
                  new Node{ Y=300,X=0},
            };

            var f = new Simulation<Node>().AddForce("y", new ForceY<Node>(200));
            f.Nodes = nodes;
            f.Tick(30);

            Assert.IsTrue(nodes[0].Y > 190);
            Assert.IsTrue(nodes[0].Vy > 0);
            Assert.AreEqual(nodes[1].Y, 200);
            Assert.AreEqual(nodes[1].Vy, 0);
            Assert.IsTrue(nodes[2].Y < 210);
            Assert.IsTrue(nodes[2].Vy < 0);
        }

        [TestMethod]
        public void ForceXFixedPosions()
        {
            var collide = new ForceCollide<Node>();
            var nodes = new List<Node>
            {
                new Node{ Fx=0,Fy=0},
                 new Node(),
                  new Node(),
            };

            var f = new Simulation<Node>().AddForce("x", new ForceX<Node>(200));
            f.Nodes = nodes;
            f.Tick();

            Assert.IsTrue(NodeEquals.Equals(nodes[0], new Node
            {
                X = 0,
                Y = 0,
                Index = 0,
                Vx = 0,
                Vy = 0,
                Fx = 0,
                Fy = 0
            }));
        }

        [TestMethod]
        public void ForceYFixedPosions()
        {
            var collide = new ForceCollide<Node>();
            var nodes = new List<Node>
            {
                new Node{ Fx=0,Fy=0},
                 new Node(),
                  new Node(),
            };

            var f = new Simulation<Node>().AddForce("y", new ForceY<Node>(200));
            f.Nodes = nodes;
            f.Tick();

            Assert.IsTrue(NodeEquals.Equals(nodes[0], new Node
            {
                X = 0,
                Y = 0,
                Index = 0,
                Vx = 0,
                Vy = 0,
                Fx = 0,
                Fy = 0
            }));
        }

        [TestMethod]
        public void ForceXAccessor()
        {

            var collide = new ForceCollide<XYNode>();
            var nodes = new List<XYNode>
            {
                new XYNode{ X=100,Y=0,x0=300},
                 new XYNode{ X=200,Y=0,x0=200},
                  new XYNode{ X=300,Y=0,x0=100},
            };

            var f = new Simulation<XYNode>().AddForce("x",
                new ForceX<XYNode>((d, _, __) => d.x0));
            f.Nodes = nodes;
            f.Tick(30);

            Assert.IsTrue(nodes[0].X > 290);
            Assert.IsTrue(nodes[0].Vx > 0);
            Assert.AreEqual(nodes[1].X, 200);
            Assert.AreEqual(nodes[1].Vx, 0);
            Assert.IsTrue(nodes[2].X < 110);
            Assert.IsTrue(nodes[2].Vx < 0);
        }

        [TestMethod]
        public void ForceYAccessor()
        {

            var collide = new ForceCollide<XYNode>();
            var nodes = new List<XYNode>
            {
                new XYNode{ Y=100,X=0,y0=300},
                 new XYNode{ Y=200,X=0,y0=200},
                  new XYNode{ Y=300,X=0,y0=100},
            };

            var f = new Simulation<XYNode>().AddForce("y",
                new ForceY<XYNode>((d, _, __) => d.y0));
            f.Nodes = nodes;
            f.Tick(30);

            Assert.IsTrue(nodes[0].Y > 290);
            Assert.IsTrue(nodes[0].Vy > 0);
            Assert.AreEqual(nodes[1].Y, 200);
            Assert.AreEqual(nodes[1].Vy, 0);
            Assert.IsTrue(nodes[2].Y < 110);
            Assert.IsTrue(nodes[2].Vy < 0);
        }
    }

    public class XYNode : Node
    {
        public double x0 { get; set; }
        public double y0 { get; set; }
    }
}
