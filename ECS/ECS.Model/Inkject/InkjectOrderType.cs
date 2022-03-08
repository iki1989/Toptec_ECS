using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Inkject
{
    public class InkjectOrderType
    {
        public string BoxType { get; set; }
        public int FromCount { get; set; } = 0;
        public int ToCount { get; set; } = 8999999;
        public int CurrentCount { get; set; }
    }
}
