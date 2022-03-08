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
using System.Text;

namespace ECS.Core.Equipments
{
    public class TouchPcSmartPackingEquipment : TouchPcGeneralEquipment
    {
        #region Field
        public SmartPackingConnectionState SmartPackingConnectionState { get; set; } = new SmartPackingConnectionState();
        #endregion

        #region Ctor
        public TouchPcSmartPackingEquipment(PcEquipmenttSetting setting) : base(setting)
        {
            this.Setting = setting ?? new PcEquipmenttSetting();
        }
        #endregion

        #region Method

        protected override void OnCreate()
        {
            base.OnCreate();

            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.TouchSmartPacking));

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

                if (typeof(SmartPackingManualBoxValidationRequest).AssemblyQualifiedName == name)
                {
                    var SmartPackingManualBoxValidationRequest = this.JsonDeserialize<SmartPackingManualBoxValidationRequest>(touchMessageFrame.Data);
                    this.Logger?.Write($"Recieved {name} : {SmartPackingManualBoxValidationRequest}");

                    var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcSmartPackingEquipment>();

                    if (eq != null)
                    {
                        eq.TouchManualBoxValidationRequest(SmartPackingManualBoxValidationRequest);
                    }
                }
                else if (typeof(BcrAlarmSetReset).AssemblyQualifiedName == name)
                {
                    var BcrAlarmResetRequest = this.JsonDeserialize<BcrAlarmSetReset>(touchMessageFrame.Data);
                    this.Logger?.Write($"Recieved {name} : {BcrAlarmResetRequest}");

                    {
                        var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<RouteLogicalEquipment>();
                        if (eq != null)
                            eq.BcrAlarmSet(false, WeightInvoicBcrEnum.Route);
                    }

                    {
                        var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcSmartPackingEquipment>();
                        if (eq != null)
                            eq.BcrAlarmSet(false);
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void EquipmentCoonectionStateSend(SmartPackingConnectionState smartPackingConnectionState)
        {
            try
            {
                #region EquipmentConnectionStateSend
                this.SendMessage(smartPackingConnectionState);

                StringBuilder sb = new StringBuilder();
                sb.Append("SmartPackingConnection :").Append($"{smartPackingConnectionState.SmartPackingConnection}, ");
                sb.Append("SmartPackingBcrConnection :").Append($"{smartPackingConnectionState.SmartPackingBcrConnection}, ");
                
                this.Logger?.Write(sb.ToString());
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

        }

        public void EquipmentCoonectionStateSend()
        {
            this.EquipmentCoonectionStateSend(this.SmartPackingConnectionState);
        }

        public void SmartPackingDbIndexSend(long index)
        {
            try
            {
                #region SmartPackingDbIndexSend
                SmartPackingBcrRead smartPackingBcrRead = new SmartPackingBcrRead();
                smartPackingBcrRead.SmartPackingIndex = index;

                this.SendMessage(smartPackingBcrRead);

                StringBuilder sb = new StringBuilder();
                sb.Append("SmartPackingBcrReadIndex :").Append($"{smartPackingBcrRead.SmartPackingIndex}, ");

                this.Logger?.Write(sb.ToString());
                #endregion
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
                this.EquipmentCoonectionStateSend(this.SmartPackingConnectionState);

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcSmartPackingEquipment>();
                if (eq != null)
                {
                    if (eq.IsAlarmSet())
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
