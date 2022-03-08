using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ECS.Model.Restfuls
{
    #region 박스 ID 생성 수신 (WCS-01)
    public class BoxID : WcsFormat
    {
        public class DataClass
        {
            [JsonProperty("box_id")]
            [StringLength(20)]
            [Description("박스 ID")]
            public string box_id;

            [JsonProperty("box_type")]
            [StringLength(2)]
            [Description("박스 타입")]
            public string box_type;

            [JsonProperty("floor")]
            [StringLength(2)]
            [Description("층")]
            public string floor;

            [JsonProperty("eqp_id")]
            [StringLength(20)]
            [Description("설비ID")]
            public string eqp_id;
        }

        [JsonProperty("data")]
        public List<DataClass> data = new List<DataClass>();
    }
    #endregion

    #region 중량검수 운송장 정보 수신 (WCS-03)
    public class WeightAndInvoice : WcsFormat
    {
        public class DataClass
        {
            public string wh_id;

            [JsonProperty("cst_cd")]
            [StringLength(32)]
            public string cst_cd;

            [JsonProperty("wave_no")]
            [StringLength(40)]
            public string wave_no;

            [JsonProperty("wave_line_no")]
            [StringLength(10)]
            public string wave_line_no;

            [JsonProperty("ord_no")]
            [StringLength(35)]
            public string ord_no;

            [JsonProperty("ord_line_no")]
            [StringLength(10)]
            public string ord_line_no;

            [JsonProperty("box_id")]
            [StringLength(40)]
            public string box_id;

            [JsonProperty("box_no")]
            [StringLength(40)]
            public string box_no;

            [JsonProperty("store_loc_cd")]
            [StringLength(30)]
            public string store_loc_cd;

            [JsonProperty("box_type")]
            [StringLength(30)]
            public string box_type;

            [JsonProperty("floor")]
            [StringLength(2)]
            public string floor;

            [JsonProperty("invoice_id")]
            [StringLength(40)]
            public string invoice_id;

            [JsonProperty("status")]
            [StringLength(2)]
            public string status;

            [JsonProperty("eqp_id")]
            [StringLength(20)]
            public string eqp_id;

            [JsonProperty("box_wt")]
            public double box_wt;

            [JsonProperty("result_cd")]
            [StringLength(10)]
            public string result_cd;

            [JsonProperty("result_text")]
            [StringLength(50)]
            public string result_text;
        }

        [JsonProperty("data")]
        public List<DataClass> data = new List<DataClass>();
    }
    #endregion
}
