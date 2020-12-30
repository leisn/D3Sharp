using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace D3Sharp.Force
{
    public delegate double ForceDelegate<T>(T t = default, int i = 0, List<T> list = null);

    public class Simulation<TNode> where TNode : Node
    {
        //static Simulation<TNode> simulation;
        //public static Simulation<TNode> CreateSimulation => simulation ??= new Simulation<TNode>(null);
        #region fields
        const double initialRadius = 10;
        readonly double initialAngle = Math.PI * (3 - Math.Sqrt(5));
        double velocityDecayReve = 0.6;

        public double Alpha { get; set; } = 1;
        public double AlphaMin { get; set; } = 0.001;
        public double AlphaDecay { get; set; }
        public double AlphaTarget { get; set; } = 0;
        private int fps = 30;


        List<TNode> _nodes;
        IRandom _randomSource;
        Dictionary<string, Force<TNode>> Forces { get; }
        Timer stepper;

        public event EventHandler<string> Events;
        #endregion

        #region properties
        public IRandom RandomSource
        {
            get => this._randomSource;
            set
            {
                if (value == null)
                    return;
                this._randomSource = value;
                initializeForces();
            }
        }
        public List<TNode> Nodes
        {
            get => this._nodes;
            set
            {
                this._nodes = value;
                initializeNodes();
                initializeForces();
            }
        }
        public double VelocityDecay
        {
            get => 1 - velocityDecayReve;
            set
            {
                this.velocityDecayReve = 1 - value;
            }
        }
        public int FPS
        {
            get => this.fps;
            set
            {
                this.fps = value;
                stepper.Interval = 1000 / value;
            }
        }
        #endregion

        public Simulation(List<TNode> nodes = null, IRandom random = null, int fps = 30)
        {
            this.fps = fps;
            AlphaDecay = 1 - Math.Pow(AlphaMin, 1d / 300);
            stepper = new Timer(1000 / fps);
            stepper.Elapsed += step;
            Forces = new Dictionary<string, Force<TNode>>();
            this._randomSource = random ?? new Lcg();
            this._nodes = nodes ?? new List<TNode>();
            initializeNodes();
            //stepper.Start();
        }

        public TNode Find(double x, double y, double radius = double.NaN)
        {
            double dx, dy, d2;
            TNode node;
            TNode closest = default;
            if (double.IsNaN(radius))
                radius = double.PositiveInfinity;
            else
                radius *= radius;
            for (int i = 0; i < Nodes.Count; i++)
            {
                node = Nodes[i];
                dx = x - node.X;
                dy = y - node.Y;
                d2 = dx * dx + dy * dy;
                if (d2 < radius)
                {
                    closest = node;
                    radius = d2;
                }
            }
            return closest;
        }

        #region private methods
        private object obj = new object();
        private void step(object sender, ElapsedEventArgs e)
        {
            //var thread = System.Threading.Thread.CurrentThread;
            //Debug.WriteLine($"Tick start on Thead{{{thread.ManagedThreadId}}}");
            lock (obj)
            {
                Tick();
                Events?.Invoke(this, "tick");
                if (Alpha < AlphaMin)
                {
                    Stop();
                    Events?.Invoke(this, "end");
                }
            }
            //Debug.WriteLine($"Tick stop on Thead{{{thread.ManagedThreadId}}}");
        }

        private void initializeNodes()
        {
            TNode node;
            for (int i = 0, n = Nodes.Count; i < n; ++i)
            {
                node = Nodes[i];
                node.Index = i;
                if (!double.IsNaN(node.Fx)) node.X = node.Fx;
                if (!double.IsNaN(node.Fy)) node.Y = node.Fy;
                if (double.IsNaN(node.X) || double.IsNaN(node.Y))
                {
                    var radius = initialRadius * Math.Sqrt(0.5 + i);
                    var angle = i * initialAngle;
                    node.X = radius * Math.Cos(angle);
                    node.Y = radius * Math.Sin(angle);
                }
                if (double.IsNaN(node.Vx) || double.IsNaN(node.Vy))
                {
                    node.Vx = node.Vy = 0;
                }
            }
        }

        private Force<TNode> initializeForce(Force<TNode> force)
        {
            force.Initialize(Nodes, RandomSource);
            return force;
        }

        private void initializeForces()
        {
            foreach (var item in Forces)
            {
                initializeForce(item.Value);
            }
        }
        #endregion

        #region setters
        public Simulation<TNode> SetVelocityDecay(double velocityDecay)
        {
            this.VelocityDecay = velocityDecay;
            return this;
        }
        public Simulation<TNode> SetAlpha(double alpha)
        {
            this.Alpha = alpha;
            return this;
        }
        public Simulation<TNode> SetAlphaMin(double alphaMin)
        {
            this.AlphaMin = alphaMin;
            return this;
        }
        public Simulation<TNode> SetAlphaDecay(double alphaDecay)
        {
            this.AlphaDecay = alphaDecay;
            return this;
        }
        public Simulation<TNode> SetAlphaTarget(double alphaTarget)
        {
            this.AlphaTarget = alphaTarget;
            return this;
        }
        public Simulation<TNode> SetFPS(int fps)
        {
            this.FPS = fps;
            return this;
        }

        public Simulation<TNode> SetNodes(List<TNode> nodes)
        {
            this.Nodes = nodes;
            return this;
        }
        public Simulation<TNode> SetRandomSource(IRandom randomSource)
        {
            this.RandomSource = randomSource;
            return this;
        }
        #endregion

        #region forces
        public Simulation<TNode> AddForce(string name, Force<TNode> force)
        {
            if (name == null || force == null)
                throw new ArgumentNullException();
            Forces.Add(name, initializeForce(force));
            return this;
        }
        public Simulation<TNode> RemoveForce(string name)
        {
            Forces.Remove(name);
            return this;
        }
        public Force<TNode> GetForce(string name)
        {
            return Forces[name];
        }
        #endregion

        #region timer
        public Simulation<TNode> Stop()
        {
            Events?.Invoke(this, "stopping");
            stepper.Stop();
            return this;
        }

        public Simulation<TNode> Start()
        {
            Events?.Invoke(this, "starting");
            stepper.Start();
            return this;
        }
        #endregion

        #region events 
        public Simulation<TNode> On(string name, Action action)
        {
            if (action != null)
            {
                Events += (sender, args) =>
                {
                    if (name == args)
                        action?.Invoke();
                };
            }
            return this;
        }

        public Simulation<TNode> Tick(int iterations = 1)
        {
            if (iterations == 0)
                iterations = 1;
            for (int k = 0; k < iterations; k++)
            {
                Alpha += (AlphaTarget - Alpha) * AlphaDecay;
                foreach (var force in Forces)
                {
                    force.Value.UseForce(Alpha);
                }

                int n = Nodes.Count;
                TNode node;
                for (int i = 0; i < n; i++)
                {
                    node = Nodes[i];
                    if (double.IsNaN(node.Fx))
                        node.X += node.Vx *= velocityDecayReve;
                    else
                    {
                        node.X = node.Fx;
                        node.Vx = 0;
                    }
                    if (double.IsNaN(node.Fy))
                        node.Y += node.Vy *= velocityDecayReve;
                    else
                    {
                        node.Y = node.Fy;
                        node.Vy = 0;
                    }
                }
            }

            return this;
        }
        #endregion
    }
}
