using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode;

namespace ECS.Model
{
    public class EcsAppDirectory
    {
        private static string EquipmentsLog => Path.Combine(AppDirectory.Instance.Log, "Equipments");

        #region Equipment
        public static string PlcPicking1Log => Path.Combine(EquipmentsLog, "PLC Picking#1");
        public static string PlcPicking2Log => Path.Combine(EquipmentsLog, "PLC Picking#2");
        public static string PlcCaseErectLog => Path.Combine(EquipmentsLog, "PLC CaseErect");
        public static string PlcWeightInvoiceLog => Path.Combine(EquipmentsLog, "PLC WeightInvoice");
        public static string PlcSmartPackingLog => Path.Combine(EquipmentsLog, "PLC SmartPacking");
       
        public static string Inkject1Log => Path.Combine(EquipmentsLog, "Inkject#1");
        public static string Inkject2Log => Path.Combine(EquipmentsLog, "Inkject#2");
        public static string LabelPrinterBixolonLog => Path.Combine(EquipmentsLog, "LabelPrinterBixolon");
        public static string LabelPrinterZebraZt411Log => Path.Combine(EquipmentsLog, "LabelPrinterZebraZt411");
        public static string LabelPrinterZebraZe500_SmartLog => Path.Combine(EquipmentsLog, "Label Printer Zebra Ze500 Smart");
        public static string LabelPrinterZebraZe500_NormalLog => Path.Combine(EquipmentsLog, "Label Printer Zebra Ze500 Normal");
        public static string DynamicScaleLog => Path.Combine(EquipmentsLog, "Dynamic Scale");
        public static string RouteLogical => Path.Combine(EquipmentsLog, "Route Logical");
        public static string OutLogical => Path.Combine(EquipmentsLog, "Out Logical");
        public static string TopBcr => Path.Combine(EquipmentsLog, "Top BCR");
        
        public static string TouchPcBcrLcd => Path.Combine(EquipmentsLog, "Touch PC BCR LCD");
        public static string TouchPcCaseErect => Path.Combine(EquipmentsLog, "Touch PC Case Erect");
        public static string TouchPcInvoiceReject => Path.Combine(EquipmentsLog, "Touch PC InvoiceReject");
        public static string TouchPcWeightInspector => Path.Combine(EquipmentsLog, "Touch PC Weight Inspector");
        public static string TouchPcConveyor1 => Path.Combine(EquipmentsLog, "Touch PC Conveyor#1");
        public static string TouchPcConveyor2 => Path.Combine(EquipmentsLog, "Touch PC Conveyor#2");
        public static string TouchSmartPacking => Path.Combine(EquipmentsLog, "Touch PC SmartPacking");

        public static string EcsServerPc => Path.Combine(EquipmentsLog, "ECS Server PC");
        #endregion

        public static string SqlServerLog => Path.Combine(AppDirectory.Instance.Log, "MSSQL Server");

        #region WebService
        public static string WebServicesLog => Path.Combine(AppDirectory.Instance.Log, "WebServices");
        public static string RestfulWebServiceWcsLog => Path.Combine(WebServicesLog, "Restful WebService WCS");
        public static string RestfulWebServiceRicpLog => Path.Combine(WebServicesLog, "Restful WebService RICP");
        #endregion

        public static string RestfulRequesterRicpLog => Path.Combine(AppDirectory.Instance.Log, "Restful Requester RICP");

        public static string RestfulRequesterWcsLog => Path.Combine(AppDirectory.Instance.Log, "Restful Requester WCS");

        public static string RestfulRequesterSpiralLog => Path.Combine(AppDirectory.Instance.Log, "Restful Requester Spiral");

        public static string CacheLog => Path.Combine(AppDirectory.Instance.Log, "Cache");

        public static string MssqlBackup => Path.Combine(AppDirectory.Instance.Root, "MSSQL Backup");
    }
}
