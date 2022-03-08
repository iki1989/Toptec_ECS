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
using System.Text;
using Urcis.SmartCode.Net.Tcp;

namespace ECS.Core.Equipments
{
    public class TouchPcBcrLcdEquipment : TouchPcGeneralEquipment
    {
        #region Prop
        public InvoiceBcrState InvoiceBcrState { get; set; } = new InvoiceBcrState();
        #endregion

        #region Ctor
        public TouchPcBcrLcdEquipment(PcEquipmenttSetting setting) : base(setting) 
        {
            this.InvoiceBcrState.PrintInfoList = new PrintInfo[2];
            for (int i = 0; i < this.InvoiceBcrState.PrintInfoList.Length; i++)
            {
                this.InvoiceBcrState.PrintInfoList[i] = new PrintInfo();
            }
        }
        #endregion

        #region Method
        protected override void OnCreate()
        {
            base.OnCreate();

            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.TouchPcBcrLcd));

            this.Setting.CommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
            this.Communicator?.ApplySetting(this.Setting.CommunicatorSetting);

            this.LifeState = LifeCycleStateEnum.Created;
        }


        protected override void OnParseFrame(PcMessageFrame touchMessageFrame)
        {
            base.OnParseFrame(touchMessageFrame);

            try
            {
                var name = touchMessageFrame.Type;
                if (typeof(BcrAlarmSetReset).AssemblyQualifiedName == name)
                {
                    var BcrAlarmResetRequest = this.JsonDeserialize<BcrAlarmSetReset>(touchMessageFrame.Data);
                    this.Logger?.Write($"Recieved {name} : {BcrAlarmResetRequest}");

                    {
                        var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TopBcrEquipment>();
                        if (eq != null)
                            eq.BcrAlarmSet(false, WeightInvoicBcrEnum.Top);
                    }

                    {
                        var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<OutLogicalEquipment>();
                        if (eq != null)
                            eq.BcrAlarmSet(false);
                    }

                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void EquipmentCoonectionStateSend(InvoiceBcrState invoiceBcrState)
        {
            try
            {
                this.SendMessage(invoiceBcrState);

                StringBuilder sb = new StringBuilder();
                sb.Append($"RouteBcrConnection : ").Append($"{invoiceBcrState.RouteBcrConnection}, ");

                //Normal
                sb.Append($"Normal PrintBcrConnection : ").Append($"{invoiceBcrState.PrintInfoList[0].PrintBcrConnection}, ");
                sb.Append($"Normal LabellerConnection : ").Append($"{invoiceBcrState.PrintInfoList[0].LabellerConnection}, ");
                sb.Append($"Normal LabelerState : ").Append($"{invoiceBcrState.PrintInfoList[0].LabelerState}, ");

                //Smart
                sb.Append($"Smart PrintBcrConnection : ").Append($"{invoiceBcrState.PrintInfoList[1].PrintBcrConnection}, ");
                sb.Append($"Smart LabellerConnection : ").Append($"{invoiceBcrState.PrintInfoList[1].LabellerConnection}, ");
                sb.Append($"Smart LabelerState : ").Append($"{invoiceBcrState.PrintInfoList[1].LabelerState}, ");


                sb.Append($"TopBcrConnection : ").Append($"{invoiceBcrState.TopBcrConnection}, ");
                sb.Append($"OutBcrConnection : ").Append($"{invoiceBcrState.OutBcrConnection}, ");

                sb.Append($"PlcConnection : ").Append($"{invoiceBcrState.PlcConnection}, ");
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        public void EquipmentCoonectionStateSend()
        {
            this.EquipmentCoonectionStateSend(this.InvoiceBcrState);
        }

        #endregion

        #region Event Handler
        protected override void OnCommunicator_HsmsConnectionStateChanged(object sender, HsmsConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;
            base.OnCommunicator_HsmsConnectionStateChanged(sender, e);

            if (e.Current == HsmsConnectionStateEnum.Selected)
            {
                this.EquipmentCoonectionStateSend(this.InvoiceBcrState);

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                if (eq != null)
                {
                    if (eq.IsAlarmSet(WeightInvoicBcrEnum.Top))
                    {
                        BcrAlarmSetReset bcrAlarmSetReset = new BcrAlarmSetReset();
                        bcrAlarmSetReset.Reason = "Top BCR 알람 상태";
                        bcrAlarmSetReset.AlarmResult = true;
                        this.OnBcrAlarmSetResetSend(bcrAlarmSetReset);
                    }
                }
            }
        }

        public override void OnHub_Recived(EventArgs e)
        {
            try
            {
                if (e is BcrLcdIndexArgs bcrLcdIndexArgs)
                {
                    var invoiceBcrRead = new InvoiceBcrRead() { InvoiceBcrIndex = bcrLcdIndexArgs.BcrIndex };
                    this.SendMessage(invoiceBcrRead);
                    this.Logger?.Write($"InvoiceBcrIndex : {invoiceBcrRead}");
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion
    }
}
