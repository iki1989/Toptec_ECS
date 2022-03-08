using ECS.Model.Databases;
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
    public struct TrackingData : IReaderConvertable
    {
        public static TrackingData None = new TrackingData() { };
        public string BOX_ID { get; set; }
        public string INVOICE_ID { get; set; }
        public string CST_ORD_NO { get; set; }
        public string TASK_ID { get; set; }
        public string TASK_TYPE { get; set; }
        public DateTime? TASK_TIME { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.BOX_ID = reader[nameof(BOX_ID)]?.ToString() ?? "";
            this.INVOICE_ID = reader[nameof(INVOICE_ID)]?.ToString() ?? "";
            this.CST_ORD_NO = reader[nameof(CST_ORD_NO)]?.ToString() ?? "";
            this.TASK_ID = reader[nameof(TASK_ID)]?.ToString() ?? "";
            this.TASK_TIME = DomainUtil.GetValueOrNull<DateTime>(reader, nameof(TASK_TIME));
            this.ParseTask();
        }
        private void ParseTask()
        {
            switch ((TASK_IDEnum) int.Parse(this.TASK_ID))
            {
                case TASK_IDEnum.Order:
                    this.TASK_TYPE = "주문 수신";
                    break;
                case TASK_IDEnum.RicpPicking:
                    this.TASK_TYPE = "피킹 완료";
                    break;
                case TASK_IDEnum.Weight_Fail:
                    this.TASK_TYPE = "중량 검수 실패";
                    break;
                case TASK_IDEnum.Operator_Weight_Result:
                    this.TASK_TYPE = "중량 수기 검수";
                    break;
                case TASK_IDEnum.Weight_Success:
                    this.TASK_TYPE = "중량 검수 성공";
                    break;
                case TASK_IDEnum.WcsPikcing:
                    this.TASK_TYPE = "WCS 피킹 완료";
                    break;
                case TASK_IDEnum.RouteBcr:
                    this.TASK_TYPE = "분기 BCR 통과";
                    break;
                case TASK_IDEnum.SmartPacking_Fail:
                    this.TASK_TYPE = "충진 실패";
                    break;
                case TASK_IDEnum.Operator_Packing_Result:
                    this.TASK_TYPE = "충진 수동 처리";
                    break;
                case TASK_IDEnum.SmartPacking_Success:
                    this.TASK_TYPE = "충진 통과";
                    break;
                case TASK_IDEnum.SmartInvoice:
                    this.TASK_TYPE = "스마트 송장 발행";
                    break;
                case TASK_IDEnum.NormaInvoice:
                    this.TASK_TYPE = "일반 송장 발행";
                    break;
                case TASK_IDEnum.TopBcr_Fail:
                    this.TASK_TYPE = "상면 검증 실패";
                    break;
                case TASK_IDEnum.TopBcr_Success:
                    this.TASK_TYPE = "상면 검증 성공";
                    break;
                case TASK_IDEnum.Operator_Invoice_Result:
                    this.TASK_TYPE = "상면 수동 검증";
                    break;
                case TASK_IDEnum.OutBcr:
                    this.TASK_TYPE = "2층 출고 완료";
                    break;
                case TASK_IDEnum.OrderCancel:
                    this.TASK_TYPE = "주문 취소";
                    break;
                default:
                    this.TASK_TYPE = "";
                    break;
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
