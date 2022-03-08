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
    public struct SmartPackingRejectData : IReaderConvertable
    {
        public static SmartPackingRejectData None = new SmartPackingRejectData();
        public int NO { get; set; }
        public string WH_ID { get; set; }
        public DateTime? REJECT_TIME { get; set; }
        public string WAVE_NO { get; set; }
        public string CST_CD { get; set; }
        public string CST_ORD_NO { get; set; }
        public string BOX_TYPE_CD { get; set; }
        public string BOX_ID { get; set; }
        public string INVOICE_ID { get; set; }
        public string RESULT { get; set; }
        public DateTime? OUT_TIME { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.NO = int.Parse(reader[nameof(NO)]?.ToString() ?? "0");
            this.WH_ID = reader[nameof(WH_ID)]?.ToString() ?? "";
            this.REJECT_TIME = DomainUtil.GetValueOrNull<DateTime>(reader, nameof(REJECT_TIME));
            this.WAVE_NO = reader[nameof(WAVE_NO)]?.ToString() ?? "";
            this.CST_CD = reader[nameof(CST_CD)]?.ToString() ?? "";
            this.CST_ORD_NO = reader[nameof(CST_ORD_NO)]?.ToString() ?? "";
            this.INVOICE_ID = reader[nameof(INVOICE_ID)]?.ToString() ?? "";
            this.BOX_TYPE_CD = reader[nameof(BOX_TYPE_CD)]?.ToString() ?? "";
            this.BOX_ID = reader[nameof(BOX_ID)]?.ToString() ?? "";
            this.RESULT = reader[nameof(RESULT)]?.ToString() ?? "";
            this.OUT_TIME = DomainUtil.GetValueOrNull<DateTime>(reader, nameof(OUT_TIME));
            this.CovertResult();
        }
        private void CovertResult()
        {
            switch (this.RESULT)
            {
                case "OK":
                    this.RESULT = "정상";
                    break;
                case "BYPASS":
                    this.RESULT = "강제진행";
                    break;
                case "NOREAD":
                    this.RESULT = "미인식";
                    break;
                case "NOWEIGHT":
                    this.RESULT = "무게없음";
                    break;
                case "WING_FAIL":
                    this.RESULT = "날개접힘";
                    break;
                case "NOSKU":
                    this.RESULT = "상품없음";
                    break;
                case "HEIGHT_OVER":
                    this.RESULT = "높이초과";
                    break;
                case "VOLUME_OVER":
                    this.RESULT = "부피초과";
                    break;
                case "WEIGHT_FAIL":
                    this.RESULT = "중량실패";
                    break;
                case "NODATA":
                    this.RESULT = "정보없음";
                    break;
                case "MULTIERROR":
                    this.RESULT = "복합오류";
                    break;
                default: break;
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
