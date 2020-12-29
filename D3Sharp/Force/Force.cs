using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.Force
{
    public abstract class Force<TNode> where TNode : Node
    {
        protected List<TNode> nodes;

        public List<TNode> Nodes
        {
            get => nodes;
            internal set
            {
                this.nodes = value;
            }
        }

        public IRandom RandomSource { get; set; }

        public Force() { }

        public Force<TNode> Initialize(List<TNode> nodes, IRandom random)
        {
            this.Nodes = nodes;
            this.RandomSource = random;
            Initialize();
            return this;
        }

        public abstract Force<TNode> UseForce(double alpha = 0);

        protected abstract void Initialize();
    }
}
