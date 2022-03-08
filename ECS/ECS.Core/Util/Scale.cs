using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.Util
{
    public static class Scale
    {
        public static double GetDecimalConversion(int number, int point) => Convert.ToDouble(number) / Math.Pow(10, point);
    }
}
