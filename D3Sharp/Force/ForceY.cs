using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.Force
{
    public class ForceY<TNode> : Force<TNode> where TNode : Node
    {
        double[] strengths, yz;
        ForceDelegate<TNode> yFunc;
        ForceDelegate<TNode> strengthFunc;

        public ForceY(ForceDelegate<TNode> yFunc = null)
        {
            strengthFunc = (___, _, __) => { return 0.1; };
            if (yFunc != null)
                this.yFunc = yFunc;
            else
                this.yFunc = (_, __, ___) => { return 0; };
        }

        public ForceY(double y) : this((_, __, ___) => { return y; })
        {
        }

        public ForceDelegate<TNode> YFunc => this.yFunc;

        public double Y
        {
            set
            {
                this.yFunc = (_, __, ___) => { return value; };
                Initialize();
            }
        }

        public ForceDelegate<TNode> StrengthFunc
        {
            get => this.strengthFunc;
            set
            {
                this.strengthFunc = value;
                Initialize();
            }
        }

        public override Force<TNode> UseForce(double alpha = 0)
        {
            TNode node;
            for (int i = 0, n = nodes.Count; i < n; i++)
            {
                node = nodes[i];
                node.Vy += (yz[i] - node.Y) * strengths[i] * alpha;
            }
            return this;
        }

        protected override void Initialize()
        {
            if (nodes.IsNullOrEmpty()) return;
            int n = nodes.Count;
            strengths = new double[n];
            yz = new double[n];
            for (int i = 0; i < n; i++)
            {
                strengths[i] = double.IsNaN(yz[i] = yFunc(Nodes[i], i, nodes)) ? 0 : strengthFunc(nodes[i], i, nodes);
            }
        }
    }
}
