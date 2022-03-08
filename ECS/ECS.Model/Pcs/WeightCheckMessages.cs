using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Pcs
{
    #region Touch -> Server
    [Serializable]
    public class WeightCheck
    {
        public long WeightCheckIndex { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.WeightCheckIndex)} : {this.WeightCheckIndex}" );

            return sb.ToString();
        }
    }

    [Serializable]
    public class WeightCheckBcrState
    {
        public bool WeightCheckBcrConnection { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.WeightCheckBcrConnection)} : {this.WeightCheckBcrConnection}");

            return sb.ToString();
        }
    }
    #endregion
}
