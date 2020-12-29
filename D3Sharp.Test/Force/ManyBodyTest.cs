using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.Force;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.Force
{
    [TestClass]
    public class ManyBodyTest
    {
        [TestMethod]
        public void RepositionNodes()
        {
            var mb = new ForceManyBody<Node>();
            var nodes = new List<Node>
            {
                new Node(),
                 new Node(),
                  new Node(),
            };
            var f = new Simulation<Node>(null)
                .AddForce("charge", mb).Stop();
            f.Nodes = nodes;
            f.Tick(10);

            Assert.IsTrue(NodeEquals.Equals(nodes[0], new Node
            {
                Index = 0,
                X = 23.543801233055913,
                Y = 7.000581643076774,
                Vy = 0.517906401931719,
                Vx = 1.2544752274469713
            }));
            Assert.IsTrue(NodeEquals.Equals(nodes[1], new Node
            {
                Index = 1,
                X = -23.850904378439182,
                Y = 21.28389725298747,
                Vy = 0.914257118557985,
                Vx = -1.1020909771461325
            }));
            Assert.IsTrue(NodeEquals.Equals(nodes[2], new Node
            {
                Index = 2,
                X = -0.27039447927081395,
                Y = -35.76229330151591,
                Vy = -1.432163520489704,
                Vx = -0.15238425030083882
            }));

            mb.SetStrength(-300);
            f.Tick(10);

            Assert.IsTrue(NodeEquals.Equals(nodes[0], new Node
            {
                Index = 0,
                X = 75.51481478403578,
                Y = 28.27281541254175,
                Vy = 1.4774660259027388,
                Vx = 3.6177951664030035
            }));
            Assert.IsTrue(NodeEquals.Equals(nodes[1], new Node
            {
                Index = 1,
                X = -68.77261279544413,
                Y = 57.176314237578026,
                Vy = 2.438763218070549,
                Vx = -3.1042619113625594
            }));
            Assert.IsTrue(NodeEquals.Equals(nodes[2], new Node
            {
                Index = 2,
                X = -7.319699613245737,
                Y = -92.92694405557147,
                Vy = -3.916229243973288,
                Vx = -0.5135332550404446
            }));
        }


        [TestMethod]
        public void RepositionNodesWithStrength()
        {
            var mb = new ForceManyBody<Node>();
            var nodes = new List<Node>
            {
                new Node(),
                 new Node(),
                  new Node(),
            };
            var f = new Simulation<Node>(nodes)
                .AddForce("charge", new ForceManyBody<Node>().SetStrength(110));
            f.Tick(10);

            Assert.IsTrue(NodeEquals.Equals(nodes[0], new Node
            {
                Index = 0,
                X= -18.09293034190703,
                Y= -61.85522398527266,
                Vy= -3.253143684829417,
                Vx= -1.7572575345196322
            }));
            Assert.IsTrue(NodeEquals.Equals(nodes[1], new Node
            {
                Index = 1,
                X= 1.4163070367881587,
                Y= -1.422811550994962,
                Vy= -0.5737847675719954,
                Vx= 1.7868953325697519
            }));
            Assert.IsTrue(NodeEquals.Equals(nodes[2], new Node
            {
                Index = 2,
                X= 14.213419687223059,
                Y= 53.52135594511722,
                Vy= 3.9532863174478163,
                Vx= 0.12091965478188131
            }));

        }
    }
}
