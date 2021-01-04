using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

using D3Sharp.Demo.ViewModel;
using D3Sharp.Force;

namespace D3Sharp.Demo.Model
{
    public enum NodeType
    {
        Class, Interface, Abstract
    }
    public class DemoNode : BaseViewModel, INode
    {
        private double x = 0;
        private double y = 0;

        public int Index { get; set; }
        public double Fx { get; set; } = double.NaN;
        public double Fy { get; set; } = double.NaN;
        public double Vx { get; set; } = double.NaN;
        public double Vy { get; set; } = double.NaN;

        public Brush Brush { get; set; }
        public string Name { get; set; }
        public NodeType Type { get; set; } = NodeType.Class;

        public double X
        {
            get => x;
            set => SetProperty(ref x, value);
        }
        public double Y
        {
            get => y;
            set => SetProperty(ref y, value);
        }

        public override string ToString()
        {
            return $"{{{Name}-X:{X}, Y:{Y}, Index:{Index}, Vx:{Vx}, Vy:{Vy}, Fx:{Fx}, Fy:{Fy}}}";
        }
    }
}
