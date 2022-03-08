using System;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Net.Tcp;
using Urcis.SmartCode;
using ECS.Core.Communicators;
using ECS.Model;
using ECS.Model.Hub;
using ECS.Model.Plc;
using ECS.Core;
using ECS.Core.Equipments;
using ECS.Core.Managers;
using ECS.Model.LabelPrinter;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.Equipments
{
    public class LabelPrinterZebraZt411Equipment : Equipment
    {
        #region Prop
        public new LabelPrinter_ZebraZt411Communicator Communicator 
        {
            get => base.Communicator as LabelPrinter_ZebraZt411Communicator;
            protected set => base.Communicator = value;
        }

        public new LabelPrinterZebraZt411EquipmentSetting Setting
        {
            get => base.Setting as LabelPrinterZebraZt411EquipmentSetting;
            set => base.Setting = value;
        }
        #endregion

        #region Ctor
        public LabelPrinterZebraZt411Equipment(LabelPrinterZebraZt411EquipmentSetting setting)
        {
            this.Setting = setting ?? new LabelPrinterZebraZt411EquipmentSetting();
        }
        #endregion

        #region Method
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;
            base.OnCreate();
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.LabelPrinterZebraZt411Log));
            this.Communicator = new LabelPrinter_ZebraZt411Communicator(this);
            this.Setting.CommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
            this.Communicator?.ApplySetting(this.Setting.CommunicatorSetting);

            this.LifeState = LifeCycleStateEnum.Created;
        }

        protected override bool OnPrepare()
        {
            if (this.LifeState != LifeCycleStateEnum.Created)
                this.Create();

            this.LifeState = LifeCycleStateEnum.Preparing;

            try
            {
                if (this.Communicator != null && this.Communicator.IsDisposed == false)
                {
                    this.Communicator.HostStatusReturnRecived += this.Communicator_HostStatusReturnRecived;  
                   
                }
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); return false; }

            this.LifeState = LifeCycleStateEnum.Prepared;
            return true;
        }

        protected override void OnTerminate()
        {
            this.LifeState = LifeCycleStateEnum.Terminating;
            this.Stop();

            try
            {
                if (this.Communicator != null && this.Communicator.IsDisposed == false)
                {
                    this.Communicator.HostStatusReturnRecived -= this.Communicator_HostStatusReturnRecived;
                 
                    this.Communicator.Dispose();
                }
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.LifeState = LifeCycleStateEnum.Terminated;
        }

        protected override void OnStart()
        {
            if (this.LifeState != LifeCycleStateEnum.Prepared)
            {
                this.Logger?.Write($"Communicator Start Falut : {this.LifeState}");
                return;
            }

            if (this.Communicator != null || (this.Communicator.IsDisposed == false))
            {
                if (this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Disconnected)
                {
                    //동기가 느려서 비동기로 변경
                    //this.Communicator?.Start();
                    Task.Run(() => this.Communicator?.Start());
                    this.Logger?.Write("Communicator Start Async");
                }
            }
        }

        protected override void OnStop()
        {
            if (this.Communicator != null || (this.Communicator.IsDisposed == false))
            {
                if (this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Connected)
                {
                    this.Communicator.Stop();
                    this.Logger?.Write("Communicator Stop");
                }
            }
        }

        #region Send
        public bool RejectBoxPrintSend(string boxId)
        {
            this.CancelAllSend();

            StringBuilder sb = new StringBuilder();
            sb.Append($"^FO 150,260");
            sb.Append($"^A0N,72,72");
            sb.Append($"^FD{boxId}");
            sb.Append($"^FS");

            sb.Append($"^FO 104,360");
            sb.Append($"^^BY3");
            sb.Append($"^BCN,152,N,N,N");
            sb.Append($"^FD{boxId}");
            sb.Append($"^FS");

            var result = this.Communicator.Print(sb.ToString());

            return result;
        }

        public bool InvoicePrintSend(string text)
        {
            this.CancelAllSend();

            StringBuilder sb = new StringBuilder();
            sb.Append($"^FO 100,200");
            sb.Append($"^A0 B,250,250");
            sb.Append($"^FD {text}");
            sb.Append($"^FS");

            if (this.Communicator != null)
                return this.Communicator.Print(sb.ToString());

            return false;

            //this.HostStatusReturnSend();
        }

        public bool ZplPrintSend(string zpl)
        {
            zpl = zpl.Replace("\r", "").Replace("\n", "").Replace(Environment.NewLine, "");
            if (this.Communicator != null)
                return this.Communicator.SendMessage(zpl);

            return false;
        }

        public bool HostStatusReturnSend()
        {
            if (this.Communicator != null)
                return this.Communicator.SendMessage("~HS");

            return false;
        }

        public bool CancelAllSend()
        {
            if (this.Communicator != null)
                return this.Communicator.SendMessage("~JA");

            return false;
        }

        public void ReprintAfterErrorSend() => this.Communicator?.SendMessage("~JZ");
        #endregion
        #endregion

        #region Event Handler
        protected void Communicator_HostStatusReturnRecived(object sender, HostStatusReturnArg e)
        {
            this.Logger?.Write($"Host Status Return Recived : PauseFlag = {e.PauseFlag}, PaperOutFlag = {e.PaperOutFlag}");
        }

        public override void OnHub_Recived(EventArgs e)
        {

        }
        #endregion
    }

    [Serializable]
    public class LabelPrinterZebraZt411EquipmentSetting : EquipmentSetting
    {
        public new LabelPrinter_ZebraZT411CommunicatorSetting CommunicatorSetting 
        {
            get => base.CommunicatorSetting as LabelPrinter_ZebraZT411CommunicatorSetting;
            set => base.CommunicatorSetting = value;
        }

        public LabelPrinterZebraZt411EquipmentSetting()
        {
            this.CommunicatorSetting = new LabelPrinter_ZebraZT411CommunicatorSetting();
            this.Name = "ZebraZt411";
        }
    }
}
