using ECS.Core.Managers;
using ECS.Model;
using ECS.Model.Hub;
using ECS.Model.Plc;
using ECS.Model.Restfuls;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Io;

namespace ECS.Core.Equipments
{
    public class PlcPickingEquipment : PlcGeneralEquipment
    {
        #region Field
        private Timer TowerLampOffTimeoutTimer = new Timer(60 * 1000); //1m
        #endregion

        #region Prop
        private PlcPickingIoHandler PlcPickingIoHandler { get; set; } = new PlcPickingIoHandler();
        #endregion

        #region Ctor
        public PlcPickingEquipment(PlcEquipmentSetting setting) : base(setting) { }
        #endregion

        #region Method
        protected override void OnSetPlcIoHandler(string groupName)
        {
            try
            {
                base.OnSetPlcIoHandler(groupName);

                string communicatorName = this.Communicator.Name;
                string blockName = string.Empty;

                if (this.Name == HubServiceName.PlcPicking1Equipment)
                {
                    #region PLC
                    for (int i = 0; i < this.PlcPickingIoHandler.BitEventStations.Length; i++)
                    {
                        if (i == 6)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                this.PlcPickingIoHandler.BitEventStations[i].InputButtonOns[j] = IoServer.GetHandler<BitEventIoHandler>(groupName, $"Station#90 - Input Button On#{j + 1}");
                                this.PlcPickingIoHandler.BitEventStations[i].OutputButtonOns[j] = IoServer.GetHandler<BitEventIoHandler>(groupName, $"Station#90 - Output Button On#{j + 1}");
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                this.PlcPickingIoHandler.BitEventStations[i].InputButtonOns[j] = IoServer.GetHandler<BitEventIoHandler>(groupName, $"Station#{i + 1} - Input Button On#{j + 1}");
                                this.PlcPickingIoHandler.BitEventStations[i].OutputButtonOns[j] = IoServer.GetHandler<BitEventIoHandler>(groupName, $"Station#{i + 1} - Output Button On#{j + 1}");
                            }
                        }
                    }
                    #endregion

                    #region PC
                    this.PlcPickingIoHandler.P500SystemStopRequestBitCommander = IoServer.GetHandler<BitCommandIoHandler>(groupName, "P500 System Stop Request");
                    this.PlcPickingIoHandler.P500SystemResetRequestBitCommander = IoServer.GetHandler<BitCommandIoHandler>(groupName, "P500 System Reset Request");

                    for (int i = 0; i < this.PlcPickingIoHandler.IoPointStationsButtonsRespose.Length; i++)
                    {
                        blockName = "SERVER WORD";
                        if (i == 6)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                this.PlcPickingIoHandler.IoPointStationsButtonsRespose[i].InputButtonResponse[j] = IoServer.GetPoint(communicatorName, blockName, $"Station#90 - Input Button Response#{j + 1}");
                                this.PlcPickingIoHandler.IoPointStationsButtonsRespose[i].OutputButtonResponse[j] = IoServer.GetPoint(communicatorName, blockName, $"Station#90 - Output Button Response#{j + 1}");
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                this.PlcPickingIoHandler.IoPointStationsButtonsRespose[i].InputButtonResponse[j] = IoServer.GetPoint(communicatorName, blockName, $"Station#{i + 1} - Input Button Response#{j + 1}");
                                this.PlcPickingIoHandler.IoPointStationsButtonsRespose[i].OutputButtonResponse[j] = IoServer.GetPoint(communicatorName, blockName, $"Station#{i + 1} - Output Button Response#{j + 1}");
                            }
                        }
                    }
                   
                    for (int i = 0; i < this.PlcPickingIoHandler.TowerLampIoPoints.Length; i++)
                    {
                        if (i == 6)
                            this.PlcPickingIoHandler.TowerLampIoPoints[i] = IoServer.GetPoint(communicatorName, blockName, $"Station#90 - Tower Lamp On");
                        else
                            this.PlcPickingIoHandler.TowerLampIoPoints[i] = IoServer.GetPoint(communicatorName, blockName, $"Station#{i + 1} - Tower Lamp On");
                    }
                }
                else if (this.Name == HubServiceName.PlcPicking2Equipment)
                {
                    #region PLC
                    for (int i = 0; i < this.PlcPickingIoHandler.BitEventStations.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            this.PlcPickingIoHandler.BitEventStations[i].InputButtonOns[j] = IoServer.GetHandler<BitEventIoHandler>(groupName, $"Station#{i + 7} - Input Button On#{j + 1}");
                            this.PlcPickingIoHandler.BitEventStations[i].OutputButtonOns[j] = IoServer.GetHandler<BitEventIoHandler>(groupName, $"Station#{i + 7} - Output Button On#{j + 1}");
                        }
                    }

                    blockName = "PLC BIT Event";
                    for (int i = 0; i < PlcPickingIoHandler.IoPointRollTainers.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                            this.PlcPickingIoHandler.IoPointRollTainers[i].Sensors[j] = IoServer.GetPoint(communicatorName, blockName, $"RollTainer#{i + 1} - Sensor#{j + 1}");
                    }
                    #endregion

                    #region PC
                    this.PlcPickingIoHandler.P500SystemStopRequestBitCommander = IoServer.GetHandler<BitCommandIoHandler>(groupName, "P500 System Stop Request");
                    this.PlcPickingIoHandler.P500SystemResetRequestBitCommander = IoServer.GetHandler<BitCommandIoHandler>(groupName, "P500 System Reset Request");

                    blockName = "SERVER WORD";
                    for (int i = 0; i < this.PlcPickingIoHandler.IoPointStationsButtonsRespose.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            this.PlcPickingIoHandler.IoPointStationsButtonsRespose[i].InputButtonResponse[j] = IoServer.GetPoint(communicatorName, blockName, $"Station#{i + 7} - Input Button Response#{j + 1}");
                            this.PlcPickingIoHandler.IoPointStationsButtonsRespose[i].OutputButtonResponse[j] = IoServer.GetPoint(communicatorName, blockName, $"Station#{i + 7} - Output Button Response#{j + 1}");
                        }
                    }

                    for (int i = 0; i < this.PlcPickingIoHandler.TowerLampIoPoints.Length; i++)
                        this.PlcPickingIoHandler.TowerLampIoPoints[i] = IoServer.GetPoint(communicatorName, blockName, $"Station#{i + 7} - Tower Lamp On");

                    //RollTainer #1 ~ #4
                    for (int i = 0; i < this.PlcPickingIoHandler.RollTainersTowerLampIoPoints.Length; i++)
                        this.PlcPickingIoHandler.RollTainersTowerLampIoPoints[i] = IoServer.GetPoint(communicatorName, blockName, $"RollTainer#{i + 1} - Tower Lamp On");
                    #endregion
                }
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

                for (int i = 0; i < this.PlcPickingIoHandler.BitEventStations.Length; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (this.PlcPickingIoHandler.BitEventStations[i].InputButtonOns[j] != null)
                            this.PlcPickingIoHandler.BitEventStations[i].InputButtonOns[j].EventOccurred += PlcPickingEquipment_EventOccurred;

                        if (this.PlcPickingIoHandler.BitEventStations[i].OutputButtonOns[j] != null)
                            this.PlcPickingIoHandler.BitEventStations[i].OutputButtonOns[j].EventOccurred += PlcPickingEquipment_EventOccurred;
                    }
                }

                if (this.Name == HubServiceName.PlcPicking2Equipment)
                {
                    for (int i = 0; i < this.PlcPickingIoHandler.IoPointRollTainers.Length; i++)
                    {
                        for (int j = 0; j < this.PlcPickingIoHandler.IoPointRollTainers[i].Sensors.Length; j++)
                        {
                            if (this.PlcPickingIoHandler.IoPointRollTainers[i].Sensors[j] != null)
                                this.PlcPickingIoHandler.IoPointRollTainers[i].Sensors[j].ValueChanged += RollTainersSensers_ValueChanged;
                        }
                    }
                }
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

                for (int i = 0; i < this.PlcPickingIoHandler.BitEventStations.Length; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (this.PlcPickingIoHandler.BitEventStations[i].InputButtonOns[j] != null)
                            this.PlcPickingIoHandler.BitEventStations[i].InputButtonOns[j].EventOccurred -= PlcPickingEquipment_EventOccurred;

                        if (this.PlcPickingIoHandler.BitEventStations[i].OutputButtonOns[j] != null)
                            this.PlcPickingIoHandler.BitEventStations[i].OutputButtonOns[j].EventOccurred -= PlcPickingEquipment_EventOccurred;
                    }
                }

                if (this.Name == HubServiceName.PlcPicking2Equipment)
                {
                    for (int i = 0; i < this.PlcPickingIoHandler.IoPointRollTainers.Length; i++)
                    {
                        for (int j = 0; j < this.PlcPickingIoHandler.IoPointRollTainers[i].Sensors.Length; j++)
                        {
                            if (this.PlcPickingIoHandler.IoPointRollTainers[i].Sensors[j] != null)
                                this.PlcPickingIoHandler.IoPointRollTainers[i].Sensors[j].ValueChanged -= RollTainersSensers_ValueChanged;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
            }
        }

        protected override void OnSetOwnerPlcIoHandler()
        {
            try
            {
                base.OnSetOwnerPlcIoHandler();

                if (this.PlcPickingIoHandler.P500SystemStopRequestBitCommander != null)
                    this.PlcPickingIoHandler.P500SystemStopRequestBitCommander.Owner = this;

                if (this.PlcPickingIoHandler.P500SystemResetRequestBitCommander != null)
                    this.PlcPickingIoHandler.P500SystemResetRequestBitCommander.Owner = this;

                for (int i = 0; i < this.PlcPickingIoHandler.BitEventStations.Length; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (this.PlcPickingIoHandler.BitEventStations[i].InputButtonOns[j] != null)
                            this.PlcPickingIoHandler.BitEventStations[i].InputButtonOns[j].Owner = this;

                        if (this.PlcPickingIoHandler.BitEventStations[i].OutputButtonOns[j] != null)
                            this.PlcPickingIoHandler.BitEventStations[i].OutputButtonOns[j].Owner = this;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
            }
        }

        public void ButtonsResposeSetValue(LocationStatus buttonStatus)
        {
            for (int i = 0; i < this.PlcPickingIoHandler.BitEventStations.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (this.PlcPickingIoHandler.BitEventStations[i]?.InputButtonOns[j]?.Name == buttonStatus.IoName)
                    {
                        this.PlcPickingIoHandler.IoPointStationsButtonsRespose[i]?.InputButtonResponse[j]?.SetValue((short)buttonStatus.StatusCd);
                        break;
                    }
                    else if (this.PlcPickingIoHandler.BitEventStations[i]?.OutputButtonOns[j]?.Name == buttonStatus.IoName)
                    {
                        this.PlcPickingIoHandler.IoPointStationsButtonsRespose[i]?.OutputButtonResponse[j]?.SetValue((short)buttonStatus.StatusCd);
                        break;
                    }
                }
            }
        }

        public void AllTowerLampsOnSetValue(TowerLampEnum towerLampEnum)
        {
            foreach (var towerLamp in this.PlcPickingIoHandler.TowerLampIoPoints)
            {
                this.TowerLampsOnSetValue(towerLamp, towerLampEnum);
            }
        }

        private void TowerLampsOnSetValue(IoPoint towerLampOnIoPoint, TowerLampEnum towerLampEnum)
        {
            if (towerLampOnIoPoint == null) return;

            towerLampOnIoPoint.SetValue((short)towerLampEnum);
        }

        public void AgvSystemStopSetValue(AgvTypeEnum AgvTypeEnum)
        {
            switch (AgvTypeEnum)
            {
                case AgvTypeEnum.P800:
                    break;
                case AgvTypeEnum.P500:
                    if (this.PlcPickingIoHandler.P500SystemStopRequestBitCommander != null)
                        this.PlcPickingIoHandler.P500SystemStopRequestBitCommander.Execute();
                    break;
            }
        }

        public void AgvSystemResetSetValue(AgvTypeEnum agvTypeEnum)
        {
            switch (agvTypeEnum)
            {
                case AgvTypeEnum.P500:
                    if (this.PlcPickingIoHandler.P500SystemResetRequestBitCommander != null)
                        this.PlcPickingIoHandler.P500SystemResetRequestBitCommander.Execute();
                    break;
            }
        }

        public void RolltainerTowerLamapSetValue(TowerLampEnum towerLampEnum)
        {
            try
            {
                if (this.Name == HubServiceName.PlcPicking2Equipment)
                {
                    // PLC 담당자가 롤테이너 타워램프가 한개라서 한군데에 내려달라고 요청
                    this.PlcPickingIoHandler.RollTainersTowerLampIoPoints[0]?.SetValue((short)towerLampEnum);

                    //for (int i = 0; i < this.PlcCaseErectIoHandler.RollTainersTowerLampOn.Length; i++)
                    //{
                    //    if (this.PlcCaseErectIoHandler.RollTainersTowerLampOn[i]?.Name == locationStatus.IoName)
                    //    {
                    //        this.PlcCaseErectIoHandler.RollTainersTowerLampOn[i]?.SetValue((short)locationStatus.RollTainterTowerLamp);
                    //        break;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion
        #region Interface
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            base.OnCreate();
            try
            {
                if (this.Name == HubServiceName.PlcPicking1Equipment)
                    this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.PlcPicking1Log));
                else if (this.Name == HubServiceName.PlcPicking2Equipment)
                    this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.PlcPicking2Log));

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
                this.OnSetOwnerPlcIoHandler();
                this.OnSubscribePlcIoHandler();
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.TowerLampOffTimeoutTimer.Elapsed += TowerLampOffTimeoutTimer_Elapsed;

            this.LifeState = LifeCycleStateEnum.Prepared;

            return true;
        }

        protected override void OnTerminate()
        {
            this.LifeState = LifeCycleStateEnum.Terminating;
            this.Stop();

            if (this.TowerLampOffTimeoutTimer != null)
            {
                this.TowerLampOffTimeoutTimer.Elapsed -= TowerLampOffTimeoutTimer_Elapsed;
                this.TowerLampOffTimeoutTimer.Dispose();
            }

            base.OnTerminate();

            try
            {
                this.OnUnSubscribePlcIoHandler();
                this.Communicator.Terminate();
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.LifeState = LifeCycleStateEnum.Terminated;
        }

        protected override void OnStart()
        {
            base.OnStart();
            
            foreach (var towerLamp in this.PlcPickingIoHandler.TowerLampIoPoints)
            {
                if (towerLamp != null)
                {
                    if (towerLamp.GetInt16() == (short)TowerLampEnum.YEllOW_On)
                        this.TowerLampsOnSetValue(towerLamp, TowerLampEnum.OFF);
                }
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            this.TowerLampOffTimeoutTimer?.Stop();
        }
        #endregion
       
        #endregion

        #region Event Handler
        private void PlcPickingEquipment_EventOccurred(object sender, EventArgs e)
        {
            if (sender is BitEventIoHandler ioHandler)
            {
                var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                if (manager == null) return;

                for (int i = 0; i < this.PlcPickingIoHandler.BitEventStations.Length; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (this.PlcPickingIoHandler.BitEventStations[i].InputButtonOns[j] == ioHandler)
                        {
                            if (ioHandler.Name.Contains("Station#90"))
                            {
                                this.AllTowerLampsOnSetValue(TowerLampEnum.YEllOW_On);

                                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment] as PlcPickingEquipment;
                                if (eq != null)
                                    eq.AllTowerLampsOnSetValue(TowerLampEnum.YEllOW_On);

                                this.TowerLampOffTimeoutTimer.Start();
                            }
                            else
                                manager.LocationPointPushPostAsync(ioHandler.Name);

                            break;
                        }
                        else if (this.PlcPickingIoHandler.BitEventStations[i].OutputButtonOns[j] == ioHandler)
                        {
                            manager.LocationPointPushPostAsync(ioHandler.Name);
                            break;
                        }
                    }
                }
            }
        }

        private void TowerLampOffTimeoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.AllTowerLampsOnSetValue(TowerLampEnum.OFF);

            var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment] as PlcPickingEquipment;
            if (eq != null)
                eq.AllTowerLampsOnSetValue(TowerLampEnum.OFF);

            this.TowerLampOffTimeoutTimer.Stop();
        }

        private void RollTainersSensers_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                if (sender is IoPoint ioPoint)
                {
                    var rollTainers = this.PlcPickingIoHandler.IoPointRollTainers;

                    for (int i = 0; i < rollTainers.Length; i++)
                    {
                        for (int j = 0; j < rollTainers[i].Sensors.Length; j++)
                        {
                            if (ioPoint == rollTainers[i].Sensors[j])
                            {
                                bool[] senssorsOn = new bool[rollTainers[i].Sensors.Length];
                                for (int index = 0; index < senssorsOn.Length; index++)
                                    senssorsOn[index] = rollTainers[i].Sensors[index].GetBoolean();

                                var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                                if (manager != null)
                                {
                                    Regex regex = new Regex(@"RollTainer#[0-9]{1}");
                                    if (regex.IsMatch(ioPoint.Name))
                                    {
                                        Match match = regex.Match(ioPoint.Name);
                                        string name = match.Groups[0].ToString();
                                        if (senssorsOn.All(x => x))
                                            manager.RolltainerSensingStatusPostAsync(name, sensorStatusEnum.ON);
                                        else if (senssorsOn.All(x => x == false))
                                            manager.RolltainerSensingStatusPostAsync(name, sensorStatusEnum.OFF);
                                        else
                                        {
                                            foreach (var station in EcsServerAppManager.Instance.Cache.LocationStatusPikcingStations)
                                            {
                                                LocationStatus status = manager.FindlocationStatusByIoName(station, name);
                                                if (status != null)
                                                {
                                                    if (status.RollTainterTowerLamp != RollTainterTowerLampEnum.N)
                                                        this.RolltainerTowerLamapSetValue(TowerLampEnum.RED_On);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion
    }
}

