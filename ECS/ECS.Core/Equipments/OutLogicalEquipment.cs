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
    public class OutLogicalEquipment : LogicalEquipment
    {
        #region Field
        //private int BcrNoReadCount = 0;
        private DeviceStatus OutLogicalDeviceStatus = new DeviceStatus();
        #endregion

        #region Prop
        public new OutLogicalEquipmentSetting Setting
        {
            get => base.Setting as OutLogicalEquipmentSetting;
            private set => base.Setting = value;
        }
        #endregion

        #region Ctor
        public OutLogicalEquipment(OutLogicalEquipmentSetting setting)
        {
            this.Setting = setting ?? new OutLogicalEquipmentSetting();

            {
                DeviceInfo info = new DeviceInfo();
                info.deviceId = this.Setting.Id;
                info.deviceName = "출고";
                info.deviceTypeId = "CV";
                info.deviceTypeName = "컨베이어";
                this.OutLogicalDeviceStatus.deviceList.Add(info);
            }
        }
        #endregion

        #region Method

        #region Interface
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            base.OnCreate();
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.OutLogical));

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

        #region Bcr
        public void BcrAlarmSet(bool isOn)
        {
            try
            {
                #region BcrAlarmSet
                WeightInvoiceBcrAlarmArgs bcrAlarmArgs = new WeightInvoiceBcrAlarmArgs();
                bcrAlarmArgs.BcrType = WeightInvoicBcrEnum.Out;
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
            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcBcrLcdEquipment>();
            if (eq != null)
            {
                BcrAlarmSetReset bcrAlarmSetReset = new BcrAlarmSetReset();
                bcrAlarmSetReset.Reason = reason;
                bcrAlarmSetReset.AlarmResult = true;

                eq.OnBcrAlarmSetResetSend(bcrAlarmSetReset);
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
                    eq.InvoiceBcrState.OutBcrConnection = bcrConnection;
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

                var device = this.OutLogicalDeviceStatus.deviceList[0];
                if (this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Connected)
                {
                    device.deviceStatusCd = 1;
                    device.deviceErrorMsg = string.Empty;
                }
                else
                {
                    device.deviceStatusCd = 0;
                    device.deviceErrorMsg = "출고 Disconnect";
                }

                var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                restfulRequseter.DeviceStatusPostAsync(this.OutLogicalDeviceStatus);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion
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
                    this.BcrAlarmSet(false);
                else
                {
                    this.BcrAlarmSet(true);
                    this.AlarmSetTouchSend($"2F 출고 BCR{Environment.NewLine}연결 해제");
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

            this.Logger?.Write($"Bcr OperationStateChanged = {e.Current}");
        }

        private void Bcr_ReadData(object sender, string boxId)
        {
            this.Logger?.Write($"ReadData = {boxId}");

            try
            {
                #region Bcr Alarm
                //미사용. 단수상품일 경우 지속 발생
                //if (this.BcrNoReadCount >= 3)
                //    this.BcrAlarmSet(false);

                //this.BcrNoReadCount = 0;
                #endregion

                string invoiceId = string.Empty;

                var plc = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                if (plc != null)
                {
                    lock (EcsServerAppManager.Instance.Cache.ProductInfos)
                    {
                        if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(boxId))
                        {
                            var info = EcsServerAppManager.Instance.Cache.ProductInfos[boxId];
                            invoiceId = info.INVOICE_ID;

                            if (info.STATUS == ((int)statusEnum.top_invoice).ToString())
                            {
                                plc.OutResultExcute(BcrResult.OK, boxId);
                                this.Logger?.Write($"BoxId ={boxId}");

                                EcsServerAppManager.Instance.Cache.ProductInfoRemove(boxId);
                            }
                            else
                            {
                                plc.OutResultExcute(BcrResult.Reject, boxId);
                                this.Logger?.Write($"BoxId ={boxId} is not Top BCR Successed");
                                this.AlarmSetTouchSend($"2F 출고 BCR 리젝{Environment.NewLine}박스ID : {boxId}{Environment.NewLine}상면 미검증");
                            }
                        }
                        else
                        {
                            plc.OutResultExcute(BcrResult.Reject, boxId);
                            this.Logger?.Write($"BoxId ={boxId} have not ProductInfo");
                            this.AlarmSetTouchSend($"2F 출고 BCR 리젝{Environment.NewLine}박스ID : {boxId}{Environment.NewLine} 피킹실적 없음");
                        }
                    }
                }

                BcrLcdIndexArgs bcrLcdIndexArgs = new BcrLcdIndexArgs();
                bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertOut(boxId);
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, bcrLcdIndexArgs);

                OutLogicalBcrOnReadDataArgs args = new OutLogicalBcrOnReadDataArgs();
                {
                    args.BoxId = boxId;
                    args.InvoiceNo = invoiceId;
                }
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, args);
               
                EcsServerAppManager.Instance.Cache.UpdateQueuingTime();
            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
            }
        }
        private void Bcr_Noread(object sender, EventArgs e)
        {
            try
            {
                this.Logger?.Write($"ReadData = Noread");

                //미사용. 단수상품일 경우 지속 발생
                //if (this.BcrNoReadCount <= 3)
                //    this.BcrNoReadCount++;

                //if (this.BcrNoReadCount >= 3)
                //    this.BcrAlarmSet(true);

                var plc = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                if (plc != null)
                {
                    plc.OutResultExcute(BcrResult.Reject, string.Empty);
                    this.Logger?.Write($"BoxId is Noread");
                    this.AlarmSetTouchSend($"2F 출고 BCR 리젝{Environment.NewLine}박스 미인식");
                }

                BcrLcdIndexArgs bcrLcdIndexArgs = new BcrLcdIndexArgs();
                bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertOut(string.Empty);
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, bcrLcdIndexArgs);

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, new OutLogicalBcrOnReadDataArgs());
                #endregion

                EcsServerAppManager.Instance.Cache.UpdateQueuingTime();
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion
    }

    [Serializable]
    public class OutLogicalEquipmentSetting : LogicalEquipmentSetting
    {
        public OutLogicalEquipmentSetting()
        {
            this.Name = HubServiceName.OutLogicalEquipment;

            this.BcrCommunicatorSetting.Name = $"BCR#6({HubServiceName.OutLogicalEquipment})";
        }
    }
}
