using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ECS.Model.Restfuls
{
    [Serializable]
    public class RicpFormat
    {
        public string ToJson() => JsonConvert.SerializeObject(this);
    }

    public enum SystemCodeEnum
    {
        UNKNOWN,

        ROBOT_PICKING_RMS,      //P800 AGV
        BOX_MOVING_RMS,         //P500 AGV
        ROLLTAINER_MOVING_RMS,
    }

    public enum InstructionEnum
    {
        UNKNOWN,

        SYSTEM_STOP,     
        FIRE_STOP,         
        SYSTEM_RECOVER,
        TASK_STOP,
        TASK_RECOVER,
    }

    public enum SourceEnum
    {
        UNKNOWN,

        BROWSE,         //Browser front end
        INTERFACE,      // Interface
        EQUIPMENT,      // equipment, such as emergency stop controllers, etc.
        DMP,            // DMP system
    }

    public enum sensorStatusEnum
    {
        ON,
        OFF,
    }
}
