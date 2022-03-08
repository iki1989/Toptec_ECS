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
    public struct OutBcrData : IReaderConvertable
    {
        public static OutBcrData None = new OutBcrData() { };
        public int NO { get; set; }
        public string WH_ID { get; set; }
        public string OUT_TIME { get; set; }
        public string WAVE_NO { get; set; }
        public string CST_CD { get; set; }
        public string CST_ORD_NO { get; set; }
        public string BOX_TYPE_CD { get; set; }
        public string BOX_ID { get; set; }
        public string INVOICE_ID { get; set; }
        public string LINE { get; set; }
        public string WEIGHT_TIME { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.NO = int.Parse(reader[nameof(NO)]?.ToString() ?? "0");
            this.WH_ID = reader[nameof(WH_ID)]?.ToString() ?? "";
            this.OUT_TIME = "";
            this.WAVE_NO = reader[nameof(WAVE_NO)]?.ToString() ?? "";
            this.CST_CD = reader[nameof(CST_CD)]?.ToString() ?? "";
            this.CST_ORD_NO = reader[nameof(CST_ORD_NO)]?.ToString() ?? "";
            this.BOX_TYPE_CD = reader[nameof(BOX_TYPE_CD)]?.ToString() ?? "";
            this.BOX_ID = reader[nameof(BOX_ID)]?.ToString() ?? "";
            this.INVOICE_ID = reader[nameof(INVOICE_ID)]?.ToString() ?? "";
            this.LINE = reader[nameof(LINE)]?.ToString() ?? "";
            this.WEIGHT_TIME = "";
            DateTime date;
            if (DateTime.TryParse(reader[nameof(OUT_TIME)]?.ToString(), out date))
            {
                this.OUT_TIME = date.ToString("yy-MM-dd HH:mm:ss");
            }
            if (DateTime.TryParse(reader[nameof(WEIGHT_TIME)]?.ToString(), out date))
            {
                this.WEIGHT_TIME = date.ToString("yy-MM-dd HH:mm:ss");
            }
            if (this.BOX_ID == "")
                this.LINE = "";
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
