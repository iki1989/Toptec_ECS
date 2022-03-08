using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Pcs
{
    #region Touch <- Server
    [Serializable]
    public class InvoiceBcrRead
    {
        public long InvoiceBcrIndex { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.InvoiceBcrIndex)} : {this.InvoiceBcrIndex}");

            return sb.ToString();
        }
    }
    [Flags]
    public enum LabelerStateFlag
    {
        None = 0,
        PaperEmptyWarning = 0b0001,
        EtcWarning = 0b0010,
        PrintError = 0b0100,
        EtcError = 0b1000
    }
    [Serializable]
    public struct PrintInfo
    {
        public bool PrintBcrConnection { get; set; }
        public bool LabellerConnection { get; set; }
        public LabelerStateFlag LabelerState { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.PrintBcrConnection)} : {this.PrintBcrConnection}, ");
            sb.Append($"{nameof(this.LabellerConnection)} : {this.LabellerConnection}, ");
            sb.Append($"{nameof(this.LabelerState)} : {this.LabelerState}");

            return sb.ToString();
        }
    }
    [Serializable]
    public class InvoiceBcrState
    {
        public bool RouteBcrConnection { get; set; }
        public PrintInfo[] PrintInfoList { get; set; }
        public bool TopBcrConnection { get; set; }
        public bool OutBcrConnection { get; set; }
        public bool PlcConnection { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.RouteBcrConnection)} : {this.RouteBcrConnection}");
            sb.Append($"{nameof(this.PrintInfoList)} : [");
            if (PrintInfoList != null) {
                foreach (var info in PrintInfoList)
                    sb.Append($"  {info}, ");
            }
            sb.Append($"]");
            sb.Append($"{nameof(this.TopBcrConnection)} : {this.TopBcrConnection}, ");
            sb.Append($"{nameof(this.OutBcrConnection)} : {this.OutBcrConnection}, ");
            sb.Append($"{nameof(this.PlcConnection)} : {this.PlcConnection}");

            return sb.ToString();
        }
    }
    #endregion
}
