using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.Force
{
    public class ForceCenter<TNode> : Force<TNode> where TNode : Node
    {
        public double Cx { get; set; }
        public double Cy { get; set; }
        public int Strength { get; set; } = 1;

        public ForceCenter(double cx = 0, double cy = 0)
        {
            this.Cx = cx;
            this.Cy = cy;
        }

        public override Force<TNode> UseForce(double alpha = 0)
        {
            int i;
            int n = nodes.Count;
           TNode node;
            double sx = 0;
            double sy = 0;

            for (i = 0; i < n; ++i)
            {
                node = nodes[i];
                sx += node.X;
                sy += node.Y;
            }

            for (sx = (sx / n - Cx) * Strength, sy = (sy / n - Cy) * Strength, i = 0; i < n; ++i)
            {
                node = nodes[i];
                node.X -= sx;
                node.Y -= sy;
            }
            return this;
        }

        protected override void Initialize()
        {
        }
    }
}
