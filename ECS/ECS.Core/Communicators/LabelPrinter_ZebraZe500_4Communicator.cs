using System;
using System.Text;
using Urcis.SmartCode.Net.Tcp;
using ECS.Model.LabelPrinter;
using ECS.Model;

namespace ECS.Core.Communicators
{
    public class LabelPrinter_ZebraZe500_4Communicator : TcpCommunicator
    {
        #region Event
        public event EventHandler<bool> AutoStatusResponse;
        public event EventHandler LabelRow;
        public event EventHandler LabelNormal;
        public event EventHandler<LabelErrorEnum> LabelError;
        public event EventHandler PrintOkResponse;
        public event EventHandler<string> LabelAttachCompleted;
        public event EventHandler PrintSkip;
        #endregion

        #region Field
        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        #endregion

        #region Prop
        public new LabelPrinter_ZebraZe500CommunicatorSetting Setting => base.Setting as LabelPrinter_ZebraZe500CommunicatorSetting;
        #endregion

        #region Ctor
        public LabelPrinter_ZebraZe500_4Communicator(object owner) : base(owner) { }
        #endregion

        #region Method
        public void ApplySetting(LabelPrinter_ZebraZe500CommunicatorSetting setting)
        {
            setting = setting ?? new LabelPrinter_ZebraZe500CommunicatorSetting();

            base.ApplySetting(setting);
        }
        protected override bool OnReceive()
        {
            byte[] bytes = new byte[82];
            int result = 0;
            try
            {
                if (this.Client != null)
                    result = this.Client.Receive(bytes);

                if (result == 0)
                {
                    this.StartRestart(RestartReasonEnum.CommunicationFailure);
                    return false;
                }

                if (result >= 3)
                {
                    if (bytes[0] == STX && bytes[result - 1] == ETX)
                    {
                        string message = Encoding.ASCII.GetString(bytes, 1, result - 2);
                        this.Logger?.Write(message);
                        this.ProcessAsEvent(message);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                if (ex is ObjectDisposedException || ex is NullReferenceException) { }
                else
                    this.StartRestart(RestartReasonEnum.CommunicationFailure);

                return false;
            }
        }

        private void ProcessAsEvent(string message)
        {
            if (message.Equals("AUTO"))
                this.AutoStatusResponse?.Invoke(this, true);
            else if (message.Equals("MANUAL"))
                this.AutoStatusResponse?.Invoke(this, false);
            else if (message.Equals("LOW"))
                this.LabelRow?.Invoke(this, null);
            else if (message.Equals("NORMAL"))
                this.LabelNormal?.Invoke(this, null);
            else if (message.Contains("ERROR"))
            {
                string[] splited = message.Split('`');
                if (splited.Length >= 2)
                {
                    LabelErrorEnum error = LabelErrorEnum.Unknown;
                    if (int.TryParse(splited[1], out int intError))
                    {
                        switch (intError)
                        {
                            case 1: error = LabelErrorEnum.AdsorptionError; break;
                            case 2: error = LabelErrorEnum.Seobo; break;
                            case 3: error = LabelErrorEnum.Paper; break;
                            case 4: error = LabelErrorEnum.Ribbon; break;
                            case 5: error = LabelErrorEnum.SeoboOrigin; break;
                            case 6: error = LabelErrorEnum.PrinterError; break;
                        }
                    }

                    this.LabelError?.Invoke(this, error);
                }
            }
            else if (message.Contains("RSV OK"))
                this.PrintOkResponse?.Invoke(this, null);
            else if (message.Contains("COMPLETE"))
            {
                string boxId = message.Substring(0, 10);
                this.LabelAttachCompleted?.Invoke(this, boxId);
            }
               
            else if (message.Contains("PRINT SKIP"))
                this.PrintSkip?.Invoke(this, null);
        }

        public bool SendMessage(string message)
        {
            if (this.TcpConnectionState == TcpConnectionStateEnum.Connected)
            {
                message = $"{(char)STX}{message}{(char)ETX}";

                byte[] bytes = Encoding.GetEncoding(949).GetBytes(message);

                int result = 0;
                if (this.Client != null && this.Client.Connected)
                    result = this.Client.Send(bytes);

                if (result == bytes.Length)
                    return true;
            }

            return false;
        }
        #endregion
    }

    [Serializable]
    public class LabelPrinter_ZebraZe500CommunicatorSetting : PingTcpCommunicatorSetting
    {
        public LabelPrinter_ZebraZe500CommunicatorSetting()
        {
            this.Name = "Zebra ZE500-4 Communicator"; //(Zebra ZE500-4)
            this.Port = 9090;

            this.Active = true;
        }
    }
}
