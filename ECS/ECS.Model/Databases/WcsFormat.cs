using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Databases
{
    public abstract class MetaTable
    {
        public class MetaInfo
        {
            [Column("meta_from", TypeName = "VARCHAR")]
            [StringLength(50)]
            [Required]
            [Description("호출자 정보")]
            public string meta_from { get; set; }

            [Key]
            [Column("meta_group_cd", TypeName = "VARCHAR")]
            [StringLength(50)]
            [Required]
            [Description("거래건 구분 키값")]
            public string meta_group_cd { get; set; }

            [Column("meta_seq", TypeName = "INT")]
            [MaxLength(50)]
            [Required]
            [Description("현재 전송 순서")]
            public int meta_seq { get; set; } //비고 : 1

            [Column("meta_total", TypeName = "INT")]
            [MaxLength(50)]
            [Required]
            [Description("거래건의 전체 전송수")]
            public int meta_total { get; set; } //비고 : 1

            [Column("meta_complete_yn", TypeName = "CHAR")]
            [StringLength(1)]
            [Required]
            [Description("거래건의 완료 여부 ( Y / N )")]
            public string meta_complete_yn { get; set; } //비고 : Y

            [Column("meta_date", TypeName = "SMALLDATETIME")]
            [StringLength(20)]
            [Required]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
            [Description("전송일시 (yyyy-mm-dd hh24:mm:ss)")]
            public DateTime meta_date { get; set; }
        }

        public MetaInfo Meta { get; set; } = new MetaInfo();

        [Column("Insert_Time", TypeName = "SMALLDATETIME")]
        [StringLength(20)]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Description("삽입 일시 (yyyy-mm-dd hh24:mm:ss)")]
        public DateTime Insert_Time { get; set; }

        [Column("Insert_Time", TypeName = "SMALLDATETIME")]
        [StringLength(20)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Description("업데이트 일시 (yyyy-mm-dd hh24:mm:ss)")]
        public DateTime Update_Time { get; set; }
    }
}
