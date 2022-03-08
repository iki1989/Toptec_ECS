using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Inkject
{
    public class InkjectBcrReadArg : EventArgs
    {
        public int BcrId { get; set; }
        public string BoxId { get; set; }
        public string BoxType { get; set; }

        public bool IsNoread { get; set; }
    }
}
