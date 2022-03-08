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
    public struct SmartPackingData : IReaderConvertable
    {
        public static SmartPackingData None = new SmartPackingData();
        public long INDEX { get; set; }
        public string BOX_TYPE { get; set; }
        public string BOX_ID { get; set; }
        public DateTime? INSERT_TIME { get; set; }
        public string RESULT { get; set; }
        public double? VOLUME { get; set; }
        public double? HEIGHT { get; set; }
        public bool? IS_MANUAL { get; set; }
        public int? PACKING_AMOUNT { get; set; }
        public DateTime? OUT_TIME { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.INDEX = DomainUtil.GetValueOrNull<long>(reader, nameof(INDEX)) ?? -1;
            this.BOX_ID = reader[nameof(BOX_ID)]?.ToString() ?? "";
            this.BOX_TYPE = this.BOX_ID.Length > 2 ? this.BOX_ID.Substring(1, 1) : "";
            this.INSERT_TIME = DomainUtil.GetValueOrNull<DateTime>(reader, nameof(INSERT_TIME));
            this.RESULT = reader[nameof(RESULT)]?.ToString() ?? "";
            this.VOLUME = DomainUtil.GetValueOrNull<double>(reader, nameof(VOLUME));
            this.HEIGHT = DomainUtil.GetValueOrNull<double>(reader, nameof(HEIGHT));
            this.IS_MANUAL = DomainUtil.GetValueOrNull<bool>(reader, nameof(IS_MANUAL));
            this.PACKING_AMOUNT = DomainUtil.GetValueOrNull<int>(reader, nameof(PACKING_AMOUNT));
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
