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
using ECS.Core.Util;

namespace ECS.Core.Equipments
{
    public class ServerPcEquipment : PcEquipment
    {
        //공통 이벤트
        public event Action<TimeSyncronize> TimeSynconizeReceived;
        public event Action<BcrAlarmSetReset> BcrAlarmSetResetReceived;
        //제함 이벤트
        public event Action<InkjectInk> InkjetInkReceived;
        public event Action<ErectorConnectionState> ErectorStateReceived;
        public event Action<CaseErectBcrRead> CaseErectBcrReceived;
        public event Action<ManualBoxValidationResponse> ManualBoxValidationResponseReceived;
        //중량 검수 이벤트
        public event Action<WeightCheck> WeightCheckReceived;
        public event Action<WeightCheckBcrState> WeightCheckBcrStateReceived;
        //송장 발행/검증 이벤트
        public event Action<InvoiceBcrRead> InvoiceBcrReadReceived;
        public event Action<InvoiceBcrState> InvoiceBcrStateReceived;
        //송장 재발행 이벤트
        public event Action<InvoiceReprintResponse> InvoiceReprintResponseReceived;
        //컨베이어 이벤트
        public event Action<ConveyorCvSpeed> ConveyorCvSpeedReceived;
        public event Action<RouteMode> RouteModeReceived;
        //스마트 충진 이벤트
        public event Action<SmartPackingConnectionState> SmartPackingConnectionStateReceived;
        public event Action<SmartPackingBcrRead> SmartPackingBcrReadReceived;
        #region Ctor
        public ServerPcEquipment(PcEquipmenttSetting setting) : base(setting)
        {
        }
        #endregion

        #region Method
        protected override void OnCreate()
        {
            base.OnCreate();

            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.EcsServerPc));

            this.Setting.CommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
            this.Communicator?.ApplySetting(this.Setting.CommunicatorSetting);

            this.LifeState = LifeCycleStateEnum.Created;
        }

        #region Send

        #region 제함
        public void InkjectResumeSend(int line)
        {
            var inkjetResume = new InkjectResume { Line = line };
            this.SendMessage(inkjetResume);
            this.Logger?.Write($"Send InkjectResume : {inkjetResume}");
        }
        public void ManualBoxValidationRequestSend(string boxId, string boxType)
        {
            var manualBoxValidationRequest = new ManualBoxValidationRequest { BoxId = boxId, BoxType = boxType };
            this.SendMessage(manualBoxValidationRequest);
            this.Logger?.Write($"Send ManualBoxValidationRequest : {manualBoxValidationRequest}");
        }
        public void BoxNumberUpdate(int line)
        {
            var boxNumber = new BoxNumber() { Line = line };
            this.SendMessage(boxNumber);
            this.Logger?.Write($"Send BoxNumberUpdate : {boxNumber}");
        }
        #endregion

        #region 송장 재발행
        public void InvoiceReprintRequest(string boxId)
        {
            var invoiceReprintRequest = new InvoiceReprintRequest { BoxId = boxId };
            this.SendMessage(invoiceReprintRequest);
            this.Logger?.Write($"Send InvoiceReprintRequest : {invoiceReprintRequest}");
        }
        #endregion

        #region 컨베이어
        public void CvSpeedRequest(ConveyorSpeedEnum conveyorSpeed)
        {
            var cvSpeedRequest = new CvSpeedRequest() { ConveyorSpeed = (int)conveyorSpeed };
            this.SendMessage(cvSpeedRequest);
            this.Logger?.Write($"Send CvSpeedRequest : {cvSpeedRequest }");
        }

        public void ConveyorCvSpeed(ConveyorSpeedEnum conveyorSpeed, double sv)
        {
            var conveyorCvSpeed = new ConveyorCvSpeed { ConveyorSpeed = (int)conveyorSpeed, Sv = sv };
            this.SendMessage(conveyorCvSpeed);
            this.Logger?.Write($"Send ConveyorCvSpeed : {conveyorCvSpeed }");
        }
        public void RouteModeRequest()
        {
            var routeModeRequest = new RouteModeRequest { };
            this.SendMessage(routeModeRequest);
            this.Logger?.Write($"Send RouteModeRequest : {routeModeRequest  }");
        }
        public void RouteMode(string mode, int smart, int normal)
        {
            var routeMode = new RouteMode { Mode = mode, SmartRatio = smart, NormalRatio = normal };
            this.SendMessage(routeMode);
            this.Logger?.Write($"Send RouteMode : {routeMode}");
        }

        #endregion

        #region 중량기
        public void BcrAlarmResetRequest(string reason, bool result)
        {
            var bcrAlarm = new BcrAlarmSetReset { Reason = reason, AlarmResult = result };
            this.SendMessage(bcrAlarm);
            this.Logger?.Write($"Send BcrAlarmResetRequest : {bcrAlarm }");
        }
        #endregion

        #region 스마트패킹
        public void SmartPackingManualBoxValidationRequest(SmartPackingManualBoxValidationRequest smartPackingManualBoxValidationRequest)
        {
            this.SendMessage(smartPackingManualBoxValidationRequest);
            this.Logger?.Write($"Send SmartPackingManualBoxValidationRequest : {smartPackingManualBoxValidationRequest }");
        }

        #endregion

        #endregion

        protected override void OnParseFrame(PcMessageFrame touchMessageFrame)
        {
            var name = touchMessageFrame.Type;
            #region 제함
            if (typeof(TimeSyncronize).AssemblyQualifiedName == name)
            {
                var timeSyncronize = this.ParseData<TimeSyncronize>(touchMessageFrame);
                if (timeSyncronize != null)
                {
                    // 두번Log 찍혀서 삭제
                    //this.Logger?.Write($"Recieved {name} : {timeSyncronize}");
                    TimeSynconizeReceived?.Invoke(timeSyncronize);
                }
            }
            else if (typeof(ErectorConnectionState).AssemblyQualifiedName == name)
            {
                var erectorState = this.ParseData<ErectorConnectionState>(touchMessageFrame);
                if (erectorState != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {erectorState}");
                    ErectorStateReceived?.Invoke(erectorState);
                }

            }
            else if (typeof(CaseErectBcrRead).AssemblyQualifiedName == name)
            {
                var caseErectBcrRead = this.ParseData<CaseErectBcrRead>(touchMessageFrame);
                if (caseErectBcrRead != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {caseErectBcrRead}");
                    CaseErectBcrReceived?.Invoke(caseErectBcrRead);
                }

            }
            else if (typeof(InkjectInk).AssemblyQualifiedName == name)
            {
                var inkjectInk = this.ParseData<InkjectInk>(touchMessageFrame);
                if (inkjectInk != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {inkjectInk}");
                    InkjetInkReceived?.Invoke(inkjectInk);
                }
            }
            else if (typeof(ManualBoxValidationResponse).AssemblyQualifiedName == name)
            {
                var manualBoxValidationResponse = this.ParseData<ManualBoxValidationResponse>(touchMessageFrame);
                if (manualBoxValidationResponse != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {manualBoxValidationResponse}");
                    ManualBoxValidationResponseReceived?.Invoke(manualBoxValidationResponse);
                }
            }
            #endregion
            #region 중량검수
            else if (typeof(WeightCheck).AssemblyQualifiedName == name)
            {
                var weightCheck = this.ParseData<WeightCheck>(touchMessageFrame);
                if (weightCheck != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {weightCheck}");
                    WeightCheckReceived?.Invoke(weightCheck);
                }
            }
            else if (typeof(WeightCheckBcrState).AssemblyQualifiedName == name)
            {
                var weightCheckBcrState = this.ParseData<WeightCheckBcrState>(touchMessageFrame);
                if (weightCheckBcrState != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {weightCheckBcrState}");
                    WeightCheckBcrStateReceived?.Invoke(weightCheckBcrState);
                }
            }
            else if (typeof(BcrAlarmSetReset).AssemblyQualifiedName == name)
            {
                var bcrAlarmSetReset = this.ParseData<BcrAlarmSetReset>(touchMessageFrame);
                BcrAlarmSetResetReceived?.Invoke(bcrAlarmSetReset);
            }
            #endregion
            #region 송장 발행/검증
            else if (typeof(InvoiceBcrRead).AssemblyQualifiedName == name)
            {
                var invoiceBcrRead = this.ParseData<InvoiceBcrRead>(touchMessageFrame);
                if (invoiceBcrRead != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {invoiceBcrRead}");
                    InvoiceBcrReadReceived?.Invoke(invoiceBcrRead);
                }
            }
            else if (typeof(InvoiceBcrState).AssemblyQualifiedName == name)
            {
                var invoiceBcrState = this.ParseData<InvoiceBcrState>(touchMessageFrame);
                if (invoiceBcrState != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {invoiceBcrState}");
                    InvoiceBcrStateReceived?.Invoke(invoiceBcrState);
                }
            }
            #endregion
            #region 송장 재발행
            else if (typeof(InvoiceReprintResponse).AssemblyQualifiedName == name)
            {
                var invoiceReprintResponse = this.ParseData<InvoiceReprintResponse>(touchMessageFrame);
                if (invoiceReprintResponse != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {invoiceReprintResponse}");
                    InvoiceReprintResponseReceived?.Invoke(invoiceReprintResponse);
                }
            }
            #endregion
            #region 컨베이어
            else if (typeof(ConveyorCvSpeed).AssemblyQualifiedName == name)
            {
                var conveyorCvSpeed = this.ParseData<ConveyorCvSpeed>(touchMessageFrame);
                if (conveyorCvSpeed != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {invoiceReprintResponse}");
                    ConveyorCvSpeedReceived?.Invoke(conveyorCvSpeed);
                }
            }
            else if (typeof(RouteMode).AssemblyQualifiedName == name)
            {
                var routeMode = this.ParseData<RouteMode>(touchMessageFrame);
                if (routeMode != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {invoiceReprintResponse}");
                    RouteModeReceived?.Invoke(routeMode);
                }
            }
            #endregion
            #region 스마트패킹
            else if (typeof(SmartPackingConnectionState).AssemblyQualifiedName == name)
            {
                var smartPackingConnectionState = this.ParseData<SmartPackingConnectionState>(touchMessageFrame);
                if (smartPackingConnectionState != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {invoiceReprintResponse}");
                    SmartPackingConnectionStateReceived?.Invoke(smartPackingConnectionState);
                }
            }
            else if (typeof(SmartPackingBcrRead).AssemblyQualifiedName == name)
            {
                var smartPackingBcrRead = this.ParseData<SmartPackingBcrRead>(touchMessageFrame);
                if (smartPackingBcrRead != null)
                {
                    //this.Logger?.Write($"Recieved {name} : {invoiceReprintResponse}");
                    SmartPackingBcrReadReceived?.Invoke(smartPackingBcrRead);
                }
            }
            #endregion
            else
                this.Logger?.Write($"Recieved {name} : is Not Define Message");
        }

        #endregion

        #region Event Handler
        protected override void OnCommunicator_HsmsConnectionStateChanged(object sender, HsmsConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            //Server와 연결됬을때동작
        }
        #endregion
    }
}
