using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Domain.Wcs
{
    #region 박스 ID 생성 (WCS-01)
    [Description("WCS-01")]
    public class BoxId : WcsCommon<BoxIdData> { }
    #endregion

    #region 중량검수정보 수신 (WCS-02)
    [Description("WCS-02")]
    public class WeightResult : WcsCommon<WeightResultData> { }
    #endregion

    #region BCR스캔 정보 (WCS-03)
    [Description("WCS-03")]
    public class BcrScanResult : WcsCommon<BcrScanResultData> { }
    #endregion

    #region 분류확정 (WCS-04)
    [Description("WCS-04")]
    class SortResult : WcsCommon<BcrScanResultData> { }
    #endregion
}
