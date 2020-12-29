using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.Force
{
    public class Helper
    {
        public static int times = 0;
        public static double Jiggle(IRandom random)
        {
            System.Diagnostics.Debug.WriteLine($"Jiggle: {++times}");
            return (random.Next() - 0.5) * 1e-6;
        }
    }
}
