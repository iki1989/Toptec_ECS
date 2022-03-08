using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode.Io;

namespace ECS.Model.Plc
{
    public class PlcSmartPackingIoHandler
    {
        #region PLC Priority
        public DataEventIoHandler InputResult { get; set; }
        public DataEventIoHandler OutputResult { get; set; }
        public IoPointEquipmentStatus IoPointSmartPackingEquipmentStatuse { get; set; } = new IoPointEquipmentStatus();
        #endregion

        #region PC Priority
        public DataCommandIoHandler BCRReadingResult { get; set; }
        public IoPoint BcrAlarm { get; set; }
        #endregion
    }
}
