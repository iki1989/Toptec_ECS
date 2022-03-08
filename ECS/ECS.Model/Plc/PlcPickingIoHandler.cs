
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode.Io;

namespace ECS.Model.Plc
{
    public class PlcPickingIoHandler
    {
        #region Nested
        public class BitEventStation
        {
            public BitEventIoHandler[] InputButtonOns { get; set; } = new BitEventIoHandler[2];

            public BitEventIoHandler[] OutputButtonOns { get; set; } = new BitEventIoHandler[2];
        }

        public class IoPointStation
        {
            public IoPoint[] InputButtonResponse { get; set; } = new IoPoint[2];

            public IoPoint[] OutputButtonResponse { get; set; } = new IoPoint[2];
        }
        #endregion

        #region PLC Priority
        public BitEventStation[] BitEventStations { get; set; } = new BitEventStation[7];

        public IoPointRollTainer[] IoPointRollTainers { get; set; } = new IoPointRollTainer[4];
        #endregion

        #region PC Priority
        public BitCommandIoHandler P500SystemStopRequestBitCommander { get; set; }
        public BitCommandIoHandler P500SystemResetRequestBitCommander { get; set; }

        public IoPointStation[] IoPointStationsButtonsRespose { get; set; } = new IoPointStation[7];

        public IoPoint[] TowerLampIoPoints { get; set; } = new IoPoint[7];

        public IoPoint[] RollTainersTowerLampIoPoints { get; set; } = new IoPoint[4];
        #endregion

        public PlcPickingIoHandler()
        {
            for (int i = 0; i < this.BitEventStations.Length; i++)
                this.BitEventStations[i] = new BitEventStation();

            for (int i = 0; i < this.IoPointStationsButtonsRespose.Length; i++)
                this.IoPointStationsButtonsRespose[i] = new IoPointStation();

            for (int i = 0; i < this.IoPointRollTainers.Length; i++)
                this.IoPointRollTainers[i] = new IoPointRollTainer();
        }
    }
}
