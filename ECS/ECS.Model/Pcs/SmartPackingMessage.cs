using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Pcs
{
    #region Touch -> Server
    [Serializable]
    public class SmartPackingManualBoxValidationRequest
    {
        public string BoxId { get; set; }
        public int Result { get; set; }
        public int BufferCount { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.BoxId)} : {this.BoxId}");
            sb.Append($"{nameof(this.Result)} : {this.Result}");
            sb.Append($"{nameof(this.BufferCount)} : {this.BufferCount}");

            return sb.ToString();
        }
    }
    #endregion

    #region Touch <- Server
    [Serializable]
    public class SmartPackingConnectionState : BaseConnectionState
    {
        public bool SmartPackingConnection { get; set; }
        public bool SmartPackingBcrConnection { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.SmartPackingConnection)} : {this.SmartPackingConnection}");
            sb.Append($"{nameof(this.SmartPackingBcrConnection)} : {this.SmartPackingBcrConnection}");

            return sb.ToString();
        }
    }

    [Serializable]
    public class SmartPackingBcrRead
    {
        public long SmartPackingIndex { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.SmartPackingIndex)} : {this.SmartPackingIndex}");

            return sb.ToString();
        }
    }
    #endregion
}
