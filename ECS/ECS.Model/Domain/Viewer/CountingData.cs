using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Domain.Viewer
{
    public struct CountingData : IReaderConvertable
    {
        public static CountingData None = new CountingData() { };
        public int ORDER_COUNT { get; set; }
        public int CASE_ERECT_COUNT { get; set; }
        public int CASE_ERECT_REJECT_COUNT { get; set; }
        public int WEIGHT_COUNT { get; set; }
        public int WEIGHT_REJECT_COUNT { get; set; }
        public int SMART_PRINT_COUNT { get; set; }
        public int NORMAL_PRINT_COUNT { get; set; }
        public int TOP_COUNT { get; set; }
        public int TOP_REJECT_COUNT { get; set; }
        public int OUT_COUNT { get; set; }
        public int REAL_OUT_COUNT { get; set; }
        public int PACKING_COUNT { get; set; }
        public int PACKING_REJECT_COUNT { get; set; }
        public int ORDER_CANCEL_COUNT { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.ORDER_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(ORDER_COUNT)) ?? 0;
            this.CASE_ERECT_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(CASE_ERECT_COUNT)) ?? 0;
            this.CASE_ERECT_REJECT_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(CASE_ERECT_REJECT_COUNT)) ?? 0;
            this.WEIGHT_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(WEIGHT_COUNT)) ?? 0;
            this.NORMAL_PRINT_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(NORMAL_PRINT_COUNT)) ?? 0;
            this.WEIGHT_REJECT_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(WEIGHT_REJECT_COUNT)) ?? 0;
            this.SMART_PRINT_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(SMART_PRINT_COUNT)) ?? 0;
            this.TOP_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(TOP_COUNT)) ?? 0;
            this.TOP_REJECT_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(TOP_REJECT_COUNT)) ?? 0;
            this.OUT_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(OUT_COUNT)) ?? 0;
            this.REAL_OUT_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(REAL_OUT_COUNT)) ?? 0;
            this.PACKING_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(PACKING_COUNT)) ?? 0;
            this.PACKING_REJECT_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(PACKING_REJECT_COUNT)) ?? 0;
            this.ORDER_CANCEL_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(ORDER_CANCEL_COUNT)) ?? 0;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
