using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.QuadTree
{
    public partial class QuadTree<T> where T : IQuadTreeData
    {
        public QuadTree<T> VisitAfter(Action<QuadTreeNode<T>, double, double, double, double> callback)
        {
            Stack<Quad<T>> quads = new Stack<Quad<T>>();
            Stack<Quad<T>> next = new Stack<Quad<T>>();
            Quad<T> q;
            if (this.Root != null)
                quads.Push(new Quad<T>(this.Root, this.X0, this.Y0, this.X1, this.Y1));
            while (quads.Count > 0)
            {
                q = quads.Pop();
                var node = q.Node;
                if (!node.IsLeaf)
                {
                    QuadTreeNode<T> child;
                    double x0 = q.X0, x1 = q.X1, y0 = q.Y0, y1 = q.Y1;
                    double xm = (x0 + x1) / 2; double ym = (y0 + y1) / 2;
                    if ((child = node[0]) != null) quads.Push(new Quad<T>(child, x0, y0, xm, ym));
                    if ((child = node[1]) != null) quads.Push(new Quad<T>(child, xm, y0, x1, ym));
                    if ((child = node[2]) != null) quads.Push(new Quad<T>(child, x0, ym, xm, y1));
                    if ((child = node[3]) != null) quads.Push(new Quad<T>(child, xm, ym, x1, y1));
                }
                next.Push(q);
            }
            while (quads.Count > 0)
            {
                q = quads.Pop();
                callback(q.Node, q.X0, q.Y0, q.X1, q.Y1);
            }
            return this;
        }

        public QuadTree<T> Visit(Func<QuadTreeNode<T>, double, double, double, double, bool> callback)
        {
            Stack<Quad<T>> quads = new Stack<Quad<T>>();
            Quad<T> q;
            QuadTreeNode<T> node = this.Root;
            QuadTreeNode<T> child;
            double x0, y0, x1, y1;
            if (node != null)
                quads.Push(new Quad<T>(node, this.X0, this.Y0, this.X1, this.Y1));
            while (quads.Count > 0)
            {
                q = quads.Pop();
                var re = !callback(node = q.Node, x0 = q.X0, y0 = q.Y0, x1 = q.X1, y1 = q.Y1);
                if (re && !node.IsLeaf)
                {
                    var xm = (x0 + x1) / 2; var ym = (y0 + y1) / 2;
                    if ((child = node[3]) != null) quads.Push(new Quad<T>(child, xm, ym, x1, y1));
                    if ((child = node[2]) != null) quads.Push(new Quad<T>(child, x0, ym, xm, y1));
                    if ((child = node[1]) != null) quads.Push(new Quad<T>(child, xm, y0, x1, ym));
                    if ((child = node[0]) != null) quads.Push(new Quad<T>(child, x0, y0, xm, ym));
                }
            }
            return this;
        }
    }
}
