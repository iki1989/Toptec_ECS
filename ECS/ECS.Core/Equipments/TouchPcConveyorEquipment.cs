using System;
using Urcis.Secl;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using ECS.Model.Pcs;
using ECS.Model;
using ECS.Model.Hub;
using ECS.Model.Plc;
using ECS.Core.Managers;

namespace ECS.Core.Equipments
{
    public class TouchPcConveyorEquipment : TouchPcGeneralEquipment
    {
        #region Ctor
        public TouchPcConveyorEquipment(PcEquipmenttSetting setting) : base(setting) { }
        #endregion

        #region Method

        protected override void OnCreate()
        {
            base.OnCreate();

            if (this.Name == HubServiceName.TouchPcConveyorEquipment1)
                this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.TouchPcConveyor1));
            else if (this.Name == HubServiceName.TouchPcConveyorEquipment2)
                this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.TouchPcConveyor2));
           
            this.Setting.CommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
            this.Communicator?.ApplySetting(this.Setting.CommunicatorSetting);

            this.LifeState = LifeCycleStateEnum.Created;
        }

        protected override void OnParseFrame(PcMessageFrame touchMessageFrame)
        {
            base.OnParseFrame(touchMessageFrame);

            var name = touchMessageFrame.Type;
            if (typeof(CvSpeedRequest).AssemblyQualifiedName == name)
            {
                var CaseErectCvSpeedRequest = this.JsonDeserialize<CvSpeedRequest>(touchMessageFrame.Data);
                this.Logger?.Write($"Recieved {name} : {CaseErectCvSpeedRequest}");

                this.CvSpeedResponse((ConveyorSpeedEnum)CaseErectCvSpeedRequest.ConveyorSpeed);
            }
            else if (typeof(ConveyorCvSpeed).AssemblyQualifiedName == name)
            {
                #region CV속도 설정
                var conveyorCvSpeed = this.JsonDeserialize<ConveyorCvSpeed>(touchMessageFrame.Data);
                this.Logger?.Write($"Recieved {name} : {conveyorCvSpeed}");

                this.CvSpeedExcute(conveyorCvSpeed);
                #endregion
            }
            else if (typeof(RouteModeRequest).AssemblyQualifiedName == name)
            {
                #region 분기모드요청
                var routeModeRequest = this.JsonDeserialize<RouteModeRequest>(touchMessageFrame.Data);
                this.Logger?.Write($"Recieved {name} : {routeModeRequest}");

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                if (eq != null)
                {
                    RouteMode routeMode = new RouteMode();
                    routeMode.Mode = eq.WeightInvoiceRatio.Mode == (short)RouteModeResult.Auto ? $"{RouteModeResult.Auto}" : $"{RouteModeResult.Ratio}";
                    routeMode.NormalRatio = eq.WeightInvoiceRatio.NormalWayRatio;
                    routeMode.SmartRatio = eq.WeightInvoiceRatio.SmartWayRatio;

                    this.SendMessage(routeMode);
                }
                #endregion
            }
            else if (typeof(RouteMode).AssemblyQualifiedName == name)
            {
                #region 분기 모드 설정
                var routeMode = this.JsonDeserialize<RouteMode>(touchMessageFrame.Data);
                this.Logger?.Write($"Recieved {name} : {routeMode}");

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                if (eq != null)
                    eq.RouteModeExcute(routeMode);
                #endregion
            }
        }
        private void CvSpeedResponse(ConveyorSpeedEnum conveyorSpeedEnum)
        {
            try
            {
                ConveyorCvSpeed conveyorCvSpeed = null;
                switch (conveyorSpeedEnum)
                {
                    case ConveyorSpeedEnum.CaseErector_Speed1:
                    case ConveyorSpeedEnum.CaseErector_Speed2:
                    case ConveyorSpeedEnum.CaseErector_Speed3:
                    case ConveyorSpeedEnum.CaseErector_Speed4:
                    case ConveyorSpeedEnum.CaseErector_Speed5:
                        {
                            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcCaseErectEquipment>();
                            if (eq != null)
                                conveyorCvSpeed = eq.GetConveyorCvSpeed((int)conveyorSpeedEnum);
                        }
                        break;
                    case ConveyorSpeedEnum.WeightInvoice_Speed1:
                    case ConveyorSpeedEnum.WeightInvoice_Speed2:
                    case ConveyorSpeedEnum.WeightInvoice_Speed3:
                    case ConveyorSpeedEnum.WeightInvoice_Speed4:
                    case ConveyorSpeedEnum.WeightInvoice_Speed5:
                    case ConveyorSpeedEnum.WeightInvoice_Speed6:
                    case ConveyorSpeedEnum.WeightInvoice_Speed7:
                    case ConveyorSpeedEnum.WeightInvoice_Speed8:
                    case ConveyorSpeedEnum.WeightInvoice_Speed9:
                    case ConveyorSpeedEnum.WeightInvoice_Speed10:
                    case ConveyorSpeedEnum.WeightInvoice_Speed11:
                    case ConveyorSpeedEnum.WeightInvoice_Speed12:
                    case ConveyorSpeedEnum.WeightInvoice_Speed13:
                    case ConveyorSpeedEnum.WeightInvoice_Speed14:
                    case ConveyorSpeedEnum.WeightInvoice_Speed15:
                    case ConveyorSpeedEnum.WeightInvoice_Speed16:
                    case ConveyorSpeedEnum.WeightInvoice_Speed17:
                    case ConveyorSpeedEnum.WeightInvoice_Speed18:
                    case ConveyorSpeedEnum.WeightInvoice_Speed19:
                    case ConveyorSpeedEnum.WeightInvoice_Speed20:
                        {
                            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                            if (eq != null)
                                conveyorCvSpeed = eq.GetConveyorCvSpeed((int)conveyorSpeedEnum);
                        }
                        break;
                }

                if (conveyorCvSpeed != null)
                    this.SendMessage(conveyorCvSpeed);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void CvSpeedExcute(ConveyorCvSpeed conveyorCvSpeed)
        {
            switch ((ConveyorSpeedEnum)conveyorCvSpeed.ConveyorSpeed)
            {
                case ConveyorSpeedEnum.CaseErector_Speed1:
                case ConveyorSpeedEnum.CaseErector_Speed2:
                case ConveyorSpeedEnum.CaseErector_Speed3:
                case ConveyorSpeedEnum.CaseErector_Speed4:
                case ConveyorSpeedEnum.CaseErector_Speed5:
                    {
                        var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcCaseErectEquipment>();
                        if (eq != null)
                            eq.ConveyorCvSpeedExcute(conveyorCvSpeed);
                    }
                    break;
                case ConveyorSpeedEnum.WeightInvoice_Speed1:
                case ConveyorSpeedEnum.WeightInvoice_Speed2:
                case ConveyorSpeedEnum.WeightInvoice_Speed3:
                case ConveyorSpeedEnum.WeightInvoice_Speed4:
                case ConveyorSpeedEnum.WeightInvoice_Speed5:
                case ConveyorSpeedEnum.WeightInvoice_Speed6:
                case ConveyorSpeedEnum.WeightInvoice_Speed7:
                case ConveyorSpeedEnum.WeightInvoice_Speed8:
                case ConveyorSpeedEnum.WeightInvoice_Speed9:
                case ConveyorSpeedEnum.WeightInvoice_Speed10:
                case ConveyorSpeedEnum.WeightInvoice_Speed11:
                case ConveyorSpeedEnum.WeightInvoice_Speed12:
                case ConveyorSpeedEnum.WeightInvoice_Speed13:
                case ConveyorSpeedEnum.WeightInvoice_Speed14:
                case ConveyorSpeedEnum.WeightInvoice_Speed15:
                case ConveyorSpeedEnum.WeightInvoice_Speed16:
                case ConveyorSpeedEnum.WeightInvoice_Speed17:
                case ConveyorSpeedEnum.WeightInvoice_Speed18:
                case ConveyorSpeedEnum.WeightInvoice_Speed19:
                case ConveyorSpeedEnum.WeightInvoice_Speed20:
                    {
                        var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                        if (eq != null)
                            eq.ConveyorCvSpeedExcute(conveyorCvSpeed);
                    }
                    break;
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
