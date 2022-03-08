using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Pcs
{
    #region Touch <- Server
    [Serializable]
    public class TimeSyncronize
    {
        public DateTime Time { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.Time)} : {this.Time.ToString("HHmmss.fff")}");

            return sb.ToString();
        }
    }

    public abstract class BaseConnectionState { }

    [Serializable]
    public class BcrAlarmSetReset
    {
        public string Reason { get; set; }
        public bool AlarmResult { get; set; }
    }
    #endregion
}
