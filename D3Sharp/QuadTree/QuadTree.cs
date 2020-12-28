using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.QuadTree
{
    public partial class QuadTree<T> where T : IQuadTreeData
    {
        QuadTreeNode<T> root;

        public QuadTreeNode<T> Root => root;

        double X0 { get; set; } = double.NaN;
        double Y0 { get; set; } = double.NaN;
        double X1 { get; set; } = double.NaN;
        double Y1 { get; set; } = double.NaN;

        /// <summary>
        /// 默认使用<see cref="QuadTreeNode{T}"/>作为节点的创建类型<br/>
        /// 如果需要添加自定义字段或者属性到QuadTreeNode，请继承QuadTreeNode，然后设置这项<br/>
        /// 由于只对空树有效，所以字段是私有的，只在构造方法中可以设置
        /// </summary>
        public Func<T, QuadTreeNode<T>> CreateTreeNode
        { get; private set; } = (d) => new QuadTreeNode<T>() { Data = d };

        public QuadTree(double x0, double y0, double x1, double y1,
              Func<T, QuadTreeNode<T>> createTreeNode = null)
            : this(null, x0, y0, x1, y1, createTreeNode)
        {
        }

        public QuadTree(List<T> datas = null,
            double x0 = double.NaN, double y0 = double.NaN,
            double x1 = double.NaN, double y1 = double.NaN,
            Func<T, QuadTreeNode<T>> createTreeNode = null)
        {
            this.X0 = x0;
            this.Y0 = y0;
            this.X1 = x1;
            this.Y1 = y1;

            if (createTreeNode != null)
                this.CreateTreeNode = createTreeNode;

            if (datas != null)
                AddAll(datas);
        }


        public int Size
        {
            get
            {
                var size = 0;
                this.Visit((node, a, s, d, f) =>
                {
                    if (node.IsLeaf)
                    {
                        do ++size;
                        while ((node = node.Next) != null);
                    }
                    return false;
                });
                return size;
            }
        }

        public double[,] Extents
        {
            get
            {
                if (double.IsNaN(this.X0) || double.IsNaN(this.X1)
                    || double.IsNaN(this.Y0) || double.IsNaN(this.Y1))
                    return null;
                return new double[,] { { this.X0, this.Y0 }, { this.X1, this.Y1 } };
            }
            set
            {
                if (value != null)
                    this.Cover(value[0, 0], value[0, 1]).Cover(value[1, 0], value[1, 1]);
                else
                {
                    this.X0 = double.NaN;
                    this.X1 = double.NaN;
                    this.Y0 = double.NaN;
                    this.Y1 = double.NaN;
                }

            }
        }

        public List<T> Data
        {
            get
            {
                var data = new List<T>();
                this.Visit((node, a, s, d, f) =>
                {
                    if (node.IsLeaf)
                    {
                        do data.Add(node.Data);
                        while ((node = node.Next) != null);
                    }
                    return false;
                });
                return data;
            }
        }

        #region inner class
        class Quad<Tq>
        {
            public QuadTreeNode<Tq> Node { get; set; }
            public double X0 { get; set; }
            public double Y0 { get; set; }
            public double X1 { get; set; }
            public double Y1 { get; set; }

            public Quad(QuadTreeNode<Tq> node, double x0, double y0, double x1, double y1)
            {
                this.Node = node;
                this.X0 = x0;
                this.Y0 = y0;
                this.X1 = x1;
                this.Y1 = y1;
            }
        }
        #endregion
    }
}