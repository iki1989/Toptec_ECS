using System;
using System.Text;
using Urcis.SmartCode.Net.Tcp;
using ECS.Model.Bcr;
using Urcis.SmartCode.Diagnostics;
using ECS.Core.Equipments;
using ECS.Model;

namespace ECS.Core.Communicators
{
    public class BcrCommunicator : TcpCommunicator
    {
        #region Field
        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const string NOREAD = "NOREAD";
        #endregion

        #region Event
        public event EventHandler<string> ReadData;
        public event EventHandler Noread;
        #endregion

        #region Prop
        public new BcrCommunicatorSetting Setting => base.Setting as BcrCommunicatorSetting;

        #endregion

        #region Ctor
        public BcrCommunicator(object owner) : base(owner) { }
        #endregion

        #region Method
        public void ApplySetting(BcrCommunicatorSetting setting)
        {
            setting = setting ?? new BcrCommunicatorSetting();

            base.ApplySetting(setting);
        }

        protected override bool OnReceive() 
        {
            byte[] bytes = new byte[64];
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

                        if (message == NOREAD)
                            this.Noread?.Invoke(this, null);
                        else
                            this.ReadData?.Invoke(this, message);
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
        #endregion
    }

    [Serializable]
    public class BcrCommunicatorSetting : PingTcpCommunicatorSetting
    {
        public BcrCommunicatorSetting()
        {
            //Communicator에서 로깅은 안하지만, SmartCode에서 Logger 자동 생성. 중복 이름 Logger 생성시 익셉션
            this.Name = "BCR Matrix300N";
            //this.Ip = "192.168.3.100";
            this.Port = 8005;
        }
    }
}
