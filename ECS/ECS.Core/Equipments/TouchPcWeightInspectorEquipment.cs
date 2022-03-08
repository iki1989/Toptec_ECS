using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Urcis.Secl;
using Urcis.SmartCode;
using Urcis.SmartCode.Serialization;
using Newtonsoft.Json;
using ECS.Model.Pcs;
using ECS.Model;
using ECS.Model.Hub;
using ECS.Model.Plc;
using ECS.Core.Managers;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Net.Tcp;

namespace ECS.Core.Equipments
{
    public class TouchPcWeightInspectorEquipment : TouchPcGeneralEquipment
    {
        #region Prop
        public WeightCheckBcrState WeightCheckBcrState { get; set; } = new WeightCheckBcrState();
        private WeightCheck WeightCheck { get; set; } = new WeightCheck();
        #endregion

        #region Ctor
        public TouchPcWeightInspectorEquipment(PcEquipmenttSetting setting) : base(setting) 
        {
            this.Setting = setting ?? new PcEquipmenttSetting();
        }
        #endregion

        #region Method

        protected override void OnCreate()
        {
            base.OnCreate();

            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.TouchPcWeightInspector));
            
            this.Setting.CommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
            this.Communicator?.ApplySetting(this.Setting.CommunicatorSetting);

            this.LifeState = LifeCycleStateEnum.Created;
        }

        protected override void OnParseFrame(PcMessageFrame touchMessageFrame)
        {
            base.OnParseFrame(touchMessageFrame);

            try
            {
                #region OnParseFrame
                var name = touchMessageFrame.Type;
                if (typeof(BcrAlarmSetReset).AssemblyQualifiedName == name)
                {
                    var BcrAlarmResetRequest = this.JsonDeserialize<BcrAlarmSetReset>(touchMessageFrame.Data);
                    this.Logger?.Write($"Recieved {name} : {BcrAlarmResetRequest}");

                    var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<DynamicScaleEquipment>();
                    if (eq != null)
                        eq.BcrAlarmSet(false);
                }
               
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        public override void OnHub_Recived(EventArgs e)
        {
            try
            {
                if (e is WeightCheckIndexArgs weightCheckIndexArgs)
                {
                    this.WeightCheck.WeightCheckIndex = weightCheckIndexArgs.WeightCheckIndex;
                    this.WeightCheckIndexSend(this.WeightCheck);
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void EquipmentCoonectionStateSend(WeightCheckBcrState weightCheckBcrState)
        {
            try
            {
                this.SendMessage(weightCheckBcrState);
                this.Logger?.Write($"WeightCheckBcrConnection : {weightCheckBcrState.WeightCheckBcrConnection}");
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
           
        }

        public void EquipmentCoonectionStateSend()
        {
            this.EquipmentCoonectionStateSend(this.WeightCheckBcrState);
        }

        private void WeightCheckIndexSend(WeightCheck weightCheck)
        {
            try
            {
                this.Logger?.Write($"WeightCheckIndex : {weightCheck.WeightCheckIndex}");

                this.SendMessage(new WeightCheck { WeightCheckIndex = weightCheck.WeightCheckIndex });
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
            if (e.Current == e.Previous) return;
            base.OnCommunicator_HsmsConnectionStateChanged(sender, e);

            if (e.Current == HsmsConnectionStateEnum.Selected)
            {
                this.EquipmentCoonectionStateSend(this.WeightCheckBcrState);

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                if (eq != null)
                {
                    if (eq.IsAlarmSet(WeightInvoicBcrEnum.DynamicScale))
                    {
                        BcrAlarmSetReset bcrAlarmSetReset = new BcrAlarmSetReset();
                        bcrAlarmSetReset.Reason = "BCR 알람 상태";
                        bcrAlarmSetReset.AlarmResult = true;
                        this.OnBcrAlarmSetResetSend(bcrAlarmSetReset);
                    }
                }
            }
                
        }
        #endregion
    }
}
