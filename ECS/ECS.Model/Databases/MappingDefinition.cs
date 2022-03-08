namespace ECS.Model.Databases
{
    public enum statusEnum : int
    {
        order_received = 10, //출고계획(설비작업지시) 수신
        order_canceled = 99, //주문 취소

        pick_create_plan = 40, //피킹작업에 대한 계획 생성
        pick_picking = 45, //오더피킹 설비를 통한 실시간 피킹실적(부분피킹 중)
        pick_finish_picking = 50, //오더피킹 설비를 통한 피킹완료실적(전체피킹 완료)

        inspect_pass_sys = 60, //피킹실적에 대한 검수완료실적
        insepct_reject_scale = 65, //중량검수 오차범위 초과에 따른 실패
        inspect_pass_scale = 66, //중량검수 오차범위 내에 따른 성공
        
        top_invoice = 75,        //TopBcr 검증 내에 따른 성공
    }

    public enum WT_CHECK_FLAGEnum
    {
        Y, //중량검수 활성화
        N, //중량검수 비활성화
        M, //강제 리젝
        W, // 운송장부터 진행
        B, // BCR부터 진행
    }

    public enum MNL_PACKING_FLAGEnum
    {
        Y, //개별 포장
        N, //완제품 포장
    }

    public enum TASK_IDEnum
    {
        Unknown = -1,

        Order = 0, //주문정보

        RicpPicking = 10,               //RICP 피킹

        Weight_Fail = 20,               //중량검수 실패
        Operator_Weight_Result = 21,    //수기검수
        Weight_Success = 22,            //중량검수 성공
        
        WcsPikcing = 23,                //WCS 피킹

        RouteBcr = 30,                  //분기 BCR

        SmartPacking_Fail = 40,         //충진 실패
        Operator_Packing_Result = 41,   //충진 수기검수
        SmartPacking_Success = 42,      //충진 성공

        SmartInvoice = 50,              //스마트 송장
        NormaInvoice = 51,              //일반 송장

        TopBcr_Fail = 60,              //상면 실패
        TopBcr_Success = 61,           //상면 성공
        Operator_Invoice_Result = 62,  //송장 재발행

        OutBcr = 90,                   //출고 BCR

        OrderCancel = 99,              //주문취소
    }
}
