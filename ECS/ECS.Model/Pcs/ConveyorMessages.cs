using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS.Model.Plc;

namespace ECS.Model.Pcs
{
    #region Touch -> Server
    [Serializable]
    public class CvSpeedRequest
    {
        public int ConveyorSpeed { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.ConveyorSpeed)} : {this.ConveyorSpeed}");
            return sb.ToString();
        }
    }

    #endregion

    #region Touch <- Server
    [Serializable]
    public class ConveyorCvSpeed
    {
        public int ConveyorSpeed { get; set; }
        public double Sv { get; set; }
        public double Pv { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.ConveyorSpeed)} : {this.ConveyorSpeed}");
            sb.Append($"{nameof(this.Sv)} : {this.Sv}");
            sb.Append($"{nameof(this.Pv)} : {this.Pv}");

            return sb.ToString();
        }
    }
    #endregion

    public class RouteModeRequest
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"RouteModeRequest : RouteModeRun");
            return base.ToString();
        }
    }

    public class RouteMode
    {
        public string Mode { get; set; }
        public int SmartRatio { get; set; }
        public int NormalRatio { get; set; }
    }
}
