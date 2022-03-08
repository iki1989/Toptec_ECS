using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECS.Model.Pcs;
using ECS.Model.Plc;
using ECS.Model.Restfuls;

namespace ECS.Model.Hub
{

    #region DynamicScale
    public class DynamicScaleBcrOnReadDataArgs : EventArgs
    {
        public string Id { get; set; }
        public string BoxID { get; set; }
        public bool NoReadCheck { get; set; }
    }

    public class WeightCheckIndexArgs : EventArgs
    {
        public long WeightCheckIndex { get; set; }
    }
    #endregion

    #region OutLogical
    public class OutLogicalBcrOnReadDataArgs : EventArgs
    {
        public string InvoiceNo { get; set; }
        public string BoxId { get; set; }
    }
    #endregion

    #region PlcCaseErect
    public class CaseErectBoxTypeArgs : EventArgs
    {
        public string BoxType { get; set; }
    }

    public class CaseErectBcrResultArgs : EventArgs
    {
        public string EqpId { get; set; }
        public int BcrId { get; set; }
        public long BcrIndex { get; set; }
        public string BoxId { get; set; }
        public string BoxType { get; set; }
        public BcrResult Result { get; set; }
    }

    public class CaseErectInkJectCompleteArgs : EventArgs
    {
        public string BoxID { get; set; }
    }

    public class CaseErectBcrAlarmArgs : EventArgs
    {
        public int BcrNumber { get; set; }
        public bool Result { get; set; }
    }

    public class CaseErectInkJectAlarmArgs : EventArgs
    {
        public int InkjectNumber { get; set; }
        public bool Result { get; set; }
    }
    #endregion

    #region PlcWeightInvoice
    public class WeightOrInvoiceResultArgs : EventArgs
    {
        public WeightAndInvoice WeightAndInvoice { get; set; } = new WeightAndInvoice();
        public DynamicResult Result { get; set; } = DynamicResult.Reject;
        public string BoxId { get; set; }
    }

    public class RouteResultArgs : EventArgs
    {
        public RouteResult Result { get; set; } = RouteResult.NORMAL;

        public string BoxId { get; set; }
    }


    public class TopBcrResultArgs : EventArgs
    {
        public string BoxId { get; set; }
        public string Invoice { get; set; }
        public BcrResult Result { get; set; } = BcrResult.Reject;
    }


    public class WeightInvoiceDynamiScaleAlarmArgs : EventArgs
    {
        public bool Result { get; set; }
    }

    public class WeightInvoiceBcrAlarmArgs : EventArgs
    {
        public WeightInvoicBcrEnum BcrType { get; set; }
        public bool Result { get; set; }
    }

    public class WeightInvoiceLabelPrintAlarmArgs : EventArgs
    {
        public int LabelPrintNumber { get; set; }
        public bool Result { get; set; }
    }
    #endregion

    #region Pc CaseErect
    public class InkjectInkInformationArgs : EventArgs
    {
        public int Line { get; set; }
        public int InkPercent { get; set; }
    }
    public class CaseErectEquipmentConnectionArgs : EventArgs
    {
        public ErectorConnectionState ConnectionState { get; set; } = new ErectorConnectionState();
    }
    public class CaseErectInkjectResumeArgs : EventArgs
    {
        public int Line { get; set; }
    }
    public class CaseErectBoxNumberArgs : EventArgs
    {
        public int Line { get; set; }
    }
    #endregion

    #region Pc BcrLcd
    public class BcrLcdIndexArgs : EventArgs
    {
        public long BcrIndex { get; set; }
    }

    public class TopRequestBoxIdArgs : EventArgs
    {
        public string BoxId { get; set; }
        public bool Result { get; set; }
    }
    #endregion

}
