using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.QuadTree
{
    public class QuadTreeNode<T>
    {
        protected QuadTreeNode<T>[] children = new QuadTreeNode<T>[4];

        public T Data { get; set; }

        public QuadTreeNode<T> Next { get; set; }

        public QuadTreeNode<T>[] Children => children;

        public QuadTreeNode<T> this[int index]
        {
            get
            {
                if (index < 0 || index > 3)
                    throw new IndexOutOfRangeException();
                return Children[index];
            }
            set
            {
                Children[index] = value;
            }
        }

        public bool IsLeaf => Length == 0;

        public int Length
        {
            get
            {
                int length = 0;
                foreach (var item in Children)
                {
                    if (item != null)
                        length++;
                }
                return length;
            }
        }
    }
}
