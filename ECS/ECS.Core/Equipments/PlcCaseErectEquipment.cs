using ECS.Model;
using ECS.Model.Pcs;
using ECS.Model.Plc;
using ECS.Model.Hub;
using ECS.Core.Managers;
using ECS.Core.Util;
using ECS.Core.Equipments;
using System;
using System.IO;
using Urcis.SmartCode.Io;
using Urcis.SmartCode.Threading;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Helpers;
using ECS.Model.Inkject;
using System.Collections.Generic;
using System.Timers;
using Urcis.SmartCode.Events;
using ECS.Model.Restfuls;
using System.Linq;
using System.Text.RegularExpressions;

namespace ECS.Core.Equipments
{
    public class PlcCaseErectEquipment : PlcGeneralEquipment
    {
        #region Prop
        private PlcCaseErectIoHandler PlcCaseErectIoHandler { get; set; } = new PlcCaseErectIoHandler();

        public DeviceStatus CaseErecterDeviceStatus { get; private set; } = new DeviceStatus();

        private CaseErectCvSpeed[] CaseErectCvSpeed { get; set; }
        #endregion

        #region Ctor
        public PlcCaseErectEquipment(PlcEquipmentSetting setting) : base(setting)
        {
            {
                DeviceInfo info = new DeviceInfo();
                info.deviceId = "GB21";
                info.deviceName = "제함기 #1";
                info.deviceTypeId = "EQ";
                info.deviceTypeName = "설비";
                this.CaseErecterDeviceStatus.deviceList.Add(info);
            }
            {
                DeviceInfo info = new DeviceInfo();
                info.deviceId = "GB22";
                info.deviceName = "제함기 #2";
                info.deviceTypeId = "EQ";
                info.deviceTypeName = "설비";
                this.CaseErecterDeviceStatus.deviceList.Add(info);
            }

            this.CaseErectCvSpeed = new CaseErectCvSpeed[this.PlcCaseErectIoHandler.IoPointCVSpeeds.Length];
            for (int i = 0; i < this.CaseErectCvSpeed.Length; i++)
            {
                this.CaseErectCvSpeed[i] = new CaseErectCvSpeed();
            }
        }
        #endregion

        #region Method
        protected override void OnSetPlcIoHandler(string groupName)
        {
            try
            {
                base.OnSetPlcIoHandler(groupName);

                string blockName = string.Empty;
                string communicatorName = this.Communicator.Name;

                #region PLC Priority
                blockName = "PLC BIT Event";
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 2; j++)
                        this.PlcCaseErectIoHandler.IoPointRollTainers[i].Sensors[j] = IoServer.GetPoint(communicatorName, blockName, $"RollTainer#{i + 1} - Sensor#{j + 1}");
                }

                this.PlcCaseErectIoHandler.P500EMGBitEvent = IoServer.GetHandler<BitEventIoHandler>(groupName, $"P500 EMG");

                blockName = "PLC WORD";
                for (int i = 0; i < 2; i++)
                {
                    this.PlcCaseErectIoHandler.IoPointCaseErectEquipmentStatuses[i].StatusCd = IoServer.GetPoint(communicatorName, blockName, $"CaseErect Equipment Status#{i + 1} - StatusCd");
                    this.PlcCaseErectIoHandler.IoPointCaseErectEquipmentStatuses[i].ErrorMsg = IoServer.GetPoint(communicatorName, blockName, $"CaseErect Equipment Status#{i + 1} - ErrorMsg");
                }

                for (int i = 0; i < 2; i++)
                {
                    this.PlcCaseErectIoHandler.IoPointCaseErectInfos[i].BoxType = IoServer.GetPoint(communicatorName, blockName, $"CaseErect  Info#{i + 1} - BoxType");
                    this.PlcCaseErectIoHandler.IoPointCaseErectInfos[i].BoxQty = IoServer.GetPoint(communicatorName, blockName, $"CaseErect  Info#{i + 1} - Box Qty");
                }

                for (int i = 0; i < 5; i++)
                {
                    this.PlcCaseErectIoHandler.IoPointCVSpeeds[i].SV = IoServer.GetPoint(communicatorName, blockName, $"C/V Speed#{i + 1} - SV");
                    this.PlcCaseErectIoHandler.IoPointCVSpeeds[i].PV = IoServer.GetPoint(communicatorName, blockName, $"C/V Speed#{i + 1} - PV");
                }

                //미사용
                //this.PlcCaseErectIoHandler.IoPointRejectCylinderOperationTime.BottomSV = IoServer.GetPoint(communicatorName, blockName, $"Reject Cylinder Operation Time - Bottom SV");
                //this.PlcCaseErectIoHandler.IoPointRejectCylinderOperationTime.TopSV = IoServer.GetPoint(communicatorName, blockName, $"Reject Cylinder Operation Time - Top SV");
                #endregion

                #region PC Priority
                blockName = "SERVER BIT Command";
                for (int i = 0; i < 2; i++)
                    this.PlcCaseErectIoHandler.InkjectAlarms[i] = IoServer.GetPoint(communicatorName, blockName, $"Inkject#{i + 1} Alarm");

                for (int i = 0; i < 2; i++)
                    this.PlcCaseErectIoHandler.BcrAlarms[i] = IoServer.GetPoint(communicatorName, blockName, $"BCR#1-{i + 1} Alarm");

                for (int i = 0; i < 2; i++)
                    this.PlcCaseErectIoHandler.BcrReadingResults[i] = IoServer.GetHandler<DataCommandIoHandler>(groupName, $"BCR Reading Result#1-{i + 1}");

                for (int i = 0; i < 6; i++)
                    this.PlcCaseErectIoHandler.CVSpeeds[i] = IoServer.GetHandler<DataCommandIoHandler>(groupName, $"C/V Speed#{i + 1}");

                //미사용
                //this.PlcCaseErectIoHandler.RejecCylinderOperationTime = IoServer.GetHandler<DataCommandIoHandler>(groupName, $"Rejec Cylinder Operation Time");
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
            }
        }
        protected override void OnSubscribePlcIoHandler()
        {
            try
            {
                base.OnSubscribePlcIoHandler();

                for (int i = 0; i < 2; i++)
                {
                    this.PlcCaseErectIoHandler.IoPointCaseErectEquipmentStatuses[i].StatusCd.ValueChanged += EquipmentStatusCd_ValueChanged;
                    this.PlcCaseErectIoHandler.IoPointCaseErectInfos[i].BoxType.ValueChanged += CaseErectBoxType_ValueChanged;
                }

                for (int i = 0; i < 5; i++)
                {
                    this.PlcCaseErectIoHandler.IoPointCVSpeeds[i].SV.ValueChanged += SV_ValueChanged;
                    this.PlcCaseErectIoHandler.IoPointCVSpeeds[i].PV.ValueChanged += PV_ValueChanged;
                }

                if (this.PlcCaseErectIoHandler.P500EMGBitEvent != null)
                    this.PlcCaseErectIoHandler.P500EMGBitEvent.EventOccurred += P500EMGBitEvent_EventOccurred;
            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
            }
        }

        protected override void OnUnSubscribePlcIoHandler()
        {
            try
            {
                base.OnUnSubscribePlcIoHandler();

                for (int i = 0; i < 2; i++)
                {
                    this.PlcCaseErectIoHandler.IoPointCaseErectEquipmentStatuses[i].StatusCd.ValueChanged -= EquipmentStatusCd_ValueChanged;
                    this.PlcCaseErectIoHandler.IoPointCaseErectInfos[i].BoxType.ValueChanged -= CaseErectBoxType_ValueChanged;
                }

                for (int i = 0; i < 5; i++)
                {
                    this.PlcCaseErectIoHandler.IoPointCVSpeeds[i].SV.ValueChanged -= SV_ValueChanged;
                    this.PlcCaseErectIoHandler.IoPointCVSpeeds[i].PV.ValueChanged -= PV_ValueChanged;
                }

                if (this.PlcCaseErectIoHandler.P500EMGBitEvent != null)
                    this.PlcCaseErectIoHandler.P500EMGBitEvent.EventOccurred -= P500EMGBitEvent_EventOccurred;
            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
            }
        }
        protected override void OnSetOwnerPlcIoHandler()
        {
            base.OnSetOwnerPlcIoHandler();

            #region PLC Priority
            if (this.PlcCaseErectIoHandler.P500EMGBitEvent != null)
                this.PlcCaseErectIoHandler.P500EMGBitEvent.Owner = this;
            #endregion

            #region PC Priority
            for (int i = 0; i < 2; i++)
            {
                if (this.PlcCaseErectIoHandler.BcrReadingResults[i] != null)
                    this.PlcCaseErectIoHandler.BcrReadingResults[i].Owner = this;
            }

            for (int i = 0; i < 6; i++)
            {
                if (this.PlcCaseErectIoHandler.CVSpeeds[i] != null)
                    this.PlcCaseErectIoHandler.CVSpeeds[i].Owner = this;
            }
            #endregion
        }

        public override void OnEquipmentConnectionUpdateTouchSend()
        {
            base.OnEquipmentConnectionUpdateTouchSend();

            try
            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcCaseErectEquipment>();

                if (eq != null)
                {
                    if (this.IsConnected)
                    {
                        for (int i = 0; i < this.PlcCaseErectIoHandler.IoPointCaseErectEquipmentStatuses.Length; i++)
                        {
                            var device = this.CaseErecterDeviceStatus.deviceList[i];
                            device.deviceStatusCd = (short)this.PlcCaseErectIoHandler.IoPointCaseErectEquipmentStatuses[i].StatusCd.Value;

                            if (Enum.TryParse(device.deviceStatusCd.ToString(), out EquipmentStateEnum stateEnum))
                            {
                                bool erectorConnection = false;
                                switch (stateEnum)
                                {
                                    case EquipmentStateEnum.Run:
                                    case EquipmentStateEnum.Standby:
                                    case EquipmentStateEnum.Error:
                                        erectorConnection = true;
                                        break;
                                }

                                if (i == (int)CaseErecter.Line1)
                                    eq.ErectorConnectionState.Erector1Connection = erectorConnection;
                                else if (i == (int)CaseErecter.Line2)
                                    eq.ErectorConnectionState.Erector2Connection = erectorConnection;
                            }
                        }
                    }
                    else
                    {
                        eq.ErectorConnectionState.Erector1Connection = false;
                        eq.ErectorConnectionState.Erector2Connection = false;
                    }

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
            base.OnEquipmentStateRicpPost();

            try
            {
                if (this.IsConnected)
                {
                    for (int i = 0; i < this.CaseErecterDeviceStatus.deviceList.Count; i++)
                    {
                        var device = this.CaseErecterDeviceStatus.deviceList[i];

                        if (this.PlcCaseErectIoHandler.IoPointCaseErectEquipmentStatuses[i] != null)
                        {
                            int statusCd = this.PlcCaseErectIoHandler.IoPointCaseErectEquipmentStatuses[i].StatusCd.GetInt16();
                            device.deviceStatusCd = statusCd;

                            if (statusCd == 0 || statusCd == 9)
                                device.deviceErrorMsg = "PLC Disconnect";
                            else
                                device.deviceErrorMsg = string.Empty;
                        }
                    }
                }
                else
                {
                    foreach (var device in this.CaseErecterDeviceStatus.deviceList)
                    {
                        device.deviceStatusCd = (int)EquipmentStateEnum.Error;
                        device.deviceErrorMsg = "PLC Disconnect";
                    }
                }

                var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                restfulRequseter.DeviceStatusPostAsync(this.CaseErecterDeviceStatus);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void InitCurrentData()
        {
            try
            {
                #region BoxTypeInit
                var caseErecterCount = Enum.GetValues(typeof(CaseErecter)).Length;
                for (int i = 0; i < caseErecterCount; i++)
                {
                    CaseErectBoxTypeArgs caseErectBoxTypeArgs = new CaseErectBoxTypeArgs();
                    caseErectBoxTypeArgs.BoxType = StringHelper.TrimZeroOrWhiteSpace(this.PlcCaseErectIoHandler.IoPointCaseErectInfos[i].BoxType.GetString());

                    if ((int)CaseErecter.Line1 == i)
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.InkjectEquipment1, caseErectBoxTypeArgs);
                    else if ((int)CaseErecter.Line2 == i)
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.InkjectEquipment2, caseErectBoxTypeArgs);
                }
                #endregion

                #region CVSpeed
                for (int i = 0; i < this.PlcCaseErectIoHandler.IoPointCVSpeeds.Length; i++)
                {
                    this.CaseErectCvSpeed[i].Sv = Scale.GetDecimalConversion(this.PlcCaseErectIoHandler.IoPointCVSpeeds[i].SV.GetInt16(), 2);
                    this.CaseErectCvSpeed[i].Pv = Scale.GetDecimalConversion(this.PlcCaseErectIoHandler.IoPointCVSpeeds[i].PV.GetInt16(), 2);
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        public ConveyorCvSpeed GetConveyorCvSpeed(int conveyorSpeed)
        {
            ConveyorCvSpeed conveyorCvSpeed = new ConveyorCvSpeed();
            conveyorCvSpeed.ConveyorSpeed = conveyorSpeed;

            try
            {
                if (this.CaseErectCvSpeed.Length > conveyorSpeed && conveyorSpeed >= 0)
                {
                    var cvSpeed = this.CaseErectCvSpeed[conveyorSpeed];
                    conveyorCvSpeed.Pv = cvSpeed.Pv;
                    conveyorCvSpeed.Sv = cvSpeed.Sv;
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            return conveyorCvSpeed;
        }

        public void ConveyorCvSpeedExcute(ConveyorCvSpeed conveyorSpeed)
        {
            try
            {
                var index = conveyorSpeed.ConveyorSpeed;
                double floatingpointnumbers = conveyorSpeed.Sv * 100;
                short sv = Convert.ToInt16(floatingpointnumbers);

                if (this.PlcCaseErectIoHandler.CVSpeeds.Length > index && index >= 0)
                    this.PlcCaseErectIoHandler.CVSpeeds[index].Execute(sv);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
           
        }

        #region Interface
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            base.OnCreate();
            try
            {
                this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.PlcCaseErectLog));

                this.OnSetPlcIoHandler(this.Setting.HandlerGroupName);
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.LifeState = LifeCycleStateEnum.Created;
        }
        protected override bool OnPrepare()
        {
            if (this.LifeState != LifeCycleStateEnum.Created)
                this.Create();

            base.OnPrepare();

            try
            {
                this.OnSubscribePlcIoHandler();
                this.OnSetOwnerPlcIoHandler();
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.LifeState = LifeCycleStateEnum.Prepared;

            return true;
        }
        protected override void OnStart()
        {
            base.OnStart();

            this.InitCurrentData();
        }

        protected override void OnTerminate()
        {
            this.LifeState = LifeCycleStateEnum.Terminating;
            this.Stop();

            base.OnTerminate();

            try
            {
                this.OnUnSubscribePlcIoHandler();
                this.Communicator.Terminate();
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.LifeState = LifeCycleStateEnum.Terminated;
        }
        #endregion

        public override void OnHub_Recived(EventArgs e)
        {
            #region OnHub_Recived
            try
            {
                if (e is CaseErectBcrResultArgs caseErectBcrResultArgs)
                {
                    int bcrIndex = caseErectBcrResultArgs.BcrId - 1;
                    if (0 <= bcrIndex && bcrIndex < this.PlcCaseErectIoHandler.BcrReadingResults.Length)
                    {
                        this.PlcCaseErectIoHandler.BcrReadingResults[bcrIndex].Execute((short)caseErectBcrResultArgs.Result, caseErectBcrResultArgs.BoxId);
                        this.Communicator.Logger.Write($"BoxId = {caseErectBcrResultArgs.BoxId}, Result = {caseErectBcrResultArgs.Result}");
                    }
                }
                else if (e is CaseErectBcrAlarmArgs caseErectBcrAlarmArgs)
                {
                    int bcrIndex = caseErectBcrAlarmArgs.BcrNumber - 1;

                    if (0 <= bcrIndex && bcrIndex < this.PlcCaseErectIoHandler.BcrAlarms.Length)
                        this.PlcCaseErectIoHandler.BcrAlarms[bcrIndex].SetValue(caseErectBcrAlarmArgs.Result);
                }
                else if (e is CaseErectInkJectAlarmArgs caseErectInkJectAlarmArgs)
                {
                    int inkjectindex = caseErectInkJectAlarmArgs.InkjectNumber - 1;

                    if (0 <= inkjectindex && inkjectindex < this.PlcCaseErectIoHandler.InkjectAlarms.Length)
                        this.PlcCaseErectIoHandler.InkjectAlarms[inkjectindex].SetValue(caseErectInkJectAlarmArgs.Result);
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
            #endregion
        }
        #endregion

        #region Event Handler
        private void SV_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                #region SV_ValueChanged
                var conveyorEquipment1 = EcsServerAppManager.Instance.Equipments[HubServiceName.TouchPcConveyorEquipment1] as TouchPcConveyorEquipment;
                var conveyorEquipment2 = EcsServerAppManager.Instance.Equipments[HubServiceName.TouchPcConveyorEquipment2] as TouchPcConveyorEquipment;

                if (conveyorEquipment1 == null &&
                  conveyorEquipment2 == null) return;

                if (sender is IoPoint ioPoint)
                {
                    IoPointCVSpeed[] currentValues = e.Current as IoPointCVSpeed[];

                    for (int i = 0; i < currentValues.Length; i++)
                    {
                        this.CaseErectCvSpeed[i].Sv = Scale.GetDecimalConversion(currentValues[i].SV.GetInt16(), 2);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void PV_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                #region PV_ValueChanged

                if (sender is IoPoint ioPoint)
                {
                    IoPointCVSpeed[] currentValues = e.Current as IoPointCVSpeed[];

                    for (int i = 0; i < currentValues.Length; i++)
                    {
                        this.CaseErectCvSpeed[i].Pv = Scale.GetDecimalConversion(currentValues[i].PV.GetInt16(), 2);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void EquipmentStatusCd_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                if (sender is IoPoint ioPoint)
                {
                    int errorMsg = -1;

                    for (int i = 0; i < this.PlcCaseErectIoHandler.IoPointCaseErectEquipmentStatuses.Length; i++)
                    {
                        if (ioPoint == this.PlcCaseErectIoHandler.IoPointCaseErectEquipmentStatuses[i].StatusCd)
                        {
                            var device = this.CaseErecterDeviceStatus.deviceList[i];
                            device.deviceStatusCd = e.GetCurrentInt16();
                            errorMsg = this.PlcCaseErectIoHandler.IoPointCaseErectEquipmentStatuses[i].ErrorMsg.GetInt16();

                            //BIN값을 Message로 변환하는 기능 필요
                            device.deviceErrorMsg = errorMsg.ToString();

                            #region Touch PC Send
                            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcCaseErectEquipment>();
                            if (eq != null)
                            {
                                if (Enum.TryParse(device.deviceStatusCd.ToString(), out EquipmentStateEnum stateEnum))
                                {
                                    bool erectorConnection = false;
                                    switch (stateEnum)
                                    {
                                        case EquipmentStateEnum.Run:
                                        case EquipmentStateEnum.Standby:
                                        case EquipmentStateEnum.Error:
                                            erectorConnection = true;
                                            break;
                                    }

                                    if (i == (int)CaseErecter.Line1)
                                        eq.ErectorConnectionState.Erector1Connection = erectorConnection;
                                    else if (i == (int)CaseErecter.Line2)
                                        eq.ErectorConnectionState.Erector2Connection = erectorConnection;
                                }

                                eq.EquipmentCoonectionStateSend();

                            }
                            #endregion

                            #region RICP Send
                            var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                            restfulRequseter.DeviceStatusPostAsync(this.CaseErecterDeviceStatus);
                            #endregion
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void CaseErectBoxType_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                #region BoxType
                if (sender is IoPoint ioPoint)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (ioPoint == this.PlcCaseErectIoHandler.IoPointCaseErectInfos[i].BoxType)
                        {
                            CaseErectBoxTypeArgs caseErectBoxTypeArgs = new CaseErectBoxTypeArgs();

                            caseErectBoxTypeArgs.BoxType = StringHelper.TrimZeroOrWhiteSpace(this.PlcCaseErectIoHandler.IoPointCaseErectInfos[i].BoxType.GetString());

                            switch (i)
                            {
                                case 0:
                                    {
                                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.InkjectEquipment1, caseErectBoxTypeArgs);
                                    }
                                    break;
                                case 1:
                                    {
                                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.InkjectEquipment2, caseErectBoxTypeArgs);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void P500EMGBitEvent_EventOccurred(object sender, EventArgs e)
        {
            try
            {
                bool emgState = true;

                emgState = this.GetP500EMGState();

                if (emgState == false)
                {
                    this.Logger?.Write($"{HubServiceName.PlcCaseErectEquipment} P500 EMG State :{emgState}");
                    return;
                }

                var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                if (manager != null)
                {
                    manager.P500EmgRequested = true;
                    manager.RmsStatusSettingPostAsync(SystemCodeEnum.BOX_MOVING_RMS, InstructionEnum.SYSTEM_STOP);
                }
                   
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion
    }

    #region Define
    public enum CaseErecter
    {
        Line1 = 0,
        Line2 = 1,
    }
    #endregion
}
