using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ECS.Model.Domain.Wcs
{
    #region Wcs Common Type
    public interface IWcsData { }
    [Serializable]
    public abstract class WcsCommon<DataType> where DataType : IWcsData
    {
        [JsonProperty("meta")]
        public WcsMetaData meta { get; set; }

        [JsonProperty("data")]
        public List<DataType> data { get; set; }
    }
    [Serializable]
    public class WcsResponse
    {
        [JsonProperty("result_cd")]
        public int result_cd;

        [JsonProperty("result_msg")]
        public string result_msg;

        public WcsResponse(int cd, string msg)
        {
            result_cd = cd;
            result_msg = msg;
        }

        public string ToJson() => JsonConvert.SerializeObject(this);
    }
    #endregion

    #region Wcs Data
    [Serializable]
    public class WcsMetaData
    {
        [JsonProperty("meta_from")]
        [StringLength(50)]
        [Description("호출자 정보")]
        public string meta_from { get; set; } //비고 : WCS

        [JsonProperty("meta_group_cd")]
        [StringLength(50)]
        [Description("거래건 구분 키값")]
        public string meta_group_cd { get; set; }

        [JsonProperty("meta_seq")]
        [StringLength(50)]
        [Description("현재 전송 순서")]
        public string meta_seq { get; set; } //비고 : 1

        [JsonProperty("meta_total")]
        [StringLength(50)]
        [Description("거래건의 전체 전송수")]
        public string meta_total { get; set; } //비고 : 1

        [JsonProperty("meta_complete_yn")]
        [StringLength(1)]
        [Description("거래건의 완료 여부 ( Y / N )")]
        public string meta_complete_yn { get; set; } //비고 : Y

        [JsonProperty("meta_date")]
        [StringLength(20)]
        [Description("전송일시 (yyyy-mm-dd hh24:mi:ss)")]
        public string meta_date { get; set; }
    }

    #region To Wcs Data
    [Serializable]
    public class BoxIdData : IWcsData
    {
        [JsonProperty("box_id")]
        [StringLength(40)]
        [Description("박스 아이디")]
        public string box_id { get; set; }

        [JsonProperty("box_type")]
        [StringLength(30)]
        [Description("호수")]
        public string box_type { get; set; }

        [JsonProperty("floor")]
        [StringLength(2)]
        [Description("작업 층")]
        public string floor { get; set; }

        [JsonProperty("eqp_id")]
        [StringLength(10)]
        [Description("설비 id")]
        public string eqp_id { get; set; }
    }
    [Serializable]
    public class WeightResultData : IWcsData
    {
        [JsonProperty("box_id")]
        [StringLength(40)]
        [Description("박스 아이디")]
        public string box_id { get; set; }

        [JsonProperty("box_type")]
        [StringLength(30)]
        [Description("호수")]
        public string box_type { get; set; }

        [JsonProperty("floor")]
        [StringLength(2)]
        [Description("작업 층")]
        public string floor { get; set; }

        [JsonProperty("eqp_id")]
        [StringLength(10)]
        [Description("설비ID")]
        public string eqp_id { get; set; }

        [JsonProperty("inspect_flag")]
        [StringLength(4)]
        [Description("검수 결과")]
        public string inspect_flag { get; set; }  //비고 : 코드 

        [JsonProperty("box_wt")]
        [StringLength(30)]
        [Description("박스 중량")]
        public string box_wt { get; set; }   //비고 : 숫자 
    }
    [Serializable]
    public class BcrScanResultData : IWcsData
    {
        [JsonProperty("box_id")]
        [StringLength(40)]
        [Description("박스 아이디")]
        public string box_id { get; set; }

        [JsonProperty("box_type")]
        [StringLength(30)]
        [Description("호수")]
        public string box_type { get; set; }

        [JsonProperty("invoice_no")]
        [StringLength(40)]
        [Description("택배 운송장 번호")]
        public string invoice_no { get; set; }

        [JsonProperty("eqp_id")]
        [StringLength(10)]
        [Description("설비ID")]
        public string eqp_id { get; set; }

        [JsonProperty("sorting_flag")]
        [StringLength(4)]
        [Description("분류결과")]
        public string sorting_flag { get; set; }     //비고 : T/F 

    }
    #endregion

    #region From Wcs Data
    [Serializable]
    public class BoxMasterData : IWcsData
    {
        [JsonProperty("box_type_cd")]
        [StringLength(30)]
        [Description("박스 유형 코드")]
        public string box_type_cd { get; set; }

        [JsonProperty("box_type_nm")]
        [StringLength(100)]
        [Description("박스 유형 이름")]
        public string box_type_nm { get; set; }

        [JsonProperty("box_color")]
        [StringLength(10)]
        [Description("박스 색상")]
        public string box_color { get; set; }

        [JsonProperty("box_wt")]
        [StringLength(15)]
        [Description("박스 무게")]
        public string box_wt { get; set; } //비고 : 숫자

        [JsonProperty("box_len")]
        [StringLength(15)]
        [Description("박스 세로")]
        public string box_len { get; set; } //비고 : 숫자

        [JsonProperty("box_wd")]
        [StringLength(15)]
        [Description("박스 가로")]
        public string box_wd { get; set; } //비고 : 숫자

        [JsonProperty("box_ht")]
        [StringLength(15)]
        public string box_ht { get; set; } //비고 : 숫자

        [JsonProperty("box_vol")]
        [StringLength(15)]
        [Description("박스 체적")]
        public string box_vol { get; set; } //비고 : 숫자

        [JsonProperty("error_range")]
        [StringLength(255)]
        [Description("오차 범위")]
        public string error_range { get; set; }

        [JsonProperty("box_wt_min")]
        [StringLength(15)]
        [Description("박스 최소 무게")]
        public string box_wt_min { get; set; } //비고 : 숫자

        [JsonProperty("box_wt_max")]
        [StringLength(15)]
        [Description("박스 최고 무게")]
        public string box_wt_max { get; set; } //비고 : 숫자

        [JsonProperty("len_unit")]
        [StringLength(6)]
        [Description("사이즈 단위")]
        public string len_unit { get; set; }

        [JsonProperty("vol_unit")]
        [StringLength(6)]
        [Description("부피 단위")]
        public string vol_unit { get; set; }

        [JsonProperty("com_cd")]
        [StringLength(32)]
        [Description("고객사 코드")]
        public string com_cd { get; set; }

        [JsonProperty("sort_seq")]
        [StringLength(4)]
        [Description("정렬순서")]
        public string sort_seq { get; set; } //비고 : 숫자
    }
    [Serializable]
    public class OrderData : IWcsData
    {
        [JsonProperty("batch_id")]
        [StringLength(40)]
        [Description("배치 ID")]
        public string batch_id { get; set; }

        [JsonProperty("region_cd")]
        [StringLength(30)]
        [Description("호기 코드")]
        public string region_cd { get; set; }

        [JsonProperty("order_id")]
        [StringLength(40)]
        [Description("주문 번호")]
        public string order_id { get; set; }

        [JsonProperty("invoice_id")]
        [StringLength(40)]
        [Description("송장 번호")]
        public string invoice_id { get; set; }

        [JsonProperty("box_id")]
        [StringLength(40)]
        [Description("박스 ID")]
        public string box_id { get; set; }

        [JsonProperty("box_type_cd")]
        [StringLength(30)]
        [Description("박스 유형")]
        public string box_type_cd { get; set; }

        [JsonProperty("box_wt")]
        [StringLength(19)]
        [Description("중량")]
        public string box_wt { get; set; }    //비고 : 숫자 

        [JsonProperty("cancel_flag")]
        [StringLength(1)]
        [Description("주문 취소 여부")]
        public string cancel_flag { get; set; }     //비고 : 0/1 

        [JsonProperty("pick_end_flag")]
        [StringLength(1)]
        [Description("피킹 완료 여부")]
        public string pick_end_flag { get; set; }     //비고 : 0/1 

        [JsonProperty("wt_check_flag")]
        [StringLength(1)]
        [Description("중량 검수 활성화 여부")]
        public string wt_check_flag { get; set; }     //비고 : 0/1 

        [JsonProperty("variant_sku_flag")]
        [StringLength(1)]
        [Description("이형제품 추가 여부")]
        public string variant_sku_flag { get; set; }     //비고 : 0/1 

        [JsonProperty("invoice_zpl")]
        [Description("송장 ZPL 정보")]
        public string invoice_zpl { get; set; }

        [JsonProperty("created_at")]
        [Description("생성시간")]
        public string created_at { get; set; }

        [JsonProperty("updated_at")]
        [Description("수정시간")]
        public string updated_at { get; set; }
    }
    [Serializable]
    public class PickingResultData : IWcsData
    {

        [JsonProperty("batch_id")]
        [StringLength(40)]
        [Description("배치 ID")]
        public string batch_id { get; set; }

        [JsonProperty("region_cd")]
        [StringLength(30)]
        [Description("호기 코드")]
        public string region_cd { get; set; }

        [JsonProperty("order_id")]
        [StringLength(40)]
        [Description("주문 번호")]
        public string order_id { get; set; }

        [JsonProperty("invoice_id")]
        [StringLength(40)]
        [Description("송장 번호")]
        public string invoice_id { get; set; }

        [JsonProperty("box_id")]
        [StringLength(40)]
        [Description("박스 ID")]
        public string box_id { get; set; }

        [JsonProperty("box_type_cd")]
        [StringLength(30)]
        [Description("박스 유형")]
        public string box_type_cd { get; set; }

        [JsonProperty("box_wt")]
        [StringLength(19)]
        [Description("중량")]
        public string box_wt { get; set; }    //비고 : 숫자 

        [JsonProperty("cancel_flag")]
        [StringLength(1)]
        [Description("주문 취소 여부")]
        public string cancel_flag { get; set; }     //비고 : 0/1 

        [JsonProperty("pick_end_flag")]
        [StringLength(4)]
        [Description("피킹 완료 여부")]
        public string pick_end_flag { get; set; }     //비고 : 코드 

        [JsonProperty("wt_check_flag")]
        [StringLength(1)]
        [Description("중량 검수 활성화 여부")]
        public string wt_check_flag { get; set; }     //비고 : 0/1 

        [JsonProperty("variant_sku_flag")]
        [StringLength(1)]
        [Description("이형제품 추가 여부")]
        public string variant_sku_flag { get; set; }     //비고 : 0/1 

        [JsonProperty("invoice_zpl")]
        [Description("송장 ZPL 정보")]
        public string invoice_zpl { get; set; }

        [JsonProperty("created_at")]
        [Description("생성시간")]
        public string created_at { get; set; }

        [JsonProperty("updated_at")]
        [Description("수정시간")]
        public string updated_at { get; set; }
    }
    #endregion
    #endregion
}
