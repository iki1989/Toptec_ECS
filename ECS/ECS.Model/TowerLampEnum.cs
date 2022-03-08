using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model
{
    public enum TowerLampEnum
    {
        OFF = 0,

        GREEN_On = 1,
        GREEN_Blink = 2,

        YEllOW_On = 3,
        YEllOW_Blink = 4,

        RED_On = 5,
        RED_Blink = 6,

        RED_YEllOW_GREEN_Blink = 7
    }
}
