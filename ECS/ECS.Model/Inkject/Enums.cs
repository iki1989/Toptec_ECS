using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Inkject
{
    public enum AutoDataResponseEnum
    {
        XON, //프린트 대기열 공간이 있을때
        XOFF, //프린트 대기열 공간이 없을때
    }

    public enum WriteAutoDataResponseEnum
    {
        Received, //프린트 대기열 공간이 있을때
        XOFF, //프린트 대기열 공간이 없을때
        QueueCleared //다른 데이터도 없고 끝에 틸드(~)도 없을때
    }

    public enum DirectionEnum
    {
       Up,
       Down
    }

    public enum TypeEnum
    {
        Numeric,
        Alpha,
        Alphanumeric,
    }

    public enum BcrResultEnum
    {
        OK = 0,
        Reject = 1,
    }
}
