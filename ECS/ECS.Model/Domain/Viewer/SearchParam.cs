using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Domain.Viewer
{
    public struct SearchParam
    {
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public string WaveNo { get; set; }
        public string CstCd { get; set; }
        public string CstOrdNo { get; set; }
        public string BoxId { get; set; }
        public string InvoiceId { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
