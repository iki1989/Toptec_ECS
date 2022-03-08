using System;
using System.Threading.Tasks;
using Urcis.SmartCode.Net;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Net.Tcp;
using Urcis.SmartCode;
using ECS.Core.Communicators;
using ECS.Core.Communicators.BixolonSlpD420;
using ECS.Core.Managers;
using ECS.Model;

namespace ECS.Core.Equipments
{
    public class LabelPrinterBixolonEquipment : Equipment
    {
        #region Prop
        public new LabelPrinter_BixolonSlpD420Communicator Communicator 
        {
            get => base.Communicator as LabelPrinter_BixolonSlpD420Communicator;
            private set => base.Communicator = value;
        }

        public new LabelPrinterBixolonEquipmentSetting Setting
        {
            get => base.Setting as LabelPrinterBixolonEquipmentSetting;
            private set => base.Setting = value;
        }

        public bool IsConnected { get; set; }
        #endregion

        #region Ctor
        public LabelPrinterBixolonEquipment(LabelPrinterBixolonEquipmentSetting setting)
        {
            this.Setting = setting ?? new LabelPrinterBixolonEquipmentSetting();
        }
        #endregion

        #region Method
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            base.OnCreate();
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.LabelPrinterBixolonLog));
            this.Communicator = new LabelPrinter_BixolonSlpD420Communicator(this);
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
                    this.Communicator.Error += this.Communicator_OnError;
                    this.Communicator.TcpConnectionStateChanged += this.Communicator_TcpConnectionStateChanged;
                    this.Communicator.OperationStateChanged += this.Communicator_OperationStateChanged;
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
                    this.Communicator.Error -= this.Communicator_OnError;
                    this.Communicator.TcpConnectionStateChanged -= this.Communicator_TcpConnectionStateChanged;
                    this.Communicator.OperationStateChanged -= this.Communicator_OperationStateChanged;
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

        public async Task<bool> PrintSendAsync(string ZplCommand) => await this.Communicator?.ZplPrintSendAsync(ZplCommand);

        public async Task<bool> ReprintSendAsync() => await this.Communicator?.ReprintSendAsync();
        #endregion

        #region Event Handler
        private void Communicator_TcpConnectionStateChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            this.Logger?.Write(e.Current.ToString());
        }

        private async void Communicator_OperationStateChanged(object sender, CommunicatorOperationStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;
            this.Logger?.Write(e.Current.ToString());

            if (e.Current == CommunicatorOperationStateEnum.Started)
                await this.Communicator.StartAsync();
            else
                await this.Communicator.StopAsync();        
        }

        private void Communicator_OnError(object sender, SLCS_ERROR_CODE e) => this.Logger?.Write(e.ToString());

        public override void OnHub_Recived(EventArgs e)
        {

        }
        #endregion
    }

    [Serializable]
    public class LabelPrinterBixolonEquipmentSetting : EquipmentSetting
    {
        public new BixolonCommunicatorSetting CommunicatorSetting 
        {
            get => base.CommunicatorSetting as BixolonCommunicatorSetting;
            set => base.CommunicatorSetting = value;
        }

        public LabelPrinterBixolonEquipmentSetting()
        {
            this.CommunicatorSetting = new BixolonCommunicatorSetting();
            this.Name = "Bixolon";
        }
    }
}
