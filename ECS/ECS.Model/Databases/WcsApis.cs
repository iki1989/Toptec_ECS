using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECS.Model.Databases
{
    #region 박스 마스터 (ECS-01)
    [Table("BoxMasterMeta")]
    public class BoxMasterMetaTable : MetaTable { }

    [Table("BoxMasterData")]
    public class BoxMasterDataTable
    {
        [Key]
        [ForeignKey("meta_group_cd")]
        [Column("meta_group_cd", TypeName = "VARCHAR")]
        [StringLength(50)]
        [Required]
        [Description("거래건 구분 키값")]
        public string meta_group_cd { get; set; }

        [Column("box_type_cd", TypeName = "VARCHAR")]
        [StringLength(30)]
        [Required]
        [Description("박스 유형 코드")]
        public string box_type_cd { get; set; }

        [Column("box_type_nm", TypeName = "VARCHAR")]
        [StringLength(100)]
        [Required]
        [Description("박스 유형 이름")]
        public string box_type_nm { get; set; }

        [Column("box_color", TypeName = "VARCHAR")]
        [StringLength(10)]
        [Description("박스 색상")]
        public string box_color { get; set; }

        [Column("box_wt")]
        [MaxLength(15)]
        [Required]
        [Description("박스 무게")]
        public double box_wt { get; set; } //비고 : 숫자

        [Column("box_len")]
        [MaxLength(15)]
        [Required]
        [Description("박스 세로")]
        public double box_len { get; set; } //비고 : 숫자

        [Column("box_wd")]
        [MaxLength(15)]
        [Required]
        [Description("박스 가로")]
        public double box_wd { get; set; } //비고 : 숫자

        [Column("box_ht")]
        [MaxLength(15)]
        [Required]
        [Description("박스 높이")]
        public double box_ht { get; set; } //비고 : 숫자

        [Column("box_vol")]
        [MaxLength(15)]
        [Required]
        [Description("박스 체적")]
        public double box_vol { get; set; } //비고 : 숫자

        [Column("error_range", TypeName = "VARCHAR")]
        [StringLength(255)]
        [Required]
        [Description("오차 범위")]
        public string error_range { get; set; }

        [Column("box_wt_min", TypeName = "VARCHAR")]
        [StringLength(15)]
        [Required]
        [Description("박스 최소 무게")]
        public double box_wt_min { get; set; } //비고 : 숫자

        [Column("box_wt_max")]
        [MaxLength(15)]
        [Required]
        [Description("박스 최고 무게")]
        public double box_wt_max { get; set; } //비고 : 숫자

        [Column("len_unit", TypeName = "VARCHAR")]
        [StringLength(6)]
        [Required]
        [Description("사이즈 단위")]
        public string len_unit { get; set; }

        [Column("vol_unit", TypeName = "VARCHAR")]
        [StringLength(6)]
        [Required]
        [Description("부피 단위")]
        public string vol_unit { get; set; }

        [Column("com_cd", TypeName = "VARCHAR")]
        [StringLength(32)]
        [Required]
        [Description("고객사 코드")]
        public string com_cd { get; set; }

        [Column("sort_seq", TypeName = "INT")]
        [MaxLength(4)]
        [Required]
        [Description("정렬순서")]
        public int sort_seq { get; set; } //비고 : 숫자
    }
    #endregion

    #region 주문정보 송신 (ECS-02)
    [Table("OrderMeta")]
    public class OrderMetaTableObject : MetaTable { }

    [Table("OrderData")]
    public class OrderDataTable
    {
        [Key]
        [ForeignKey("meta_group_cd")]
        [Column("meta_group_cd", TypeName = "VARCHAR")]
        [StringLength(50)]
        [Required]
        [Description("거래건 구분 키값")]
        public string meta_group_cd { get; set; }

        [Column("batch_id", TypeName = "VARCHAR")]
        [StringLength(40)]
        [Required]
        [Description("배치 ID")]
        public string batch_id { get; set; }

        [Column("region_cd ", TypeName = "VARCHAR")]
        [StringLength(30)]
        [Required]
        [Description("호기 코드")]
        public string region_cd { get; set; }

        [Column("order_id", TypeName = "VARCHAR")]
        [StringLength(40)]
        [Description("주문 번호")]
        public string order_id { get; set; }

        [Column("invoice_id", TypeName = "VARCHAR")]
        [StringLength(40)]
        [Required]
        [Description("송장 번호")]
        public string invoice_id { get; set; }

        [Column("box_id", TypeName = "VARCHAR")]
        [StringLength(40)]
        [Required]
        [Description("박스 ID")]
        public string box_id { get; set; }

        [Column("box_type_cd", TypeName = "VARCHAR")]
        [StringLength(30)]
        [Required]
        [Description("박스 유형")]
        public string box_type_cd { get; set; }

        [Column("box_wt", TypeName = "DOUBLE")]
        [JsonProperty("box_wt")]
        [MaxLength(19)]
        [Required]
        [Description("중량")]
        public double box_wt { get; set; }    //비고 : 숫자 

        [Column("cancel_flag", TypeName = "INT")]
        [MaxLength(1)]
        [Required]
        [Description("주문 취소 여부")]
        public int cancel_flag { get; set; }     //비고 : 0/1 

        [Column("pick_end_flag", TypeName = "INT")]
        [MaxLength(1)]
        [Required]
        [Description("피킹 완료 여부")]
        public int pick_end_flag { get; set; }     //비고 : 0/1 

        [Column("wt_check_flag", TypeName = "INT")]
        [MaxLength(1)]
        [Required]
        [Description("중량 검수 활성화 여부")]
        public int wt_check_flag { get; set; }     //비고 : 0/1 

        [Column("variant_sku_flag", TypeName = "INT")]
        [MaxLength(1)]
        [Required]
        [Description("이형제품 추가 여부")]
        public int variant_sku_flag { get; set; }     //비고 : 0/1 

        [Column("invoice_zpl", TypeName = "VARCHAR")]
        [Required]
        [Description("송장 ZPL 정보")]
        public string invoice_zpl { get; set; }

        [Column("created_at", TypeName = "SMALLDATETIME")]
        [StringLength(20)]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Description("생성시간 (yyyy-mm-dd hh24:mm:ss)")]
        public DateTime created_at { get; set; }

        [Column("updated_at", TypeName = "SMALLDATETIME")]
        [StringLength(20)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Description("수정시간 (yyyy-mm-dd hh24:mm:ss)")]
        public DateTime updated_at { get; set; }
    }
    #endregion

    #region 주문취소정보 송신 (ECS-03) 
    [Table("OrderCancelMeta")]
    public class OrderCancelMetaTable : MetaTable { }

    [Table("OrderCancelData")]
    public class OrderCancelDataTable : OrderDataTable { }
    #endregion

    #region 피킹실적 송신 (ECS-04)
    [Table("PickingResultMeta")]
    public class PickingResultMetaTable : MetaTable { }

    [Table("PickingResultData")]
    public class PickingResultDataTable
    {
        [Key]
        [ForeignKey("meta_group_cd")]
        [Column("meta_group_cd", TypeName = "VARCHAR")]
        [StringLength(50)]
        [Required]
        [Description("거래건 구분 키값")]
        public string meta_group_cd { get; set; }

        [Column("batch_id", TypeName = "VARCHAR")]
        [StringLength(40)]
        [Required]
        [Description("배치 ID")]
        public string batch_id { get; set; }

        [Column("region_cd ", TypeName = "VARCHAR")]
        [StringLength(30)]
        [Required]
        [Description("호기 코드")]
        public string region_cd { get; set; }

        [Column("order_id", TypeName = "VARCHAR")]
        [StringLength(40)]
        [Description("주문 번호")]
        public string order_id { get; set; }

        [Column("invoice_id", TypeName = "VARCHAR")]
        [StringLength(40)]
        [Required]
        [Description("송장 번호")]
        public string invoice_id { get; set; }

        [Column("box_id", TypeName = "VARCHAR")]
        [StringLength(40)]
        [Required]
        [Description("박스 ID")]
        public string box_id { get; set; }

        [Column("box_type_cd", TypeName = "VARCHAR")]
        [StringLength(30)]
        [Required]
        [Description("박스 유형")]
        public string box_type_cd { get; set; }

        [Column("box_wt", TypeName = "DOUBLE")]
        [JsonProperty("box_wt")]
        [MaxLength(19)]
        [Required]
        [Description("중량")]
        public double box_wt { get; set; }    //비고 : 숫자 

        [Column("cancel_flag", TypeName = "INT")]
        [MaxLength(1)]
        [Required]
        [Description("주문 취소 여부")]
        public int cancel_flag { get; set; }     //비고 : 0/1 

        [Column("pick_end_flag", TypeName = "VARCHAR")]
        [StringLength(4)]
        [Required]
        [Description("피킹 완료 여부")]
        public string pick_end_flag { get; set; }     //비고 : 코드 

        [Column("wt_check_flag", TypeName = "INT")]
        [MaxLength(1)]
        [Required]
        [Description("중량 검수 활성화 여부")]
        public int wt_check_flag { get; set; }     //비고 : 0/1 

        [Column("variant_sku_flag", TypeName = "INT")]
        [MaxLength(1)]
        [Required]
        [Description("이형제품 추가 여부")]
        public int variant_sku_flag { get; set; }     //비고 : 0/1 

        [Column("invoice_zpl", TypeName = "VARCHAR")]
        [Required]
        [Description("송장 ZPL 정보")]
        public string invoice_zpl { get; set; }

        [Column("created_at", TypeName = "SMALLDATETIME")]
        [StringLength(20)]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Description("생성시간 (yyyy-mm-dd hh24:mm:ss)")]
        public DateTime created_at { get; set; }

        [Column("updated_at", TypeName = "SMALLDATETIME")]
        [StringLength(20)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Description("수정시간 (yyyy-mm-dd hh24:mm:ss)")]
        public DateTime updated_at { get; set; }
    }
    #endregion
}
