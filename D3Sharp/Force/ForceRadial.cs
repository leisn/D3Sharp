using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.Force
{
    public class ForceRadial<TNode> : Force<TNode> where TNode : Node
    {

        ForceDelegate<TNode> strengthFunc;
        ForceDelegate<TNode> radiusFunc;
        double[] strengths;
        double[] radiuses;

        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;

        public ForceRadial(ForceDelegate<TNode> radiusFunc = null, double x = 0, double y = 0)
        {
            strengthFunc = (___, _, __) => { return 0.1; };
            this.X = x;
            this.Y = y;

            if (radiusFunc != null)
                this.radiusFunc = radiusFunc;
            else
                this.radiusFunc = (_, __, ___) => { return 0; };
        }

        public ForceRadial(double radius, double x = 0, double y = 0)
            : this((___, _, __) => { return radius; }, x, y)
        {
        }


        public ForceDelegate<TNode> RadiusFunc => this.radiusFunc;

        public double Radius
        {
            set
            {
                this.radiusFunc = (_, __, ___) => { return value; };
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

        protected override void Initialize()
        {
            if (nodes.IsNullOrEmpty()) return;
            int n = nodes.Count;
            strengths = new double[n];
            radiuses = new double[n];
            for (int i = 0; i < n; i++)
            {
                radiuses[i] = radiusFunc(nodes[i], i, nodes);
                strengths[i] = double.IsNaN(radiuses[i]) ? 0 : strengthFunc(nodes[i], i, nodes);
            }
        }

        public override Force<TNode> UseForce(double alpha = 0)
        {
            TNode node;
            for (int i = 0, n = nodes.Count; i < n; i++)
            {
                node = nodes[i];
                var dx = node.X - X;
                if (dx == 0) dx = 1e-6;
                var dy = node.Y - Y;
                if (dy == 0) dy = 1e-6;
                var r = Math.Sqrt(dx * dx + dy * dy);
                var k = (radiuses[i] - r) * strengths[i] * alpha / r;
                node.Vx += dx * k;
                node.Vy += dy * k;
            }

            return this;
        }
    }
}
