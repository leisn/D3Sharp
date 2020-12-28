using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.QuadTree;

namespace D3Sharp.Test.QuadTree
{
    public class CustomNode<T> : QuadNode<T> where T : CustomData
    {
        public int Index { get; set; }

        public override T Data
        {
            get => base.Data;
            set
            {
                base.Data = value;
                this.Index = value.Id;
            }
        }
    }
}
