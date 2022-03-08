using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Net.Tcp;
using ECS.Core.Communicators;
using ECS.Core.Managers;
using ECS.Model;
using ECS.Model.DynamicScales;
using ECS.Model.Hub;
using ECS.Model.Plc;
using System.Data;
using ECS.Model.Restfuls;
using ECS.Model.Pcs;
using ECS.Model.Databases;
using System.Threading;
using Urcis.SmartCode.Threading;
using System.Diagnostics;
using ECS.Model.Domain.Touch;

namespace ECS.Core.Equipments
{
    public class DynamicScaleEquipment : Equipment
    {
        #region Field
        private TimeBoxQueue TimeBoxQueue = null;
        private int DynamiScaleBcrNoReadCount = 0;

        private readonly int WeightBindingTimeoutSecond = 3;
        private Stuck WeightBCR_CV_Stuck = new Stuck();
        #endregion

        #region Porp
        public new DynamicScale_TLW150Communicator Communicator
        {
            get => base.Communicator as DynamicScale_TLW150Communicator;
            private set => base.Communicator = value;
        }

        public new DynamicScaleEquipmentSetting Setting
        {
            get => base.Setting as DynamicScaleEquipmentSetting;
            private set => base.Setting = value;
        }

        public BcrCommunicator Bcr { get; set; }

        public DeviceStatus DynamicScaleDeviceStatus { get; private set; } = new DeviceStatus();
        #endregion

        #region Ctor
        public DynamicScaleEquipment(DynamicScaleEquipmentSetting setting)
        {
            this.Setting = setting ?? new DynamicScaleEquipmentSetting();
            {
                DeviceInfo info = new DeviceInfo();
                info.deviceId = this.Setting.Id;
                info.deviceName = "중량검수기 #1";
                info.deviceTypeId = "EQ";
                info.deviceTypeName = "설비";
                this.DynamicScaleDeviceStatus.deviceList.Add(info);
            }

            this.WeightBindingTimeoutSecond = this.Setting.WeightBindingTimeoutSecond;
        }
        #endregion

        #region Method
        #region Interface
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            base.OnCreate();

            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.DynamicScaleLog));

            this.Communicator = new DynamicScale_TLW150Communicator(this);
            this.Setting.CommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
            this.Communicator?.ApplySetting(this.Setting.CommunicatorSetting);

            this.Bcr = new BcrCommunicator(this);
            this.Setting.BcrCommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
            this.Bcr?.ApplySetting(this.Setting.BcrCommunicatorSetting);

            this.TimeBoxQueue = new TimeBoxQueue(this.WeightBindingTimeoutSecond * 1000);

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
                    this.Communicator.DynamicScaleResult += this.Communicator_DynamicScaleResult;
                    this.Communicator.TcpConnectionStateChanged += this.Communicator_TcpConnectionStateChanged;
                    this.Communicator.OperationStateChanged += this.Communicator_OperationStateChanged;
                }

                if (this.Bcr != null && this.Bcr.IsDisposed == false)
                {
                    this.Bcr.Noread += this.Bcr_OnNoread;
                    this.Bcr.ReadData += this.Bcr_OnReadData;
                    this.Bcr.TcpConnectionStateChanged += this.Bcr_TcpConnectionStateChanged;
                    this.Bcr.OperationStateChanged += this.Bcr_OperationStateChanged;
                }

                if (this.TimeBoxQueue != null)
                {
                    this.TimeBoxQueue.EnqueueTimeBoxEvent += TimeBoxQueue_EnqueueTimeBoxEvent;
                    this.TimeBoxQueue.DequeueTimeBoxEvent += TimeBoxQueue_DequeueTimeBoxEvent;
                    this.TimeBoxQueue.DequeueTimerTimeBoxEvent += TimeBoxQueue_DequeueTimerTimeBoxEvent;
                    this.TimeBoxQueue.EnqueueDeniedTickEvent += TimeBoxQueue_EnqueueDeniedTickEvent;
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
                    this.Communicator.DynamicScaleResult -= this.Communicator_DynamicScaleResult;
                    this.Communicator.TcpConnectionStateChanged -= this.Communicator_TcpConnectionStateChanged;
                    this.Communicator.OperationStateChanged -= this.Communicator_OperationStateChanged;
                    this.Communicator.Dispose();
                }

                if (this.Bcr != null && this.Bcr.IsDisposed == false)
                {
                    this.Bcr.Noread -= this.Bcr_OnNoread;
                    this.Bcr.ReadData -= this.Bcr_OnReadData;
                    this.Bcr.TcpConnectionStateChanged -= this.Bcr_TcpConnectionStateChanged;
                    this.Bcr.OperationStateChanged -= this.Bcr_OperationStateChanged;
                    this.Bcr.Dispose();
                }

                if (this.TimeBoxQueue != null)
                {
                    this.TimeBoxQueue.EnqueueTimeBoxEvent -= TimeBoxQueue_EnqueueTimeBoxEvent;
                    this.TimeBoxQueue.DequeueTimeBoxEvent -= TimeBoxQueue_DequeueTimeBoxEvent;
                    this.TimeBoxQueue.DequeueTimerTimeBoxEvent -= TimeBoxQueue_DequeueTimerTimeBoxEvent;
                    this.TimeBoxQueue.EnqueueDeniedTickEvent -= TimeBoxQueue_EnqueueDeniedTickEvent;
                    this.TimeBoxQueue.Dispose();
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

            this.TimeBoxQueue.DequeueTimerStop();
        }
        #endregion

        #region DynamicScale
        private void DynamicScaleAlarmSet(bool isOn)
        {
            try
            {
                #region DynamicScaleAlarmSet
                WeightInvoiceDynamiScaleAlarmArgs arg = new WeightInvoiceDynamiScaleAlarmArgs();
                arg.Result = isOn;

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, arg);
                #endregion
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
                var dynamiScaleConnection = this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Connected ? true : false;
                var bcrConnection = this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Connected ? true : false;

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcWeightInspectorEquipment>();
                if (eq != null)
                {
                    eq.WeightCheckBcrState.WeightCheckBcrConnection = bcrConnection;
                    eq.EquipmentCoonectionStateSend();
                }
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

                var device = this.DynamicScaleDeviceStatus.deviceList[0];
                if (this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Connected)
                {
                    device.deviceStatusCd = 1;
                    device.deviceErrorMsg = string.Empty;
                }
                else
                {
                    device.deviceStatusCd = 0;
                    device.deviceErrorMsg = "중량검수기 #1 Disconnect";
                }

                var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                restfulRequseter.DeviceStatusPostAsync(this.DynamicScaleDeviceStatus);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        public bool WeightFailCheck(string WT_CHECK_FLAG, string BOX_WT, double WEIGHT_SUM)
        {
            if (double.TryParse(BOX_WT, out double box_wt)) { }
            else return true;

            if (Enum.TryParse(WT_CHECK_FLAG, out WT_CHECK_FLAGEnum wt_check)) { }
            else return true;

            switch (wt_check)
            {
                case WT_CHECK_FLAGEnum.Y:
                    {
                        var dynamicResult = this.WeightCheck(box_wt, WEIGHT_SUM);
                        if (dynamicResult == DynamicResult.NormalBox || dynamicResult == DynamicResult.SmartBox)
                            return false;
                    }
                    break;
                case WT_CHECK_FLAGEnum.N:
                case WT_CHECK_FLAGEnum.W:
                case WT_CHECK_FLAGEnum.B:
                    return false;
                case WT_CHECK_FLAGEnum.M:
                    return true;
            }

            return true;
        }

        private DynamicResult WeightCheck(double BOX_WT, double WEIGHT_SUM)
        {
            double allow = WEIGHT_SUM * (0.05);
            double min = WEIGHT_SUM - allow;
            double max = WEIGHT_SUM + allow;

            if ((min <= BOX_WT) && (BOX_WT <= max))
                return DynamicResult.NormalBox;
            else if (BOX_WT > max)
                return DynamicResult.OVER;
            else if (BOX_WT < min)
                return DynamicResult.UNDER;

            return DynamicResult.Reject;
        }

        private (DynamicResult DynamicResult, string Verification) GetWeightResult(WT_CHECK_FLAGEnum wtcheckflag, MNL_PACKING_FLAGEnum mnlPackingFlag, double box_wt, double weight_sum)
        {
            DynamicResult dynamicResult = DynamicResult.Reject;
            string verification = $"{dynamicResult}";

            try
            {
                #region 무게 체크
                switch (wtcheckflag)
                {
                    case WT_CHECK_FLAGEnum.Y:
                        dynamicResult = this.WeightCheck(box_wt, weight_sum);
                        if (dynamicResult == DynamicResult.NormalBox) //OK 여부 확인
                        {
                            switch (mnlPackingFlag)
                            {
                                case MNL_PACKING_FLAGEnum.Y:
                                    dynamicResult = DynamicResult.NormalBox;
                                    break;
                                case MNL_PACKING_FLAGEnum.N:
                                    dynamicResult = DynamicResult.SmartBox;
                                    break;
                            }

                            verification = $"{ResultType.OK}";
                        }
                        else if (dynamicResult == DynamicResult.UNDER || dynamicResult == DynamicResult.OVER || dynamicResult == DynamicResult.Reject)
                        {
                            //PLC에게는 Reject 숫자만 허용
                            verification = $"{dynamicResult}";
                            dynamicResult = DynamicResult.Reject;
                        }
                        break;
                    #endregion
                    case WT_CHECK_FLAGEnum.N:
                    case WT_CHECK_FLAGEnum.W:
                    case WT_CHECK_FLAGEnum.B:
                        #region N : 중량검수 통과, W : 운송장부터 진행, B : BCR부터 진행(출고 이후)

                        switch (mnlPackingFlag)
                        {
                            case MNL_PACKING_FLAGEnum.Y:
                                dynamicResult = DynamicResult.NormalBox;
                                break;
                            case MNL_PACKING_FLAGEnum.N:
                                dynamicResult = DynamicResult.SmartBox;
                                break;
                        }
                        verification = $"{ResultType.OK}";
                        break;
                        #endregion
                    case WT_CHECK_FLAGEnum.M:
                        #region M : 강제 리젝
                        dynamicResult = DynamicResult.Reject;
                        verification = $"{ResultType.NOWEIGHT}";
                        #endregion
                        break;
                }
               
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            return (dynamicResult, verification);
        }

        private TimeBox GetNoTimeoutBoxReucrsive()
        {
            var box = this.TimeBoxQueue.BoxDequeue();
            if (box == null)
                return null;
            else
            {
                if (this.WeightBCR_CV_Stuck.IsStuck)
                    return box;
                else
                {
                    if (this.WeightBCR_CV_Stuck.StuckedTime >= box.BcrReadTime)
                        return box;
                    else
                    {
                        var span = DateTime.Now - box.BcrReadTime;
                        if (span.TotalSeconds <= this.WeightBindingTimeoutSecond)
                            return box;
                        else
                        {
                            WeightCheckIndexArgs weightCheckIndexArgs = new WeightCheckIndexArgs();
                            weightCheckIndexArgs.WeightCheckIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertWeightCheck(box.BoxId, 0, $"{ResultType.NOWEIGHT}");
                            EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcWeightInspectorEquipment, weightCheckIndexArgs);

                            this.Logger?.Write($"BoxId = {box.BoxId} Timeout(BcrReadTime = {box.BcrReadTime})");
                            return this.GetNoTimeoutBoxReucrsive();
                        }
                    }
                }
            }
        }
        #endregion

        #region Bcr
        public void BcrAlarmSet(bool isOn)
        {
            try
            {
                #region BcrAlarmSet
                WeightInvoiceBcrAlarmArgs bcrAlarmArgs = new WeightInvoiceBcrAlarmArgs();
                bcrAlarmArgs.BcrType = WeightInvoicBcrEnum.DynamicScale;
                bcrAlarmArgs.Result = isOn;

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, bcrAlarmArgs);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void AlarmSetTouchSend(string reason)
        {
            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcWeightInspectorEquipment>();
            if (eq != null)
            {
                BcrAlarmSetReset bcrAlarmSetReset = new BcrAlarmSetReset();
                bcrAlarmSetReset.Reason = reason;
                bcrAlarmSetReset.AlarmResult = true;

                eq.OnBcrAlarmSetResetSend(bcrAlarmSetReset);
            }
        }
        #endregion

        #region PLC
        public void SetIsStuck(bool isStuck)
        {
            this.WeightBCR_CV_Stuck.IsStuck = isStuck;
            this.Logger?.Write($"WeightBCR_CV_StuckOn = {isStuck}");

            if (this.WeightBCR_CV_Stuck.IsStuck)
                this.TimeBoxQueue.DequeueTimerStop();
        }
        #endregion
        #endregion

        #region Event Handler
        #region DynamicScale
        private void Communicator_TcpConnectionStateChanged(object sender, TcpConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            try
            {
                #region Communicator_TcpConnectionStateChanged
                this.Logger?.Write($"DynamicScale TcpConnectionStateChanged = {e.Current}");

                if (e.Current == TcpConnectionStateEnum.Connected)
                    this.DynamicScaleAlarmSet(false);
                else
                    this.DynamicScaleAlarmSet(true);

                this.OnEquipmentConnectionUpdateTouchSend();
                this.OnEquipmentStateRicpPost();
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

        private void Communicator_DynamicScaleResult(object sender, TLW150DataFormat e)
        {
            this.Logger?.Write($"DynamicScaleWeight = {e.IW0104}{e.WT0103}");

            try
            {
                WeightOrInvoiceResultArgs weightResultArg = new WeightOrInvoiceResultArgs();
                weightResultArg.Result = DynamicResult.Reject;
                WeightCheckIndexArgs weightCheckIndexArgs = new WeightCheckIndexArgs();
                string boxId = string.Empty;

                if (double.TryParse(e.IW0104, out double weight))
                {
                    TimeBox box = this.GetNoTimeoutBoxReucrsive();

                    if (box == null)
                    {
                        //PlC 속도를 위해 우선처리
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, weightResultArg);
                        weightCheckIndexArgs.WeightCheckIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertWeightCheck(string.Empty, weight, $"{ResultType.NOREAD}");
                        this.Logger?.Write("BoxId Binding is Null");
                    }
                    else
                    {
                        boxId = box.BoxId;

                        //if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(boxId) == false)
                        //{
                        //    DataTable dataTable = EcsServerAppManager.Instance.DataBaseManagerForServer.SelectAfterPickingBoxId(boxId);

                        //    EcsServerAppManager.Instance.Cache.ProductInfoLoad(dataTable);
                        //}

                        #region 메모리 확인
                        if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(boxId))
                        {
                            #region Box 분기 확인
                            ProductInfo info = EcsServerAppManager.Instance.Cache.ProductInfos[boxId];
                            info.BOX_WT = weight.ToString();

                            WT_CHECK_FLAGEnum wt_check_flag = WT_CHECK_FLAGEnum.Y;
                            if (Enum.TryParse(info.WT_CHECK_FLAG, out WT_CHECK_FLAGEnum wt_check))
                                wt_check_flag = wt_check;

                            MNL_PACKING_FLAGEnum mnl_packing_flag = MNL_PACKING_FLAGEnum.Y;
                            if (Enum.TryParse(info.MNL_PACKING_FLAG, out MNL_PACKING_FLAGEnum mnl_packing))
                                mnl_packing_flag = mnl_packing;

                            (DynamicResult result, string verification) = this.GetWeightResult(wt_check_flag, mnl_packing_flag, weight, info.WEIGHT_SUM);
                            #endregion

                            weightResultArg.BoxId = boxId;
                            weightResultArg.Result = result;
                            //PlC 속도를 위해 우선처리
                            EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, weightResultArg);

                            weightCheckIndexArgs.WeightCheckIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertWeightCheck(boxId, weight, verification);

                            #region WCS Format Set
                            switch (result)
                            {
                                case DynamicResult.NormalBox:
                                case DynamicResult.SmartBox:
                                    info.STATUS = ((int)statusEnum.inspect_pass_scale).ToString();
                                    break;
                                case DynamicResult.Reject:
                                    info.STATUS = ((int)statusEnum.insepct_reject_scale).ToString();
                                    break;
                            }

                            WeightAndInvoice.DataClass dataClass = new WeightAndInvoice.DataClass();
                            {
                                dataClass.wh_id = info.WH_ID;
                                dataClass.cst_cd = info.CST_CD;
                                dataClass.wave_no = info.WAVE_NO;
                                dataClass.wave_line_no = info.WAVE_LINE_NO;
                                dataClass.ord_no = info.ORD_NO;
                                dataClass.ord_line_no = info.ORD_LINE_NO;
                                dataClass.box_id = info.BOX_ID;
                                dataClass.box_no = info.BOX_NO;
                                dataClass.store_loc_cd = info.STORE_LOC_CD;
                                dataClass.box_type = info.BOX_TYPE;
                                dataClass.floor = "2";
                                dataClass.invoice_id = info.INVOICE_ID;
                                dataClass.status = info.STATUS;
                                dataClass.eqp_id = this.Id;
                                dataClass.box_wt = weight;
                                dataClass.result_cd = "E000";
                                dataClass.result_text = "정상처리";
                            }
                            weightResultArg.WeightAndInvoice.data.Add(dataClass);
                            #endregion

                            var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetWcsManager>();
                            if (manager !=null)
                                manager.WeightPostAsync(weightResultArg.WeightAndInvoice);

                            this.Logger?.Write($"BoxId ={boxId}, Result = {result}");
                        }
                        else
                        {
                            //PlC 속도를 위해 우선처리
                            weightResultArg.BoxId = box.BoxId;
                            EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, weightResultArg);
                            weightCheckIndexArgs.WeightCheckIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertWeightCheck(boxId, weight, $"{ResultType.NOREAD}");

                            CanceledInvoice info = EcsServerAppManager.Instance.DataBaseManagerForServer.SelectCanceledInvoiceById(boxId);
                            if (info != null)
                            {
                                #region WCS Format Set
                                WeightAndInvoice.DataClass dataClass = new WeightAndInvoice.DataClass();
                                {
                                    dataClass.wh_id = info.WH_ID;
                                    dataClass.cst_cd = info.CST_CD;
                                    dataClass.wave_no = info.WAVE_NO;
                                    dataClass.wave_line_no = info.WAVE_LINE_NO;
                                    dataClass.ord_no = info.ORD_NO;
                                    dataClass.ord_line_no = info.ORD_LINE_NO;
                                    dataClass.box_id = boxId;
                                    dataClass.box_no = info.BOX_NO;
                                    dataClass.store_loc_cd = info.STORE_LOC_CD;
                                    dataClass.box_type = info.BOX_TYPE;
                                    dataClass.floor = "2";
                                    dataClass.invoice_id = info.INVOICE_ID;
                                    dataClass.status = ((int)statusEnum.insepct_reject_scale).ToString();
                                    dataClass.eqp_id = this.Id;
                                    dataClass.box_wt = weight;
                                    dataClass.result_cd = "E021";
                                    dataClass.result_text = "취소된 송장";
                                }
                                weightResultArg.WeightAndInvoice.data.Add(dataClass);
                                #endregion

                                var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetWcsManager>();
                                if (manager != null)
                                    manager.WeightPostAsync(weightResultArg.WeightAndInvoice);

                                this.Logger?.Write($"BoxId ={boxId} have not ProductInfo and Order Cancel");
                            }
                            else
                                this.Logger?.Write($"BoxId ={boxId} have not ProductInfo");
                        }
                        #endregion
                       
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, weightResultArg);
                    }
                }
                else
                {
                    //중량검수기에서 에러시, 무게값 empty로 보고받음
                    TimeBox box = this.GetNoTimeoutBoxReucrsive();
                    if (box == null)
                    {
                        //PlC 속도를 위해 우선처리
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, weightResultArg);
                        this.Logger?.Write("BoxId Binding is Null And NoWeight");
                    }
                    else
                    {
                        boxId = box.BoxId;
                        #region 메모리 확인
                        if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(boxId))
                        {
                            var info = EcsServerAppManager.Instance.Cache.ProductInfos[boxId];
                            info.STATUS = ((int)statusEnum.insepct_reject_scale).ToString();

                            #region WCS Format Set
                            weightResultArg.BoxId = boxId;
                            weightResultArg.Result = DynamicResult.Reject;

                            //PlC 속도를 위해 우선처리
                            EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, weightResultArg);

                            WeightAndInvoice.DataClass dataClass = new WeightAndInvoice.DataClass();
                            {
                                dataClass.wh_id = info.WH_ID;
                                dataClass.cst_cd = info.CST_CD;
                                dataClass.wave_no = info.WAVE_NO;
                                dataClass.wave_line_no = info.WAVE_LINE_NO;
                                dataClass.ord_no = info.ORD_NO;
                                dataClass.ord_line_no = info.ORD_LINE_NO;
                                dataClass.box_id = info.BOX_ID;
                                dataClass.box_no = info.BOX_NO;
                                dataClass.store_loc_cd = info.STORE_LOC_CD;
                                dataClass.box_type = info.BOX_TYPE;
                                dataClass.floor = "2";
                                dataClass.invoice_id = info.INVOICE_ID;
                                dataClass.status = info.STATUS;
                                dataClass.eqp_id = this.Id;
                                dataClass.box_wt = weight;
                                dataClass.result_cd = "E023";
                                dataClass.result_text = "중량 측정 실패";
                            }
                            weightResultArg.WeightAndInvoice.data.Add(dataClass);
                            #endregion

                            var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetWcsManager>();
                            if (manager != null)
                                manager.WeightPostAsync(weightResultArg.WeightAndInvoice);

                            this.Logger?.Write($"BoxId = {boxId} is NoWeight");
                        }
                        else
                        {
                            //PlC 속도를 위해 우선처리
                            weightResultArg.BoxId = box.BoxId;
                            EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, weightResultArg);
                            this.Logger?.Write($"BoxId = {boxId} have not ProductInfo And NoWeight");
                        }
                        #endregion

                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, weightResultArg);
                    }

                    weightCheckIndexArgs.WeightCheckIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertWeightCheck(boxId, 0, $"{ResultType.NOWEIGHT}");
                }

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcWeightInspectorEquipment, weightCheckIndexArgs);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
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
                    this.BcrAlarmSet(false);
                else
                {
                    this.BcrAlarmSet(true);
                    this.AlarmSetTouchSend("BCR 미연결");
                }

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

        private void Bcr_OnNoread(object sender, EventArgs e)
        {
            try
            {
                #region BcrNoread
                this.Logger?.Write($"ReadData = Noread");

                this.TimeBoxQueue.BoxEnqueue(new TimeBox());

                if (this.DynamiScaleBcrNoReadCount <= 3)
                    this.DynamiScaleBcrNoReadCount++;

                if (this.DynamiScaleBcrNoReadCount >= 3)
                {
                    this.BcrAlarmSet(true);
                    this.AlarmSetTouchSend("BCR 미인식 3회");
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void Bcr_OnReadData(object sender, string e)
        {
            try
            {
                if (this.DynamiScaleBcrNoReadCount >= 3)
                    this.BcrAlarmSet(false);

                this.DynamiScaleBcrNoReadCount = 0;

                #region BcrRead
                this.Logger?.Write($"BcrReadData = {e}");

                DynamicScaleBcrOnReadDataArgs args = new DynamicScaleBcrOnReadDataArgs();
                args.Id = this.Id;
                args.BoxID = e;
                args.NoReadCheck = false;

                TimeBox box = new TimeBox();
                box.BoxId = args.BoxID;

                this.TimeBoxQueue.BoxEnqueue(box);

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, args);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion

        #region TimeBoxQueue
        private void TimeBoxQueue_EnqueueTimeBoxEvent(object sender, TimeBox box)
        {
            this.Logger?.Write($"Enqueue : {box}");
        }

        private void TimeBoxQueue_DequeueTimeBoxEvent(object sender, TimeBox box)
        {
            this.Logger?.Write($"Dequeue : {box}");
        }

        private void TimeBoxQueue_DequeueTimerTimeBoxEvent(object sender, TimeBox box)
        {
            this.Logger?.Write($"Dequeue from Timer: {box}");

            WeightOrInvoiceResultArgs weightResultArg = new WeightOrInvoiceResultArgs();
            weightResultArg.BoxId = box.BoxId;
            weightResultArg.Result = DynamicResult.Reject;
            EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, weightResultArg);

            WeightCheckIndexArgs weightCheckIndexArgs = new WeightCheckIndexArgs();
            weightCheckIndexArgs.WeightCheckIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertWeightCheck(box.BoxId, 0, $"{ResultType.NOWEIGHT}");

            this.Logger?.Write($"BoxId ={box.BoxId} is not Binding by Timeout");

            EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcWeightInspectorEquipment, weightCheckIndexArgs);
            EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, weightResultArg);
        }

        private void TimeBoxQueue_EnqueueDeniedTickEvent(object sender, TimeBox box)
        {
            this.Logger?.Write($"Enqueue Denined By Tick Inbound : {box}");
        }
        #endregion
        #endregion
    }

    [Serializable]
    public class DynamicScaleEquipmentSetting : EquipmentSetting
    {
        public new DynamicScaleCommunicatorSetting CommunicatorSetting
        {
            get => base.CommunicatorSetting as DynamicScaleCommunicatorSetting;
            set => base.CommunicatorSetting = value;
        }

        public BcrCommunicatorSetting BcrCommunicatorSetting { get; set; } = new BcrCommunicatorSetting();

        public int WeightBindingTimeoutSecond { get; set; } = 3;

        public DynamicScaleEquipmentSetting()
        {
            this.CommunicatorSetting = new DynamicScaleCommunicatorSetting();

            this.Id = "GW21";
            this.Name = HubServiceName.DynamicScaleEquipment;

            this.CommunicatorSetting.Name = $"{HubServiceName.DynamicScaleEquipment} Communicator)";
            this.BcrCommunicatorSetting.Name = $"BCR#2({HubServiceName.DynamicScaleEquipment} Front Communicator)";
        }
    }
}
