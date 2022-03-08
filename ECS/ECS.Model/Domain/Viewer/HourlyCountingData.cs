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
    public struct HourlyCountingData : IReaderConvertable
    {
        public static HourlyCountingData None = new HourlyCountingData() { };
        public string DATE { get; set; }
        public int HOUR { get; set; }
        public CountingData DATA { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.DATE = DomainUtil.GetValueOrNull<DateTime>(reader, nameof(DATE))?.ToString("yyyy-MM-dd") ?? "";
            this.HOUR = DomainUtil.GetValueOrNull<int>(reader, nameof(HOUR)) ?? 0;
            var data = new CountingData();
            data.Convert(reader);
            this.DATA = data;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
