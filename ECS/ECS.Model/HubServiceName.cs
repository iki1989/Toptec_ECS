using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model
{
    public class HubServiceName
    {
        #region Equipment
        public static string PlcPicking1Equipment => "Picking#1";
        public static string PlcPicking2Equipment => "Picking#2";
        public static string PlcCaseErectEquipment => "CaseErect";
        public static string PlcWeightInvoiceEquipment => "WeightInvoice";
        public static string PlcSmartPackingEquipment => "SmartPacking";


        public static string DynamicScaleEquipment => "DynamicScale";
        public static string InkjectEquipment1 => "Inkject#1";
        public static string InkjectEquipment2 => "Inkject#2";

        public static string NormalLabelPrinterZebraZe500Equipment => "Normal Invoice Label Printer";
        public static string SmartLabelPrinterZebraZe500Equipment => "Smart Invoice Label Printer";

        public static string TopBcrEquipment => "Top BCR";

        public static string OutLogicalEquipment => "Out Logical";
        public static string RouteLogicalEquipment => "Route Logical";
        #endregion


        #region Touch PC
        public static string TouchPcCaseErectEquipment => "Touch PC CaseErect";
        public static string TouchPcWeightInspectorEquipment => "Touch PC Weight Inspector";
        public static string TouchPcInvoiceRejectEquipment => "Touch PC Invoice Reject";

        public static string TouchPcBcrLcdEquipment => "Touch PC Bcr Lcd";

        public static string TouchPcConveyorEquipment1 => "Touch PC Conveyor1";

        public static string TouchPcConveyorEquipment2 => "Touch PC Conveyor2";

        public static string TouchPcSmartPackingEquipment => "Touch PC Smart Packing";
        #endregion


        public static string RicpPost => "Ricp Post";
        public static string WcsPost => "Wcs Post";
        public static string SpiralPost => "Spiral Post";

    }
}
