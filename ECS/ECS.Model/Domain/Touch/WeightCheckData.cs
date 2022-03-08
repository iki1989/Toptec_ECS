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
    public struct WeightCheckData : IReaderConvertable
    {
        private const string TIMESTRING = "yy-MM-dd HH:mm:ss";

        public static WeightCheckData None = new WeightCheckData();
        public string BoxId { get; set; }
        public string BoxType { get; set; }
        public string CaseErectedAt { get; set; }
        public string MinWeight { get; set; }
        public string StandardWht { get; set; }
        public string MeasureWht { get; set; }
        public string MaxWeight { get; set; }
        public string CheckedAt { get; set; }
        public string Verification { get; set; }
        public void Convert(SqlDataReader reader)
        {
            #region Init
            this.BoxId = string.Empty;
            this.BoxType = string.Empty;
            this.CaseErectedAt = string.Empty;
            this.MinWeight = string.Empty;
            this.StandardWht = string.Empty;
            this.MeasureWht = string.Empty;
            this.MaxWeight = string.Empty;
            this.CheckedAt = string.Empty;
            this.Verification = string.Empty;
            #endregion

            this.BoxId = reader["BOX_ID"].ToString().Trim();
            this.CaseErectedAt = reader["CASE_ERECTED_AT"].ToString().Trim();
            this.StandardWht = reader["STANDARD_WHT"].ToString().Trim();
            this.MeasureWht = reader["MEASURE_WHT"].ToString().Trim();
            this.CheckedAt = reader["CHECKED_AT"].ToString().Trim();
            this.Verification = reader["VERIFICATION"].ToString().Trim();

            if (DateTime.TryParse(this.CaseErectedAt, out DateTime caseErectedAt))
                this.CaseErectedAt = caseErectedAt.ToString(TIMESTRING);

            if (DateTime.TryParse(this.CheckedAt, out DateTime checkedAt))
                this.CheckedAt = checkedAt.ToString(TIMESTRING);

            double minWeight = 0;
            double maxWeight = 0;
            if (double.TryParse(this.StandardWht, out double standardWht))
            {
                if (standardWht > 0)
                {
                    minWeight = (standardWht - (standardWht * 0.05));
                    maxWeight = (standardWht + (standardWht * 0.05));
                    if (minWeight < 0)
                        minWeight = 0;

                    this.MinWeight = this.RoemoveOverDecimalPoint($"{minWeight}", 3);
                    this.MaxWeight = this.RoemoveOverDecimalPoint($"{maxWeight}", 3);
                }
            }

            //Floor(1자) + BoxType(1자) + 호기(1자) + 박스번호(7자)
            if (this.BoxId.Length > 1)
                this.BoxType = this.BoxId.Substring(1, 1);

            switch (this.Verification)
            {
                case "OK":
                    this.Verification = "정상";
                    break;
                case "NOREAD":
                    this.Verification = "미인식";
                    break;
                case "NOWEIGHT":
                    this.Verification = "무게없음";
                    break;
                case "OVER":
                    this.Verification = "초과";
                    break;
                case "UNDER":
                    this.Verification = "미만";
                    break;
                case "ORDERCANCEL":
                    this.Verification = "주문취소";
                    break;
                default:
                    break;
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        private string RoemoveOverDecimalPoint(string value, int needPoint)
        {
            var splited = value.Split('.');
            if (splited.Length != 2) return value;

            if (splited[1].Length < needPoint) return value;

            return $"{splited[0]}.{splited[1].Substring(0, needPoint)}"; ;
        }
    }
}
