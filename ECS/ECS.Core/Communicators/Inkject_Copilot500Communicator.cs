using ECS.Model;
using ECS.Model.Inkject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;
using Urcis.SmartCode.Net.Tcp;
using Urcis.SmartCode.Threading;

namespace ECS.Core.Communicators
{
    public class Inkject_Copilot500Communicator : TcpCommunicator
    {
        #region Event
        public event EventHandler<bool> FirstConnectedRecived;
        public event EventHandler EnablePrintComplete;
        public event EventHandler PrintCompleteResponse;
        public event EventHandler<AutoDataResponseEnum> AutoDataRecordResponse;
        public event EventHandler WriteAutoDataRecivedResponse;
        public event EventHandler WriteAutoDataQueueClearResponse;
        public event EventHandler<int> ReadInkLevelResponse;
        public event EventHandler<string> GetAutoDataStringResponse;
        #region Old
        //public event EventHandler<DateTime> WrtieSystemDateAndTimeResponse;
        //public event EventHandler<int> ProductionCounterResponse;
        //public event EventHandler<CounterInfo> GetCounterInfoResponse;
        //public event EventHandler<int> SetCounterInfoSuccessfulResponse;
        #endregion

        #endregion

        #region Field
        private const char LF = (char)0x0A; //End
        private readonly object syncRoot = new object();
        #endregion

        #region Prop
        public new InkjetCommunicatorSetting Setting => base.Setting as InkjetCommunicatorSetting;

        private bool m_IsFirstConnectedRecived;
        public bool IsFirstConnectedRecived
        {
            get => this.m_IsFirstConnectedRecived;
            private set 
            {
                if (this.m_IsFirstConnectedRecived == value) return;

                this.m_IsFirstConnectedRecived = value;

                if (this.m_IsFirstConnectedRecived)
                    this.FirstConnectedRecived?.Invoke(this, true);
            }
        }
        #endregion

        #region Ctor
        public Inkject_Copilot500Communicator(object owner) : base(owner) { }
        #endregion

        #region Method
        public void ApplySetting(InkjetCommunicatorSetting setting)
        {
            setting = setting ?? new InkjetCommunicatorSetting();
            base.ApplySetting(setting);
        }

        public bool SendMessage(string message)
        {
            if (this.TcpConnectionState == TcpConnectionStateEnum.Connected
                && this.IsFirstConnectedRecived == true)
            {
                lock(this.syncRoot) 
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(message + LF);

                    int result = 0;
                    if (this.Client != null && this.Client.Connected)
                        result = this.Client.Send(bytes);

                    if (result == bytes.Length)
                        return true;
                } 
            }

            return false;
        }

        private void ProcessAsEvent(string message)
        {
            if (message.Equals("Connected to Copilot printer"))
                this.IsFirstConnectedRecived = true;
            else if (message.Equals("ACK-Print Complete Enabled"))
                this.EnablePrintComplete?.Invoke(this, null);
            else if (message.Contains("ACK-Auto Data"))
            {
                if (message.Equals("ACK-Auto Data XON"))
                    this.AutoDataRecordResponse?.Invoke(this, AutoDataResponseEnum.XON);
                else if(message.Equals("ACK-Auto Data XOFF"))
                    this.AutoDataRecordResponse?.Invoke(this, AutoDataResponseEnum.XOFF);
                else if (message.Equals("ACK-Auto Data Received"))
                    this.WriteAutoDataRecivedResponse?.Invoke(this, null);
                else if (message.Equals("ACK-Auto Data Received - Auto Data queue cleared"))
                    this.WriteAutoDataQueueClearResponse?.Invoke(this, null);
            }
            else if (message.Equals("ACK-Print Complete"))
                this.PrintCompleteResponse?.Invoke(this, null);
            else if (message.Contains("ACK-Ink Level"))
            {
                string[] splited = message.Split('=');
                if (splited.Length == 2)
                {
                    string strValue = splited[1].Replace("%", "");
                    if (int.TryParse(strValue, out int result))
                        this.ReadInkLevelResponse?.Invoke(this, result);
                    else
                    {
                        //Error
                        this.ReadInkLevelResponse?.Invoke(this, -1);
                    }
                }
            }
           
            else if (message.Contains("ACK-AUTO_DATA_STRING"))
            {
                string[] splited = message.Split('=');
                if (splited.Length == 2)
                {
                    string strValue = splited[1].Replace("\n", "");
                    this.GetAutoDataStringResponse?.Invoke(this, strValue);
                }
            }
            #region Old
            //else if (message.Contains("ACK-DateTime"))
            //{
            //    string[] splited = message.Split('=');
            //    if (splited.Length == 2)
            //    {
            //        if (DateTime.TryParse(splited[1], out DateTime result))
            //            this.WrtieSystemDateAndTimeResponse?.Invoke(this, result);
            //    }
            //}
            //else if (message.Contains("ACK-Production_Counter"))
            //{
            //    string[] splited = message.Split('=');
            //    if (splited.Length == 2)
            //    {
            //        if (int.TryParse(splited[1], out int count))
            //            this.ProductionCounterResponse?.Invoke(this, count);
            //    }
            //}
            //else if (message.Contains("ACK-GET_COUNTER_INFO"))
            //{
            //    string[] splited = message.Split('=');
            //    if (splited.Length == 2)
            //    {
            //        string[] responseSplite = splited[1].Split(',');
            //        if (responseSplite.Length == 6)
            //        {
            //            string strart = responseSplite[0];
            //            string stop = responseSplite[1];
            //            string current = responseSplite[2];
            //            if (Enum.TryParse(responseSplite[3], out DirectionEnum direction) == false) return;
            //            if (Enum.TryParse(responseSplite[4], out TypeEnum type) == false) return;

            //            CounterInfo counterInfo = new CounterInfo()
            //            {
            //                Start = strart,
            //                Stop = stop,
            //                Current = current,
            //                Direction = direction,
            //                Type = type,
            //            };
            //            this.GetCounterInfoResponse?.Invoke(this, counterInfo);
            //        }
            //    }
            //}
            //else if (message.Contains("ACK-SET_COUNTER_INFO"))
            //{
            //    string s = "ACK-SET_COUNTER_INFO Successful, New vlaue is ";
            //    string strValue = message.Substring(s.Length, message.Length - s.Length);
            //    if (int.TryParse(strValue, out int value))
            //        this.SetCounterInfoSuccessfulResponse?.Invoke(this, value);
            //}
            #endregion
        }

        protected override bool OnReceive()
        {
            byte[] bytes = new byte[128];
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

                string message = Encoding.ASCII.GetString(bytes, 0, result);
                var index = message.IndexOf(LF);
                if (index > -1)
                {
                    string msg = message.Substring(0, message.Length - (message.Length - index));
                    this.ProcessAsEvent(msg);
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
        #endregion

        #region Event Handler
        protected override void OnTcpConnectionStateChanged(TcpConnectionStateChangedEventArgs e)
        {
            if (e.Current == TcpConnectionStateEnum.Disconnected)
                this.IsFirstConnectedRecived = false;

            base.OnTcpConnectionStateChanged(e);
        }
        #endregion
    }

    [Serializable]
    public class InkjetCommunicatorSetting : PingTcpCommunicatorSetting
    {
        public InkjetCommunicatorSetting()
        {
            this.Name = "Copilot500";
            this.Port = 4000;
            this.Active = true;
        }
    }
}
