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
    public struct DailyCountingData : IReaderConvertable
    {
        public static DailyCountingData None = new DailyCountingData() { };
        public string DATE { get; set; }
        public CountingData DATA { get; set; }
        public int NON_OUT_COUNT { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.DATE = DomainUtil.GetValueOrNull<DateTime>(reader, nameof(DATE))?.ToString("yyyy-MM-dd") ?? "";
            var data = new CountingData();
            data.Convert(reader);
            this.DATA = data;
            this.NON_OUT_COUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(NON_OUT_COUNT)) ?? 0;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
