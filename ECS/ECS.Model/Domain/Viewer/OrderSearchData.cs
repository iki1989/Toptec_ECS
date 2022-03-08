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
    public struct OrderSearchData : IReaderConvertable
    {
        public static OrderSearchData None = new OrderSearchData() { };
        public int NO { get; set; }
        public string WH_ID { get; set; }
        public string WAVE_NO { get; set; }
        public string CST_CD { get; set; }
        public string CST_ORD_NO { get; set; }
        public string BOX_ID { get; set; }
        public string INVOICE_ID { get; set; }
        public string ORDER_CANCEL { get; set; }
        public string BOX_TYPE_CD { get; set; }
        public string WEIGHT_SUM { get; set; }
        public string ORDER_TIME { get; set; }
        public string OUT_TIME { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.NO = int.Parse(reader["NO"]?.ToString() ?? "0");
            this.WH_ID = reader["WH_ID"]?.ToString() ?? "";
            this.WAVE_NO = reader["WAVE_NO"]?.ToString() ?? "";
            this.CST_CD = reader["CST_CD"]?.ToString() ?? "";
            this.CST_ORD_NO = reader["CST_ORD_NO"]?.ToString() ?? "";
            this.BOX_ID = reader["BOX_ID"]?.ToString() ?? "";
            this.INVOICE_ID = reader["INVOICE_ID"]?.ToString() ?? "";
            this.ORDER_CANCEL = (reader["ORDER_CANCEL"]?.ToString() ?? "") == "Y" ? "O" : "";
            this.BOX_TYPE_CD = reader["BOX_TYPE_CD"]?.ToString() ?? "";
            this.WEIGHT_SUM = reader["WEIGHT_SUM"]?.ToString() ?? "";
            this.ORDER_TIME = "";
            this.OUT_TIME = "";
            DateTime date;
            if (DateTime.TryParse(reader["ORDER_TIME"]?.ToString(), out date))
            {
                this.ORDER_TIME = date.ToString("yy-MM-dd HH:mm:ss");
            }
            if (DateTime.TryParse(reader["OUT_TIME"]?.ToString(), out date))
            {
                this.OUT_TIME = date.ToString("yy-MM-dd HH:mm:ss");
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
