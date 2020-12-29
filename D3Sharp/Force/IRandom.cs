using System;
using System.Collections.Generic;
using System.Text;

namespace D3Sharp.Force
{
    public interface IRandom
    {
        /// <summary>
        /// 返回0-1之间的小数
        /// </summary>
        /// <returns></returns>
        double Next();
    }

    public class Lcg : IRandom
    {
        const long a = 1664525;
        const long c = 1013904223;
        const long m = 4294967296; // 2^32
        double seed = 1;

        public Lcg()
        {
            //Next();
        }

        public double Next()
        {
            seed = (a * seed + c) % m;
            System.Diagnostics.Debug.WriteLine(seed);
            return seed / m;
        }
    }
}
