using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Domain.Touch
{
    public struct TouchParam
    {
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public string CstOrdNo { get; set; }
        public string BoxId { get; set; }
        public string InvoiceId { get; set; }
        public long? BcrIndex { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
