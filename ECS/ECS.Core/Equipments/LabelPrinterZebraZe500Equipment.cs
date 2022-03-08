using System;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Net.Tcp;
using Urcis.SmartCode;
using ECS.Core.Communicators;
using ECS.Model;
using ECS.Core.Managers;
using ECS.Model.LabelPrinter;
using System.Text;
using System.Threading.Tasks;
using ECS.Model.Hub;
using ECS.Model.Plc;
using ECS.Model.Restfuls;
using ECS.Model.Domain.Touch;
using ECS.Model.Databases;
using System.Linq;
using ECS.Model.Pcs;

namespace ECS.Core.Equipments
{
    public class LabelPrinterZebraZe500Equipment : Equipment
    {
        #region Field
        private int LabelPrintScaleBcrNoReadCount = 0;
        private DeviceStatus DeviceStatus = new DeviceStatus();

        private DateTime LatestBcrReadTime = DateTime.MinValue; //BCR 순간 연속보고 오류 해결을 위함
        #endregion

        #region Prop
        public new LabelPrinter_ZebraZe500_4Communicator Communicator
        {
            get => base.Communicator as LabelPrinter_ZebraZe500_4Communicator;
            private set => base.Communicator = value;
        }

        public new LabelPrinterZebraZe500EquipmentSetting Setting
        {
            get => base.Setting as LabelPrinterZebraZe500EquipmentSetting;
            private set => base.Setting = value;
        }

        public BcrCommunicator Bcr { get; set; }
        #endregion

        #region Ctor
        public LabelPrinterZebraZe500Equipment(LabelPrinterZebraZe500EquipmentSetting setting)
        {
            this.Setting = setting ?? new LabelPrinterZebraZe500EquipmentSetting();

            DeviceInfo info = new DeviceInfo();
            info.deviceTypeId = "EQ";
            info.deviceTypeName = "설비";

            this.DeviceStatus.deviceList.Add(info);
        }
        #endregion

        #region Method

        #region Interface
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            base.OnCreate();

            DeviceInfo info = this.DeviceStatus.deviceList[0];

            if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
            {
                info.deviceId = "GL21";
                info.deviceName = "오토라벨러 #1(송장)";
                this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.LabelPrinterZebraZe500_NormalLog));
            }
            else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
            {
                info.deviceId = "GL22";
                info.deviceName = "오토라벨러 #2(송장)";
                this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.LabelPrinterZebraZe500_SmartLog));
            }

            info.deviceTypeId = "EQ";
            info.deviceTypeName = "설비";

            this.Communicator = new LabelPrinter_ZebraZe500_4Communicator(this);
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
                    this.Communicator.AutoStatusResponse += this.Communicator_AutoStatusResponse;
                    this.Communicator.LabelRow += this.Communicator_LabelRow;
                    this.Communicator.LabelNormal += this.Communicator_LabelNormal;
                    this.Communicator.LabelError += this.Communicator_LabelError;
                    this.Communicator.PrintOkResponse += this.Communicator_PrintOkResponse;
                    this.Communicator.LabelAttachCompleted += Communicator_LabelAttachCompleted;
                    this.Communicator.PrintSkip += Communicator_PrintSkip;

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
                    this.Communicator.AutoStatusResponse -= this.Communicator_AutoStatusResponse;
                    this.Communicator.LabelRow -= this.Communicator_LabelRow;
                    this.Communicator.LabelNormal -= this.Communicator_LabelNormal;
                    this.Communicator.LabelError -= this.Communicator_LabelError;
                    this.Communicator.PrintOkResponse -= this.Communicator_PrintOkResponse;
                    this.Communicator.LabelAttachCompleted -= this.Communicator_LabelAttachCompleted;
                    this.Communicator.PrintSkip -= Communicator_PrintSkip;

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

            this.Bcr?.Start();

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

        #region LabelPrinter
        #region Send
        public void StateRequestSend() => this.Communicator?.SendMessage("STATE");

        public void BufferClearSend() => this.Communicator?.SendMessage($"CLEAR");

        public void ZplPrintSend(string boxId, string zpl)
        {
            //this.BufferClearSend();
            zpl = zpl.Replace("\r", "").Replace("\n", "").Replace(Environment.NewLine, ""); ;
            this.Communicator?.SendMessage($"{boxId}{zpl}");
            this.Logger?.Write($"ZPL Message : {boxId}{zpl}");
        }

        public void InvoicePrintSend(string boxId, string text)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"^XA");

            sb.Append($"^FO 100,200");
            sb.Append($"^A0 B,250,250");
            sb.Append($"^FD {text}");
            sb.Append($"^FS");

            sb.Append($"^FO 600,200");
            sb.Append($"^A0 B,150,150");
            sb.Append($"^FD{boxId}");
            sb.Append($"^FS");

            sb.Append($"^XZ");

            this.ZplPrintSend(boxId, sb.ToString());
        }
        #endregion

        public void LabelPrinterAlarmSet(bool isOn)
        {
            try
            {
                #region LabelPrinterAlarmSet

                WeightInvoiceLabelPrintAlarmArgs arg = new WeightInvoiceLabelPrintAlarmArgs();
                if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
                    arg.LabelPrintNumber = 1;
                else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
                    arg.LabelPrintNumber = 2;

                arg.Result = isOn;

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, arg);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion

        #region Bcr
        private void BcrAlarmSet(bool isOn)
        {
            try
            {
                #region BcrAlarmSet
                WeightInvoiceBcrAlarmArgs bcrAlarmArgs = new WeightInvoiceBcrAlarmArgs();

                if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
                    bcrAlarmArgs.BcrType = WeightInvoicBcrEnum.NormalLabel;
                else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
                    bcrAlarmArgs.BcrType = WeightInvoicBcrEnum.SmartLabel;
                bcrAlarmArgs.Result = isOn;

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, bcrAlarmArgs);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        public bool BcrTick()
        {
            var now = DateTime.Now;
            var span = now - this.LatestBcrReadTime;
            if (span.TotalMilliseconds < 400)
            {
                this.Logger?.Write($"ReadData Tick = {span.TotalMilliseconds}ms");
                return true;
            }

            this.LatestBcrReadTime = now;

            return false;
        }
        #endregion

        public override void OnEquipmentConnectionUpdateTouchSend()
        {
            try
            {
                #region OnEquipmentConnectionUpdateTouchSend
                var labelPrinterConnection = this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Connected ? true : false;
                var bcrConnection = this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Connected ? true : false;

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcBcrLcdEquipment>();
                if (eq != null)
                {
                    if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
                    {
                        eq.InvoiceBcrState.PrintInfoList[0].LabellerConnection = labelPrinterConnection;
                        eq.InvoiceBcrState.PrintInfoList[0].PrintBcrConnection = bcrConnection;
                    }
                    else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
                    {
                        eq.InvoiceBcrState.PrintInfoList[1].LabellerConnection = labelPrinterConnection;
                        eq.InvoiceBcrState.PrintInfoList[1].PrintBcrConnection = bcrConnection;
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

        protected override void OnEquipmentStateRicpPost()
        {
            try
            {
                #region OnEquipmentStateRicdpPost
                base.OnEquipmentStateRicpPost();

                var info = this.DeviceStatus.deviceList[0];

                if (this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Connected)
                {
                    info.deviceStatusCd = (int)EquipmentStateEnum.Run;
                    info.deviceErrorMsg = string.Empty;
                }
                else
                {
                    info.deviceStatusCd = (int)EquipmentStateEnum.Stop;
                    info.deviceErrorMsg = "Disconnect";
                }

                var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                if (restfulRequseter != null)
                    restfulRequseter.DeviceStatusPostAsync(this.DeviceStatus);
                
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private LabelerStateFlag GetLabelError(LabelErrorEnum labelErrorEnum)
        {
            switch (labelErrorEnum)
            {
                case LabelErrorEnum.PrinterError:
                    {
                        return LabelerStateFlag.PrintError;
                    }
                case LabelErrorEnum.Paper:
                    {
                        return LabelerStateFlag.PaperEmptyWarning;
                    }
                case LabelErrorEnum.AdsorptionError:
                case LabelErrorEnum.Ribbon:
                case LabelErrorEnum.SeoboOrigin:
                    {
                       return LabelerStateFlag.EtcError;
                    }
                case LabelErrorEnum.Seobo:
                    {
                        return LabelerStateFlag.EtcWarning;
                    }
            }

            return LabelerStateFlag.None;
        }

        private long DbInsertPrint(string boxId, ResultType resultType)
        {
            long index = 0;
            if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
                index = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertPrint(boxId, true, $"{resultType}");
            else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
                index = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertPrint(boxId, false, $"{resultType}");

            return index;
        }

        private bool NoWeightCheck(string status, string BOX_WT)
        {
            if (int.TryParse(status, out int s) == false) 
                return true;

            if (double.TryParse(BOX_WT, out double box_wt) == false)
                return true;

            if ((s >= (int)statusEnum.inspect_pass_sys)
                   && (box_wt > 0))
                return false;

            return true;
        }

        private bool DuplicateInvoicePrintCheck(string taskId)
        {
            if (int.TryParse(taskId, out int t))
            {
                if (t <= (int)TASK_IDEnum.SmartInvoice)
                    return false;
            }
            else
                return false; //이상한 값이 넘어와도 송장인쇄는 하겠다.

            return true;
        }

        private void InvoicePrint(string boxId)
        {
            try
            {
                #region InvoiceFind
                BcrLcdIndexArgs bcrLcdIndexArgs = new BcrLcdIndexArgs();

                if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(boxId))
                {
                    var info = EcsServerAppManager.Instance.Cache.ProductInfos[boxId];

                    if (this.DuplicateInvoicePrintCheck(info.TASK_ID))
                    {
                        this.InvoicePrintSend(boxId, $"{ResultType.DUPLICATE}");
                        bcrLcdIndexArgs.BcrIndex = this.DbInsertPrint(boxId, ResultType.DUPLICATE);
                        this.Logger?.Write($"Invoice Print Requset : BoxId = {boxId}, Invoice = {info.INVOICE_ID}, is Duplicate");
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, bcrLcdIndexArgs);

                        return;
                    }

                    var dynamicEq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<DynamicScaleEquipment>();
                    if (dynamicEq != null)
                    {
                        if (this.NoWeightCheck(info.STATUS, info.BOX_WT))
                        {
                            this.InvoicePrintSend(boxId, $"{ResultType.NOWEIGHT}");
                            bcrLcdIndexArgs.BcrIndex = this.DbInsertPrint(boxId, ResultType.NOWEIGHT);
                            this.Logger?.Write($"Invoice Print Requset : BoxId = {boxId}, Invoice = {info.INVOICE_ID}, is No weight");
                            EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, bcrLcdIndexArgs);

                            return;
                        }
                        else if (dynamicEq.WeightFailCheck(info.WT_CHECK_FLAG, info.BOX_WT, info.WEIGHT_SUM) ||
                                 info.STATUS == $"{(int)statusEnum.insepct_reject_scale}")
                        {
                            //중량 리젝된 Box가 송장발행되기 막기 위함
                            this.InvoicePrintSend(boxId, $"{ResultType.WEIGHT_FAIL}");
                            bcrLcdIndexArgs.BcrIndex = this.DbInsertPrint(boxId, ResultType.WEIGHT_FAIL);
                            this.Logger?.Write($"Invoice Print Requset : BoxId = {boxId}, Invoice = {info.INVOICE_ID}, is Weight Fail");
                            EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, bcrLcdIndexArgs);

                            return;
                        }
                    }

                    if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
                        info.TASK_ID = ((int)TASK_IDEnum.NormaInvoice).ToString();
                    else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
                        info.TASK_ID = ((int)TASK_IDEnum.SmartInvoice).ToString();

                    string zpl = info.INVOICE_ZPL;

                    this.ZplPrintSend(boxId, zpl);
                    bcrLcdIndexArgs.BcrIndex = this.DbInsertPrint(boxId, ResultType.OK);
                    this.Logger?.Write($"Invoice Print Requset : BoxId = {boxId}, Invoice = {info.INVOICE_ID}");
                }
                else
                {
                    this.InvoicePrintSend(boxId, $"{ResultType.NOWEIGHT}");
                    bcrLcdIndexArgs.BcrIndex = this.DbInsertPrint(boxId, ResultType.NOWEIGHT);
                    this.Logger?.Write($"BoxId = {boxId} have not ProductInfo And NoWeight");
                }  

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, bcrLcdIndexArgs);
                #endregion
            }
            catch (Exception ex)
            {
                Log.WriteException(ex.Message);
            }
        }
        #endregion

        #region Event Handler

        #region LabelPrinter
        private void Communicator_TcpConnectionStateChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            try
            {
                #region Communicator_TcpConnectionStateChanged
                this.Logger?.Write($"LabelPrinterZebraZe500 TcpConnectionStateChanged = {e.Current}");

                if (e.Current == TcpConnectionStateEnum.Connected)
                {
                    this.LabelPrinterAlarmSet(false);
                    //this.BufferClearSend();
                }
                else
                    this.LabelPrinterAlarmSet(true);

                this.OnEquipmentStateRicpPost();
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

        private void Communicator_LabelAttachCompleted(object sender, string boxId)
        {
            this.Logger?.Write($"Label Attach Completed : BoxId = {boxId}");
            //this.BufferClearSend();
        }

        private void Communicator_PrintOkResponse(object sender, EventArgs e)
        {
            this.Logger?.Write($"Print Ok Response");
        }

        private void Communicator_LabelError(object sender, LabelErrorEnum lableError)
        {
            // 에러 메세지
            try
            {
                this.Logger?.Write($"Label State Error : {lableError}");
                //this.BufferClearSend();

                this.LabelPrinterAlarmSet(true);

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcBcrLcdEquipment>();
                if (eq != null)
                {
                    LabelerStateFlag labelerStateFlag = this.GetLabelError(lableError);

                    if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
                        eq.InvoiceBcrState.PrintInfoList[0].LabelerState = labelerStateFlag;
                    else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
                        eq.InvoiceBcrState.PrintInfoList[1].LabelerState = labelerStateFlag;

                    eq.OnEquipmentConnectionUpdateTouchSend();
                }


                var info = this.DeviceStatus.deviceList[0];
                info.deviceStatusCd = (int)EquipmentStateEnum.Error;
                info.deviceErrorMsg = $"{lableError}";

                var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                if (restfulRequseter != null)
                    restfulRequseter.DeviceStatusPostAsync(this.DeviceStatus);
            }
            catch (Exception ex )
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void Communicator_LabelNormal(object sender, EventArgs e)
        {
            this.Logger?.Write($"Label State Normal");

            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcBcrLcdEquipment>();
            if (eq != null)
            {
                if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
                    eq.InvoiceBcrState.PrintInfoList[0].LabelerState = LabelerStateFlag.None;
                else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
                    eq.InvoiceBcrState.PrintInfoList[1].LabelerState = LabelerStateFlag.None;

                eq.OnEquipmentConnectionUpdateTouchSend();
            }

            var info = this.DeviceStatus.deviceList[0];
            info.deviceStatusCd = (int)EquipmentStateEnum.Run;
            info.deviceErrorMsg = string.Empty;

            var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
            if (restfulRequseter != null)
                restfulRequseter.DeviceStatusPostAsync(this.DeviceStatus);
        }

        private void Communicator_LabelRow(object sender, EventArgs e)
        {
            this.Logger?.Write($"Label State Row");

            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcBcrLcdEquipment>();
            if (eq != null)
            {
                if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
                    eq.InvoiceBcrState.PrintInfoList[0].LabelerState = LabelerStateFlag.PaperEmptyWarning;
                else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
                    eq.InvoiceBcrState.PrintInfoList[1].LabelerState = LabelerStateFlag.PaperEmptyWarning;

                eq.OnEquipmentConnectionUpdateTouchSend();

                var info = this.DeviceStatus.deviceList[0];
                info.deviceStatusCd = (int)EquipmentStateEnum.Error;
                info.deviceErrorMsg = "Label State Row";

                var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                if (restfulRequseter != null)
                    restfulRequseter.DeviceStatusPostAsync(this.DeviceStatus);
            }
        }

        private void Communicator_AutoStatusResponse(object sender, bool isAuto)
        {
            // 상태확인
            this.Logger?.Write($"Auto Status Response : {isAuto}");
            //this.BufferClearSend();

            if (isAuto)
            {
                this.LabelPrinterAlarmSet(false);
                this.BcrAlarmSet(false);
            }
            else
            {
                this.LabelPrinterAlarmSet(true);
                this.BcrAlarmSet(true);
            }

            if (isAuto == false)
            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcBcrLcdEquipment>();
                if (eq != null)
                {
                    if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
                        eq.InvoiceBcrState.PrintInfoList[0].LabelerState = LabelerStateFlag.EtcWarning;
                    else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
                        eq.InvoiceBcrState.PrintInfoList[1].LabelerState = LabelerStateFlag.EtcWarning;

                    eq.OnEquipmentConnectionUpdateTouchSend();
                }
            }
        }

        private void Communicator_PrintSkip(object sender, EventArgs e)
        {
            this.Logger?.Write($"Print Skip");
            //this.BufferClearSend();

            this.LabelPrinterAlarmSet(true);

            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcBcrLcdEquipment>();
            if (eq != null)
            {
                if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
                    eq.InvoiceBcrState.PrintInfoList[0].LabelerState = LabelerStateFlag.EtcWarning;
                else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
                    eq.InvoiceBcrState.PrintInfoList[1].LabelerState = LabelerStateFlag.EtcWarning;

                eq.OnEquipmentConnectionUpdateTouchSend();
            }
        }

        #endregion

        #region Bcr
        private void Bcr_TcpConnectionStateChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            this.Logger?.Write($"Bcr Tcp Connection State Changed = {e.Current}");

            if (e.Current == TcpConnectionStateEnum.Connected)
                this.BcrAlarmSet(false);
            else
                this.BcrAlarmSet(true);

            this.OnEquipmentConnectionUpdateTouchSend();

        }

        private void Bcr_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            this.Logger?.Write(e.Current.ToString());
        }

        private void Bcr_ReadData(object sender, string boxId)
        {
            this.Logger?.Write($"BcrReadData = {boxId}");
            try
            {
                #region Bcr Alarm

                if (this.LabelPrintScaleBcrNoReadCount >= 3)
                    this.BcrAlarmSet(false);

                this.LabelPrintScaleBcrNoReadCount = 0;
                #endregion

                if (this.BcrTick())
                    return;

                this.InvoicePrint(boxId);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void Bcr_Noread(object sender, EventArgs e)
        {
            try
            {
                if (this.LabelPrintScaleBcrNoReadCount <= 3)
                    this.LabelPrintScaleBcrNoReadCount++;

                if (this.LabelPrintScaleBcrNoReadCount >= 3)

                    this.BcrAlarmSet(true);
                if (this.BcrTick())
                    return;

                BcrLcdIndexArgs bcrLcdIndexArgs = new BcrLcdIndexArgs();

                if (this.Name == HubServiceName.NormalLabelPrinterZebraZe500Equipment)
                    bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertPrint(string.Empty, true, "NOREAD");
                else if (this.Name == HubServiceName.SmartLabelPrinterZebraZe500Equipment)
                    bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertPrint(string.Empty, false, "NOREAD");

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, bcrLcdIndexArgs);
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
    public class LabelPrinterZebraZe500EquipmentSetting : EquipmentSetting
    {
        public new LabelPrinter_ZebraZe500CommunicatorSetting CommunicatorSetting
        {
            get => base.CommunicatorSetting as LabelPrinter_ZebraZe500CommunicatorSetting;
            set => base.CommunicatorSetting = value;
        }

        public BcrCommunicatorSetting BcrCommunicatorSetting { get; set; } = new BcrCommunicatorSetting();

        public LabelPrinterZebraZe500EquipmentSetting()
        {
            this.CommunicatorSetting = new LabelPrinter_ZebraZe500CommunicatorSetting();
            this.Name = "ZebraZe500";
        }
    }
}
