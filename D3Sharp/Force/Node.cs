using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.Force
{
    public class Node : QuadTree.IQuadData
    {
        /// <summary>
        /// the node’s zero-based index into nodes
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// the node’s fixed x-position
        /// </summary>
        public double Fx { get; set; } = double.NaN;
        /// <summary>
        /// the node’s fixed y-position
        /// </summary>
        public double Fy { get; set; } = double.NaN;
        /// <summary>
        /// the node’s current x-velocity
        /// </summary>
        public double Vx { get; set; } = double.NaN;
        /// <summary>
        /// the node’s current y-velocity
        /// </summary>
        public double Vy { get; set; } = double.NaN;
        /// <summary>
        /// the node’s current x-position
        /// </summary>
        public virtual double X { get; set; } = double.NaN;
        /// <summary>
        /// the node’s current y-position
        /// </summary>
        public virtual double Y { get; set; } = double.NaN;

        public override string ToString()
        {
            return $"{{X:{X}, Y:{Y}, Index:{Index}, Vx:{Vx}, Vy:{Vy}, Fx:{Fx}, Fy:{Fy}}}";
        }
    }
}
