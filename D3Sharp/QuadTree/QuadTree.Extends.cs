using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.QuadTree
{
    public partial class QuadTree<T> where T : IQuadTreeData
    {
        public QuadTree<T> Cover(double x, double y)
        {
            if (double.IsNaN(x) || double.IsNaN(y))
                return this;

            var x0 = this.X0;
            var y0 = this.Y0;
            var x1 = this.X1;
            var y1 = this.Y1;

            if (double.IsNaN(x0))
            {
                x1 = (x0 = Math.Floor(x)) + 1;
                y1 = (y0 = Math.Floor(y)) + 1;
            }
            else
            {
                double z = x1 - x0;
                if (z == 0) z = 1;
                var node = Root;
                QuadTreeNode<T> parent = null;
                int i = 0;
                while (x0 > x || x >= x1 || y0 > y || y >= y1)
                {
                    i = (y < y0).ToInt() << 1 | (x < x0).ToInt();
                    parent = CreateTreeNode(default);
                    parent[i] = node;
                    node = parent;
                    z *= 2;
                    switch (i)
                    {
                        case 0: x1 = x0 + z; y1 = y0 + z; break;
                        case 1: x0 = x1 - z; y1 = y0 + z; break;
                        case 2: x1 = x0 + z; y0 = y1 - z; break;
                        case 3: x0 = x1 - z; y0 = y1 - z; break;
                    }
                }
                if (Root != null && !Root.IsLeaf)
                    this.root = node;
            }
            this.X0 = x0;
            this.Y0 = y0;
            this.X1 = x1;
            this.Y1 = y1;
            return this;
        }

        public QuadTree<T> Extent(double[,] extents)
        {
            this.Extents = extents;
            return this;
        }

        public QuadTree<T> Extent(double x0, double y0, double x1, double y1)
        {
            this.Extents = new double[,] { { x0, y0 }, { x1, y1 } };
            return this;
        }

        public QuadTree<T> Copy()
        {
            var copy = new QuadTree<T>(this.X0, this.Y0, this.X1, this.Y1);
            var node = this.Root;
            if (node == null)
                return copy;

            if (node.IsLeaf)
            {
                copy.root = leafCopy(node);
                return copy;
            }

            var nodes = new List<KeyValuePair<QuadTreeNode<T>, QuadTreeNode<T>>>()
            {
                new KeyValuePair<QuadTreeNode<T>, QuadTreeNode<T>>(
                    node,copy.root=new QuadTreeNode<T>()
                    )
            };
            QuadTreeNode<T> child;
            KeyValuePair<QuadTreeNode<T>, QuadTreeNode<T>> pair;
            while (nodes.Count > 0)
            {
                pair = nodes.Pop();
                for (int i = 0; i < 4; i++)
                {
                    if ((child = pair.Key[i]) != null)
                    {
                        if (child.IsLeaf)
                            pair.Value[i] = leafCopy(child);
                        else
                        {
                            nodes.Push(
                                new KeyValuePair<QuadTreeNode<T>, QuadTreeNode<T>>(
                                    child, pair.Value[i] = new QuadTreeNode<T>()
                                ));
                        }
                    }
                }
            }
            return copy;
        }

        private QuadTreeNode<T> leafCopy(QuadTreeNode<T> leaf)
        {
            var copy = new QuadTreeNode<T>() { Data = leaf.Data };
            QuadTreeNode<T> next = copy;

            while ((leaf = leaf.Next) != null)
                next = next.Next = new QuadTreeNode<T> { Data = leaf.Data };
            return copy;
        }
    }
}
