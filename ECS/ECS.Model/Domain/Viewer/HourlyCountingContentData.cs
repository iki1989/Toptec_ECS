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
    public struct HourlyCountingContentData
    {
        public static HourlyCountingContentData None = new HourlyCountingContentData() { };
        public string DATE { get; set; }
        public int[] ORDER_COUNTS { get; set; }
        public int[] CASE_ERECT_COUNTS { get; set; }
        public int[] CASE_ERECT_REJECT_COUNTS { get; set; }
        public int[] WEIGHT_COUNTS { get; set; }
        public int[] WEIGHT_REJECT_COUNTS { get; set; }
        public int[] PACKING_COUNTS { get; set; }
        public int[] PACKING_REJECT_COUNTS { get; set; }
        public int[] SMART_PRINT_COUNTS { get; set; }
        public int[] NORMAL_PRINT_COUNTS { get; set; }
        public int[] TOP_COUNTS { get; set; }
        public int[] TOP_REJECT_COUNTS { get; set; }
        public int[] OUT_COUNTS { get; set; }
        public int[] REAL_OUT_COUNTS { get; set; }
        public HourlyCountingContentData(IEnumerable<HourlyCountingData> datas)
        {
            this.DATE = datas.First().DATE;
            this.ORDER_COUNTS = new int[24];
            this.CASE_ERECT_COUNTS = new int[24];
            this.CASE_ERECT_REJECT_COUNTS = new int[24];
            this.WEIGHT_COUNTS = new int[24];
            this.WEIGHT_REJECT_COUNTS = new int[24];
            this.PACKING_COUNTS = new int[24];
            this.PACKING_REJECT_COUNTS = new int[24];
            this.SMART_PRINT_COUNTS = new int[24];
            this.NORMAL_PRINT_COUNTS = new int[24];
            this.TOP_COUNTS = new int[24];
            this.TOP_REJECT_COUNTS = new int[24];
            this.OUT_COUNTS = new int[24];
            this.REAL_OUT_COUNTS = new int[24];
            foreach (var d in datas)
            {
                this.ORDER_COUNTS[d.HOUR] = d.DATA.ORDER_COUNT;
                this.CASE_ERECT_COUNTS[d.HOUR] = d.DATA.CASE_ERECT_COUNT;
                this.CASE_ERECT_REJECT_COUNTS[d.HOUR] = d.DATA.CASE_ERECT_REJECT_COUNT;
                this.WEIGHT_COUNTS[d.HOUR] = d.DATA.WEIGHT_COUNT;
                this.WEIGHT_REJECT_COUNTS[d.HOUR] = d.DATA.WEIGHT_REJECT_COUNT;
                this.PACKING_COUNTS[d.HOUR] = d.DATA.PACKING_COUNT;
                this.PACKING_REJECT_COUNTS[d.HOUR] = d.DATA.PACKING_REJECT_COUNT;
                this.SMART_PRINT_COUNTS[d.HOUR] = d.DATA.SMART_PRINT_COUNT;
                this.NORMAL_PRINT_COUNTS[d.HOUR] = d.DATA.NORMAL_PRINT_COUNT;
                this.TOP_COUNTS[d.HOUR] = d.DATA.TOP_COUNT;
                this.TOP_REJECT_COUNTS[d.HOUR] = d.DATA.TOP_REJECT_COUNT;
                this.OUT_COUNTS[d.HOUR] = d.DATA.OUT_COUNT;
                this.REAL_OUT_COUNTS[d.HOUR] = d.DATA.REAL_OUT_COUNT;
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
