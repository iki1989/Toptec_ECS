using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode.Helpers;

namespace ECS.Model.Domain.Touch
{
    public struct CaseErectData : IReaderConvertable
    {
        public static CaseErectData None = new CaseErectData();
        public string BoxId { get; set; }
        public string BoxType { get; set; }
        public string BoxName { get; set; }
        public string ErectorType { get; set; }
        public DateTime ErectedAt { get; set; }
        public string PrintCount { get; set; }
        public string Verification { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.BoxId = reader["BOX_ID"].ToString();
            this.BoxName = "";
            this.ErectorType = reader["ERECTOR_TYPE"].ToString() + "호기";
            DateTime date;
            if (DateTime.TryParse(reader["ERECTED_AT"].ToString(), out date))
                this.ErectedAt = date;
            else
                this.ErectedAt = DateTime.MinValue;
            this.Verification = reader["VERIFICATION"].ToString();
            this.PrintCount = reader["PRINT_COUNT"].ToString();
            this.BoxType = this.BoxId != "" ? this.BoxId[1].ToString() : "";
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
