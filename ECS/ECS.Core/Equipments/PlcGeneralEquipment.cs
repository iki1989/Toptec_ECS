using ECS.Core.Managers;
using ECS.Model;
using ECS.Model.Hub;
using ECS.Model.Pcs;
using ECS.Model.Plc;
using ECS.Model.Restfuls;
using System;
using System.Collections.Generic;
using System.IO;
using Urcis.SmartCode;
using Urcis.SmartCode.Events;
using Urcis.SmartCode.Io;
using Urcis.SmartCode.Net.Tcp;

namespace ECS.Core.Equipments
{
    public abstract class PlcGeneralEquipment : Equipment
    {
        #region Prop
        protected new IoCommunicator Communicator { get; set; }

        protected new PlcEquipmentSetting Setting
        {
            get => base.Setting as PlcEquipmentSetting;
            set => base.Setting = value;
        }

        private PlcGeneralIoHandler PlcGeneralIoHandler { get; set; } = new PlcGeneralIoHandler();

        private bool m_Connected;
        public bool IsConnected 
        { 
            get => this.m_Connected;
            set
            {
                if (this.m_Connected == value) return;

                this.m_Connected = value;

                this.OnEquipmentConnectionUpdateTouchSend();
                this.OnEquipmentStateRicpPost();
                this.OnPropertyChanged(nameof(this.IsConnected));
            }
        }
        #endregion

        #region Ctor
        public PlcGeneralEquipment(PlcEquipmentSetting setting)
        {
            this.Setting = setting ?? new PlcEquipmentSetting();

            this.Communicator = IoServer.GetCommunicator(this.Setting.CommunicatorName);
            this.Name = this.Communicator?.Name;
        }
        #endregion

        #region Method
        protected virtual void OnSetPlcIoHandler(string groupName)
        {
            try
            {
                if (this.Communicator == null)
                {
                    this.Logger.Write("Initialize Fail : Communicator is null");
                    return;
                }

                if (IoServer.GetHandlerGroup(groupName) == null)
                {
                    string msg = $"'{this.Setting.HandlerGroupName}' Handler GroupName is not found";
                    this.Logger.Write(msg);
                    return;
                }

                #region PLC Priority
                this.PlcGeneralIoHandler.HeartbeatHandler = IoServer.GetHandler<HeartbeatIoHandler>(groupName, "Heartbeat");

                string blockName = string.Empty;
                string communicatorName = this.Communicator.Name;

                blockName = "PLC BIT Event General";
                this.PlcGeneralIoHandler.FireAlarmPoint = IoServer.GetPoint(this.Communicator.Name, blockName, "Fire Alarm");
                this.PlcGeneralIoHandler.EMGAlarmPoint = IoServer.GetPoint(this.Communicator.Name, blockName, "EMG Alarm");

                this.PlcGeneralIoHandler.P800DoorsOpenRequestBitEvent = IoServer.GetHandler<BitEventIoHandler>(groupName, $"P800 Doors Open Request");
                this.PlcGeneralIoHandler.P500DoorsOpenRequestBitEvent = IoServer.GetHandler<BitEventIoHandler>(groupName, $"P500 Doors Open Request");

                this.PlcGeneralIoHandler.P800DoorsCloseRequestBitEvent = IoServer.GetHandler<BitEventIoHandler>(groupName, $"P800 Doors Close Request");
                this.PlcGeneralIoHandler.P500DoorsCloseRequestBitEvent = IoServer.GetHandler<BitEventIoHandler>(groupName, $"P500 Doors Close Request");

                this.PlcGeneralIoHandler.P800DoorsOpendStateIoPoint = IoServer.GetPoint(this.Communicator.Name, blockName, "P800 Doors Opend State");
                this.PlcGeneralIoHandler.P500DoorsOpendStateIoPoint = IoServer.GetPoint(this.Communicator.Name, blockName, "P500 Doors Opend State");
                this.PlcGeneralIoHandler.P500EMGOpendStateIoPoint = IoServer.GetPoint(this.Communicator.Name, blockName, "P500 EMG State");

                blockName = "PLC WORD General";
                this.PlcGeneralIoHandler.PlcEquipmentStatus.StatusCd = IoServer.GetPoint(this.Communicator.Name, blockName, $"PLC Equipment Status - StatusCd");
                this.PlcGeneralIoHandler.PlcEquipmentStatus.ErrorMsg = IoServer.GetPoint(this.Communicator.Name, blockName, $"PLC Equipment Status - ErrorMsg");

                #endregion

                #region PC Priority
                this.PlcGeneralIoHandler.TimeSetSyncCommander = IoServer.GetHandler<DataCommandIoHandler>(groupName, "Time Set Sync");
                this.PlcGeneralIoHandler.FireAlarmBitCommander = IoServer.GetHandler<BitCommandIoHandler>(groupName, "Fire Alarm");
                this.PlcGeneralIoHandler.FireAlarmResetBitCommander = IoServer.GetHandler<BitCommandIoHandler>(groupName, "Fire Alarm Reset");

                this.PlcGeneralIoHandler.P800DoorsOpenEnableCommander = IoServer.GetHandler<BitCommandIoHandler>(groupName, "P800 Doors Open Enable");
                this.PlcGeneralIoHandler.P500DoorsOpenEnableCommander = IoServer.GetHandler<BitCommandIoHandler>(groupName, "P500 Doors Open Enable");

                this.PlcGeneralIoHandler.P800DoorsCloseEnableCommander = IoServer.GetHandler<BitCommandIoHandler>(groupName, "P800 Doors Close Enable");
                this.PlcGeneralIoHandler.P500DoorsCloseEnableCommander = IoServer.GetHandler<BitCommandIoHandler>(groupName, "P500 Doors Close Enable");
                #endregion

            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
            }
        }

        protected virtual void OnSubscribePlcIoHandler()
        {
            try
            {
                if (this.PlcGeneralIoHandler.HeartbeatHandler != null)
                    this.PlcGeneralIoHandler.HeartbeatHandler.InputAliveChanged += this.HeartbeatHandler_InputAliveChanged;

                if (this.PlcGeneralIoHandler.FireAlarmPoint != null)
                    this.PlcGeneralIoHandler.FireAlarmPoint.ValueChanged += OnFireAlarmPoint_ValueChanged;

                if (this.PlcGeneralIoHandler.EMGAlarmPoint != null)
                    this.PlcGeneralIoHandler.EMGAlarmPoint.ValueChanged += EMGAlarmPoint_ValueChanged;

                if (this.PlcGeneralIoHandler.P800DoorsOpenRequestBitEvent != null)
                    this.PlcGeneralIoHandler.P800DoorsOpenRequestBitEvent.EventOccurred += P800DoorsOpenRequestBitEvent_EventOccurred;

                if (this.PlcGeneralIoHandler.P500DoorsOpenRequestBitEvent != null)
                    this.PlcGeneralIoHandler.P500DoorsOpenRequestBitEvent.EventOccurred += P500DoorsOpenRequestBitEvent_EventOccurred;

                if (this.PlcGeneralIoHandler.P800DoorsCloseRequestBitEvent != null)
                    this.PlcGeneralIoHandler.P800DoorsCloseRequestBitEvent.EventOccurred += P800DoorsCloseRequestBitEvent_EventOccurred;

                if (this.PlcGeneralIoHandler.P500DoorsCloseRequestBitEvent != null)
                    this.PlcGeneralIoHandler.P500DoorsCloseRequestBitEvent.EventOccurred += P500DoorsCloseRequestBitEvent_EventOccurred;

                if (this.PlcGeneralIoHandler.P500EMGOpendStateIoPoint != null)
                    this.PlcGeneralIoHandler.P500EMGOpendStateIoPoint.ValueChanged += P500EMGDisableStateIoPoint_ValueChanged;

                if (this.PlcGeneralIoHandler.PlcEquipmentStatus.StatusCd != null)
                    this.PlcGeneralIoHandler.PlcEquipmentStatus.StatusCd.ValueChanged += PlcEquipmentStatusStatusCd_ValueChanged;
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        protected virtual void OnUnSubscribePlcIoHandler()
        {
            try
            {
                if (this.PlcGeneralIoHandler.HeartbeatHandler != null)
                    this.PlcGeneralIoHandler.HeartbeatHandler.InputAliveChanged -= this.HeartbeatHandler_InputAliveChanged;

                if (this.PlcGeneralIoHandler.FireAlarmPoint != null)
                    this.PlcGeneralIoHandler.FireAlarmPoint.ValueChanged -= OnFireAlarmPoint_ValueChanged;

                if (this.PlcGeneralIoHandler.EMGAlarmPoint != null)
                    this.PlcGeneralIoHandler.EMGAlarmPoint.ValueChanged -= EMGAlarmPoint_ValueChanged;

                if (this.PlcGeneralIoHandler.P800DoorsOpenRequestBitEvent != null)
                    this.PlcGeneralIoHandler.P800DoorsOpenRequestBitEvent.EventOccurred -= P800DoorsOpenRequestBitEvent_EventOccurred;

                if (this.PlcGeneralIoHandler.P500DoorsOpenRequestBitEvent != null)
                    this.PlcGeneralIoHandler.P500DoorsOpenRequestBitEvent.EventOccurred -= P500DoorsOpenRequestBitEvent_EventOccurred;

                if (this.PlcGeneralIoHandler.P800DoorsCloseRequestBitEvent != null)
                    this.PlcGeneralIoHandler.P800DoorsCloseRequestBitEvent.EventOccurred -= P800DoorsCloseRequestBitEvent_EventOccurred;

                if (this.PlcGeneralIoHandler.P500DoorsCloseRequestBitEvent != null)
                    this.PlcGeneralIoHandler.P500DoorsCloseRequestBitEvent.EventOccurred -= P500DoorsCloseRequestBitEvent_EventOccurred;

                if (this.PlcGeneralIoHandler.P500EMGOpendStateIoPoint != null)
                    this.PlcGeneralIoHandler.P500EMGOpendStateIoPoint.ValueChanged -= P500EMGDisableStateIoPoint_ValueChanged;

                if (this.PlcGeneralIoHandler.PlcEquipmentStatus.StatusCd != null)
                    this.PlcGeneralIoHandler.PlcEquipmentStatus.StatusCd.ValueChanged -= PlcEquipmentStatusStatusCd_ValueChanged;
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        protected virtual void OnSetOwnerPlcIoHandler()
        {
            #region PLC Priority
            if (this.PlcGeneralIoHandler.HeartbeatHandler != null)
                this.PlcGeneralIoHandler.HeartbeatHandler.Owner = this;

            if (this.PlcGeneralIoHandler.P800DoorsOpenRequestBitEvent != null)
                this.PlcGeneralIoHandler.P800DoorsOpenRequestBitEvent.Owner = this;

            if (this.PlcGeneralIoHandler.P500DoorsOpenRequestBitEvent != null)
                this.PlcGeneralIoHandler.P500DoorsOpenRequestBitEvent.Owner = this;

            if (this.PlcGeneralIoHandler.P800DoorsCloseRequestBitEvent != null)
                this.PlcGeneralIoHandler.P800DoorsCloseRequestBitEvent.Owner = this;

            if (this.PlcGeneralIoHandler.P500DoorsCloseRequestBitEvent != null)
                this.PlcGeneralIoHandler.P500DoorsCloseRequestBitEvent.Owner = this;
            #endregion

            #region PC Priority
            if (this.PlcGeneralIoHandler.TimeSetSyncCommander != null)
                this.PlcGeneralIoHandler.TimeSetSyncCommander.Owner = this;

            if (this.PlcGeneralIoHandler.FireAlarmBitCommander != null)
                this.PlcGeneralIoHandler.FireAlarmBitCommander.Owner = this;

            if (this.PlcGeneralIoHandler.FireAlarmResetBitCommander != null)
                this.PlcGeneralIoHandler.FireAlarmResetBitCommander.Owner = this;

            if (this.PlcGeneralIoHandler.P800DoorsOpenEnableCommander != null)
                this.PlcGeneralIoHandler.P800DoorsOpenEnableCommander.Owner = this;

            if (this.PlcGeneralIoHandler.P500DoorsOpenEnableCommander != null)
                this.PlcGeneralIoHandler.P500DoorsOpenEnableCommander.Owner = this;

            if (this.PlcGeneralIoHandler.P800DoorsCloseEnableCommander != null)
                this.PlcGeneralIoHandler.P800DoorsCloseEnableCommander.Owner = this;

            if (this.PlcGeneralIoHandler.P500DoorsCloseEnableCommander != null)
                this.PlcGeneralIoHandler.P500DoorsCloseEnableCommander.Owner = this;
            #endregion
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (this.PlcGeneralIoHandler.HeartbeatHandler != null)
            {
                if (this.PlcGeneralIoHandler.HeartbeatHandler.InputAlive)
                    this.HeartbeatHandler_InputAliveChanged(this, new ValueChangedEventArgs<bool>(false, true));
            }
        }

        public void FireAlarmExcute(bool isOn)
        {
            this.PlcGeneralIoHandler.FireAlarmBitCommander.Execute(isOn);
        }

        public void FireAlarmResetExcute(bool isOn)
        {
            this.PlcGeneralIoHandler.FireAlarmResetBitCommander.Execute(isOn);
        }

        public bool GetP800DoorsOpendState()
        {
            if (this.PlcGeneralIoHandler.P800DoorsOpendStateIoPoint == null)
                return false;
            else
                return this.PlcGeneralIoHandler.P800DoorsOpendStateIoPoint.GetBoolean();
        }

        public bool GetP500DoorsOpendState()
        {
            if (this.PlcGeneralIoHandler.P500DoorsOpendStateIoPoint == null)
                return false;
            else
                return this.PlcGeneralIoHandler.P500DoorsOpendStateIoPoint.GetBoolean();
        }
        public bool GetP500EMGState()
        {
            if (this.PlcGeneralIoHandler.P500EMGOpendStateIoPoint == null)
                return false;
            else
                return this.PlcGeneralIoHandler.P500EMGOpendStateIoPoint.GetBoolean();
        }
        public void P500DoorsOpenEnableExcute()
        {
            if (this.PlcGeneralIoHandler.P500DoorsOpenEnableCommander != null)
                this.PlcGeneralIoHandler.P500DoorsOpenEnableCommander.Execute();
        }

        public void P800DoorsOpenEnableExcute()
        {
            if (this.PlcGeneralIoHandler.P800DoorsOpenEnableCommander != null)
                this.PlcGeneralIoHandler.P800DoorsOpenEnableCommander.Execute();
        }
        public void P500DoorsCloseEnableExcute()
        {
            if (this.PlcGeneralIoHandler.P500DoorsCloseEnableCommander != null)
                this.PlcGeneralIoHandler.P500DoorsCloseEnableCommander.Execute();
        }
        public void P800DoorsCloseEnableExcute()
        {
            if (this.PlcGeneralIoHandler.P800DoorsCloseEnableCommander != null)
                this.PlcGeneralIoHandler.P800DoorsCloseEnableCommander.Execute();
        }
      
        #endregion

        #region Event Handler
        protected void HeartbeatHandler_InputAliveChanged(object sender, ValueChangedEventArgs<bool> e)
        {
            if (this.IsConnected == e.Current) return;
            
            this.IsConnected = e.Current;
            if (this.IsConnected)
                this.PlcGeneralIoHandler.TimeSetSyncCommander.Execute(DateTime.Now.ToString("yyyyMMddHHmmss"));
         }

        protected virtual void OnFireAlarmPoint_ValueChanged(object sender, IoPointValueChangedEventArgs e) { }
        private void EMGAlarmPoint_ValueChanged(object sender, IoPointValueChangedEventArgs e) 
        {
            try
            {
                if (e.Current is bool isOn)
                {
                    // PLC협의 EMG Bit 받고 상위에 보고 안하기로 로봇하고는 관계없음
                    //var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                    //if (manager != null)
                    //{
                    //    if (isOn)
                    //    {
                    //        manager.RmsStatusSettingPostAsync(SystemCodeEnum.ROBOT_PICKING_RMS, InstructionEnum.SYSTEM_STOP);
                    //        manager.RmsStatusSettingPostAsync(SystemCodeEnum.BOX_MOVING_RMS, InstructionEnum.SYSTEM_STOP);
                    //        manager.RmsStatusSettingPostAsync(SystemCodeEnum.ROLLTAINER_MOVING_RMS, InstructionEnum.SYSTEM_STOP);
                    //    }
                    //    else
                    //    {
                    //        manager.RmsStatusSettingPostAsync(SystemCodeEnum.ROBOT_PICKING_RMS, InstructionEnum.SYSTEM_RECOVER);
                    //        manager.RmsStatusSettingPostAsync(SystemCodeEnum.BOX_MOVING_RMS, InstructionEnum.SYSTEM_RECOVER);
                    //        manager.RmsStatusSettingPostAsync(SystemCodeEnum.ROLLTAINER_MOVING_RMS, InstructionEnum.SYSTEM_RECOVER);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void P800DoorsOpenRequestBitEvent_EventOccurred(object sender, EventArgs e)
        {
            bool opendState = false;

            // P800은 PLC1, 3번만 사용하고 RICP에서 뿌려줄때는  PLC4개한테 뿌려 주기로 결정
            {
                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment];
                if (eq != null)
                {
                    if (eq is PlcPickingEquipment pickingEq)
                    {
                        opendState = pickingEq.GetP800DoorsOpendState();
                        if (opendState)
                        {
                            this.Logger?.Write($"{HubServiceName.PlcPicking1Equipment} P800 Doors Opend State :{opendState}");
                            return;
                        }
                    }
                }
            }
          
            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcCaseErectEquipment>();
                if (eq != null)
                {
                    opendState = eq.GetP800DoorsOpendState();
                    if (opendState)
                    {
                        this.Logger?.Write($"{HubServiceName.PlcCaseErectEquipment} P800 Doors Opend State :{opendState}");
                        return;
                    }
                }
            }

            var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
            if (manager != null)
            {
                manager.P800DoorOpenRequseted = true;
                manager.RmsStatusSettingPostAsync(SystemCodeEnum.ROBOT_PICKING_RMS, InstructionEnum.SYSTEM_STOP);
            }

        }

        private void P500DoorsOpenRequestBitEvent_EventOccurred(object sender, EventArgs e)
        {
            bool opendState = false;

            {
                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment];
                if (eq != null)
                {
                    if (eq is PlcPickingEquipment pickingEq)
                    {
                        opendState = pickingEq.GetP500DoorsOpendState();
                        if (opendState)
                        {
                            this.Logger?.Write($"{HubServiceName.PlcPicking1Equipment} P500 Doors Opend State :{opendState}");
                            return;
                        }
                    }
                }
            }

            {
                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment];
                if (eq != null)
                {
                    if (eq is PlcPickingEquipment pickingEq)
                    {
                        opendState = pickingEq.GetP500DoorsOpendState();
                        if (opendState)
                        {
                            this.Logger?.Write($"{HubServiceName.PlcPicking2Equipment} P500 Doors Opend State :{opendState}");
                            return;
                        }
                    }
                }
            }

            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcCaseErectEquipment>();
                if (eq != null)
                {
                    opendState = eq.GetP500DoorsOpendState();
                    if (opendState)
                    {
                        this.Logger?.Write($"{HubServiceName.PlcCaseErectEquipment} P500 Doors Opend State :{opendState}");
                        return;
                    }
                }
            }

            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                if (eq != null)
                {
                    opendState = eq.GetP500DoorsOpendState();
                    if (opendState)
                    {
                        this.Logger?.Write($"{HubServiceName.PlcCaseErectEquipment} P500 Doors Opend State :{opendState}");
                        return;
                    }
                }
            }

            var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
            if (manager != null)
            {
                manager.P500DoorOpenRequseted = true;
                manager.RmsStatusSettingPostAsync(SystemCodeEnum.BOX_MOVING_RMS, InstructionEnum.SYSTEM_STOP);
            }    
        }

        private void P800DoorsCloseRequestBitEvent_EventOccurred(object sender, EventArgs e)
        {
            bool opendState = true;

            {
                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment];
                if (eq != null)
                {
                    if (eq is PlcPickingEquipment pickingEq)
                    {
                        opendState = pickingEq.GetP800DoorsOpendState();
                        if (opendState == false)
                        {
                            this.Logger?.Write($"{HubServiceName.PlcPicking1Equipment} P800 Doors Opend State :{opendState}");
                            return;
                        }
                    }
                }
            }

            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcCaseErectEquipment>();
                if (eq != null)
                {
                    opendState = eq.GetP800DoorsOpendState();
                    if (opendState == false)
                    {
                        this.Logger?.Write($"{HubServiceName.PlcCaseErectEquipment} P800 Doors Opend State :{opendState}");
                        return;
                    }
                }
            }

            var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
            if (manager != null)
            {
                manager.P800DoorCloseRequested = true;
                manager.RmsStatusSettingPostAsync(SystemCodeEnum.ROBOT_PICKING_RMS, InstructionEnum.SYSTEM_RECOVER);
            }
               
        }

        private void P500DoorsCloseRequestBitEvent_EventOccurred(object sender, EventArgs e)
        {
            bool opendState = true;

            {
                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment];
                if (eq != null)
                {
                    if (eq is PlcPickingEquipment pickingEq)
                    {
                        opendState = pickingEq.GetP500DoorsOpendState();
                        if (opendState == false)
                        {
                            this.Logger?.Write($"{HubServiceName.PlcPicking1Equipment} P500 Doors Opend State :{opendState}");
                            return;
                        }
                    }
                }
            }

            {
                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment];
                if (eq != null)
                {
                    if (eq is PlcPickingEquipment pickingEq)
                    {
                        opendState = pickingEq.GetP500DoorsOpendState();
                        if (opendState == false)
                        {
                            this.Logger?.Write($"{HubServiceName.PlcPicking2Equipment} P500 Doors Opend State :{opendState}");
                            return;
                        }
                    }
                }
            }

            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcCaseErectEquipment>();
                if (eq != null)
                {
                    opendState = eq.GetP500DoorsOpendState();
                    if (opendState == false)
                    {
                        this.Logger?.Write($"{HubServiceName.PlcCaseErectEquipment} P500 Doors Opend State :{opendState}");
                        return;
                    }
                }
            }

            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                if (eq != null)
                {
                    opendState = eq.GetP500DoorsOpendState();
                    if (opendState == false)
                    {
                        this.Logger?.Write($"{HubServiceName.PlcCaseErectEquipment} P500 Doors Opend State :{opendState}");
                        return;
                    }
                }
            }

            var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
            if (manager != null)
            {
                manager.P500DoorCloseRequested = true;
                manager.RmsStatusSettingPostAsync(SystemCodeEnum.BOX_MOVING_RMS, InstructionEnum.SYSTEM_RECOVER);
            }
                
        }

        private void P500EMGDisableStateIoPoint_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            if (e.Current is bool c)
            {
                if (c == false)
                {
                    bool disableState = false;

                    {
                        var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcCaseErectEquipment>();
                        if (eq != null)
                        {
                            disableState = eq.GetP500EMGState();
                            if (disableState)
                            {
                                this.Logger?.Write($"{HubServiceName.PlcCaseErectEquipment} P500 EMG State :{disableState}");
                                return;
                            }
                        }
                    }

                    {
                        var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                        if (eq != null)
                        {
                            disableState = eq.GetP500EMGState();
                            if (disableState)
                            {
                                this.Logger?.Write($"{HubServiceName.PlcCaseErectEquipment} P500 EMG State :{disableState}");
                                return;
                            }
                        }
                    }

              
                    var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                    if (manager != null)
                        manager.RmsStatusSettingPostAsync(SystemCodeEnum.BOX_MOVING_RMS, InstructionEnum.SYSTEM_RECOVER);
                }
            }
        }

        protected virtual void PlcEquipmentStatusStatusCd_ValueChanged(object sender, IoPointValueChangedEventArgs e) { }
        #endregion
    }

    [Serializable]
    public class PlcEquipmentSetting : EquipmentSetting
    {
        public string CommunicatorName { get; set; }

        public string HandlerGroupName { get; set; }
    }
}
