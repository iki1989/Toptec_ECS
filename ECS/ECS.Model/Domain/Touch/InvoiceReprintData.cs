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
    public struct InvoiceReprintData : IReaderConvertable
    {

        public static InvoiceReprintData None = new InvoiceReprintData();
        public string BoxId { get; set; }
        public string InvoiceId { get; set; }
        public string Verification { get; set; }
        public string ReprintedAt { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.BoxId = reader["BOX_ID"].ToString();
            this.InvoiceId = reader["INVOICE_ID"].ToString();
            this.Verification = reader["VERIFICATION"].ToString();
            switch (this.Verification)
            {
                case "OK":
                    this.Verification = "정상";
                    break;
                case "":
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
            this.ReprintedAt = reader["REPRINTED_AT"].ToString();
            if (this.ReprintedAt != "")
            {
                this.ReprintedAt = DateTime.Parse(this.ReprintedAt).ToString("yy-MM-dd HH:mm:ss");
            }

        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
