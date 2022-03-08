using ECS.Core.Communicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.Equipments
{
    public abstract class LogicalEquipment : Equipment
    {
        public BcrCommunicator Bcr { get; set; }
    }

    [Serializable]
    public class LogicalEquipmentSetting : EquipmentSetting
    {
        public BcrCommunicatorSetting BcrCommunicatorSetting { get; set; } = new BcrCommunicatorSetting();
    }
}
