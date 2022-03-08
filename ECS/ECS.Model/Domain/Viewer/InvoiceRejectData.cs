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
    public struct InvoiceRejectData : IReaderConvertable
    {
        public static InvoiceRejectData None = new InvoiceRejectData() { };
        public int NO { get; set; }
        public string WH_ID { get; set; }
        public string REJECT_TIME { get; set; }
        public string WAVE_NO { get; set; }
        public string CST_CD { get; set; }
        public string CST_ORD_NO { get; set; }
        public string BOX_TYPE_CD { get; set; }
        public string BOX_ID { get; set; }
        public string INVOICE_ID { get; set; }
        public string VERIFICATION { get; set; }
        public string OUT_TIME { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.NO = int.Parse(reader[nameof(NO)]?.ToString() ?? "0");
            this.WH_ID = reader[nameof(WH_ID)]?.ToString() ?? "";
            this.REJECT_TIME = "";
            this.WAVE_NO = reader[nameof(WAVE_NO)]?.ToString() ?? "";
            this.CST_CD = reader[nameof(CST_CD)]?.ToString() ?? "";
            this.CST_ORD_NO = reader[nameof(CST_ORD_NO)]?.ToString() ?? "";
            this.INVOICE_ID = reader[nameof(INVOICE_ID)]?.ToString() ?? "";
            this.BOX_TYPE_CD = reader[nameof(BOX_TYPE_CD)]?.ToString() ?? "";
            this.BOX_ID = reader[nameof(BOX_ID)]?.ToString() ?? "";
            this.VERIFICATION = reader[nameof(VERIFICATION)]?.ToString() ?? "";
            this.OUT_TIME = "";
            DateTime date;
            if (DateTime.TryParse(reader[nameof(REJECT_TIME)]?.ToString(), out date))
            {
                this.REJECT_TIME = date.ToString("yy-MM-dd HH:mm:ss");
            }
            if (DateTime.TryParse(reader[nameof(OUT_TIME)]?.ToString(), out date))
            {
                this.OUT_TIME = date.ToString("yy-MM-dd HH:mm:ss");
            }
            this.VerificationConversion();
        }
        private void VerificationConversion()
        {
            switch (this.VERIFICATION)
            {
                case "OK":
                    this.VERIFICATION = "정상";
                    break;
                case "NOREAD":
                    this.VERIFICATION = "미식별";
                    break;
                case "ORDERCANCEL":
                    this.VERIFICATION = "주문취소";
                    break;
                case "NOWEIGHT":
                    this.VERIFICATION = "무게없음";
                    break;
                case "DUPLICATE":
                    this.VERIFICATION = "중복발행";
                    break;
                case "MISMATCH":
                    this.VERIFICATION = "미스매치";
                    break;
                default:
                    break;
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
