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
    public struct InvoicePrintData : IReaderConvertable
    {
        public static InvoicePrintData None = new InvoicePrintData();
        public string Line { get; set; }
        public string PrintResult { get; set; }
        public string PrintedAt { get; set; }
        public long? TopBcrIndex { get; set; }
        public string Verification { get; set; }
        public string VerificatedAt { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.Line = reader["LINE"].ToString();
            this.PrintResult = reader["PRINT_RESULT"].ToString();
            switch (this.PrintResult)
            {
                case "OK":
                    this.PrintResult = "정상";
                    break;
                case "NOREAD":
                    this.PrintResult = "미식별";
                    break;
                case "NOWEIGHT":
                    this.PrintResult = "무게없음";
                    break;
                case "DUPLICATE":
                    this.PrintResult = "중복발행";
                    break;
                default:
                    this.PrintResult = "";
                    break;
            }
            this.PrintedAt = reader["PRINTED_AT"].ToString();
            var bcrIndex = reader["TOP_BCR_INDEX"];
            if (bcrIndex == DBNull.Value)
                this.TopBcrIndex = null;
            else
                this.TopBcrIndex = long.Parse(bcrIndex.ToString());
            this.Verification = reader["VERIFICATION"].ToString();
            switch (this.Verification)
            {
                case "OK":
                    this.Verification = "정상";
                    break;
                case "NOREAD":
                    this.Verification = "미식별";
                    break;
                case "MISMATCH":
                    this.Verification = "미스매치";
                    break;
                case "DUPLICATE":
                    this.Verification = "중복발행";
                    break;
                default:
                    this.Verification = "";
                    break;
            }
            this.VerificatedAt = reader["VERIFICATED_AT"].ToString();
            if (this.PrintedAt != "")
            {
                this.PrintedAt = DateTime.Parse(this.PrintedAt).ToString("yy-MM-dd HH:mm:ss");
            }
            if (this.VerificatedAt != "")
            {
                this.VerificatedAt = DateTime.Parse(this.VerificatedAt).ToString("yy-MM-dd HH:mm:ss");
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
