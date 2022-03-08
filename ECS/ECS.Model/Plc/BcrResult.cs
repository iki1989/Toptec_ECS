using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Plc
{
    public enum DynamicResult : short
    {
        NormalBox = 1,
        SmartBox = 2,

        Reject = 3,
        UNDER = 4,
        OVER = 5,
    }

    public enum RouteResult : short
    {
        NORMAL = 1,
        SMART = 2,
    }
    public enum BcrResult : short
    {
        OK = 1,
        Reject = 2,
        BYPASS = 3,
    }

    public enum RouteModeResult : short
    {
        Auto = 1,
        Ratio = 2,
    }

    public enum SmartPackingNGCode : short
    {
        OK = 0,
        WING_FAIL = 1,
        NOSKU = 2,
        HEIGHT_OVER = 3,
        VOLUME_OVER = 4,
        MULTIERROR = 5,
        NOREAD = 11,
        NOWEIGHT = 12,
        WEIGHT_FAIL = 13,
        NODATA = 14,
    }

    public enum SmartPackingDBResult : short
    {
        정상 = 0,
        날개접힘 = 1,
        상품없음 = 2,
        부피초과 = 3,
        높이초과 = 4,
        복합오류 = 5,
        강제진행 = 6,
        미인식 = 11,
        무게없음 = 12,
        중량실패 = 13,
        정보없음 = 14,
    }

    public enum SmartPackingRicpResult : short
    {
        BOX날개NG = 1,
        제품없음 = 2,
        볼륨오버 = 3,
        높이오버 = 4,
        멀티에러 = 5,
        Noread = 11,
        No_Weight = 12,
        Weight_Fail = 13,
        패킹메모리없음 = 14,
    }
}
