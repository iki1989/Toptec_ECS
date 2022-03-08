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

namespace ECS.Core.Equipments
{
    public class TouchPcInvoiceRejectEquipment : TouchPcGeneralEquipment
    {
        #region Ctor
        public TouchPcInvoiceRejectEquipment(PcEquipmenttSetting setting) : base(setting) 
        {
            this.Setting = setting ?? new PcEquipmenttSetting();
        }
        #endregion

        #region Method

        protected override void OnCreate()
        {
            base.OnCreate();

            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.TouchPcInvoiceReject));
            
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

                if (typeof(InvoiceReprintRequest).AssemblyQualifiedName == name)
                {
                    var InvoiceReprintRequest = this.JsonDeserialize<InvoiceReprintRequest>(touchMessageFrame.Data);
                    this.Logger?.Write($"Recieved {name} : {InvoiceReprintRequest}");

                    TopRequestBoxIdArgs arg = new TopRequestBoxIdArgs();
                    arg.BoxId = InvoiceReprintRequest.BoxId;

                    EcsServerAppManager.Instance.Hub.Send(HubServiceName.TopBcrEquipment, arg);
                }
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
                if (e is TopRequestBoxIdArgs topRequestBoxIdArgs)
                {
                    InvoiceReprintResponse invoiceReprintResponse = new InvoiceReprintResponse();
                    invoiceReprintResponse.Result = topRequestBoxIdArgs.Result;

                    this.SendMessage(invoiceReprintResponse);

                    this.Logger?.Write($"InvoicePrintingResult : {invoiceReprintResponse.Result}");
                }
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
        }
        #endregion
    }
}
