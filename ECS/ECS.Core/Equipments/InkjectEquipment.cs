using System;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode.Net.Tcp;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode;
using ECS.Core.Communicators;
using ECS.Core.Managers;
using ECS.Model;
using ECS.Model.Hub;
using ECS.Model.Pcs;
using ECS.Model.Inkject;
using ECS.Model.Plc;
using ECS.Model.Domain.Touch;

namespace ECS.Core.Equipments
{
    public class InkjectEquipment : Equipment
    {
        #region Define
        private const int FLOOR = 2; 
        #endregion

        #region Field
        private string CurrentBoxType = string.Empty;
        private int CurrentCount = 0;
        private int InkjectBcrNoReadCount = 0;
        #endregion

        #region Prop
        public new Inkject_Copilot500Communicator Communicator
        {
            get => base.Communicator as Inkject_Copilot500Communicator;
            private set => base.Communicator = value;
        }

        public new InkjectEquipmentSetting Setting
        {
            get => base.Setting as InkjectEquipmentSetting;
            private set => base.Setting = value;
        }

        public BcrCommunicator Bcr { get; set; }

        private int m_InkPercet = 0;
        public int InkPercet 
        {
            get => this.m_InkPercet;
            private set
            {
                if (this.m_InkPercet == value) return;

                this.m_InkPercet = value;

                //Send Touch PC
                InkjectInkInformationArgs args = new InkjectInkInformationArgs();
                if (this.Name == HubServiceName.InkjectEquipment1)
                    args.Line = 1;
                else if (this.Name == HubServiceName.InkjectEquipment2)
                    args.Line = 2;

                args.InkPercent = this.m_InkPercet;
              
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcCaseErectEquipment, args);
            }
        }
        #endregion

        #region Ctor
        public InkjectEquipment(InkjectEquipmentSetting setting)
        {
            this.Setting = setting ?? new InkjectEquipmentSetting();
        }
        #endregion

        #region Method
        #region Interface
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            base.OnCreate();
            if (this.Name == HubServiceName.InkjectEquipment1)
                this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.Inkject1Log));
            else if (this.Name == HubServiceName.InkjectEquipment2)
                this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.Inkject2Log));

            this.Communicator = new Inkject_Copilot500Communicator(this);
            this.Setting.CommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
            this.Communicator?.ApplySetting(this.Setting.CommunicatorSetting);

            this.Bcr = new BcrCommunicator(this);
            this.Setting.BcrCommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
            this.Bcr?.ApplySetting(this.Setting.BcrCommunicatorSetting);

            this.LifeState = LifeCycleStateEnum.Created;
        }

        protected override bool OnPrepare()
        {
            if (this.LifeState != LifeCycleStateEnum.Created)
                this.Create();

            this.LifeState = LifeCycleStateEnum.Preparing;

            try
            {
                if (this.Communicator != null && this.Communicator.IsDisposed == false)
                {
                    this.Communicator.PrintCompleteResponse += this.Communicator_PrintComplete;
                    this.Communicator.FirstConnectedRecived += this.Communicator_FirstConnectedRecived;
                    this.Communicator.EnablePrintComplete += this.Communicator_EnablePrintComplete;
                    this.Communicator.AutoDataRecordResponse += this.Communicator_AutoDataRecordResponse;
                    this.Communicator.WriteAutoDataRecivedResponse += this.Communicator_WriteAutoDataRecivedResponse;
                    this.Communicator.WriteAutoDataQueueClearResponse += this.Communicator_WriteAutoDataQueueClearResponse;
                    this.Communicator.ReadInkLevelResponse += this.Communicator_ReadInkLevelResponse;
                    this.Communicator.GetAutoDataStringResponse += this.Communicator_GetAutoDataStringResponse;
                    this.Communicator.TcpConnectionStateChanged += this.Communicator_TcpConnectionStateChanged;
                    this.Communicator.OperationStateChanged += this.Communicator_OperationStateChanged;
                }

                if (this.Bcr != null && this.Bcr.IsDisposed == false)
                {
                    this.Bcr.Noread += this.Bcr_Noread;
                    this.Bcr.ReadData += this.Bcr_ReadData;
                    this.Bcr.TcpConnectionStateChanged += this.Bcr_TcpConnectionStateChanged;
                    this.Bcr.OperationStateChanged += this.Bcr_OperationStateChanged;
                }
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); return false; }

            this.LifeState = LifeCycleStateEnum.Prepared;
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
                    this.Communicator.PrintCompleteResponse -= this.Communicator_PrintComplete;
                    this.Communicator.FirstConnectedRecived -= this.Communicator_FirstConnectedRecived;
                    this.Communicator.EnablePrintComplete -= this.Communicator_EnablePrintComplete;
                    this.Communicator.AutoDataRecordResponse -= this.Communicator_AutoDataRecordResponse;
                    this.Communicator.WriteAutoDataRecivedResponse -= this.Communicator_WriteAutoDataRecivedResponse;
                    this.Communicator.WriteAutoDataQueueClearResponse -= this.Communicator_WriteAutoDataQueueClearResponse;
                    this.Communicator.ReadInkLevelResponse -= this.Communicator_ReadInkLevelResponse;
                    this.Communicator.GetAutoDataStringResponse -= this.Communicator_GetAutoDataStringResponse;
                    this.Communicator.TcpConnectionStateChanged -= this.Communicator_TcpConnectionStateChanged;
                    this.Communicator.OperationStateChanged -= this.Communicator_OperationStateChanged;
                    this.Communicator.Dispose();
                }

                if (this.Bcr != null && this.Bcr.IsDisposed == false)
                {
                    this.Bcr.Noread -= this.Bcr_Noread;
                    this.Bcr.ReadData -= this.Bcr_ReadData;
                    this.Bcr.TcpConnectionStateChanged -= this.Bcr_TcpConnectionStateChanged;
                    this.Bcr.OperationStateChanged -= this.Bcr_OperationStateChanged;
                    this.Bcr.Dispose();
                }
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.LifeState = LifeCycleStateEnum.Terminated;
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
                if (this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Disconnected)
                {
                    //동기가 느려서 비동기로 변경
                    //this.Communicator?.Start();
                    Task.Run(() => this.Communicator?.Start());
                    this.Logger?.Write("Communicator Start Async");
                }
            }

            if (this.Bcr != null || (this.Bcr.IsDisposed == false))
            {
                if (this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Disconnected)
                {
                    //동기가 느려서 비동기로 변경
                    //this.Communicator?.Start();
                    Task.Run(() => this.Bcr?.Start());
                    this.Logger?.Write("Bcr Communicator Start Async");
                }
            }
        }

        protected override void OnStop()
        {
            if (this.Communicator != null || (this.Communicator.IsDisposed == false))
            {
                if (this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Connected)
                {
                    this.Communicator.Stop();
                    this.Logger?.Write("Communicator Stop");
                }
            }

            if (this.Bcr != null || (this.Bcr.IsDisposed == false))
            {
                if (this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Connected)
                {
                    this.Bcr.Stop();
                    this.Logger?.Write("Bcr Communicator Stop");
                }
            }
        }
        #endregion

        #region Send Commands
        public bool EnablPrintCompleteAcknowledgementSend()
        {
            bool result = this.Communicator.SendMessage("A");
            this.Logger?.Write($"Enabl Print Complete Acknowledgement Send : {result}");

            return result;
        }

        public bool ReadAutoDataStatusSend()
        {
            bool result = this.Communicator.SendMessage("C");
            this.Logger?.Write($"Read Auto Data Status Send : {result}");

            return result;
        }

        public bool WriteAutoDataRecordSend(int floor, string boxType, int inkejctNo, int currentBoxCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("D");
            sb.Append(floor);
            sb.Append(boxType);
            sb.Append(inkejctNo);
            sb.Append($"{currentBoxCount}".PadLeft(7, '0'));

            bool result = this.Communicator.SendMessage(sb.ToString());
            this.Logger?.Write($"Write Auto Data Record Send(floor:{floor}, boxType:{boxType}, inkejctNo:{inkejctNo}), currentBoxCount:{currentBoxCount} : {result}");

            return result;
        }

        public bool ClearAutoDataQueueSend()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("D");
            sb.Append("_CLEAR_ADQ_");

            bool result = this.Communicator.SendMessage(sb.ToString());
            this.Logger?.Write($"Clear Auto Data Queue Send : {result}");

            return result;
        }

        public bool ReadInkLevelSend()
        {
            bool result = this.Communicator.SendMessage("o");
            this.Logger?.Write($"Read Ink Level Send : {result}");

            return result;
        }

        public bool GetAutoDataStringSend()
        {
            bool result = this.Communicator.SendMessage("GET_AUTO_DATA_STRING");
            this.Logger?.Write($"Get Auto Data String Send : {result}");

            return result;
        }
        #endregion

        private void WriteAutoDataRecordSend()
        {
            try
            {
                #region WriteAutoDataRecordSend
                if (string.IsNullOrEmpty(this.CurrentBoxType))
                {
                    this.InkjectAlarmSet(true);
                    return;
                }
                
                CaseErectInkJectAlarmArgs caseErectInkJectAlarmArgs = new CaseErectInkJectAlarmArgs();

                if (this.Name == HubServiceName.InkjectEquipment1)
                    caseErectInkJectAlarmArgs.InkjectNumber = 1;
                else if (this.Name == HubServiceName.InkjectEquipment2)
                    caseErectInkJectAlarmArgs.InkjectNumber = 2;

                int boxNumber = EcsServerAppManager.Instance.DataBaseManagerForServer.GetBoxNumber(this.CurrentBoxType, $"{caseErectInkJectAlarmArgs.InkjectNumber}");

                if (boxNumber == -1)
                    InkjectAlarmSet(true);
                else
                {
                    this.WriteAutoDataRecordSend(FLOOR, this.CurrentBoxType, caseErectInkJectAlarmArgs.InkjectNumber, boxNumber);
                    this.CurrentCount = boxNumber;
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
                if (e is CaseErectBoxTypeArgs caseErectBoxTypeArgs)
                {
                    #region BoxType
                    if (caseErectBoxTypeArgs == null) return;

                    this.CurrentBoxType = caseErectBoxTypeArgs.BoxType;
                    this.WriteAutoDataRecordSend();
                    #endregion
                }
                else if (e is CaseErectInkjectResumeArgs caseErectInkjectResumeArgs)
                {
                    // 잉크젯 재가동
                    CaseErectInkJectAlarmArgs caseErectInkJectAlarmArgs = new CaseErectInkJectAlarmArgs();
                    if (this.Name == HubServiceName.InkjectEquipment1)
                        caseErectInkJectAlarmArgs.InkjectNumber = 1;
                    else if (this.Name == HubServiceName.InkjectEquipment2)
                        caseErectInkJectAlarmArgs.InkjectNumber = 2;
                    caseErectInkJectAlarmArgs.Result = false;

                    EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcCaseErectEquipment, caseErectInkJectAlarmArgs);
                    
                    // 프린터 발행
                    this.WriteAutoDataRecordSend();
                    // 잉크레벨체크
                    this.ReadInkLevelSend();

                    this.BcrAlarmSet(false);
                }
                else if (e is InkjectInkInformationArgs inkjectInkInformationArgs)
                {
                    this.ReadInkLevelSend();
                }
                else if (e is CaseErectBoxNumberArgs caseErectBoxNumberArgs)
                {
                    // 제함 터치에서 From를 초기화
                    int boxNumber = EcsServerAppManager.Instance.DataBaseManagerForServer.GetBoxNumber(this.CurrentBoxType, $"{caseErectBoxNumberArgs.Line}");

                    if (boxNumber == -1)
                        InkjectAlarmSet(true);
                    else
                    {
                        this.WriteAutoDataRecordSend(FLOOR, this.CurrentBoxType, caseErectBoxNumberArgs.Line, boxNumber);
                        this.CurrentCount = boxNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        public override void OnEquipmentConnectionUpdateTouchSend()
        {
            try
            {
                #region OnEquipmentConnectionUpdateTouchSend
                var inkjectConnection = this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Connected ? true : false;
                var bcrConnection = this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Connected ? true : false;

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcCaseErectEquipment>();
                if (eq != null)
                {
                    if (this.Name == HubServiceName.InkjectEquipment1)
                    {
                        eq.ErectorConnectionState.Inkjet1Connection = inkjectConnection;
                        eq.ErectorConnectionState.ErectorBcr1Connection = bcrConnection;
                    }
                    else if (this.Name == HubServiceName.InkjectEquipment2)
                    {
                        eq.ErectorConnectionState.Inkjet2Connection = inkjectConnection;
                        eq.ErectorConnectionState.ErectorBcr2Connection = bcrConnection;
                    }

                    eq.EquipmentCoonectionStateSend();
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void InkjectAlarmSet(bool isOn)
        {
            try
            {
                #region InkjectAlarmSet
                CaseErectInkJectAlarmArgs arg = new CaseErectInkJectAlarmArgs();
                if (this.Name == HubServiceName.InkjectEquipment1)
                    arg.InkjectNumber = 1;
                else if (this.Name == HubServiceName.InkjectEquipment2)
                    arg.InkjectNumber = 2;

                arg.Result = isOn;

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcCaseErectEquipment, arg);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }        
        }

        private void BcrAlarmSet(bool isOn)
        {
            try
            {
                #region BcrAlarmSet
                CaseErectBcrAlarmArgs arg = new CaseErectBcrAlarmArgs();

                if (this.Name == HubServiceName.InkjectEquipment1)
                    arg.BcrNumber = 1;
                else if (this.Name == HubServiceName.InkjectEquipment2)
                    arg.BcrNumber = 2;

                arg.Result = isOn;

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcCaseErectEquipment, arg);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion

        #region Event Handler
        #region Inkject
        private void Communicator_PrintComplete(object sender, EventArgs e)
        {
            this.Logger?.Write($"Print Complete");

            #region Communicator_PrintComplete
            try
            {
                #region 디비저장
                CaseErectInkJectCompleteArgs args = new CaseErectInkJectCompleteArgs();

                string labelCount = $"{ this.CurrentCount}".PadLeft(7, '0');

                if (this.Name == HubServiceName.InkjectEquipment1)
                    args.BoxID = $"{FLOOR}{this.CurrentBoxType}{1}{labelCount}";
                else if (this.Name == HubServiceName.InkjectEquipment2)
                    args.BoxID = $"{FLOOR}{this.CurrentBoxType}{2}{labelCount}";

                // 디비저장
                EcsServerAppManager.Instance.DataBaseManagerForServer.PrintBox(args.BoxID);
                #endregion

                this.Logger?.Write($"Inkject - BoxId : {args.BoxID}");

                // 잉크젯 버퍼 클리어
                this.ClearAutoDataQueueSend();

                // 프린터 요청
                this.WriteAutoDataRecordSend();

                 // 잉크잔량 문의, Touch
                 this.ReadInkLevelSend();

                // 잉크젯 상태 문의
                //this.ReadAutoDataStatusSend();
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
            #endregion
        }

        private void Communicator_FirstConnectedRecived(object sender, bool e)
        {
            try
            {
                #region Communicator_FirstConnectedRecived
                this.Logger?.Write($"First Connected Recived : {e}");

                this.EnablPrintCompleteAcknowledgementSend();
                this.GetAutoDataStringSend();
                this.ReadInkLevelSend();
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void Communicator_EnablePrintComplete(object sender, EventArgs e)
        {
            try
            {
                #region Communicator_EnablePrintComplete
                this.Logger?.Write("Enable Print Complete");
                
                this.GetAutoDataStringSend();
                this.ReadInkLevelSend();
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void Communicator_GetAutoDataStringResponse(object sender, string e)
        {
            this.Logger?.Write($"Get Auto Data String Response : {e}");
        }

        private void Communicator_ReadInkLevelResponse(object sender, int e)
        {
            this.Logger?.Write($"Read Ink Level Response : {e}");
            this.InkPercet = e;
        }

        private void Communicator_AutoDataRecordResponse(object sender, AutoDataResponseEnum e)
        {
            this.Logger?.Write($"Read Auto Data Record Response : {e}");

            if (e == Model.Inkject.AutoDataResponseEnum.XOFF)
                this.ClearAutoDataQueueSend();
        }

        private void Communicator_WriteAutoDataRecivedResponse(object sender, EventArgs e)
        {
            this.Logger?.Write($"Write Auto Data Recived Response");

            //Todo. Write이후 카운트는 이전 카운트 숫자로 증감
            //따라서 기억한 값을 가지고 와야함.
            //this.SetCounterInfoSend();
        }

        private void Communicator_WriteAutoDataQueueClearResponse(object sender, EventArgs e)
        {
            this.Logger?.Write($"Write Auto Data Queue Clear Response");
        }

        private void Communicator_TcpConnectionStateChanged(object sender, TcpConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            try
            {
                #region Communicator_TcpConnectionStateChanged
                this.Logger?.Write($"Inkject TcpConnectionStateChanged = {e.Current}");

                if (e.Current == TcpConnectionStateEnum.Connected)
                {
                    this.InkjectAlarmSet(false);
                }
                else
                {
                    this.InkjectAlarmSet(true);
                }

                this.OnEquipmentConnectionUpdateTouchSend();
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void Communicator_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            this.Logger?.Write(e.Current.ToString());
        }
        #endregion

        #region Bcr
        private void Bcr_TcpConnectionStateChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            try
            {
                #region Bcr_TcpConnectionStateChanged
                this.Logger?.Write($"Bcr TcpConnectionStateChanged = {e.Current}");

                if (e.Current == TcpConnectionStateEnum.Connected)
                {
                    this.BcrAlarmSet(false);
                    this.WriteAutoDataRecordSend();
                }
                    
                else
                    this.BcrAlarmSet(true);

                this.OnEquipmentConnectionUpdateTouchSend();
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        private void Bcr_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            this.Logger?.Write($"Bcr OperationStateChanged = {e.Current}");
        }
        private void Bcr_ReadData(object sender, string boxId)
        {
            try
            {
                #region Bcr_ReadData
                this.Logger?.Write($"Bcr ReadData = {boxId}");

                this.InkjectBcrNoReadCount = 0;

                CaseErectBcrResultArgs arg = new CaseErectBcrResultArgs();

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcCaseErectEquipment>();
                if (eq != null)
                {
                    if (this.Name == HubServiceName.InkjectEquipment1)
                    {
                        arg.EqpId = eq.CaseErecterDeviceStatus.deviceList[(int)CaseErecter.Line1].deviceId;
                        arg.BcrId = 1;
                    }
                    else if (this.Name == HubServiceName.InkjectEquipment2)
                    {
                        arg.EqpId = eq.CaseErecterDeviceStatus.deviceList[(int)CaseErecter.Line2].deviceId;
                        arg.BcrId = 2;
                    }
                }

                arg.BoxId = boxId;
                //arg.BoxType = this.CurrentBoxType;

                if (boxId.Length > 2)
                    arg.BoxType = boxId.Substring(1, 1);

                (string verification, long bcrIndex) = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertBox(arg.BoxId, $"{arg.BcrId}");

                ResultType dbResultType = ResultType.NOREAD;
                if (Enum.TryParse(verification, out ResultType result))
                    dbResultType = result;
                arg.BcrIndex = bcrIndex;

                switch (dbResultType)
                {
                    case ResultType.OK:
                        arg.Result = BcrResult.OK;
                        //속도를 위해 우선
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcCaseErectEquipment, arg);
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.WcsPost, arg);
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, arg);
                        break;
                    case ResultType.NOREAD:
                    case ResultType.NOWEIGHT:
                    case ResultType.DUPLICATE:
                        arg.Result = BcrResult.Reject;
                        //속도를 위해 우선
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcCaseErectEquipment, arg);
                        break;
                }

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcCaseErectEquipment, arg);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        private void Bcr_Noread(object sender, EventArgs e)
        {
            this.Logger?.Write($"ReadData = NOREAD");

            try
            {
                #region BcrNoread
                if (this.InkjectBcrNoReadCount < 3)
                    this.InkjectBcrNoReadCount++;

                if (this.InkjectBcrNoReadCount >= 3 )
                    this.BcrAlarmSet(true);

                CaseErectBcrResultArgs arg = new CaseErectBcrResultArgs();
                if (this.Name == HubServiceName.InkjectEquipment1)
                    arg.BcrId = 1;
                else if (this.Name == HubServiceName.InkjectEquipment2)
                    arg.BcrId = 2;

                arg.BoxId = string.Empty;
                arg.Result = BcrResult.Reject;

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcCaseErectEquipment, arg);
                EcsServerAppManager.Instance.DataBaseManagerForServer.InsertBox("", $"{arg.BcrId}");
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcCaseErectEquipment, arg);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            } 
        }
        #endregion
        #endregion
    }

    [Serializable]
    public class InkjectEquipmentSetting : EquipmentSetting
    {
        public new InkjetCommunicatorSetting CommunicatorSetting
        {
            get => base.CommunicatorSetting as InkjetCommunicatorSetting;
            set => base.CommunicatorSetting = value;
        }

        public BcrCommunicatorSetting BcrCommunicatorSetting { get; set; } = new BcrCommunicatorSetting();

        public InkjectEquipmentSetting()
        {
            this.CommunicatorSetting = new InkjetCommunicatorSetting();
            this.Name = "Inkject";
        }
    }
}
