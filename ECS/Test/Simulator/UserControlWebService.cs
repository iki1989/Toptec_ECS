using ECS.Core;
using ECS.Core.Managers;
using ECS.Core.Restful;
using ECS.Core.WebServices;
using ECS.Model;
using ECS.Model.Restfuls;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using System.Windows.Forms;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;

namespace Simulator
{
    public partial class UserControlWebService : UserControl
    {
        WebServiceManager WebService;

        public UserControlWebService(string ip, int port) : base()
        {
            InitializeComponent();
            this.groupBox1.Text = "RestfulService";

            WebServiceManagerSetting setting = new WebServiceManagerSetting();
            setting.WebServiceName = "Restful Server";
            setting.IP = ip;
            setting.Port = port;
            this.propertyGrid1.SelectedObject = setting;

            this.WebService = new WebServiceManager(setting);
            this.WebService.Create();
            this.WebService.Prepare();

            this.buttonStart_Click(this, null);
        }

        public void WriteLog(string text)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() => 
                {
                    this.WriteLog(text);
                }));
            }
            else
            {
                this.richTextBoxLog.AppendText($"{DateTime.Now.ToString("HH:mm:ss.fff")} > {text}{Environment.NewLine}");
                this.richTextBoxLog.ScrollToCaret();
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.WebService.Start();
            this.WriteLog("WebService Start");
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.WebService.Stop();
            this.WriteLog("WebService Stop");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.richTextBoxLog.Clear();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            this.WriteLog($"");
        }
    }

    public class WebServiceManager : ECS.Core.Managers.WebServiceManager
    {
        public WebServiceManager(WebServiceManagerSetting setting) : base(setting) { }
     
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            this.EcsWebServiceHosts.Clear();

            {
                var host = this.CreateWebServiceHost(typeof(DefaultRestfulServerService));
                if (host != null)
                {
                    ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IDefaultRestfulServerService), this.HttpBinding, "");
                    ep.EndpointBehaviors.Add(new WebHttpBehavior());
                    this.EcsWebServiceHosts.Add(host);
                }
            }

            {
                var host = this.CreateWebServiceHost(typeof(RicpServerService));
                if (host != null)
                {
                    ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IRicpServerService), this.HttpBinding, "ricp/api/ecs");
                    ep.EndpointBehaviors.Add(new WebHttpBehavior());
                    this.EcsWebServiceHosts.Add(host);
                }
            }

            {
                var host = this.CreateWebServiceHost(typeof(WcsServerService));
                if (host != null)
                {
                    ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IWcsServerService), this.HttpBinding, "api/v1/wcs");
                    ep.EndpointBehaviors.Add(new WebHttpBehavior());
                    this.EcsWebServiceHosts.Add(host);
                }
            }

            this.LifeState = LifeCycleStateEnum.Created;
        }
    }

    [ServiceContract]
    public interface IRicpServerService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "device/status")]
        [return: MessageParameter(Name = "Json")]
        Task<RicpResponse> DeviceStatusPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "container/scan")]
        [return: MessageParameter(Name = "Json")]
        Task<RicpResponse> ContainerScanPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "invoice/scan")]
        [return: MessageParameter(Name = "Json")]
        Task<RicpResponse> InvoiceScanPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "outinvoice/scan")]
        [return: MessageParameter(Name = "Json")]
        Task<RicpResponse> OutInvoiceScanPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "location/point/push")]
        [return: MessageParameter(Name = "Json")]
        Task<RicpLocationPointPushResponse> LocationPointPushPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "location/point/status")]
        [return: MessageParameter(Name = "Json")]
        Task<RicpLocationPointStatusResponse> LocationPointStatusPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "rolltainer/sensor/status")]
        [return: MessageParameter(Name = "Json")]
        Task<RicpResponse> RolltainerSensorStatusPostAsync(Stream body);
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class RicpServerService : BaseService, IRicpServerService
    {
        public RicpServerService()
        {
            string name = "RICP Service";
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(name, EcsAppDirectory.RestfulWebServiceRicpLog));
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<RicpResponse> DeviceStatusPostAsync(Stream body)
        {
            RicpResponse response = new RicpResponse().SetBadRequset();

            DeviceStatus deviceStatus = await this.ParseBody<DeviceStatus>(body) as DeviceStatus;
            if (deviceStatus == null)
                return response;

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(DeviceStatus), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<RicpResponse> ContainerScanPostAsync(Stream body)
        {
            RicpResponse response = new RicpResponse().SetBadRequset();

            ContainerScan containerScan = await this.ParseBody<ContainerScan>(body) as ContainerScan;
            if (containerScan == null)
                return response;

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(ContainerScan), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<RicpResponse> InvoiceScanPostAsync(Stream body)
        {
            RicpResponse response = new RicpResponse().SetBadRequset();

            InvoiceScan invoiceScan = await this.ParseBody<InvoiceScan>(body) as InvoiceScan;
            if (invoiceScan == null)
                return response;

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(InvoiceScan), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<RicpResponse> OutInvoiceScanPostAsync(Stream body)
        {
            RicpResponse response = new RicpResponse().SetBadRequset();

            OutInvoiceScan outInvoiceScan = await this.ParseBody<OutInvoiceScan>(body) as OutInvoiceScan;
            if (outInvoiceScan == null)
                return response;

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(OutInvoiceScan), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<RicpLocationPointPushResponse> LocationPointPushPostAsync(Stream body)
        {
            RicpLocationPointPushResponse response = new RicpLocationPointPushResponse().SetBadRequset();

            LocationPointPush locationPointPush = await this.ParseBody<LocationPointPush>(body) as LocationPointPush;
            if (locationPointPush == null)
                return response;

            locationPointPush.locationPointCode = "12051005";
            
            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(LocationPointPush), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<RicpLocationPointStatusResponse> LocationPointStatusPostAsync(Stream body)
        {
            RicpLocationPointStatusResponse response = new RicpLocationPointStatusResponse().SetBadRequset();

            LocationPointStatus locationPointStatus = await this.ParseBody<LocationPointStatus>(body) as LocationPointStatus;
            if (locationPointStatus == null)
                return response;

            locationPointStatus.locationPointCode = "12051005";
            locationPointStatus.pushWorkId = "123";

            response.success = true;
            response.resultData = new RicpLocationPointStatusResponse.result();
            response.resultData.locationPointCode = locationPointStatus.locationPointCode;
            response.resultData.pushWorkId = locationPointStatus.pushWorkId;
            response.resultData.pushWorkStatusCd = PushWorkStatusCdEnum.ING;

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(LocationPointStatus), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<RicpResponse> RolltainerSensorStatusPostAsync(Stream body)
        {
            RicpResponse response = new RicpResponse().SetBadRequset();

            RolltainerSensorStatus rolltainerSensorStatus = await this.ParseBody<RolltainerSensorStatus>(body) as RolltainerSensorStatus;
            if (rolltainerSensorStatus == null)
                return response;

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(RolltainerSensorStatus), response.ToJson());
            return response;
        }
    }

    [ServiceContract]
    public interface IWcsServerService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "boxId")]
        [return: MessageParameter(Name = "Json")]
        Task<WcsResponse> BoxIdPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "rsltWgt")]
        [return: MessageParameter(Name = "Json")]
        Task<WcsResponse> RsltWgtPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "rsltWaybill")]
        [return: MessageParameter(Name = "Json")]
        Task<WcsResponse> RsltWaybillPostAsync(Stream body);
    }
   
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class WcsServerService : BaseService, IWcsServerService
    {
        public WcsServerService()
        {
            string name = "WCS Service";
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(name, EcsAppDirectory.RestfulWebServiceWcsLog));
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<WcsResponse> BoxIdPostAsync(Stream body)
        {
            WcsResponse response = new WcsResponse().SetBadRequset();

            BoxID boxID = await this.ParseBody<BoxID>(body) as BoxID;
            if (boxID == null)
                return response;

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(BoxID), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<WcsResponse> RsltWgtPostAsync(Stream body)
        {
            WcsResponse response = new WcsResponse().SetBadRequset();

            WeightAndInvoice weightInvoice = await this.ParseBody<WeightAndInvoice>(body) as WeightAndInvoice;
            if (weightInvoice == null)
                return response;

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(WeightAndInvoice), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<WcsResponse> RsltWaybillPostAsync(Stream body)
        {
            WcsResponse response = new WcsResponse().SetBadRequset();

            WeightAndInvoice weightInvoice = await this.ParseBody<WeightAndInvoice>(body) as WeightAndInvoice;
            if (weightInvoice == null)
                return response;

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(WeightAndInvoice), response.ToJson());
            return response;
        }
    }
}
