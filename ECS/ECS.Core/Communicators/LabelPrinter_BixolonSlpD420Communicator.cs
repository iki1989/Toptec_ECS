using System;
using System.Text;
using System.Timers;
using Urcis.SmartCode.Net.Tcp;
using Urcis.SmartCode.Net;
using System.Threading.Tasks;
using ECS.Core.Communicators.BixolonSlpD420;
using ECS.Model;

namespace ECS.Core.Communicators
{
    public class LabelPrinter_BixolonSlpD420Communicator : Communicator
    {
        #region Event
        public event EventHandler<SLCS_ERROR_CODE> Error;
        public event EventHandler<TcpConnectionStateChangedEventArgs> TcpConnectionStateChanged;
        #endregion

        #region Field
        private Timer connectionStateTimer = new Timer(100);

        //private const int ISerial = 0;
        //private const int IParallel = 1;
        //private const int IUsb = 2;
        private const int ILan = 3;
        //private const int IBluetooth = 5;
        #endregion

        #region Prop
        public new BixolonCommunicatorSetting Setting => base.Setting as BixolonCommunicatorSetting;

        private TcpConnectionStateEnum m_TcpConnectionState;
        public TcpConnectionStateEnum TcpConnectionState
        {
            get => this.m_TcpConnectionState;
            private set 
            {
                if (this.m_TcpConnectionState == value) return;
                
                this.m_TcpConnectionState = value;
                if (this.m_TcpConnectionState == TcpConnectionStateEnum.Connected)
                    this.TcpConnectionStateChanged?.Invoke(this, new TcpConnectionStateChangedEventArgs(TcpConnectionStateEnum.Disconnected, TcpConnectionStateEnum.Connected));
                else
                    this.TcpConnectionStateChanged?.Invoke(this, new TcpConnectionStateChangedEventArgs(TcpConnectionStateEnum.Connected, TcpConnectionStateEnum.Disconnected));
            }
        }

        public SLCS_ERROR_CODE m_CurrentError = SLCS_ERROR_CODE.ERR_CODE_CONNECT;
        public SLCS_ERROR_CODE CurrentError
        {
            get => this.m_CurrentError;
            set 
            {
                if (this.m_CurrentError == value) return;

                this.m_CurrentError = value;
                this.Error?.Invoke(this, this.m_CurrentError);
            }
        }
        #endregion

        #region Ctor
        public LabelPrinter_BixolonSlpD420Communicator(object owner) : base(owner)
        {
            this.connectionStateTimer.Elapsed += this.ConnectionStateTimer_Elapsed;
        }
        #endregion

        #region Method
        public void ApplySetting(BixolonCommunicatorSetting setting)
        {
            setting = setting ?? new BixolonCommunicatorSetting();
            
            base.ApplySetting(setting);
        }

        public async Task StartAsync()
        {
            int nStatus = (int)SLCS_ERROR_CODE.ERR_CODE_NO_ERROR;

            int nInterface = ILan;
            string strPort = this.Setting.Ip;
            int nBaudrate = this.Setting.Port;
            int nDatabits = 8;
            int nParity = 0;
            int nStopbits = 0;

            nStatus = await Task.Run(() => BXLLApi.ConnectPrinterEx(nInterface, strPort, nBaudrate, nDatabits, nParity, nStopbits));
           
            if (Enum.TryParse(nStatus.ToString(), out SLCS_ERROR_CODE errorCode))
            {
                this.CurrentError = errorCode;

                if (this.CurrentError != SLCS_ERROR_CODE.ERR_CODE_NO_ERROR)
                {
                    await this.StopAsync();
                    return;
                }
            }
            this.TcpConnectionState = TcpConnectionStateEnum.Connected;
            return;
        }

        public async Task StopAsync() 
        {
            await Task.Run(() => BXLLApi.DisconnectPrinter());
            this.TcpConnectionState = TcpConnectionStateEnum.Disconnected;
        }

        public async Task<bool> ZplPrintSendAsync(string command)
        {
            this.CurrentError = this.CheckStatus();

            if (this.CurrentError != SLCS_ERROR_CODE.ERR_CODE_NO_ERROR
                && this.CurrentError != SLCS_ERROR_CODE.ERR_CODE_CONNECT) return false;

            return await Task.Run(() =>
            {
                byte[] bytes = Encoding.Default.GetBytes(command);
                int lResult = 0;
                bool result = false;

                result = BXLLApi.ClearBuffer();

                if (result)
                    result = BXLLApi.WriteBuff(bytes, bytes.Length, ref lResult);

                return result;
            });
        }

        public async Task<bool> ReprintSendAsync()
        {
            //Todo. 정상동작여부 확인 필요
            this.CurrentError = this.CheckStatus();

            if (this.CurrentError != SLCS_ERROR_CODE.ERR_CODE_NO_ERROR
                && this.CurrentError != SLCS_ERROR_CODE.ERR_CODE_CONNECT) return false;

            return await Task.Run(() =>
            {
                byte[] pBuffer = new byte[1024];
                int dwReaded = 0;
                int lResult = 0;
                bool result = false;

                result = BXLLApi.ReadBuff(pBuffer, pBuffer.Length, ref dwReaded);

                if (result)
                    result = BXLLApi.WriteBuff(pBuffer, pBuffer.Length, ref lResult);

                return result;
            });
        }

        private SLCS_ERROR_CODE CheckStatus() => (SLCS_ERROR_CODE)BXLLApi.CheckStatus();

        protected override async void OnDispose(bool disposing)
        {
            await this.StopAsync();
            this.connectionStateTimer.Elapsed -= this.ConnectionStateTimer_Elapsed;
            this.connectionStateTimer.Stop();
            this.connectionStateTimer.Dispose();
            base.OnDispose(disposing);
        }
        #endregion

        #region Event Handler
        private void StatusScanTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.IsDisposed) return;

            SLCS_ERROR_CODE FreshError = this.CheckStatus();
            if (this.CurrentError == FreshError) return;

            this.CurrentError = FreshError;
            this.Error?.Invoke(this, this.CurrentError);
        }

        private async void ConnectionStateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.IsDisposed) return;

            if (this.TcpConnectionState == TcpConnectionStateEnum.Disconnected)
                await this.StartAsync();
        }
        #endregion
    }

    [Serializable]
    public class BixolonCommunicatorSetting : PingTcpCommunicatorSetting
    {
        public BixolonCommunicatorSetting()
        {
            this.Name = "Bixolon SLP-D420";
            this.Ip = "192.168.1.1";
            this.Port = 9100;
            this.Active = true;
        }

        #region Old
        #region Nested
        //public class PaperSizeSetting
        //{
        //    public double Width { get; set; } = 10.4;
        //    public double Height { get; set; } = 8.3;
        //}

        //public class MarginSetting
        //{
        //    public int X_Margin { get; set; }
        //    public int Y_Margin { get; set; }
        //}
        //#endregion

        //#region Prop
        //public PaperSizeSetting PaperSize { get; set; } = new PaperSizeSetting();

        //public MarginSetting Margin { get; set; } = new MarginSetting();

        //public bool TopToBottom { get; set; }

        //public int Density { get; set; } = 14;
        #endregion
        #endregion
    }
}
