using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.Force
{
    public class ForceX<TNode> : Force<TNode> where TNode : Node
    {
        double[] strengths, xz;
        ForceDelegate<TNode> xFunc;
        ForceDelegate<TNode> strengthFunc;

        public ForceX(ForceDelegate<TNode> xFunc = null)
        {
            strengthFunc = (___, _, __) => { return 0.1; };
            if (xFunc != null)
                this.xFunc = xFunc;
            else
                this.xFunc = (_, __, ___) => { return 0; };
        }

        public ForceX(double x) : this((_, __, ___) => { return x; })
        {
        }

        public ForceDelegate<TNode> XFunc => this.xFunc;

        public double X
        {
            set
            {
                this.xFunc = (_, __, ___) => { return value; };
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
                node.Vx += (xz[i] - node.X) * strengths[i] * alpha;
            }
            return this;
        }

        protected override void Initialize()
        {
            if (nodes.IsNullOrEmpty()) return;
            int n = nodes.Count;
            strengths = new double[n];
            xz = new double[n];
            for (int i = 0; i < n; i++)
            {
                strengths[i] = double.IsNaN(xz[i] = XFunc(Nodes[i], i, nodes)) ? 0 : strengthFunc(nodes[i], i, nodes);
            }
        }
    }
}
