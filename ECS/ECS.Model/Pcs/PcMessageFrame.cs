using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Pcs
{
    [Serializable]
    public class PcMessageFrame
    {
        public string Type { get; set; }

        public object Data { get; set; }
    }
}
