using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model
{

    //Picking 실적 이후 ~ 출고/30일 이후/주문취소
    public class ProductInfo
    {
        public string WH_ID { get; set; }
        public string CST_CD { get; set; }
        public string WAVE_NO { get; set; }
        public string WAVE_LINE_NO { get; set; }
        public string ORD_NO { get; set; }
        public string ORD_LINE_NO { get; set; }
        public string BOX_ID { get; set; }
        public string BOX_NO { get; set; }
        public string STORE_LOC_CD { get; set; }
        public string BOX_TYPE { get; set; }
        public string INVOICE_ID { get; set; }
        public string EQP_ID { get; set; }
        public string BOX_WT { get; set; } //실제 측정 무게
        public string MNL_PACKING_FLAG { get; set; }
        public string WT_CHECK_FLAG { get; set; }
        public double WEIGHT_SUM { get; set; } //기준무게(Order 기준무게 + BOX무게)
        private string m_INVOICE_ZPL;
        public string INVOICE_ZPL
        {
            get => this.m_INVOICE_ZPL;
            set
            {
                this.m_INVOICE_ZPL = value;
                this.m_INVOICE_ZPL = this.m_INVOICE_ZPL.Replace("\r", "").Replace("\n", "").Replace(Environment.NewLine, "");
            }
        }
        public string STATUS { get; set; }
        public string TASK_ID { get; set; }
    }
}
