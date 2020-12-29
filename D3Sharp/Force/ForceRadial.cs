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

        #region Constructors
        public ForceRadial(ForceDelegate<TNode> radiusFunc = null, double x = 0, double y = 0)
        {
            this.radiusFunc = radiusFunc == null ? defaultRadius : radiusFunc;
            this.X = x;
            this.Y = y;
            strengthFunc = defaultStrength;
        }

        public ForceRadial(double radius, double x = 0, double y = 0)
        {
            this.SetRadius(radius);
            this.X = x;
            this.Y = y;
            strengthFunc = defaultStrength;
        }
        #endregion

        #region func properties
        double defaultStrength(TNode node, int i, List<TNode> nodes) => 0.1;
        public ForceDelegate<TNode> StrengthFunc
        {
            get => this.strengthFunc;
            set
            {
                this.strengthFunc = value == null ? defaultStrength : value;
                Initialize();
            }
        }
        public ForceRadial<TNode> SetStrength(double strength)
        {
            this.strengthFunc = (_, __, ___) => strength;
            return this;
        }

        double defaultRadius(TNode node, int i, List<TNode> nodes) => 0;
        public ForceDelegate<TNode> RadiusFunc
        {
            get => this.radiusFunc;
            set
            {
                this.radiusFunc = value == null ? defaultRadius : value;
                Initialize();
            }
        }
        public ForceRadial<TNode> SetRadius(double radius)
        {
            this.radiusFunc = (_, __, ___) => radius;
            return this;
        }
        #endregion

        protected override void Initialize()
        {
            if (Nodes.IsNullOrEmpty()) return;
            int n = Nodes.Count;
            strengths = new double[n];
            radiuses = new double[n];
            for (int i = 0; i < n; i++)
            {
                radiuses[i] = radiusFunc(Nodes[i], i, Nodes);
                strengths[i] = double.IsNaN(radiuses[i]) ? 0 : strengthFunc(Nodes[i], i, Nodes);
            }
        }

        public override Force<TNode> UseForce(double alpha = 0)
        {
            TNode node;
            for (int i = 0, n = Nodes.Count; i < n; i++)
            {
                node = Nodes[i];
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
