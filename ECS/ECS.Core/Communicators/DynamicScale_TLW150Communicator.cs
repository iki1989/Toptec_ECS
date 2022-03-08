using System;
using System.Text;
using Urcis.SmartCode.Net.Tcp;
using ECS.Model.DynamicScales;
using ECS.Model;

namespace ECS.Core.Communicators
{
    public class DynamicScale_TLW150Communicator : TcpCommunicator
    {
        #region Event
        public event EventHandler<TLW150DataFormat> DynamicScaleResult;
        #endregion

        #region Prop
        public new DynamicScaleCommunicatorSetting Setting => base.Setting as DynamicScaleCommunicatorSetting;
        #endregion

        #region Ctor
        public DynamicScale_TLW150Communicator(object owner) : base(owner) { }
        #endregion

        #region Method
        public void ApplySetting(DynamicScaleCommunicatorSetting setting)
        {
            setting = setting ?? new DynamicScaleCommunicatorSetting();

            base.ApplySetting(setting);
        }

        protected override bool OnReceive()
        {
            try
            {
                byte[] bytes = new byte[16];
                if (this.ReceiveBytes(bytes))
                {
                    if (bytes[10] == (byte)TLW150DataFormat.Space
                    && bytes[14] == TLW150DataFormat.CRLF[0]
                    && bytes[15] == TLW150DataFormat.CRLF[1])
                    {
                        TLW150DataFormat dataFormat = new TLW150DataFormat();
                        dataFormat.IW0104 = Encoding.ASCII.GetString(bytes, 0, 10);
                        dataFormat.WT0103 = Encoding.ASCII.GetString(bytes, 11, 3);

                        this.DynamicScaleResult?.Invoke(this, dataFormat);
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
    public class DynamicScaleCommunicatorSetting : PingTcpCommunicatorSetting
    {
        public DynamicScaleCommunicatorSetting()
        {
            this.Name = "DynamicScale TLW150";
            //this.Ip = "192.168.127.2";
            this.Port = 1749;
        }
    }
}
