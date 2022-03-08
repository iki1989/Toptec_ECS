
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode.Io;

namespace ECS.Model.Plc
{
    public class PlcGeneralIoHandler
    {
        #region PLC Priority
        public HeartbeatIoHandler HeartbeatHandler { get; set; }

        public IoPoint FireAlarmPoint { get; set; }

        public IoPoint EMGAlarmPoint { get; set; }


        public BitEventIoHandler P800DoorsOpenRequestBitEvent { get; set; }
        public BitEventIoHandler P500DoorsOpenRequestBitEvent { get; set; }

        public BitEventIoHandler P800DoorsCloseRequestBitEvent { get; set; }
        public BitEventIoHandler P500DoorsCloseRequestBitEvent { get; set; }

        public IoPoint P800DoorsOpendStateIoPoint { get; set; }
        public IoPoint P500DoorsOpendStateIoPoint { get; set; }
        public IoPoint P500EMGOpendStateIoPoint { get; set; }

        public IoPointEquipmentStatus PlcEquipmentStatus { get; set; } = new IoPointEquipmentStatus();
        #endregion


        #region PC Priority
        public DataCommandIoHandler TimeSetSyncCommander { get; set; }
        public BitCommandIoHandler FireAlarmBitCommander { get; set; }
        public BitCommandIoHandler FireAlarmResetBitCommander { get; set; }

        public BitCommandIoHandler P800DoorsOpenEnableCommander { get; set; }
        public BitCommandIoHandler P500DoorsOpenEnableCommander { get; set; }
        public BitCommandIoHandler P800DoorsCloseEnableCommander { get; set; }
        public BitCommandIoHandler P500DoorsCloseEnableCommander { get; set; }
        #endregion
    }

    public class IoPointEquipmentStatus
    {
        public IoPoint StatusCd { get; set; }

        public IoPoint ErrorMsg { get; set; }
    }

    public class IoPointCVSpeed
    {
        public IoPoint SV { get; set; }

        public IoPoint PV { get; set; }
    }

    public class IoPointRejectCylinderOperationTime
    {
        public IoPoint BottomSV { get; set; }
        public IoPoint TopSV { get; set; }
    }

    public enum AgvTypeEnum
    {
        P800,
        P500,
    }

    public class IoPointRollTainer
    {
        public IoPoint[] Sensors { get; set; } = new IoPoint[2];
    }

    public class DataCommandRollTainer
    {
        public DataCommandIoHandler[] Sensors { get; set; } = new DataCommandIoHandler[2];
    }
}
