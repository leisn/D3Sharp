using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.Force
{
    public abstract class Force<TNode> where TNode : Node
    {
        private List<TNode> nodes;
        private IRandom random;

        public List<TNode> Nodes
        {
            get => nodes;
            set { this.nodes = value; Initialize(); }
        }

        public IRandom RandomSource
        {
            get => random;
            set { this.random = value; Initialize(); }
        }

        public Force() { }

        protected abstract void Initialize();

        public Force<TNode> Initialize(List<TNode> nodes, IRandom randomSource)
        {
            this.nodes = nodes;
            this.random = randomSource;
            Initialize();
            return this;
        }

        public abstract Force<TNode> UseForce(double alpha = 0);

    }
}
