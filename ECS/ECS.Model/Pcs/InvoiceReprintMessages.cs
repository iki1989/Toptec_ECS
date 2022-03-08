using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Pcs
{
    #region Touch -> Server
    [Serializable]
    public class InvoiceReprintRequest
    {

        public string BoxId { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.BoxId)} : {this.BoxId}");

            return sb.ToString();
        }
    }
    #endregion

    #region Touch <- Server
    [Serializable]
    public class InvoiceReprintResponse
    {

        public bool Result { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.Result)} : {this.Result}");

            return sb.ToString();
        }
    }
    #endregion
}
