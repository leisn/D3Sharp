using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Media;

using D3Sharp.Demo.Model;
using D3Sharp.Force;

namespace D3Sharp.Demo.ViewModel
{
    public class ForceDemoViewModel : BaseViewModel
    {
        int iterations = 0;
        bool isRnning = false;
        DemoNode fixedItem;
        DemoNode oldFixedItem;

        public List<DemoNode> Nodes;
        public List<DemoLink> Links;
        public Simulation<DemoNode> simulation;

        public event Action<SimulationState> SimulationEvent;

        #region forces
        ForceLink<DemoNode, DemoLink> forceLink;
        ForceCenter<DemoNode> forceCenter;
        ForceCollide<DemoNode> forceCollide;
        ForceManyBody<DemoNode> forceManyBody;
        ForceRadial<DemoNode> forceRadial;
        ForceX<DemoNode> forceX;
        ForceY<DemoNode> forceY;
        ForceBounded<DemoNode> forceBounded;
        #endregion

        public ForceDemoViewModel()
        {
            inintNodesAndLinks();
            initSimulationAndForces();
        }

        public DemoNode FixedItem
        {
            get => fixedItem;
            set
            {
                if (fixedItem != null)
                    fixedItem.Fx = fixedItem.Fy = double.NaN;
                SetProperty(ref fixedItem, value);
                if (value != null)
                {
                    oldFixedItem = value;
                    this.fixedItem.Fx = Cx;
                    this.fixedItem.Fy = Cy;
                }
            }
        }

        public void RestoreFixedItem() => this.FixedItem = oldFixedItem;

        public int Iterations
        {
            get => iterations;
            set => SetProperty(ref iterations, value);
        }
        public bool IsRunning
        {
            get => isRnning;
            set => SetProperty(ref isRnning, value, onChanged: () => { OnPropertyChanged(nameof(RunBunttonEnabled)); });
        }

        public bool RunBunttonEnabled => !IsRunning;

        public void Reset(double cx, double cy)
        {
            if (isRnning)
                return;
            Cx = cx;
            Cy = cy;
            XX = cx;
            YY = cy;
            simulation.ResetRandom();
            foreach (var item in Nodes)
            {
                if (item == FixedItem)
                {
                    item.Fx = cx;
                    item.Fy = cy;
                }
                else
                {
                    item.Fx = double.NaN;
                    item.Fy = double.NaN;
                }
                item.X = cx;
                item.Y = cy;
                item.Vx = 0;
                item.Vy = 0;
            }
        }

        public void Excute()
        {
            if (isRnning)
                return;
            Iterations = 0;
            simulation.Start();
        }

        public string FinalExcuteString
        {
            get
            {
                var str = $"var simulation = new Simulation<DemoNode>(Nodes, interval: {Interval})";
                str += Environment.NewLine +
                   $"    .SetAlphaMin({AlphaMin}).SetAlphaTarget({AlphaTarget}).SetAlpha({Alpha}).SetAlphaDecay({AlphaDecay}).SetVelocityDecay({VelocityDecay})";
                if (LinksEnable)
                    str += Environment.NewLine + "    .AddForce(\"Links\", new ForceLink<DemoNode, DemoLink>(Links)" +
                        $".SetIterations({LinksIterations})" +
                        $".SetDistance({LinksDistance}))";
                if (CenteringEnable)
                    str += Environment.NewLine + $"    .AddForce(\"Centering\", new ForceCenter<DemoNode>({Cx},{Cy})" +
                        $".SetStrength({CenteringStrength}))";
                if (CollisionEnable)
                    str += Environment.NewLine + $"    .AddForce(\"Collision\", new ForceCollide<DemoNode>({collisionRadius})" +
                        $".SetIterations({CollisionIterations})" +
                        $".SetStrength({CollisionStrength}))";
                if (ManyBodyEnable)
                    str += Environment.NewLine + "    .AddForce(\"Many-Body\", new ForceManyBody<DemoNode>()" +
                        $".SetStrength({ManyBodyStrength})" +
                        $".SetDistanceMin({ManyBodyDistanceMin})" +
                        $".SetDistanceMax({ManyBodyDistanceMax}).SetTheta({ManyBodyTheta}))";
                if (RadialEnable)
                    str += Environment.NewLine + $"    .AddForce(\"Radial\", new ForceRadial<DemoNode>({RadialRadius}, {RadialX}, {RadialY}))";
                if (XEnable)
                    str += Environment.NewLine + $"    .AddForce(\"X\", new ForceX<DemoNode>({XX}).SetStrength({XStrength}))";
                if (YEnable)
                    str += Environment.NewLine + $"    .AddForce(\"Y\", new ForceY<DemoNode>({YY}).SetStrength({YStrength}))";
                if (BoundedEnable)
                    str += Environment.NewLine + $"    .AddForce(\"Bounded\", new ForceBounded<DemoNode>({BoundedX1}, {BoundedY1}, {BoundedX2}, {BoundedY2})" +
                        $".SetStrength({BoundedStrength}))";
                return str + ";" + Environment.NewLine + "simulation.Start();";
            }
        }

        void inintNodesAndLinks()
        {
            Nodes = new List<DemoNode>
            {
               /*0*/ new DemoNode { Name = "Simulation", Brush = new SolidColorBrush(Color.FromRgb(0xE7, 0x12, 0x24)) },
               /*1*/ new DemoNode { Name = "Force", Brush = new SolidColorBrush(Color.FromRgb(0x00, 0xA0, 0xD7)), Type = NodeType.Abstract },
               /*2*/ new DemoNode { Name = "ForceCenter", Brush = new SolidColorBrush(Color.FromRgb(0x00, 0x7a, 0xcc)) },
               /*3*/ new DemoNode { Name = "ForceCollide", Brush = new SolidColorBrush(Color.FromRgb(0xF6, 0x63, 0x0D)) },
               /*4*/ new DemoNode { Name = "ForceLink", Brush = new SolidColorBrush(Color.FromRgb(0xC1, 0x00, 0x52)) },
               /*5*/ new DemoNode { Name = "ForceManyBody", Brush = new SolidColorBrush(Color.FromRgb(0x00, 0x69, 0xAF)) },
               /*6*/ new DemoNode { Name = "ForceRadial", Brush = new SolidColorBrush(Color.FromRgb(0x5B, 0x2D, 0x90)) },
               /*7*/ new DemoNode { Name = "ForceX", Brush = new SolidColorBrush(Color.FromRgb(0x00, 0x4F, 0x8B)) },
               /*8*/ new DemoNode { Name = "ForceY", Brush = new SolidColorBrush(Color.FromRgb(0xAB, 0x00, 0x8B)) },
               /*9*/ new DemoNode { Name = "IRandom", Brush = new SolidColorBrush(Color.FromRgb(0x00, 0xB4, 0x4B)), Type = NodeType.Interface },
              /*10*/ new DemoNode { Name = "Link", Brush = new SolidColorBrush(Color.FromRgb(0x7E, 0xC4, 0x00)) },
              /*11*/ new DemoNode { Name = "INode", Brush = new SolidColorBrush(Color.FromRgb(0xC6, 0xA3, 0x77)), Type = NodeType.Interface },
              /*12*/ new DemoNode { Name = "Node", Brush = new SolidColorBrush(Color.FromRgb(0xD2, 0x00, 0x7B)) },
              /*13*/ new DemoNode { Name = "QuadTree", Brush = new SolidColorBrush(Color.FromRgb(0x00, 0x8C, 0x3A)) },
              /*14*/ new DemoNode { Name = "QuadNode", Brush = new SolidColorBrush(Color.FromRgb(0x1E, 0xC3, 0xB4)) },
              /*15*/ new DemoNode { Name = "IQuadData", Brush = new SolidColorBrush(Color.FromRgb(0x84, 0x93, 0x98)), Type = NodeType.Interface },
              /*16*/ new DemoNode { Name = "ForceBounded", Brush = new SolidColorBrush(Color.FromRgb(0x00, 0x7C, 0xAA)) },
            };

            Links = new List<DemoLink>
            {
                new DemoLink { Source = Nodes[11], Target = Nodes[15], Relationship=Relationship.Generalization},
                new DemoLink { Source = Nodes[12], Target = Nodes[11], Relationship = Relationship.Realization},

                new DemoLink { Source = Nodes[2], Target = Nodes[1], Relationship=Relationship.Generalization},
                new DemoLink { Source = Nodes[3], Target = Nodes[1], Relationship=Relationship.Generalization},
                new DemoLink { Source = Nodes[4], Target = Nodes[1], Relationship=Relationship.Generalization},
                new DemoLink { Source = Nodes[5], Target = Nodes[1], Relationship=Relationship.Generalization},
                new DemoLink { Source = Nodes[6], Target = Nodes[1], Relationship=Relationship.Generalization},
                new DemoLink { Source = Nodes[7], Target = Nodes[1], Relationship=Relationship.Generalization},
                new DemoLink { Source = Nodes[8], Target = Nodes[1], Relationship=Relationship.Generalization},
                new DemoLink { Source = Nodes[16], Target = Nodes[1], Relationship=Relationship.Generalization},

                new DemoLink { Source = Nodes[1], Target = Nodes[0], Relationship = Relationship.Association},
                new DemoLink { Source = Nodes[9], Target = Nodes[1], Relationship = Relationship.Aggregation},

                new DemoLink { Source = Nodes[0], Target = Nodes[11], Relationship = Relationship.Dependency},
                new DemoLink { Source = Nodes[1], Target = Nodes[11], Relationship = Relationship.Dependency},
                new DemoLink { Source = Nodes[5], Target = Nodes[13], Relationship = Relationship.Dependency},
                new DemoLink { Source = Nodes[3], Target = Nodes[13], Relationship = Relationship.Dependency},
                new DemoLink { Source = Nodes[13], Target = Nodes[15], Relationship = Relationship.Dependency},
                new DemoLink { Source = Nodes[4], Target = Nodes[10], Relationship = Relationship.Dependency},

                new DemoLink { Source = Nodes[14], Target = Nodes[13], Relationship = Relationship.Composition},
            };
        }

        void initSimulationAndForces()
        {
            simulation = new Simulation<DemoNode>(Nodes).SetInterval(Interval)
                .SetAlpha(Alpha).SetAlphaDecay(AlphaDecay).SetAlphaMin(AlphaMin).SetAlphaTarget(AlphaTarget)
                .SetVelocityDecay(VelocityDecay);
            forceLink = new ForceLink<DemoNode, DemoLink>(Links)
                .SetIterations(LinksIterations).SetDistance(LinksDistance);
            forceCenter = new ForceCenter<DemoNode>(Cx, Cy).SetStrength(CenteringStrength);
            forceCollide = new ForceCollide<DemoNode>(CollisionRadius)
                 .SetIterations(CollisionIterations).SetStrength(CollisionStrength);
            forceManyBody = new ForceManyBody<DemoNode>()
                .SetStrength(ManyBodyStrength).SetDistanceMin(ManyBodyDistanceMin)
                .SetDistanceMax(ManyBodyDistanceMax).SetTheta(ManyBodyTheta);
            forceRadial = new ForceRadial<DemoNode>(RadialRadius, RadialX, RadialY)
                .SetStrength(RadialStrength);
            forceX = new ForceX<DemoNode>(XX).SetStrength(XStrength);
            forceY = new ForceY<DemoNode>(YY).SetStrength(YStrength);
            forceBounded = new ForceBounded<DemoNode>(BoundedX1, BoundedY1, BoundedX2, BoundedY2)
                .SetStrength(BoundedStrength);
            simulation
                .AddForce(nameof(forceLink), forceLink)
                .AddForce(nameof(forceCenter), forceCenter)
                .AddForce(nameof(forceCollide), forceCollide)
                .AddForce(nameof(forceManyBody), forceManyBody)
                .AddForce(nameof(forceRadial), forceRadial)
                .AddForce(nameof(forceX), forceX)
                .AddForce(nameof(forceY), forceY)
                .AddForce(nameof(forceBounded), forceBounded);
            simulation.Events += Simulation_Events;
        }

        private void Simulation_Events(object sender, SimulationEventArgs e)
        {
            switch (e.State)
            {
                case SimulationState.Starting:
                    IsRunning = true;
                    break;
                case SimulationState.Ticked:
                    Iterations++;
                    break;
                case SimulationState.End:
                    IsRunning = false;
                    break;
                case SimulationState.Stopping:
                case SimulationState.BeforeTick:
                default:
                    break;
            }
            SimulationEvent?.Invoke(e.State);
        }

        void notifyExcuteStringChanged()
        {
            OnPropertyChanged(nameof(FinalExcuteString));
            //Debug.WriteLine($"link={simulation[nameof(forceLink)]}, ");
            //Debug.WriteLine($"center={simulation[nameof(forceCenter)]}, ");
            //Debug.WriteLine($"collide={simulation[nameof(forceCollide)]}, ");
            //Debug.WriteLine($"manybody={simulation[nameof(forceManyBody)]}, ");
            //Debug.WriteLine($"radial={simulation[nameof(forceRadial)]}, ");
            //Debug.WriteLine($"x={simulation[nameof(forceX)]}, ");
            //Debug.WriteLine($"y={simulation[nameof(forceY)]}.");
        }

        #region Simulation
        double velocityDecay = 0.4;
        public double VelocityDecay
        {
            get => velocityDecay;
            set => SetProperty(ref velocityDecay, value,
                onChanged: () =>
                {
                    simulation.SetVelocityDecay(velocityDecay);
                    notifyExcuteStringChanged();
                });
        }

        double alpha = 1;
        public double Alpha
        {
            get => alpha;
            set => SetProperty(ref alpha, value,
                onChanged: () =>
                {
                    simulation.SetAlpha(alpha);
                    notifyExcuteStringChanged();
                });
        }
        double alphaMin = 0.001;
        public double AlphaMin
        {
            get => alphaMin;
            set => SetProperty(ref alphaMin, value,
                 onChanged: () =>
                 {
                     simulation.SetAlphaMin(alphaMin);
                     notifyExcuteStringChanged();
                 });
        }
        double alphaDecay = 0.0228;
        public double AlphaDecay
        {
            get => alphaDecay;
            set => SetProperty(ref alphaDecay, value,
                onChanged: () =>
                {
                    simulation.SetAlphaDecay(alphaDecay);
                    notifyExcuteStringChanged();
                });
        }
        double alphaTarget = 0;
        public double AlphaTarget
        {
            get => alphaTarget;
            set => SetProperty(ref alphaTarget, value,
                onChanged: () =>
                {
                    simulation.SetAlphaTarget(alphaTarget);
                    notifyExcuteStringChanged();
                });
        }
        int interval = 5;
        public int Interval
        {
            get => interval;
            set => SetProperty(ref interval, value,
                onChanged: () =>
                {
                    simulation.SetInterval(interval);
                    notifyExcuteStringChanged();
                });
        }
        #endregion

        #region Centering
        bool centeringEnable = true;
        public bool CenteringEnable
        {
            get => centeringEnable;
            set => SetProperty(ref centeringEnable, value,
                onChanged: () =>
                {
                    if (centeringEnable)
                        simulation[nameof(forceCenter)] = forceCenter;
                    else
                        simulation.RemoveForce(nameof(forceCenter));
                    notifyExcuteStringChanged();
                });
        }

        double cx = 0;
        public double Cx
        {
            get => cx;
            set => SetProperty(ref cx, value,
                onChanged: () =>
                {
                    forceCenter.SetCx(cx);
                    notifyExcuteStringChanged();
                });
        }

        double cy = 0;
        public double Cy
        {
            get => cy;
            set => SetProperty(ref cy, value,
                 onChanged: () =>
                 {
                     forceCenter.SetCy(cy);
                     notifyExcuteStringChanged();
                 });
        }

        double centeringStrength = 1;
        public double CenteringStrength
        {
            get => centeringStrength;
            set => SetProperty(ref centeringStrength, value,
                onChanged: () =>
                {
                    forceCenter.SetStrength(centeringStrength);
                    notifyExcuteStringChanged();
                });
        }
        #endregion

        #region Collision
        bool collisionEnable = true;
        public bool CollisionEnable
        {
            get => collisionEnable;
            set => SetProperty(ref collisionEnable, value,
                onChanged: () =>
                {
                    if (collisionEnable)
                        simulation[nameof(forceCollide)] = forceCollide;
                    else
                        simulation.RemoveForce(nameof(forceCollide));
                    notifyExcuteStringChanged();
                });
        }

        double collisionRadius = 49;
        public double CollisionRadius
        {
            get => collisionRadius;
            set => SetProperty(ref collisionRadius, value,
                 onChanged: () =>
                 {
                     forceCollide.SetRadius(collisionRadius);
                     notifyExcuteStringChanged();
                     OnPropertyChanged(nameof(Cx));
                     OnPropertyChanged(nameof(Cy));
                 });
        }
        int collisionIterations = 5;
        public int CollisionIterations
        {
            get => collisionIterations;
            set => SetProperty(ref collisionIterations, value,
                onChanged: () =>
                {
                    forceCollide.SetIterations(collisionIterations);
                    notifyExcuteStringChanged();
                });
        }
        double collisionStrength = 0.3;
        public double CollisionStrength
        {
            get => collisionStrength;
            set => SetProperty(ref collisionStrength, value,
                 onChanged: () =>
                 {
                     forceCollide.SetStrength(collisionStrength);
                     notifyExcuteStringChanged();
                 });
        }
        #endregion

        #region Links
        bool linksEnable = true;
        public bool LinksEnable
        {
            get => linksEnable;
            set => SetProperty(ref linksEnable, value,
                onChanged: () =>
                {
                    if (linksEnable)
                        simulation[nameof(forceLink)] = forceLink;
                    else
                        simulation.RemoveForce(nameof(forceLink));
                    notifyExcuteStringChanged();
                });
        }

        int linksIterations = 1;
        public int LinksIterations
        {
            get => linksIterations;
            set => SetProperty(ref linksIterations, value,
                 onChanged: () =>
                 {
                     forceLink.SetIterations(linksIterations);
                     notifyExcuteStringChanged();
                 });
        }
        double linksDistance = 30;
        public double LinksDistance
        {
            get => linksDistance;
            set => SetProperty(ref linksDistance, value,
                 onChanged: () =>
                 {
                     forceLink.SetDistance(linksDistance);
                     notifyExcuteStringChanged();
                 });
        }
        #endregion

        #region Many-Body
        bool manyBodyEnable = true;
        public bool ManyBodyEnable
        {
            get => manyBodyEnable;
            set => SetProperty(ref manyBodyEnable, value,
                onChanged: () =>
                {
                    if (manyBodyEnable)
                        simulation[nameof(forceManyBody)] = forceManyBody;
                    else
                        simulation.RemoveForce(nameof(forceManyBody));
                    notifyExcuteStringChanged();
                });
        }

        double manyBodyStrength = -100;
        public double ManyBodyStrength
        {
            get => manyBodyStrength;
            set => SetProperty(ref manyBodyStrength, value,
                 onChanged: () =>
                 {
                     forceManyBody.SetStrength(manyBodyStrength);
                     notifyExcuteStringChanged();
                 });
        }
        double manyBodyDistanceMin = 1;
        public double ManyBodyDistanceMin
        {
            get => manyBodyDistanceMin;
            set => SetProperty(ref manyBodyDistanceMin, value,
                onChanged: () =>
                {
                    forceManyBody.SetDistanceMin(manyBodyDistanceMin);
                    notifyExcuteStringChanged();
                });
        }
        double manyBodyDistanceMax = double.PositiveInfinity;
        public double ManyBodyDistanceMax
        {
            get => manyBodyDistanceMax;
            set => SetProperty(ref manyBodyDistanceMax, value,
                onChanged: () =>
                {
                    forceManyBody.SetDistanceMax(manyBodyDistanceMax);
                    notifyExcuteStringChanged();
                });
        }
        double manyBodyTheta = 0.9;
        public double ManyBodyTheta
        {
            get => manyBodyTheta;
            set => SetProperty(ref manyBodyTheta, value,
                 onChanged: () =>
                 {
                     forceManyBody.SetTheta(manyBodyTheta);
                     notifyExcuteStringChanged();
                 });
        }
        #endregion

        #region Radial
        bool radialEnable = true;
        public bool RadialEnable
        {
            get => radialEnable;
            set => SetProperty(ref radialEnable, value,
                onChanged: () =>
                {
                    if (radialEnable)
                        simulation[nameof(forceRadial)] = forceRadial;
                    else
                        simulation.RemoveForce(nameof(forceRadial));
                    notifyExcuteStringChanged();
                });
        }

        double radialRadius = 0;
        public double RadialRadius
        {
            get => radialRadius;
            set => SetProperty(ref radialRadius, value,
                 onChanged: () =>
                 {
                     forceRadial.SetRadius(radialRadius);
                     notifyExcuteStringChanged();
                 });
        }
        double radialX = 0;
        public double RadialX
        {
            get => radialX;
            set => SetProperty(ref radialX, value,
                onChanged: () =>
                {
                    forceRadial.SetRadialX(radialX);
                    notifyExcuteStringChanged();
                });
        }
        double radialY = 0;
        public double RadialY
        {
            get => radialY;
            set => SetProperty(ref radialY, value,
                 onChanged: () =>
                 {
                     forceRadial.SetRadialY(radialY);
                     notifyExcuteStringChanged();
                 });
        }
        double radialStrength = 0.1;
        public double RadialStrength
        {
            get => radialStrength;
            set => SetProperty(ref radialStrength, value,
                 onChanged: () =>
                 {
                     forceRadial.SetStrength(radialStrength);
                     notifyExcuteStringChanged();
                 });
        }
        #endregion

        #region X
        bool xEnable = true;
        public bool XEnable
        {
            get => xEnable;
            set => SetProperty(ref xEnable, value,
                onChanged: () =>
                {
                    if (xEnable)
                        simulation[nameof(forceX)] = forceX;
                    else
                        simulation.RemoveForce(nameof(forceX));
                    notifyExcuteStringChanged();
                });
        }

        double xX = 0;
        public double XX
        {
            get => xX;
            set => SetProperty(ref xX, value,
                 onChanged: () =>
                 {
                     forceX.SetX(xX);
                     notifyExcuteStringChanged();
                 });
        }
        double xStrength = 0.1;
        public double XStrength
        {
            get => xStrength;
            set => SetProperty(ref xStrength, value,
                onChanged: () =>
                {
                    forceX.SetStrength(xStrength);
                    notifyExcuteStringChanged();
                });
        }
        #endregion

        #region Y
        bool yEnable = true;
        public bool YEnable
        {
            get => yEnable;
            set => SetProperty(ref yEnable, value,
                onChanged: () =>
                {
                    if (yEnable)
                        simulation[nameof(forceY)] = forceY;
                    else
                        simulation.RemoveForce(nameof(forceY));
                    notifyExcuteStringChanged();
                });
        }

        double yY = 0;
        public double YY
        {
            get => yY;
            set => SetProperty(ref yY, value,
                onChanged: () =>
                {
                    forceY.SetY(yY);
                    notifyExcuteStringChanged();
                });
        }
        double yStrength = 0.1;
        public double YStrength
        {
            get => yStrength;
            set => SetProperty(ref yStrength, value,
                onChanged: () =>
                {
                    forceY.SetStrength(yStrength);
                    notifyExcuteStringChanged();
                });
        }
        #endregion

        #region Bounded
        bool boundedEnable = true;
        public bool BoundedEnable
        {
            get => boundedEnable;
            set => SetProperty(ref boundedEnable, value,
                onChanged: () =>
                {
                    if (boundedEnable)
                        simulation[nameof(forceBounded)] = forceBounded;
                    else
                        simulation.RemoveForce(nameof(forceBounded));
                    notifyExcuteStringChanged();
                });
        }

        double boundedX1 = 20;
        public double BoundedX1
        {
            get => boundedX1;
            set => SetProperty(ref boundedX1, value,
                onChanged: () =>
                {
                    forceBounded.X1 = boundedX1;
                    notifyExcuteStringChanged();
                });
        }

        double boundedX2 = double.PositiveInfinity;
        public double BoundedX2
        {
            get => boundedX2;
            set => SetProperty(ref boundedX2, value,
                onChanged: () =>
                {
                    forceBounded.X2 = boundedX2;
                    notifyExcuteStringChanged();
                });
        }

        double boundedY1 = 20;
        public double BoundedY1
        {
            get => boundedY1;
            set => SetProperty(ref boundedY1, value,
                onChanged: () =>
                {
                    forceBounded.Y1 = boundedY1;
                    notifyExcuteStringChanged();
                });
        }

        double boundedY2 = double.PositiveInfinity;
        public double BoundedY2
        {
            get => boundedY2;
            set => SetProperty(ref boundedY2, value,
                onChanged: () =>
                {
                    forceBounded.Y2 = boundedY2;
                    notifyExcuteStringChanged();
                });
        }

        double boundedStrength = 1;
        public double BoundedStrength
        {
            get => boundedStrength;
            set => SetProperty(ref boundedStrength, value,
                onChanged: () =>
                {
                    forceBounded.SetStrength(boundedStrength);
                    notifyExcuteStringChanged();
                });
        }
        #endregion

    }
}
