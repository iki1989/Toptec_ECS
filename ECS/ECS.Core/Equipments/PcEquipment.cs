using System;
using System.Threading.Tasks;
using Urcis.Secl;
using Urcis.SmartCode;
using Urcis.SmartCode.Net.Tcp;
using Urcis.SmartCode.Serialization;
using ECS.Model.Pcs;
using Newtonsoft.Json;
using Urcis.SmartCode.Diagnostics;
using ECS.Model;
using ECS.Core.Util;
using Urcis.SmartCode.Threading;

namespace ECS.Core.Equipments
{
    public abstract class PcEquipment : Equipment
    {
        #region Prop
        public new HsmsCommunicator Communicator
        {
            get => base.Communicator as HsmsCommunicator;
            set => base.Communicator = value;
        }

        public new PcEquipmenttSetting Setting
        {
            get => base.Setting as PcEquipmenttSetting;
            set => base.Setting = value;
        }
        #endregion

        #region Ctor
        public PcEquipment(PcEquipmenttSetting setting)
        {
            this.Setting = setting ?? new PcEquipmenttSetting();
            this.Name = this.Setting.Name;
        }
        #endregion

        #region Method
        #region Interface
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;
            try
            {
                base.OnCreate();
                this.Communicator = new HsmsCommunicator(this);
            }
            catch (Exception ex) { this.Logger.Write(ex.Message); }
        }

        protected override bool OnPrepare()
        {
            this.LifeState = LifeCycleStateEnum.Preparing;
            try
            {
                if (this.Communicator != null && this.Communicator.IsDisposed == false)
                {
                    this.Communicator.DataMessageReceived += this.Communicator_MessageReceived;
                    this.Communicator.HsmsConnectionStateChanged += OnCommunicator_HsmsConnectionStateChanged;
                    this.Communicator.TcpConnectionStateChanged += OnCommunicator_TcpConnectionStateChanged;
                    
                }
                this.LifeState = LifeCycleStateEnum.Prepared;
            }
            catch (Exception ex) { this.Logger.Write(ex.Message); return false; }
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
                    this.Communicator.TcpConnectionStateChanged -= OnCommunicator_TcpConnectionStateChanged;
                    this.Communicator.HsmsConnectionStateChanged -= OnCommunicator_HsmsConnectionStateChanged;
                    this.Communicator.DataMessageReceived -= this.Communicator_MessageReceived;
                }
                this.LifeState = LifeCycleStateEnum.Terminated;
            }
            catch (Exception ex) { this.Logger.Write(ex.Message); }
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
                if (this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Disconnected
                    && this.Communicator.HsmsConnectionState != HsmsConnectionStateEnum.Selected)
                {
                    this.Communicator.Start();
                    this.Logger?.Write("Communicator Start Async");
                }
            }
        }

        protected override void OnStop()
        {
            if (this.Communicator != null || (this.Communicator.IsDisposed == false))
            {
                if (this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Connected
                    || this.Communicator.HsmsConnectionState == HsmsConnectionStateEnum.Selected)
                {
                    this.Communicator.Stop();
                    this.Logger?.Write("Communicator Stop");
                }
            }
        }
        #endregion

        protected HsmsTransaction SendMessage(object o)
        {
            try
            {
                PcMessageFrame frame = new PcMessageFrame();
                frame.Type = o.GetType().AssemblyQualifiedName;
                frame.Data = new ScJsonSerializer().Serialize(o);

                return this.SendBinary(frame);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
                return null;
            }
        }

        private HsmsTransaction SendBinary(object o)
        {
            try
            {
                HsmsBinaryMessage msg = new HsmsBinaryMessage();
                msg.Data = new ScBinarySerializer().Serialize(o);

                return this.Communicator.Send(msg);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
                return null;
            }
        }

        protected abstract void OnParseFrame(PcMessageFrame touchMessageFrame);

        protected T JsonDeserialize<T>(object data)
        {
            object deserialized = null;
            try
            {
                deserialized = JsonConvert.DeserializeObject<T>(data.ToString());
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
                return default;
            }
           
            return (T)Convert.ChangeType(deserialized, typeof(T));
        }
        protected T ParseData<T>(PcMessageFrame frame)
        {
            var data = this.JsonDeserialize<T>(frame.Data);
            if (data == null)
                this.Logger?.Write($"Recieved {frame.Type} : JsonDeserialize is null");
            else
                this.Logger?.Write($"Recieved {frame.Type} : {data}");
            return data;
        }

        #endregion

        #region Event Handler
        protected void Communicator_MessageReceived(object sender, HsmsMessageEventArgs e)
        {
            try
            {
                byte[] bytes = e.Message.BinaryMessage.Data;
                object frame = new ScBinarySerializer().Deserialize(bytes);

                ScTask.Run(() =>
                {
                    if (frame is PcMessageFrame touchMessageFrame)
                        this.OnParseFrame(touchMessageFrame);
                });
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        protected virtual void OnCommunicator_HsmsConnectionStateChanged(object sender, HsmsConnectionStateChangedEventArgs e) { }

        protected virtual void OnCommunicator_TcpConnectionStateChanged(object sender, TcpConnectionStateChangedEventArgs e) { }
       
        #endregion
    }

    [Serializable]
    public class PcEquipmenttSetting : EquipmentSetting
    {
        public new HsmsCommunicatorSetting CommunicatorSetting
        {
            get => base.CommunicatorSetting as HsmsCommunicatorSetting;
            set => base.CommunicatorSetting = value;
        }

        public PcEquipmenttSetting()
        {
            this.Name = "Hsms";
            this.CommunicatorSetting = new HsmsCommunicatorSetting();
            this.CommunicatorSetting.Name = $"{this.Name} Communicator";
            this.CommunicatorSetting.Ip = "127.0.0.1";
            this.CommunicatorSetting.Port = 7000;
        }
    }
}
