using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace ECS.Model.Restfuls
{
    #region 주문정보 송신 (ECS-01)
    [Serializable]
    public class Order : WcsFormat
    {
        public class DataClass : WcsFormatBaseData
        {
            [JsonProperty("WH_ID")]
            [StringLength(30)]
            [Description("창고코드")]
            public string WH_ID;

            [JsonProperty("CST_CD")]
            [StringLength(32)]
            [Description("화주코드")]
            public string CST_CD;

            [JsonProperty("WAVE_NO")]
            [StringLength(40)]
            [Description("WAVE번호 상세번호")]
            public string WAVE_NO;

            [JsonProperty("WAVE_LINE_NO")]
            [StringLength(10)]
            [Description("WAVE번호 상세번호")]
            public string WAVE_LINE_NO;

            [JsonProperty("ORD_NO")]
            [StringLength(35)]
            [Description("JDA_주문번호")]
            public string ORD_NO;

            [JsonProperty("ORD_LINE_NO")]
            [StringLength(10)]
            [Description("JDA_주문 라인번호")]
            public string ORD_LINE_NO;

            [JsonProperty("BOX_NO")]
            [StringLength(40)]
            [Description("주문번호_박스번호 (WMS에서 발행)")]
            public string BOX_NO;

            [JsonProperty("STORE_LOC_CD")]
            [StringLength(30)]
            [Description("WMS 로케이션")]
            public string STORE_LOC_CD;

            [JsonProperty("ORD_TYPE")]
            public string ORD_TYPE;

            [JsonProperty("ORD_CLASS")]
            public string ORD_CLASS;

            [JsonProperty("INVOICE_ID")]
            [StringLength(40)]
            [Description("송장 번호")]
            public string INVOICE_ID;

            [JsonProperty("BOX_ID")]
            [StringLength(40)]
            [Description("박스 ID")]
            public string BOX_ID;

            [JsonProperty("CST_ORD_NO")]
            [StringLength(35)]
            [Description("고객_주문번호")]
            public string CST_ORD_NO;

            [JsonProperty("CST_ORD_LINE_NO")]
            [StringLength(10)]
            [Description("고객_주문 라인번호")]
            public string CST_ORD_LINE_NO;

            [JsonProperty("SKU_CD")]
            [StringLength(30)]
            [Description("상품 코드")]
            public string SKU_CD;

            [JsonProperty("SKU_QTY")]
            [Description("Status에 대한 처리수량")]
            public double? SKU_QTY;

            [JsonProperty("BOX_TYPE_CD")]
            [StringLength(30)]
            [Description("박스 유형")]
            public string BOX_TYPE_CD;

            [JsonProperty("BOX_TYPE")]
            public string BOX_TYPE;

            [JsonProperty("FLOOR")]
            public string FLOOR;

            [JsonProperty("INVOICE_NO")]
            public string INVOICE_NO;

            [JsonProperty("BOX_WT")]
            public double? BOX_WT;

            [JsonProperty("STATUS")]
            [StringLength(2)]
            [Description("작업상태코드 (2BYTES)")]
            public string STATUS;

            [JsonProperty("EQP_ID")]
            [StringLength(20)]
            [Description("설비ID")]
            public string EQP_ID;

            [JsonProperty("INVOICE_ZPL")]
            [Description("추가 ZPL (단수, 이형에서 활용)")]
            public string INVOICE_ZPL;

            [JsonProperty("INVOICE_ZPL_300DPI")]
            [Description("기존 ZPL(단포, 이종합포 waybill 실적받는곳에서 사용")]
            public string INVOICE_ZPL_300DPI;

            /// <summary>
            /// Y: 중량검수부터 진행
            /// N: 중량검수 통과
            /// M: 강제 리젝
            /// W: 운송장부터 진행
            /// B: BCR부터 진행
            /// </summary>
            [JsonProperty("WT_CHECK_FLAG")]
            [StringLength(1)]
            [Description("WAVE번호 상세번호")]
            public string WT_CHECK_FLAG;

            [JsonProperty("DIV_CLS_CD")]
            public string DIV_CLS_CD;

            [JsonProperty("DIV_SUB_CLS_CD")]
            public string DIV_SUB_CLS_CD;

            [JsonProperty("VARIANT_SKU_FLAG")]
            public string VARIANT_SKU_FLAG;

            [JsonProperty("DELIVERY_TYPE")]
            public string DELIVERY_TYPE;

            [JsonProperty("ATTRIBUTE01")]
            [StringLength(100)]
            public string ATTRIBUTE01;

            [JsonProperty("ATTRIBUTE02")]
            [StringLength(100)]
            public string ATTRIBUTE02;

            [JsonProperty("ATTRIBUTE03")]
            [StringLength(100)]
            public string ATTRIBUTE03;

            [JsonProperty("ATTRIBUTE04")]
            [StringLength(100)]
            public string ATTRIBUTE04;

            [JsonProperty("ATTRIBUTE05")]
            [StringLength(100)]
            public string ATTRIBUTE05;

            [JsonProperty("ATTRIBUTE06")]
            [StringLength(100)]
            public string ATTRIBUTE06;

            [JsonProperty("ATTRIBUTE07")]
            [StringLength(100)]
            public string ATTRIBUTE07;

            [JsonProperty("ATTRIBUTE08")]
            [StringLength(100)]
            public string ATTRIBUTE08;

            [JsonProperty("ATTRIBUTE09")]
            [StringLength(100)]
            public string ATTRIBUTE09;

            [JsonProperty("ATTRIBUTE10")]
            [StringLength(100)]
            public string ATTRIBUTE10;

            [JsonProperty("MNL_PACKING_FLAG")]
            [StringLength(1)]
            [Description("박스 내 상품 개별포장 유무 표시")]
            public string MNL_PACKING_FLAG;

            [JsonProperty("EMPTY_RATE")]
            [Description("박스 여유 공간 %")]
            public int? EMPTY_RATE;

            [JsonProperty("RESULT_CD")]
            public string RESULT_CD;

            [JsonProperty("RESULT_TEXT")]
            public string RESULT_TEXT;

            [JsonProperty("STANDARD_WHT")]
            [StringLength(20)]
            [Description("중량 기준값")]
            public string STANDARD_WHT;

            [JsonProperty("JOB_DATE")]
            [StringLength(10)]
            [Description("작업 일자")]
            public string JOB_DATE;

            [JsonProperty("ORDER_DATE")]
            [StringLength(10)]
            [Description("주문 일자")]
            public string ORDER_DATE;

            [JsonProperty("ORDER_DETAIL_ID")]
            [StringLength(40)]
            [Description("주문 디테일 ID (주문 레코드 별 유니크 ID)")]
            public string ORDER_DETAIL_ID;

            [JsonProperty("MULTI_BOX")]
            [StringLength(10)]
            [Description("박스온도대 종류")]
            public string MULTI_BOX;

            [JsonProperty("FRE_DRY_QTY")]
            [StringLength(10)]
            [Description("냉동 드라이아이스 수량")]
            public long? FRE_DRY_QTY;

            [JsonProperty("FRE_PACK_QTY")]
            [StringLength(10)]
            [Description("냉동 아이스팩 수량")]
            public long? FRE_PACK_QTY;

            [JsonProperty("REF_DRY_QTY")]
            [StringLength(10)]
            [Description("냉장 드라이아이스 수량")]
            public long? REF_DRY_QTY;

            [JsonProperty("REF_PACK_QTY")]
            [StringLength(10)]
            [Description("냉장 아이스팩 수량")]
            public long? REF_PACK_QTY;

            [JsonProperty("DIVIDER")]
            [StringLength(10)]
            [Description("칸막이위치")]
            public string DIVIDER;

            [JsonProperty("PACK_TYPE")]
            [StringLength(20)]
            [Description("포장 유형")]
            public string PACK_TYPE;

            [JsonProperty("BOX_IN_QTY")]
            [Description("박스내 SKU 입수 수량")]
            public double? BOX_IN_QTY;

            [JsonProperty("SKU_BARCD")]
            [StringLength(60)]
            [Description("상품 바코드")]
            public string SKU_BARCD;

            [JsonProperty("SKU_NM")]
            [StringLength(385)]
            [Description("상품명")]
            public string SKU_NM;

            [JsonProperty("QTY_UNIT")]
            [StringLength(6)]
            [Description("수량 단위")]
            public string QTY_UNIT;

            [JsonProperty("STORE_ZONE_CD")]
            [StringLength(30)]
            [Description("WMS 존")]
            public string STORE_ZONE_CD;

            [JsonProperty("ALLOC_NO")]
            [StringLength(40)]
            [Description("배차 번호")]
            public string ALLOC_NO;

            [JsonProperty("PASS_STOP_CD")]
            [StringLength(30)]
            [Description("경유지 코드")]
            public string PASS_STOP_CD;

            [JsonProperty("PASS_STOP_NM")]
            [StringLength(100)]
            [Description("경유지 명")]
            public string PASS_STOP_NM;

            [JsonProperty("PAYMENT_TYPE")]
            [StringLength(20)]
            [Description("정산구분")]
            public string PAYMENT_TYPE;

            [JsonProperty("SND_CUST_NO")]
            [StringLength(40)]
            [Description("택배 계약 코드, 택배 거래처 코드")]
            public string SND_CUST_NO;

            [JsonProperty("B2C_CUST_ID")]
            [StringLength(40)]
            [Description("고객ID")]
            public string B2C_CUST_ID;

            [JsonProperty("B2C_CUST_MGR_ID")]
            [StringLength(40)]
            [Description("고객관리거래처코드")]
            public string B2C_CUST_MGR_ID;

            [JsonProperty("RESV_TYPE")]
            [StringLength(20)]
            [Description("예약구분")]
            public string RESV_TYPE;

            [JsonProperty("ORDER_SKU_STATUS")]
            [StringLength(10)]
            [Description("상품상태")]
            public string ORDER_SKU_STATUS;

            [JsonProperty("BUYER_PO_CD")]
            [StringLength(30)]
            [Description("바이어 코드, 주문자 PO 번호")]
            public string BUYER_PO_CD;

            [JsonProperty("SND_NM")]
            [StringLength(100)]
            [Description("보내는 고객명")]
            public string SND_NM;

            public override string ToString()
            {
                List<string> list = new List<string>();
                var fields = typeof(Order.DataClass).GetFields();
                foreach (var field in fields)
                {
                    if (field.Name != nameof(this.INVOICE_ZPL_300DPI))
                        list.Add($"{field.GetValue(this)}");
                }


                return string.Join(",", list);
            }
        }

        [JsonProperty("data")]
        public List<DataClass> data = new List<DataClass>();
    }
    #endregion

    #region 주문취소정보 송신 (ECS-02)
    [Serializable]
    public class OrderCancel : WcsFormat
    {
        public class DataClass : WcsFormatBaseData
        {
            [JsonProperty("WH_ID")]
            [StringLength(30)]
            public string WH_ID;

            [JsonProperty("CST_CD")]
            [StringLength(32)]
            public string CST_CD;

            [JsonProperty("WAVE_NO")]
            [StringLength(40)]
            public string WAVE_NO;

            [JsonProperty("WAVE_LINE_NO")]
            [StringLength(10)]
            public string WAVE_LINE_NO;

            [JsonProperty("ORD_NO")]
            [StringLength(35)]
            public string ORD_NO;

            [JsonProperty("ORD_LINE_NO")]
            [StringLength(10)]
            public string ORD_LINE_NO;

            [JsonProperty("BOX_NO")]
            [StringLength(40)]
            public string BOX_NO;

            [JsonProperty("STORE_LOC_CD")]
            [StringLength(30)]
            public string STORE_LOC_CD;

            [JsonProperty("STANDARD_WHT")]
            [StringLength(20)]
            public string STANDARD_WHT;

            [JsonProperty("INVOICE_ID")]
            [StringLength(40)]
            public string INVOICE_ID;

            [JsonProperty("BOX_ID")]
            [StringLength(40)]
            public string BOX_ID;

            [JsonProperty("CST_ORD_NO")]
            [StringLength(35)]
            public string CST_ORD_NO;

            [JsonProperty("CST_ORD_LINE_NO")]
            [StringLength(10)]
            public string CST_ORD_LINE_NO;

            [JsonProperty("SKU_CD")]
            [StringLength(50)]
            public string SKU_CD;

            [JsonProperty("SKU_QTY")]
            public double SKU_QTY;

            [JsonProperty("BOX_TYPE_CD")]
            [StringLength(30)]
            public string BOX_TYPE_CD;

            [JsonProperty("STATUS")]
            [StringLength(2)]
            public string STATUS;

            [JsonProperty("EQP_ID")]
            [StringLength(20)]
            public string EQP_ID;

            [JsonProperty("ATTRIBUTE01")]
            [StringLength(100)]
            public string ATTRIBUTE01;

            [JsonProperty("ATTRIBUTE02")]
            [StringLength(100)]
            public string ATTRIBUTE02;

            [JsonProperty("ATTRIBUTE03")]
            [StringLength(100)]
            public string ATTRIBUTE03;

            [JsonProperty("ATTRIBUTE04")]
            [StringLength(100)]
            public string ATTRIBUTE04;

            [JsonProperty("ATTRIBUTE05")]
            [StringLength(100)]
            public long? ATTRIBUTE05;

            [JsonProperty("ATTRIBUTE06")]
            [StringLength(100)]
            public long? ATTRIBUTE06;

            [JsonProperty("ATTRIBUTE07")]
            [StringLength(100)]
            public long? ATTRIBUTE07;

            [JsonProperty("ATTRIBUTE08")]
            [StringLength(100)]
            public long? ATTRIBUTE08;

            [JsonProperty("ATTRIBUTE09")]
            [StringLength(100)]
            public string ATTRIBUTE09;

            [JsonProperty("ATTRIBUTE10")]
            [StringLength(100)]
            public string ATTRIBUTE10;
        }

        [JsonProperty("data")]
        public List<DataClass> data = new List<DataClass>();
    }
    #endregion

    #region 주문삭제정보 송신 (ECS-03)
    [Serializable]
    public class OrderDelete : WcsFormat
    {
        public class DataClass : WcsFormatBaseData
        {
            public class OrdNoItem
            {
                [JsonProperty("ORD_NO")]
                [StringLength(35)]
                public string ORD_NO;
                public override string ToString() => ORD_NO;
            }
            [JsonProperty("WAVE_NO")]
            [StringLength(40)]
            public string WAVE_NO;

            [JsonProperty("WAVE_REMOVE_FL")]
            [StringLength(1)]
            public string WAVE_REMOVE_FL;

            [JsonProperty("ORD_NO_LIST")]
            public List<OrdNoItem> ORD_NO_LIST = new List<OrdNoItem>();
        }

        [JsonProperty("data")]
        public List<DataClass> data = new List<DataClass>();
    }
    #endregion

    #region 수기검수실적 송신 (ECS-04)
    [Serializable]
    public class OperatorWeightResult : WcsFormat
    {
        public class DataClass : WcsFormatBaseData
        {
            [JsonProperty("WH_ID")]
            [StringLength(30)]
            [Description("창고코드")]
            public string WH_ID;

            [JsonProperty("CST_CD")]
            [StringLength(32)]
            [Description("화주코드")]
            public string CST_CD;

            [JsonProperty("ORD_NO")]
            [StringLength(35)]
            [Description("보내는 고객명")]
            public string ORD_NO;

            [JsonProperty("BOX_ID")]
            [StringLength(40)]
            [Description("WAVE번호 상세번호")]
            public string BOX_ID;

            /// <summary>
            /// Y: 중량검수부터 진행
            /// N: 중량검수 통과
            /// M: 강제 리젝
            /// W: 운송장부터 진행
            /// B: BCR부터 진행
            /// </summary>
            [JsonProperty("WT_CHECK_FLAG")]
            [StringLength(1)]
            [Description("WAVE번호 상세번호")]
            public string WT_CHECK_FLAG;

            [JsonProperty("EQP_ID")]
            [StringLength(20)]
            [Description("JDA_주문번호")]
            public string EQP_ID;

            [JsonProperty("INVOICE_ID")]
            [StringLength(40)]
            [Description("JDA_주문 라인번호")]
            public string INVOICE_ID;

        }

        [JsonProperty("data")]
        public List<DataClass> data = new List<DataClass>();
    }
    #endregion

    #region 상품마스터 송신 (ECS-05)
    [Serializable]
    public class SkuMaster : WcsFormat
    {
        public class DataClass : WcsFormatBaseData
        {
            [JsonProperty("WH_ID")]
            [StringLength(30)]
            public string WH_ID;

            [JsonProperty("CST_CD")]
            [StringLength(32)]
            public string CST_CD;

            [JsonProperty("SKU_CD")]
            [StringLength(50)]
            public string SKU_CD;

            [JsonProperty("SKU_CLASS")]
            [StringLength(20)]
            public string SKU_CLASS;

            [JsonProperty("SKU_NM")]
            [StringLength(250)]
            public string SKU_NM;

            [JsonProperty("SKU_NM_ENG")]
            [StringLength(200)]
            public string SKU_NM_ENG;

            [JsonProperty("SKU_NM_LOC")]
            [StringLength(200)]
            public string SKU_NM_LOC;

            [JsonProperty("DELETE_FLAG")]
            [StringLength(1)]
            public string DELETE_FLAG;

            [JsonProperty("PACKING_FLAG")]
            [StringLength(1)]
            public string PACKING_FLAG;

            [JsonProperty("ITEM_TEMP")]
            [StringLength(20)]
            public string ITEM_TEMP;

            [JsonProperty("ORD_TYPE")]
            [StringLength(10)]
            public string ORD_TYPE;

            [JsonProperty("WGT_UOM_CD")]
            [StringLength(4)]
            public string WGT_UOM_CD;

            [JsonProperty("PUR_VENDOR_CD")]
            [StringLength(15)]
            public string PUR_VENDOR_CD;

            [JsonProperty("PUR_VENDOR_NM")]
            [StringLength(50)]
            public string PUR_VENDOR_NM;

            [JsonProperty("PUR_VENDOR_NM_ENG")]
            [StringLength(50)]
            public string PUR_VENDOR_NM_ENG;

            [JsonProperty("PUR_VENDOR_NM_LOC")]
            [StringLength(50)]
            public string PUR_VENDOR_NM_LOC;

            [JsonProperty("LEN_UOM_CD")]
            [StringLength(4)]
            public string LEN_UOM_CD;

            [JsonProperty("CBM_UOM_CD")]
            [StringLength(4)]
            public string CBM_UOM_CD;

            [JsonProperty("SKU_PRICE")]
            public double? SKU_PRICE;

            [JsonProperty("STYL_CD")]
            [StringLength(60)]
            public string STYL_CD;

            [JsonProperty("STYL_NM")]
            [StringLength(100)]
            public string STYL_NM;

            [JsonProperty("SIZ_CD")]
            [StringLength(30)]
            public string SIZ_CD;

            [JsonProperty("SIZ_NM")]
            [StringLength(100)]
            public long? SIZ_NM;

            [JsonProperty("CLR_CD")]
            [StringLength(60)]
            public long? CLR_CD;

            [JsonProperty("CLR_NM")]
            [StringLength(100)]
            public long? CLR_NM;

            [JsonProperty("BRND_CD")]
            [StringLength(60)]
            public long? BRND_CD;

            [JsonProperty("BRND_NM")]
            [StringLength(100)]
            public string BRND_NM;

            [JsonProperty("SKU_TMPT_TYPE_CD")]
            [StringLength(30)]
            public string SKU_TMPT_TYPE_CD;

            [JsonProperty("EXPIRA_DATE_TERM")]
            public short? EXPIRA_DATE_TERM;

            [JsonProperty("SKU_BCR_NO1")]
            [StringLength(50)]
            public string SKU_BCR_NO1;

            [JsonProperty("SKU_BCR_NO2")]
            [StringLength(50)]
            public string SKU_BCR_NO2;

            [JsonProperty("SKU_BCR_NO3")]
            [StringLength(50)]
            public string SKU_BCR_NO3;

            [JsonProperty("BOX_BCR_NO")]
            [StringLength(50)]
            public string BOX_BCR_NO;

            [JsonProperty("CASE_BCR_NO")]
            [StringLength(50)]
            public string CASE_BCR_NO;

            [JsonProperty("CASE_YN")]
            [StringLength(1)]
            public string CASE_YN;

            [JsonProperty("BOX_IN_QTY")]
            public long? BOX_IN_QTY;

            [JsonProperty("PLT_BOX_QTY")]
            public long? PLT_BOX_QTY;

            [JsonProperty("SKU_WGT")]
            public double? SKU_WGT;

            [JsonProperty("CASE_WGT")]
            public double? CASE_WGT;

            [JsonProperty("BOX_WGT")]
            public double? BOX_WGT;

            [JsonProperty("SKU_VERT_LEN")]
            public double? SKU_VERT_LEN;

            [JsonProperty("SKU_WTH_LEN")]
            public double? SKU_WTH_LEN;

            [JsonProperty("SKU_HGT_LEN")]
            public double? SKU_HGT_LEN;

            [JsonProperty("SKU_CBM")]
            public double? SKU_CBM;

            [JsonProperty("CASE_WTH_LEN")]
            public double? CASE_WTH_LEN;

            [JsonProperty("CASE_VERT_LEN")]
            public double? CASE_VERT_LEN;

            [JsonProperty("CASE_HGT_LEN")]
            public double? CASE_HGT_LEN;

            [JsonProperty("CASE_CBM")]
            public double? CASE_CBM;

            [JsonProperty("BOX_VERT_LEN")]
            public double? BOX_VERT_LEN;

            [JsonProperty("BOX_WTH_LEN")]
            public double? BOX_WTH_LEN;

            [JsonProperty("BOX_HGT_LEN")]
            public double? BOX_HGT_LEN;

            [JsonProperty("BOX_CBM")]
            public double? BOX_CBM;

            [JsonProperty("ATTRIBUTE01")]
            [StringLength(100)]
            public string ATTRIBUTE01;

            [JsonProperty("ATTRIBUTE02")]
            [StringLength(100)]
            public string ATTRIBUTE02;

            [JsonProperty("ATTRIBUTE03")]
            [StringLength(100)]
            public string ATTRIBUTE03;

            [JsonProperty("ATTRIBUTE04")]
            [StringLength(100)]
            public string ATTRIBUTE04;

            [JsonProperty("ATTRIBUTE05")]
            [StringLength(100)]
            public string ATTRIBUTE05;

            [JsonProperty("ATTRIBUTE06")]
            [StringLength(100)]
            public string ATTRIBUTE06;

            [JsonProperty("ATTRIBUTE07")]
            [StringLength(100)]
            public string ATTRIBUTE07;

            [JsonProperty("ATTRIBUTE08")]
            [StringLength(100)]
            public string ATTRIBUTE08;

            [JsonProperty("ATTRIBUTE09")]
            [StringLength(100)]
            public string ATTRIBUTE09;

            [JsonProperty("ATTRIBUTE10")]
            [StringLength(100)]
            public string ATTRIBUTE10;
        }

        [JsonProperty("data")]
        public List<DataClass> data = new List<DataClass>();
    }
    #endregion

    #region 피킹실적 송신 (ECS-06)
    [Serializable]
    public class Picking : WcsFormat
    {
        public class DataClass : WcsFormatBaseData
        {
            [JsonProperty("WH_ID")]
            [StringLength(30)]
            public string WH_ID;

            [JsonProperty("CST_CD")]
            [StringLength(32)]
            public string CST_CD;

            [JsonProperty("WAVE_NO")]
            [StringLength(40)]
            public string WAVE_NO;

            [JsonProperty("WAVE_LINE_NO")]
            [StringLength(10)]
            public string WAVE_LINE_NO;

            [JsonProperty("ORD_NO")]
            [StringLength(35)]
            public string ORD_NO;

            [JsonProperty("ORD_LINE_NO")]
            [StringLength(10)]
            public string ORD_LINE_NO;

            [JsonProperty("BOX_NO")]
            [StringLength(40)]
            public string BOX_NO;

            [JsonProperty("STORE_LOC_CD")]
            [StringLength(30)]
            public string STORE_LOC_CD;

            [JsonProperty("INVOICE_ID")]
            [StringLength(40)]
            public string INVOICE_ID;

            [JsonProperty("BOX_ID")]
            [StringLength(40)]
            public string BOX_ID;

            [JsonProperty("BOX_TYPE_CD")]
            [StringLength(30)]
            public string BOX_TYPE_CD;

            [JsonProperty("ORDER_CLASS")]
            [StringLength(10)]
            public string ORDER_CLASS;

            [JsonProperty("STATUS")]
            [StringLength(2)]
            public string STATUS;

            [JsonProperty("EQP_ID")]
            [StringLength(20)]
            public string EQP_ID;

            [JsonProperty("CST_ORD_NO")]
            [StringLength(35)]
            public string CST_ORD_NO;

            [JsonProperty("CST_ORD_LINE_NO")]
            [StringLength(10)]
            public string CST_ORD_LINE_NO;

            [JsonProperty("WT_CHECK_FLAG")]
            [StringLength(1)]
            public string WT_CHECK_FLAG = "Y";

            [JsonProperty("SKU_CD")]
            [StringLength(50)]
            public string SKU_CD;

            [JsonProperty("SKU_NM")]
            [StringLength(385)]
            public string SKU_NM;

            [JsonProperty("SKU_QTY")]
            public double? SKU_QTY;

            [JsonProperty("DLV_CLS_CD")]
            [StringLength(30)]
            public string DLV_CLS_CD;

            [JsonProperty("DLV_SUB_CLS_CD")]
            [StringLength(30)]
            public string DLV_SUB_CLS_CD;

            [JsonProperty("DELIVERY_TYPE")]
            [StringLength(20)]
            public string DELIVERY_TYPE;

            [JsonProperty("ATTRIBUTE01")]
            [StringLength(100)]
            public string ATTRIBUTE01;

            [JsonProperty("ATTRIBUTE02")]
            [StringLength(100)]
            public string ATTRIBUTE02;

            [JsonProperty("ATTRIBUTE03")]
            [StringLength(100)]
            public string ATTRIBUTE03;

            [JsonProperty("ATTRIBUTE04")]
            [StringLength(100)]
            public string ATTRIBUTE04;

            [JsonProperty("ATTRIBUTE05")]
            [StringLength(100)]
            public string ATTRIBUTE05;

            [JsonProperty("ATTRIBUTE06")]
            [StringLength(100)]
            public string ATTRIBUTE06;

            [JsonProperty("ATTRIBUTE07")]
            [StringLength(100)]
            public string ATTRIBUTE07;

            [JsonProperty("ATTRIBUTE08")]
            [StringLength(100)]
            public string ATTRIBUTE08;

            [JsonProperty("ATTRIBUTE09")]
            [StringLength(100)]
            public string ATTRIBUTE09;

            [JsonProperty("ATTRIBUTE10")]
            [StringLength(100)]
            public string ATTRIBUTE10;
        }

        [JsonProperty("data")]
        public List<DataClass> data = new List<DataClass>();
    }
    #endregion
}
