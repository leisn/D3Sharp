using System;
using System.Collections.Generic;
using System.Text;

using D3Sharp.QuadTree;

namespace D3Sharp.Test.QuadTree
{
    public class CustomData : IQuadData
    {
        public double X { get; set; } = double.NaN;
        public double Y { get; set; } = double.NaN;

        public string Location => X + "," + Y;
        public int Id { get; set; }
    }

}
