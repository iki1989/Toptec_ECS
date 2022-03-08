using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Plc
{
    public enum EquipmentStateEnum : short
    {
        Stop = 0,
        Run = 1,
        Standby = 2,
        Error = 9,
    }
}
