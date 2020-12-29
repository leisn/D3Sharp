using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.Force
{
    public class ForceLink<TNode, TLink> : Force<TNode>
        where TNode : Node
        where TLink : Link
    {
        private List<TLink> _links;
        private int[] count;
        double[] bias, strengths, distances;

        ForceDelegate<TLink> strength;
        ForceDelegate<TLink> distance;
        public int Iterations { get; set; } = 1;

        public ForceLink(List<TLink> links)
        {
            this._links = links ?? new List<TLink>();

            strength = defaultStrength;
            distance = (___, _, __) => { return 30; };
        }

        private TNode find(Dictionary<int, TNode> map, int nodeId)
        {
            TNode node;
            if (map.TryGetValue(nodeId, out node))
                return node;
            throw new ArgumentOutOfRangeException("Node not found: " + nodeId);
        }

        public List<TLink> Links
        {
            get => _links;
            set
            {
                this._links = value;
                Initialize();
            }
        }

        public ForceDelegate<TLink> StrengthFunc
        {
            get => this.strength;
            set
            {
                this.strength = value;
                initializeStrength();
            }
        }

        public ForceDelegate<TLink> DistanceFunc
        {
            get => this.distance;
            set
            {
                this.distance = value;
                initializeDistance();
            }
        }

        private double defaultStrength(TLink link, int i = 0, List<TLink> links = null)
        {
            return 1d / Math.Min(count[((TNode)link.Source).Index], count[((TNode)link.Target).Index]);
        }


        protected override void Initialize()
        {
            if (Nodes.IsNullOrEmpty())
                return;
            var n = Nodes.Count;
            var m = _links.Count;

            var nodeById = new Dictionary<int, TNode>();
            foreach (var item in Nodes)
            {
                nodeById[item.Index] = item;
            }

            count = new int[n];

            TLink link;
            for (int i = 0; i < m; i++)
            {
                link = _links[i];
                link.Index = i;
                if (link.Source is int)
                    link.Source = find(nodeById, (int)link.Source);
                if (link.Target is int)
                    link.Target = find(nodeById, (int)link.Target);
                var source = (TNode)link.Source;
                var target = (TNode)link.Target;
                count[source.Index] = count[source.Index] + 1;
                count[target.Index] = count[target.Index] + 1;
            }

            bias = new double[m];
            for (int i = 0; i < m; i++)
            {
                link = _links[i];
                var source = (TNode)link.Source;
                var target = (TNode)link.Target;
                bias[i] = count[source.Index] / (count[source.Index] + count[target.Index]);
            }

            strengths = new double[m]; initializeStrength();
            distances = new double[m]; initializeDistance();
        }

        private void initializeStrength()
        {
            if (Nodes.IsNullOrEmpty())
                return;
            for (int i = 0, n = _links.Count; i < n; i++)
            {
                strengths[i] = StrengthFunc(_links[i], i, _links);
            }
        }

        private void initializeDistance()
        {
            if (Nodes.IsNullOrEmpty())
                return;
            for (int i = 0, n = _links.Count; i < n; i++)
            {
                distances[i] = DistanceFunc(_links[i], i, _links);
            }
        }

        public override Force<TNode> UseForce(double alpha = 0)
        {
            for (int k = 0, n = _links.Count; k < Iterations; ++k)
            {
                Link link;
                TNode source, target;
                double x, y, l, b;
                for (int i = 0; i < n; i++)
                {
                    link = Links[i];
                    source = (TNode)link.Source;
                    target = (TNode)link.Target;
                    x = target.X + target.Vx - source.X - source.Vx;
                    if (x == 0) x = Helper.Jiggle(RandomSource);
                    y = target.Y + target.Vy - source.Y - source.Vy;
                    if (y == 0) y = Helper.Jiggle(RandomSource);
                    l = Math.Sqrt(x * x + y * y);
                    l = (l - distances[i]) / l * alpha * strengths[i];
                    x *= l; y *= l;
                    target.Vx -= x * (b = bias[i]);
                    target.Vy -= y * b;
                    source.Vx += x * (b = 1 - b);
                    source.Vy += y * b;
                }
            }
            return this;
        }
    }
}
