using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode;
using ECS.Core.WebServices;
using Urcis.SmartCode.Threading;
using System.Threading.Tasks;
using ECS.Model.WebService;
using ECS.Model;

namespace ECS.Core.Managers
{
    public class WebServiceManager : ILifeCycleable, IHaveLogger, IDrive
    {
        #region Field
        protected List<CustomWebServiceHost> EcsWebServiceHosts = new List<CustomWebServiceHost>();
        #endregion

        #region Prop
        private WebHttpBinding m_HttpBinding;
        protected WebHttpBinding HttpBinding
        {
            get
            {
                if (this.m_HttpBinding == null)
                {
                    this.m_HttpBinding = new WebHttpBinding();
                    this.m_HttpBinding.Name = "httpBinding";
                    this.m_HttpBinding.OpenTimeout = TimeSpan.FromSeconds(10);
                    this.m_HttpBinding.CloseTimeout = TimeSpan.FromSeconds(10);
                    this.m_HttpBinding.SendTimeout = TimeSpan.FromMinutes(10);
                    this.m_HttpBinding.ReceiveTimeout = TimeSpan.FromMinutes(10);
                    this.m_HttpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
                    this.m_HttpBinding.MaxBufferPoolSize = 2147483647;
                    this.m_HttpBinding.MaxReceivedMessageSize = 2147483647;
                    this.m_HttpBinding.ReaderQuotas = new XmlDictionaryReaderQuotas()
                    {
                        MaxDepth = 104857600,
                        MaxStringContentLength = 104857600,
                        MaxArrayLength = 104857600,
                        MaxBytesPerRead = 104857600,
                        MaxNameTableCharCount = 104857600
                    };
                    this.m_HttpBinding.Security.Mode = WebHttpSecurityMode.None;
                }

                return this.m_HttpBinding;
            }
        }

        public Logger Logger { get; protected set; }

        private LifeCycleStateEnum m_LifeState;
        public LifeCycleStateEnum LifeState
        {
            get => this.m_LifeState;
            protected set
            {
                if (this.m_LifeState == value) return;

                this.m_LifeState = value;
                this.Logger?.Write(this.m_LifeState.ToString());
            }
        }

        public WebServiceManagerSetting Setting { get; set; }
        #endregion

        #region Ctor
        public WebServiceManager(WebServiceManagerSetting setting)
        {
            this.Setting = setting ?? new WebServiceManagerSetting();
            string name = this.Setting.WebServiceName;
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(name, EcsAppDirectory.WebServicesLog));
            this.Logger.Setting.KeepingDays = 30;
            //this.Logger.Setting.KeepingDays = EcsServerAppManager.Instance.Logger.Setting.KeepingDays;
        }
        #endregion

        #region Method
        public void Create() => this.OnCreate();

        public ScTask CreateAsync() => ScTask.Run(() => this.Create());

        public void Prepare() => this.OnPrepare();

        public ScTask PrepareAsync() => ScTask.Run(() => this.Prepare());

        public void Terminate() => this.OnTerminate();

        public ScTask TerminateAsync() => ScTask.Run(() => this.Terminate());

        protected virtual void OnCreate() 
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
                var host = this.CreateWebServiceHost(typeof(WcsRestfulServerService));
                if (host != null)
                {
                    ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IWcsRestfulServerService), this.HttpBinding, WebServicePath.Wcs);
                    ep.EndpointBehaviors.Add(new WebHttpBehavior());
                    this.EcsWebServiceHosts.Add(host);
                }
            }

            {
                var host = this.CreateWebServiceHost(typeof(RicpRestfulServerService));
                if (host != null)
                {
                    ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IRicpRestfulServerService), this.HttpBinding, WebServicePath.Ricp);
                    ep.EndpointBehaviors.Add(new WebHttpBehavior());
                    this.EcsWebServiceHosts.Add(host);
                }
            }

            this.LifeState = LifeCycleStateEnum.Created;
        }

        protected virtual bool OnPrepare()
        {
            if (this.LifeState != LifeCycleStateEnum.Created)
                this.Create();

            this.LifeState = LifeCycleStateEnum.Preparing;

            foreach (var host in this.EcsWebServiceHosts)
            {
                host.Faulted += this.Host_Faulted;
                host.UnknownMessageReceived += this.Host_UnknownMessageReceived;
                host.Closed += this.Host_Closed;
                host.Closing += this.Host_Closing;
                host.Opened += this.Host_Opened;
                host.Opening += this.Host_Opening;
            }

            this.LifeState = LifeCycleStateEnum.Prepared;
            return true;
        }

        protected virtual void OnTerminate()
        {
            this.LifeState = LifeCycleStateEnum.Terminating;
            this.Stop();

            foreach (var host in this.EcsWebServiceHosts)
            {
                host.Faulted -= this.Host_Faulted;
                host.UnknownMessageReceived -= this.Host_UnknownMessageReceived;
                host.Closed -= this.Host_Closed;
                host.Closing -= this.Host_Closing;
                host.Opened -= this.Host_Opened;
                host.Opening -= this.Host_Opening;
            }

            this.LifeState = LifeCycleStateEnum.Terminated;
        }

        public void Start()
        {
            if (this.LifeState != LifeCycleStateEnum.Prepared)
            {
                this.Logger?.Write($"Start Falut : {this.LifeState}");
                return;
            }

            foreach (var host in this.EcsWebServiceHosts)
            {
                try
                {
                    if (host.IsOpend == false)
                        host.Open();
                }
                catch (CommunicationException ex)
                {
                    this.Logger?.Write(ex.Message);
                    host.Abort();
                }
            }
        }

        public void Stop()
        {
            foreach (var host in this.EcsWebServiceHosts)
            {
                if (host.IsOpend)
                    host.Close();
            }
        }

        public Task StartAsync() => Task.Run(() => this.Start());

        public Task StopAsync() => Task.Run(() => this.Stop());

        public CustomWebServiceHost CreateWebServiceHost(Type serviceType)
        {
            try
            {
                string uriString = $"http://{this.Setting.IP}:{this.Setting.Port}/";
                CustomWebServiceHost host = new CustomWebServiceHost(serviceType, new Uri(uriString));
                host.Name = serviceType.Name;

                #region Behavior
                ServiceDebugBehavior sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
                if (sdb == null)
                {
                    sdb = new ServiceDebugBehavior();
                    host.Description.Behaviors.Add(sdb);
                }
                sdb.IncludeExceptionDetailInFaults = false;
                sdb.HttpHelpPageEnabled = false;

                ServiceMetadataBehavior smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (smb == null)
                {
                    smb = new ServiceMetadataBehavior();
                    host.Description.Behaviors.Add(smb);
                }
                smb.HttpGetEnabled = true;
                smb.HttpsGetEnabled = true;

                ServiceThrottlingBehavior stb = host.Description.Behaviors.Find<ServiceThrottlingBehavior>();
                if (stb == null)
                {
                    stb = new ServiceThrottlingBehavior();
                    host.Description.Behaviors.Add(stb);
                }
                stb.MaxConcurrentSessions = 50;

                #endregion

                return host;
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            return null;
        }
        #endregion

        #region Event Handler
        private void Host_Faulted(object sender, EventArgs e)
        {
            if (sender is CustomWebServiceHost host)
                this.Logger?.Write($"{host.Name} : Fault");
        }

        private void Host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            if (sender is CustomWebServiceHost host)
                this.Logger?.Write($"{host.Name}({host.FirstAddress}) : {e.Message}");
        }

        private void Host_Closed(object sender, EventArgs e)
        {
            if (sender is CustomWebServiceHost host)
                this.Logger?.Write($"{host.Name}({host.FirstAddress}) : Closed");
        }

        private void Host_Closing(object sender, EventArgs e)
        {
            if (sender is CustomWebServiceHost host)
                this.Logger?.Write($"{host.Name}({host.FirstAddress}) : Closing");
        }

        private void Host_Opened(object sender, EventArgs e)
        {
            if (sender is CustomWebServiceHost host)
                this.Logger?.Write($"{host.Name}({host.FirstAddress}) : Opened");
        }

        private void Host_Opening(object sender, EventArgs e)
        {
            if (sender is CustomWebServiceHost host)
                this.Logger?.Write($"{host.Name}({host.FirstAddress}) : Opening");
        }
        #endregion
    }

    [Serializable]
    public class WebServiceManagerSetting : Setting
    {
        public string WebServiceName { get; set; } = "WebService";

        public string IP { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 8080;
    }
}
