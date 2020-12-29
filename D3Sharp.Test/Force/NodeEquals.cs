using System;
using System.Collections.Generic;

using D3Sharp.Force;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D3Sharp.Test.Force
{
    public class NodeEquals
    {
        public static bool Equals(Node expected, Node actual, double delta = 1e-6)
        {
            if (expected == actual)
                return true;
            return actual.Index == expected.Index
                && Math.Abs(actual.X - expected.X) < delta
                && Math.Abs(actual.Vx - expected.Vx) < delta
                && Math.Abs(actual.Y - expected.Y) < delta
                && Math.Abs(actual.Vy - expected.Vy) < delta
                && !(Math.Abs(actual.Fx - expected.Fx) > delta)
                && !(Math.Abs(actual.Fy - expected.Fy) > delta);
        }
    }
}
