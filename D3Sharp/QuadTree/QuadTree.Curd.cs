using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.QuadTree
{
    public partial class QuadTree<T> where T : IQuadTreeData
    {
        public QuadTree<T> Add(T data)
        {
            var x = data.X;
            var y = data.Y;
            return this.Cover(x, y).add(x, y, data);
        }

        private QuadTree<T> add(double x, double y, T data)
        {
            if (double.IsNaN(x) || double.IsNaN(y))
                return this;
            QuadTreeNode<T> parent = null;
            bool right = false, bottom = false;
            double xm = 0, ym = 0, xp = 0, yp = 0;
            double x0 = this.X0, y0 = this.Y0,
                   x1 = this.X1, y1 = this.Y1;
            int i = 0, j = 0;

            QuadTreeNode<T> node = this.Root;
            QuadTreeNode<T> leaf = CreateTreeNode(data);
            if (Root == null)//empty tree
            {
                root = leaf;
                return this;
            }

            while (!node.IsLeaf)
            {
                if (right = x >= (xm = (x0 + x1) / 2)) x0 = xm; else x1 = xm;
                if (bottom = y >= (ym = (y0 + y1) / 2)) y0 = ym; else y1 = ym;
                parent = node;

                if ((node = node[i = bottom.ToInt() << 1 | right.ToInt()]) == null)
                {
                    parent[i] = leaf;
                    return this;
                }
            }
            Console.WriteLine(data);
            xp = node.Data.X; yp = node.Data.Y;
            if (x == xp && y == yp)
            {
                leaf.Next = node;
                if (parent != null)
                    parent[i] = leaf;
                else
                    root = leaf;
                return this;
            }
            do
            {
                parent = parent != null
                    ? (parent[i] = CreateTreeNode(default))
                    : (root = CreateTreeNode(default));
                if (right = x >= (xm = (x0 + x1) / 2)) x0 = xm; else x1 = xm;
                if (bottom = y >= (ym = (y0 + y1) / 2)) y0 = ym; else y1 = ym;

            } while ((i = bottom.ToInt() << 1 | right.ToInt()) == (j = (yp >= ym).ToInt() << 1 | (xp >= xm).ToInt()));
            parent[j] = node;
            parent[i] = leaf;

            return this;
        }

        public QuadTree<T> AddAll(List<T> datas)
        {
            int n = datas.Count;
            double[] xz = new double[n];
            double[] yz = new double[n];
            double x0 = double.PositiveInfinity;
            double y0 = double.PositiveInfinity;
            double x1 = double.NegativeInfinity;
            double y1 = double.NegativeInfinity;
            double x, y;
            T node;
            for (int i = 0; i < n; i++)
            {
                node = datas[i];
                if (double.IsNaN(x = node.X) || double.IsNaN(y = node.Y))
                    continue;
                xz[i] = x;
                yz[i] = y;
                if (x < x0) x0 = x;
                if (x > x1) x1 = x;
                if (y < y0) y0 = y;
                if (y > y1) y1 = y;
            }
            if (x0 > x1 || y0 > y1) return this;

            this.Cover(x0, y0).Cover(x1, y1);

            for (int i = 0; i < n; i++)
            {
                add(xz[i], yz[i], datas[i]);
            }

            return this;
        }

        public QuadTree<T> Remove(T d)
        {
            if (double.IsNaN(d.X) || double.IsNaN(d.Y))
                return this;


            var node = this.Root;
            if (node == null) return this;
            double x = d.X, y = d.Y;
            var x0 = this.X0;
            var y0 = this.Y0;
            var x1 = this.X1;
            var y1 = this.Y1;
            QuadTreeNode<T> parent = null, previous = null, next = null, retainer = null;
            double xm = 0, ym = 0;
            bool right, bottom;

            int i = 0, j = 0;
            if (!node.IsLeaf)
            {
                while (true)
                {
                    if (right = x >= (xm = (x0 + x1) / 2)) x0 = xm; else x1 = xm;
                    if (bottom = y >= (ym = (y0 + y1) / 2)) y0 = ym; else y1 = ym;
                    parent = node;
                    if ((node = node[i = bottom.ToInt() << 1 | right.ToInt()]) == null)
                        return this;
                    if (node.IsLeaf)
                        break;
                    if (parent[(i + 1) & 3] != null
                        || parent[(i + 2) & 3] != null
                        || parent[(i + 3) & 3] != null)
                    {
                        retainer = parent;
                        j = i;
                    }
                }
            }

            while (node.Data?.Equals(d) != true)
            {
                previous = node;
                node = node.Next;
                if (node == null)
                    return this;
            }
            if ((next = node.Next) != null)
                node.Next = null;

            if (previous != null)
            {
                if (next != null) previous.Next = next;
                else previous.Next = null;
                return this;
            }

            if (parent == null)
            {
                this.root = next;
                return this;
            }

            if (next != null)
                parent[i] = next;
            else
                parent[i] = null;

            if (parent.Length == 1)
            {
                node = parent[0] ?? parent[1] ?? parent[2] ?? parent[3];
                if (node.Length == 0)
                {
                    if (retainer != null)
                        retainer[j] = node;
                    else this.root = node;
                }
            }

            return this;
        }

        public QuadTree<T> RemoveAll(IList<T> datas)
        {
            foreach (var item in datas)
            {
                this.Remove(item);
            }
            return this;
        }

        public T Find(double x, double y, double radius = double.NaN)
        {
            T data = default;
            var x0 = this.X0;
            var y0 = this.Y0;
            double x1, y1, x2, y2;
            var x3 = this.X1;
            var y3 = this.Y1;
            List<Quad<T>> quads = new List<Quad<T>>();
            var node = this.Root;
            if (node != null)
                quads.Push(new Quad<T>(node, x0, y0, x3, y3));
            if (double.IsNaN(radius))
                radius = double.PositiveInfinity;
            else
            {
                x0 = x - radius; y0 = y - radius;
                x3 = x + radius; y3 = y + radius;
                radius *= radius;
            }
            Quad<T> q;
            int i = 0;
            while (quads.Count > 0)
            {
                q = quads.Pop();
                node = q.Node;
                if (node == null
                    || (x1 = q.X0) > x3
                    || (y1 = q.Y0) > y3
                    || (x2 = q.X1) < x0
                    || (y2 = q.Y1) < y0
                   ) continue;
                if (!node.IsLeaf)
                {
                    var xm = (x1 + x2) / 2;
                    var ym = (y1 + y2) / 2;
                    quads.Push(new Quad<T>(node[3], xm, ym, x2, y2));
                    quads.Push(new Quad<T>(node[2], x1, ym, xm, y2));
                    quads.Push(new Quad<T>(node[1], xm, y1, x2, ym));
                    quads.Push(new Quad<T>(node[0], x1, y1, xm, ym));

                    i = (y >= ym).ToInt() << 1 | (x >= xm).ToInt();
                    if (i != 0)
                    {
                        int count = quads.Count;
                        q = quads[count - 1];
                        quads[count - 1] = quads[count - 1 - i];
                        quads[count - 1 - i] = q;
                    }
                }
                else
                {
                    var dx = x - node.Data.X;
                    var dy = y - node.Data.Y;
                    var d2 = dx * dx + dy * dy;
                    if (d2 < radius)
                    {
                        var d = Math.Sqrt(radius = d2);
                        x0 = x - d; y0 = y - d;
                        x3 = x + d; y3 = y + d;
                        data = node.Data;
                    }
                }
            }
            return data;
        }
    }
}
