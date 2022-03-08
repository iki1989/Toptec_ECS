using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode.Io;

namespace ECS.Model.Plc
{
    public class PlcWeightInvoiceIoHandler
    {
        #region PLC Priority
        public IoPointRollTainer[] IoPointRollTainers { get; set; } = new IoPointRollTainer[2];
        public IoPointCurrentSmartBoxRouteMode IoPointCurrentSmartBoxRouteMode { get; set; } = new IoPointCurrentSmartBoxRouteMode();
        public IoPointCVSpeed[] IoPointCVSpeeds { get; set; } = new IoPointCVSpeed[20];

        public BitEventIoHandler P500EMGBitEvent { get; set; }

        public IoPoint WeightBCR_CV_StuckOn { get; set; }
        public IoPoint InvoiceBCR_CV_StuckOn { get; set; }

        public DataEventIoHandler BoxHeightSensorDataEvent { get; set; }
        #endregion

        #region PC Priority
        //#2, #3, #4-1, #4-2, #5, #6
        public IoPoint[] BcrsAlarms { get; set; } = new IoPoint[6];
        public IoPoint DynamicScaleAlarm { get; set; }
        public IoPoint[] InvocieLabelPrinterAlarms { get; set; } = new IoPoint[2];
        public IoPoint TopBcrAlarm { get; set; }

        public DataCommandIoHandler WeightInspectionResult { get; set; }
        public DataCommandIoHandler RouteResult { get; set; }
        public DataCommandIoHandler InvoiceResult { get; set; }
        public DataCommandIoHandler SmartBoxRouteMode { get; set; }

        public DataCommandIoHandler[] CVSpeeds { get; set; } = new DataCommandIoHandler[20];

        public DataCommandIoHandler OutResult { get; set; }

        public IoPoint[] RollTainersTowerLampIoPoints { get; set; } = new IoPoint[2];
        #endregion

        #region Ctor
        public PlcWeightInvoiceIoHandler()
        {
            for (int i = 0; i < this.IoPointCVSpeeds.Length; i++)
                this.IoPointCVSpeeds[i] = new IoPointCVSpeed();

            for (int i = 0; i < this.IoPointRollTainers.Length; i++)
                this.IoPointRollTainers[i] = new IoPointRollTainer();
        }
        #endregion
    }

    public class IoPointCurrentSmartBoxRouteMode
    {
        public IoPoint IoPointMode { get; set; }
        public IoPoint IoPointSmartWayRatio { get; set; }
        public IoPoint IoPointNormalWayRatio { get; set; }
    }

    public enum WeightInvoicBcrEnum
    {
        DynamicScale = 0,
        Route = 1,
        SmartLabel = 2,
        NormalLabel = 3,
        //TopSide = 4, //미사용. TopBcr과 인터페이스 통합됨
        Out = 5,


        Top = 10,
    }
}

