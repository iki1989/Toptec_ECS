using System;
using System.Text;
using Urcis.SmartCode.Net.Tcp;
using ECS.Model.Bcr;
using Urcis.SmartCode.Diagnostics;
using ECS.Core.Equipments;
using ECS.Model;

namespace ECS.Core.Communicators
{
    public class TopAndSideBcrCommunicator : TcpCommunicator
    {
        #region Field
        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const string NOREAD = "NOREAD";
        #endregion

        #region Event
        public event EventHandler<string[]> ReadDatas;
        public event EventHandler<string[]> Noread;
        #endregion

        #region Prop
        public new TopAndSideBcrCommunicatorSetting Setting => base.Setting as TopAndSideBcrCommunicatorSetting;

        #endregion

        #region Ctor
        public TopAndSideBcrCommunicator(object owner) : base(owner) { }
        #endregion

        #region Method
        public void ApplySetting(TopAndSideBcrCommunicatorSetting setting)
        {
            setting = setting ?? new TopAndSideBcrCommunicatorSetting();

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
                        string fullMessage = Encoding.ASCII.GetString(bytes, 1, result - 2);
                        this.Logger?.Write(fullMessage);

                        string[] messages = fullMessage.Split(',');
                        if (messages.Length == 2)
                        {
                            string sideBcrMessage = messages[0];
                            string topBcrMessage = messages[1];

                            if (sideBcrMessage == NOREAD || topBcrMessage == NOREAD)
                                this.Noread?.Invoke(this, messages);
                            else
                                this.ReadDatas?.Invoke(this, messages);
                        }
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
    public class TopAndSideBcrCommunicatorSetting : PingTcpCommunicatorSetting
    {
        public TopAndSideBcrCommunicatorSetting()
        {
            //Communicator에서 로깅은 안하지만, SmartCode에서 Logger 자동 생성. 중복 이름 Logger 생성시 익셉션
            this.Name = "DS-AV900 & BCR Matrix300N";
            //this.Ip = "192.168.3.100";
            this.Port = 8004;
        }
    }
}
