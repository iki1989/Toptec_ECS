using ECS.Model.Databases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Domain.Viewer
{
    public struct TrackingContentData
    {
        public static TrackingContentData None = new TrackingContentData() { };
        public string BOX_ID { get; set; }
        public string INVOICE_ID { get; set; }
        public string CST_ORD_NO { get; set; }
        public string ORDER_TIME { get; set; }
        public string ORDER_CANCEL_TIME { get; set; }
        public string PICKING_TIME { get; set; }
        public string WEIGHT_TIME { get; set; }
        public string WEIGHT_REJECT_TIME { get; set; }
        public string SMART_PACKING_TIME { get; set; }
        public string SMART_PACKING_REJECT_TIME { get; set; }
        public string SMART_PRINT_TIME { get; set; }
        public string NORMAL_PRINT_TIME { get; set; }
        public string TOP_TIME { get; set; }
        public string TOP_REJECT_TIME { get; set; }
        public string OUT_TIME { get; set; }
        private static string GetTaskTime(Dictionary<string, (string, DateTime?)> dict, TASK_IDEnum taskId)
        {
            var task = $"{(int)taskId:D2}";
            return dict.ContainsKey(task) ? dict[task].Item2?.ToString("yy-MM-dd HH:mm:ss") ?? "" : "";
        }
        public TrackingContentData(IGrouping<string, TrackingData> datas)
        {
            var first = datas.First();
            this.BOX_ID = datas.FirstOrDefault(d => d.BOX_ID != "").BOX_ID;
            this.INVOICE_ID = first.INVOICE_ID;
            this.CST_ORD_NO = first.CST_ORD_NO;
            var taskMap = (from d in datas
                           group d by d.TASK_ID into g
                           select (g.Key, g.Max(t => t.TASK_TIME))).ToDictionary(t => t.Key);
            this.ORDER_TIME = GetTaskTime(taskMap, TASK_IDEnum.Order);
            this.ORDER_CANCEL_TIME = GetTaskTime(taskMap, TASK_IDEnum.OrderCancel);
            this.PICKING_TIME = GetTaskTime(taskMap, TASK_IDEnum.RicpPicking);
            this.WEIGHT_TIME = GetTaskTime(taskMap, TASK_IDEnum.Weight_Success);
            this.WEIGHT_REJECT_TIME = GetTaskTime(taskMap, TASK_IDEnum.Weight_Fail);
            this.SMART_PACKING_TIME = GetTaskTime(taskMap, TASK_IDEnum.SmartPacking_Success);
            this.SMART_PACKING_REJECT_TIME = GetTaskTime(taskMap, TASK_IDEnum.SmartPacking_Fail);
            this.SMART_PRINT_TIME = GetTaskTime(taskMap, TASK_IDEnum.SmartInvoice);
            this.NORMAL_PRINT_TIME = GetTaskTime(taskMap, TASK_IDEnum.NormaInvoice);
            this.TOP_TIME = GetTaskTime(taskMap, TASK_IDEnum.TopBcr_Success);
            this.TOP_REJECT_TIME = GetTaskTime(taskMap, TASK_IDEnum.TopBcr_Fail);
            this.OUT_TIME = GetTaskTime(taskMap, TASK_IDEnum.OutBcr);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
