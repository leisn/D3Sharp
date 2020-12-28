using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.QuadTree
{
    public interface IQuadTreeData
    {
        double X { get; set; }
        double Y { get; set; }
    }

    public class QuadTreeData : IQuadTreeData
    {
        public double X
        {
            get;
            set;
        } = double.NaN;

        public double Y
        {
            get;
            set;
        } = double.NaN;
    }
}
