using System;
using System.Threading.Tasks;
using Urcis.Secl;
using Urcis.SmartCode;
using Urcis.SmartCode.Net.Tcp;
using Urcis.SmartCode.Serialization;
using ECS.Model.Pcs;
using ECS.Core.Managers;
using Newtonsoft.Json;
using Urcis.SmartCode.Diagnostics;
using ECS.Model;

namespace ECS.Core.Equipments
{
    public abstract class TouchPcGeneralEquipment : PcEquipment
    {       
        #region Ctor
        public TouchPcGeneralEquipment(PcEquipmenttSetting setting) : base(setting) { }
        #endregion

        #region Method
        private void TimeSyncronizeSend()
        {
            this.SendMessage(new TimeSyncronize { Time = DateTime.Now });
        }

        protected override void OnParseFrame(PcMessageFrame touchMessageFrame) { }

        protected virtual void OnEquipmentStateChangeSend(bool isConnect) { }

        public virtual void OnBcrAlarmSetResetSend(BcrAlarmSetReset bcrAlarmSetReset)
        {
            if (bcrAlarmSetReset == null) return;

            try
            {
                bcrAlarmSetReset.Reason += $"{Environment.NewLine} X 버튼 클릭시 알람 해제";
                this.SendMessage(bcrAlarmSetReset);
                this.Logger?.Write($"BcrAlarmSet : {bcrAlarmSetReset}");
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion

        #region Event Handler
        protected override void OnCommunicator_HsmsConnectionStateChanged(object sender, HsmsConnectionStateChangedEventArgs e)
        {
            base.OnCommunicator_HsmsConnectionStateChanged(sender, e);

            if (e.Current == HsmsConnectionStateEnum.Selected)
            {
                this.TimeSyncronizeSend();
                this.OnEquipmentStateChangeSend(true);
            }
        }

        protected override void OnCommunicator_TcpConnectionStateChanged(object sender, TcpConnectionStateChangedEventArgs e)
        {
            base.OnCommunicator_TcpConnectionStateChanged(sender, e);

            if (e.Current == TcpConnectionStateEnum.Disconnected)
                this.OnEquipmentStateChangeSend(false);
        }
        #endregion
    }
}
