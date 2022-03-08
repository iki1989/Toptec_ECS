using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.WebServices
{
    public class CustomWebServiceHost : WebServiceHost
    {
        public string Name { get; set; }

        public string FirstAddress
        {
            get
            {
                string address = string.Empty;

                ReadOnlyCollection<Uri> uris = this.BaseAddresses;
                if (uris == null) return address;

                foreach (Uri uri in uris)
                {
                    address = uri.AbsoluteUri;
                    break;
                }

                return address;
            }
        }

        public bool IsOpend { get; private set; }

        public CustomWebServiceHost(Type serviceType, params Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
            this.Opened += this.CustomWebServiceHost_Opened;
            this.Closed += this.CustomWebServiceHost_Closed;
        }

        private void CustomWebServiceHost_Opened(object sender, EventArgs e)
        {
            if (!this.IsDisposed)
                this.IsOpend = true;
        }

        private void CustomWebServiceHost_Closed(object sender, EventArgs e)
        {
            if (!this.IsDisposed)
                this.IsOpend = false;
        }
    }
}
