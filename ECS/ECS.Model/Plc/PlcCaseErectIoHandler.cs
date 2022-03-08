
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode.Io;

namespace ECS.Model.Plc
{
    public class PlcCaseErectIoHandler
    {
        #region Nested
        public class IoPointCaseErectInfo
        {
            public IoPoint BoxType { get; set; }

            public IoPoint BoxQty { get; set; }
        }
        #endregion

        #region PLC Priority
        public IoPointRollTainer[] IoPointRollTainers { get; set; } = new IoPointRollTainer[5];

        public IoPointEquipmentStatus[] IoPointCaseErectEquipmentStatuses { get; set; } = new IoPointEquipmentStatus[2];

        public IoPointCaseErectInfo[] IoPointCaseErectInfos { get; set; } = new IoPointCaseErectInfo[2];

        public IoPointCVSpeed[] IoPointCVSpeeds { get; set; } = new IoPointCVSpeed[5];

        public IoPointRejectCylinderOperationTime IoPointRejectCylinderOperationTime { get; set; } = new IoPointRejectCylinderOperationTime();

        public BitEventIoHandler P500EMGBitEvent { get; set; }
        #endregion

        #region PC Priority
        public IoPoint[] InkjectAlarms { get; set; } = new IoPoint[2];

        public IoPoint[] BcrAlarms { get; set; } = new IoPoint[2];

        public DataCommandIoHandler[] BcrReadingResults { get; set; } = new DataCommandIoHandler[2];

        public DataCommandIoHandler[] CVSpeeds { get; set; } = new DataCommandIoHandler[6];
        public DataCommandIoHandler RejecCylinderOperationTime { get; set; }

        #endregion

        #region Ctor
        public PlcCaseErectIoHandler()
        {
            for (int i = 0; i < this.IoPointRollTainers.Length; i++)
                this.IoPointRollTainers[i] = new IoPointRollTainer();

            for (int i = 0; i < this.IoPointCaseErectEquipmentStatuses.Length; i++)
                this.IoPointCaseErectEquipmentStatuses[i] = new IoPointEquipmentStatus();

            for (int i = 0; i < this.IoPointCaseErectInfos.Length; i++)
                this.IoPointCaseErectInfos[i] = new IoPointCaseErectInfo();

            for (int i = 0; i < this.IoPointCVSpeeds.Length; i++)
                this.IoPointCVSpeeds[i] = new IoPointCVSpeed();
        }
        #endregion
    }
}
