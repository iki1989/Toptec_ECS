using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Domain.Touch
{
    public struct SkuData : IReaderConvertable
    {
        public static SkuData None = new SkuData() { BoxInfo = WeightCheckData.None };
        public string BoxId { get; set; }
        public string SkuCd { get; set; }
        public string SkuQty { get; set; }
        public WeightCheckData BoxInfo { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.BoxId = reader["BOX_ID"].ToString();
            this.SkuCd = reader["SKU_CD"].ToString();
            this.SkuQty = reader["SKU_QTY"].ToString();
            this.BoxInfo = WeightCheckData.None;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
