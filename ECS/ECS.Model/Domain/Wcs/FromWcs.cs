using System;
using System.ComponentModel;

namespace ECS.Model.Domain.Wcs
{
    #region 박스 마스터 (ECS-01)
    [Description("ECS-01 BoxMaster")]
    [Serializable]
    public class BoxMaster : WcsCommon<BoxMasterData> { }
    #endregion

    #region 주문정보 송신 (ECS-02)
    [Description("ECS-02 Order")]
    [Serializable]
    public class Order : WcsCommon<OrderData> { }
    #endregion

    #region 주문취소정보 송신 (ECS-03)
    [Description("ECS-03 OrderCancel")]
    [Serializable]
    public class OrderCancel : WcsCommon<OrderData> { }
    #endregion

    #region 피킹실적 송신 (ECS-04)
    [Description("ECS-04 PickingResult")]
    [Serializable]
    public class PickingResult : WcsCommon<PickingResultData> { }
    #endregion
}
