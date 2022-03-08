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
    public struct InvoiceData : IReaderConvertable
    {
        public static InvoiceData None = new InvoiceData();
        public string BoxId { get; set; }
        public string InvoiceId { get; set; }
        public string CstOrdNo { get; set; }
        public string IsPicking { get; set; }
        public string IsCanceled { get; set; }
        public string Zpl { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.BoxId = reader["BOX_ID"].ToString();
            this.InvoiceId = reader["INVOICE_ID"].ToString();
            this.CstOrdNo = reader["CST_ORD_NO"].ToString();
            this.IsPicking = this.BoxId == "" ? "" : "O";
            this.IsCanceled = reader["ORDER_CANCEL"].ToString() == "Y" ? "O" : "";
            this.Zpl = reader["INVOICE_ZPL"].ToString();
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
