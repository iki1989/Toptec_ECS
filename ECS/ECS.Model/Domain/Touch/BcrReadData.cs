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
    public enum LineType
    {
        SMART, NORMAL
    }
    public enum ResultType
    {
        OK, NOREAD, NOWEIGHT, DUPLICATE, WEIGHT_FAIL
    }
    public enum VerificationType
    {
        NONE, OK, NOREAD, MISMATCH, DUPLICATE, HEIGHT_MISMATCH
    }
    public enum BcrType
    {
        ROUTE, PRINT, TOP, OUT
    }
    public struct BcrReadData : IReaderConvertable
    {
        private ResultType ResultType;
        private VerificationType VerificationType;

        public static BcrReadData None = new BcrReadData();
        public long Index { get; set; }
        public string Queried { get; set; }
        public string BoxId { get; set; }
        public string InvoiceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public BcrType BcrType { get; set; }
        public LineType Line { get; set; }
        public string Result { get; set; }
        public string Verification { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.Index = long.Parse(reader["INDEX"].ToString());
            this.Queried = "";
            this.BoxId = reader["BOX_ID"].ToString();
            this.InvoiceId = reader["INVOICE_ID"].ToString();
            this.CreatedAt = DateTime.MinValue;
            DateTime date;
            if (DateTime.TryParse(reader["CREATED_AT"].ToString(), out date))
                this.CreatedAt = date;
            this.BcrType = (BcrType)Enum.Parse(typeof(BcrType), reader["BCR_TYPE"].ToString());

            string line = reader["LINE"].ToString();
            if (Enum.TryParse(line, out LineType lineType))
                this.Line = lineType;
            else
            {
                if (line == "Y")
                    this.Line = LineType.NORMAL;
                else
                    this.Line = LineType.SMART;
            }

            this.ResultType = (ResultType)Enum.Parse(typeof(ResultType), reader["RESULT"].ToString());
            switch (this.ResultType)
            {
                case ResultType.OK:
                    this.Result = "정상";
                    break;
                case ResultType.NOREAD:
                    this.Result = "미식별";
                    break;
                case ResultType.NOWEIGHT:
                    this.Result = "무게없음";
                    break;
                case ResultType.DUPLICATE:
                    this.Result = "중복발행";
                    break;
                default:
                    this.Result = "";
                    break;
            }
            this.VerificationType = (VerificationType)Enum.Parse(typeof(VerificationType), reader["VERIFICATION"].ToString());
            switch (this.VerificationType)
            {
                case VerificationType.OK:
                    this.Verification = "정상";
                    break;
                case VerificationType.NOREAD:
                    this.Verification = "미식별";
                    break;
                case VerificationType.MISMATCH:
                    this.Verification = "미스매치";
                    break;
                case VerificationType.DUPLICATE:
                    this.Verification = "중복발행";
                    break;
                default:
                    this.Verification = "";
                    break;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public bool Equals(BcrReadData obj)
        {
            return this.Index == obj.Index;
        }


        public static bool operator <(BcrReadData left, BcrReadData right) => left.Index < right.Index;
        public static bool operator >(BcrReadData left, BcrReadData right) => !(left < right) && !left.Equals(right);
    }
}
