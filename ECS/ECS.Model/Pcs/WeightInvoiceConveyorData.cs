using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Pcs
{
    public class WeightInvoiceCvSpeed
    {
        public double Sv { get; set; }
        public double Pv { get; set; }
    }

    public class WeightInvoiceRatio
    {
        public short Mode { get; set; }
        public short SmartWayRatio { get; set; }
        public short NormalWayRatio { get; set; }
    }

    public class WeightInvoiceRejecOpTime
    {
        public double BottomSv { get; set; }
        public double TopSv { get; set; }
    }

    public class WeightInvoiceRollTainer
    {
        public bool[] Sensor1 { get; set; }
        public bool[] Sensor2 { get; set; }
    }
}
