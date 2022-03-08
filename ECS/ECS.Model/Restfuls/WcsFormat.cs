using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace ECS.Model.Restfuls
{
    [Serializable]
    public abstract class WcsFormat
    {
        [Serializable]
        public class MetaClass
        {
            /// <summary>
            /// RICP : GP_ECS_021
            /// ECS : GP_ECS_022
            /// </summary>
            [JsonProperty("meta_from")]
            [StringLength(50)]
            [Description("송신 시스템")]
            public string meta_from;

            [JsonProperty("meta_to")]
            [StringLength(50)]
            [Description("수신 시스템")]
            public string meta_to;

            [JsonProperty("meta_group_cd")]
            [StringLength(50)]
            [Description("거래건 구분 키값")]
            public string meta_group_cd;

            [JsonProperty("meta_seq")]
            [StringLength(50)]
            [Description("현재 전송 순서")]
            public string meta_seq;

            [JsonProperty("meta_total")]
            [StringLength(50)]
            [Description("거래건의 전체 전송수")]
            public string meta_total;

            [JsonProperty("meta_complete_yn")]
            [StringLength(1)]
            [Description("거래건의 완료 여부")]
            public string meta_complete_yn;

            public void SetToWcs()
            {
                this.meta_from = "GP_ECS_022";
                this.meta_to = "GP_WCS_001";
                this.meta_group_cd = $"WCS{DateTime.Now.ToString("yyyyMMddhhmmssfff")}";
                this.meta_seq = "1";
                this.meta_total = "1";
                this.meta_complete_yn = "Y";
            }
        }

        [JsonProperty("meta")]
        public MetaClass meta = new MetaClass();
    }

    [Serializable]
    public class WcsFormatBaseData
    {
        [JsonProperty("REG_DT")]
        [Description("생성일시")]
        public string REG_DT;

        [JsonProperty("RSTR_ID")]
        [StringLength(20)]
        [Description("생성자")]
        public string RSTR_ID;

        [JsonProperty("UPD_DT")]
        [Description("변경일시")]
        public string UPD_DT;

        [JsonProperty("UPDR_ID")]
        [StringLength(20)]
        [Description("변경자")]
        public string UPDR_ID;

        [JsonProperty("IF_TXN_TYPE_FL")]
        [StringLength(1)]
        [Description("ECS 처리여부")]
        public string IF_TXN_TYPE_FL;

        [JsonProperty("IF_TXN_DATE")]
        [Description("ECS 전송/처리시각")]
        public string IF_TXN_DATE;
    }

    [Serializable]
    public class WcsDatatable
    {
        public DataTable meta { get; set; }

        public DataTable datas { get; set; }
    }
}
