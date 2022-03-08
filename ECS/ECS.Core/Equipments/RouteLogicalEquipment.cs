using ECS.Core.Communicators;
using ECS.Core.Managers;
using ECS.Model;
using ECS.Model.Databases;
using ECS.Model.Hub;
using ECS.Model.Pcs;
using ECS.Model.Plc;
using ECS.Model.Restfuls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Net.Tcp;

namespace ECS.Core.Equipments
{
    public class RouteLogicalEquipment : LogicalEquipment
    {
        #region Field
        private int RouteLogicalBcrNoReadCount = 0;
        private DeviceStatus RouteLogicalDeviceStatus = new DeviceStatus();

        private DateTime LatestBcrReadTime = DateTime.MinValue; //BCR 순간 연속보고 오류 해결을 위함
        #endregion

        #region Prop
        public new RouteLogicalEquipmentSetting Setting
        {
            get => base.Setting as RouteLogicalEquipmentSetting;
            private set => base.Setting = value;
        }
        #endregion

        #region Ctor
        public RouteLogicalEquipment(RouteLogicalEquipmentSetting setting)
        {
            this.Setting = setting ?? new RouteLogicalEquipmentSetting();

            {
                DeviceInfo info = new DeviceInfo();
                info.deviceId = this.Setting.Id;
                info.deviceName = "분기 컨베이어";
                info.deviceTypeId = "CV";
                info.deviceTypeName = "컨베이어";
                this.RouteLogicalDeviceStatus.deviceList.Add(info);
            }
        }
        #endregion

        #region Method
        #region Interface
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            base.OnCreate();
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.RouteLogical));

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
                this.Logger?.Write($"Prepare Falut : {this.LifeState}");
                return;
            }

            if (this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Disconnected)
            {
                //동기가 느려서 비동기로 변경
                //this.Communicator?.Start();
                Task.Run(() => this.Bcr?.Start());
                this.Logger?.Write("Bcr Communicator Start Async");
            }
        }

        protected override void OnStop()
        {
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

        public void BcrAlarmSet(bool isOn, WeightInvoicBcrEnum weightInvoicBcrEnum)
        {
            try
            {
                #region BcrAlarmSet
                WeightInvoiceBcrAlarmArgs bcrAlarmArgs = new WeightInvoiceBcrAlarmArgs();
                bcrAlarmArgs.BcrType = weightInvoicBcrEnum;
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
            try
            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcSmartPackingEquipment>();
                if (eq != null)
                {
                    BcrAlarmSetReset bcrAlarmSetReset = new BcrAlarmSetReset();
                    bcrAlarmSetReset.Reason = reason;
                    bcrAlarmSetReset.AlarmResult = true;

                    eq.OnBcrAlarmSetResetSend(bcrAlarmSetReset);
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
                var bcrConnection = this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Connected ? true : false;

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcBcrLcdEquipment>();
                if (eq != null)
                {
                    eq.InvoiceBcrState.RouteBcrConnection = bcrConnection;
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

                var device = this.RouteLogicalDeviceStatus.deviceList[0];
                if (this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Connected)
                {
                    device.deviceStatusCd = 1;
                    device.deviceErrorMsg = string.Empty;
                }
                else
                {
                    device.deviceStatusCd = 0;
                    device.deviceErrorMsg = "분기 컨베이어 Disconnect";
                }

                var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                restfulRequseter.DeviceStatusPostAsync(this.RouteLogicalDeviceStatus);
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

        #region Event Handler
        #region Bcr
        private void Bcr_TcpConnectionStateChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            try
            {
                #region Bcr_TcpConnectionStateChanged
                this.Logger?.Write($"Bcr TcpConnectionStateChanged = {e.Current}");

                if (e.Current == TcpConnectionStateEnum.Connected)
                    this.BcrAlarmSet(false, WeightInvoicBcrEnum.Route);
                else
                {
                    this.BcrAlarmSet(true, WeightInvoicBcrEnum.Route);
                    this.AlarmSetTouchSend("BCR 미연결");
                }
                    
                this.OnEquipmentConnectionUpdateTouchSend();
                this.OnEquipmentStateRicpPost();
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

            this.Logger?.Write(e.Current.ToString());
        }

        private void Bcr_ReadData(object sender, string boxId)
        {
            this.Logger?.Write($"ReadData = {boxId}");

            try
            {
                #region Bcr Alarm
                if (this.RouteLogicalBcrNoReadCount >= 3)
                    this.BcrAlarmSet(false, WeightInvoicBcrEnum.Route);

                this.RouteLogicalBcrNoReadCount = 0;
                #endregion

                if (this.BcrTick())
                    return;

                RouteResultArgs routeResultArgs = new RouteResultArgs();
                routeResultArgs.BoxId = boxId;
                routeResultArgs.Result = RouteResult.NORMAL;
                MNL_PACKING_FLAGEnum mnlPackingFlag = MNL_PACKING_FLAGEnum.Y;

                if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(boxId))
                {
                    var info = EcsServerAppManager.Instance.Cache.ProductInfos[boxId];
                    info.TASK_ID = ((int)TASK_IDEnum.RouteBcr).ToString();

                    if (Enum.TryParse(info.MNL_PACKING_FLAG, out MNL_PACKING_FLAGEnum flag))
                        mnlPackingFlag = flag;

                    if (mnlPackingFlag == MNL_PACKING_FLAGEnum.N)
                        routeResultArgs.Result = RouteResult.SMART;
                    else
                    {
                        // Normal박스가 중량검수를 진행안했으면, SmartPacking 리젝으로 빼게 수정.
                        var dynamicEq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<DynamicScaleEquipment>();
                        if (dynamicEq != null)
                        {
                            if (this.NoWeightCheck(info.STATUS, info.BOX_WT))
                            {
                                // NoWeight
                                routeResultArgs.Result = RouteResult.SMART;
                                this.Logger?.Write($"NoWeightCheck : BoxId = {boxId}");
                            }
                            else if (dynamicEq.WeightFailCheck(info.WT_CHECK_FLAG, info.BOX_WT, info.WEIGHT_SUM) ||
                                     info.STATUS == $"{(int)statusEnum.insepct_reject_scale}")
                            {
                                // Weight Fail
                                routeResultArgs.Result = RouteResult.SMART;
                                this.Logger?.Write($"Weight Fail : BoxId = {boxId}");
                            }
                        }
                    }

                    this.Logger?.Write($"BoxId = {boxId}, Result = {routeResultArgs.Result}");
                }
                else
                {
                    // 메모리가 없으면 SmartPacking로 가게 수정
                    routeResultArgs.Result = RouteResult.SMART;
                    this.Logger?.Write($"BoxId = {boxId} have not ProductInfo");
                }
                   
                //PlC 속도를 위해 우선처리
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, routeResultArgs);

                BcrLcdIndexArgs routeLogicalBcrIndexArgs = new BcrLcdIndexArgs();
                routeLogicalBcrIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertRoute(boxId, $"{routeResultArgs.Result}");
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, routeLogicalBcrIndexArgs);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
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

        private void Bcr_Noread(object sender, EventArgs e)
        {
             this.Logger?.Write($"ReadData = Noread");

            try
            {
                #region Bcr Alarm
                if (this.RouteLogicalBcrNoReadCount <= 3)
                    this.RouteLogicalBcrNoReadCount++;

                if (this.RouteLogicalBcrNoReadCount >= 3)
                {
                    this.BcrAlarmSet(true, WeightInvoicBcrEnum.Route);
                    this.AlarmSetTouchSend("BCR 미인식 3회");
                }
                #endregion

                if (this.BcrTick())
                    return;

                RouteResultArgs routeResultArgs = new RouteResultArgs();
                routeResultArgs.Result = RouteResult.SMART; //기본Smart로 변경
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, routeResultArgs);

                BcrLcdIndexArgs routeLogicalBcrIndexArgs = new BcrLcdIndexArgs();
                routeLogicalBcrIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertRoute(string.Empty, $"{MNL_PACKING_FLAGEnum.N}");
                
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, routeLogicalBcrIndexArgs);
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
    public class RouteLogicalEquipmentSetting : LogicalEquipmentSetting
    {
        public RouteLogicalEquipmentSetting()
        {
            this.Name = HubServiceName.RouteLogicalEquipment;

            this.BcrCommunicatorSetting.Name = $"BCR#3({HubServiceName.RouteLogicalEquipment})";
        }
    }
}
