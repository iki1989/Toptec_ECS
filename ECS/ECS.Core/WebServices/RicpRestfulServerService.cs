using System;
using System.Data;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using Urcis.SmartCode.Diagnostics;
using Newtonsoft.Json.Linq;
using ECS.Model;
using ECS.Model.Restfuls;
using ECS.Model.Hub;
using ECS.Core.Managers;
using ECS.Core.Equipments;
using ECS.Model.Plc;

namespace ECS.Core.WebServices
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class RicpRestfulServerService : BaseService, IRicpRestfulServerService
    {
        private bool P800DoorOpenTowerLamp { get; set; } = false;
        private bool P500DoorOpenTowerLamp { get; set; } = false;
        private bool P500EmgTowerLamp { get; set; } = false;

        public RicpRestfulServerService()
        {
            string name = "RICP Service";
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(name, EcsAppDirectory.RestfulWebServiceRicpLog));
            this.Logger.Setting.KeepingDays = EcsServerAppManager.Instance.Logger.Setting.KeepingDays;
        }

        #region 미사용
        //[return: MessageParameter(Name = "Json")]
        //public async Task<RicpResponse> ContainerMappingPostAsync(Stream body)
        //{
        //    RicpResponse response = new RicpResponse().SetBadRequset();

        //    var containerMapping = await this.ParseBody<ContainerMapping>(body);
        //    if (containerMapping == null)
        //    {
        //        this.WrtieLog("[Send]", response.ToJson());
        //        return response;
        //    }

        //    //미사용
        //    //using (DataTable dataTable = await this.GetDataTableParseBody(body))
        //    //{
        //    //    if (dataTable == null)
        //    //    {
        //    //        this.WrtieLog("[Send]", response.ToJson());
        //    //        return response;
        //    //    }

        //    //    var selectedRow = dataTable.DefaultView.ToTable(false, "containerCode").Rows[1];
        //    //    var Result = selectedRow[0];
        //    //    EcsServerAppManager.Instance.PickingBoxIdMappingList.Add(Result.ToString(), dataTable);

        //    //    using (DataTable weightInvoice = EcsServerAppManager.Instance.DataBaseManagerForServer.MappingInvoice(dataTable))
        //    //    {
        //    //        selectedRow = weightInvoice.DefaultView.ToTable(false, "BOX_ID").Rows[6];
        //    //        Result = selectedRow[0];
        //    //        EcsServerAppManager.Instance.WeightInvoiceList.Add(Result.ToString(), weightInvoice);
        //    //    }
        //    //}

        //    response.SetSuccess();
        //    this.WrtieLog("[Send]", typeof(ContainerMapping), response.ToJson());

        //    return response;
        //}
        #endregion

        [return: MessageParameter(Name = "Json")]
        public async Task<RicpResponse> LocationPointPushCallbackPostAsync(Stream body)
        {
            RicpResponse response = new RicpResponse().SetBadRequset();

            var locationPointPushCallback = await this.ParseBody<LocationPointPushCallback>(body);
            if (locationPointPushCallback == null)
            {
                this.WrtieLog("[Send]", typeof(LocationPointPushCallback), response.ToJson());
                return response;
            }

            LocationStatus button = EcsServerAppManager.Instance.Cache.GetLocationStatusPickingStationButton(locationPointPushCallback.locationPointCode);
            if (button == null)
            {
                response.errorMsg = $"{locationPointPushCallback.locationPointCode} locationPointCode is not Exist";
                this.WrtieLog("[Send]", typeof(LocationPointPushCallback), response.ToJson());
                return response;
            }

            button.WorkId = locationPointPushCallback.pushWorkId;
            button.StatusCd = PushWorkStatusCdEnum.FINISH;
            button.UpdateTime = DateTime.Now;
            EcsServerAppManager.Instance.DataBaseManagerForServer.Updatelocationstatus(button);

            var statusEnum = EcsServerAppManager.Instance.Cache.GetLocationStatusPickingStationEnum(locationPointPushCallback.locationPointCode);
            switch (statusEnum)
            {
                case LocationStatusEnum.PickingStation1_6_90:
                    {
                        var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment] as PlcPickingEquipment;
                        if (eq != null)
                            eq.ButtonsResposeSetValue(button);
                    }
                    break;
                case LocationStatusEnum.PickingStation7_13:
                    {
                        var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment] as PlcPickingEquipment;
                        if (eq != null)
                            eq.ButtonsResposeSetValue(button);
                    }
                    break;
                case LocationStatusEnum.RollTainer1_4:
                case LocationStatusEnum.RollTainer5_6:
                    break;
            }

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(LocationPointPushCallback), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<RicpResponse> RolltainerScheduleSettingPostAsync(Stream body)
        {
            RicpResponse response = new RicpResponse().SetBadRequset();

            var rolltainerScheduleSetting = await this.ParseBody<RolltainerScheduleSetting>(body);
            if (rolltainerScheduleSetting == null)
            {
                this.WrtieLog("[Send]", typeof(RolltainerScheduleSetting), response.ToJson());
                return response;
            }

            foreach (var point in rolltainerScheduleSetting.locationPointList)
            {
                LocationStatus rollTainter = EcsServerAppManager.Instance.Cache.GetLocationStatusRollTainer(point.locationPointCode);
                if (rollTainter == null)
                {
                    response.errorMsg = $"{point.locationPointCode} locationPointCode is not Exist";
                    this.WrtieLog("[Send]", typeof(LocationPointPushCallback), response.ToJson());
                    return response;
                }

                if (Enum.TryParse(point.upcomingTypeCode, out RollTainterTowerLampEnum lamp) == false)
                {
                    response.errorMsg = $"{point.upcomingTypeCode} upcomingTypeCode is not Defined";
                    this.WrtieLog("[Send]", typeof(LocationPointPushCallback), response.ToJson());
                    return response;
                }

                rollTainter.RollTainterTowerLamp = lamp;
                rollTainter.UpdateTime = DateTime.Now;
                switch (rollTainter.RollTainterTowerLamp)
                {
                    case RollTainterTowerLampEnum.A:
                        rollTainter.StatusCd = PushWorkStatusCdEnum.READY;
                        break;
                    case RollTainterTowerLampEnum.D:
                        rollTainter.StatusCd = PushWorkStatusCdEnum.ING;
                        break;
                    case RollTainterTowerLampEnum.N:
                        rollTainter.StatusCd = PushWorkStatusCdEnum.FINISH;
                        break;
                }
                EcsServerAppManager.Instance.DataBaseManagerForServer.Updatelocationstatus(rollTainter);

                var statusEnum = EcsServerAppManager.Instance.Cache.GetLocationStatusRollTainerEnum(point.locationPointCode);
                switch (statusEnum)
                {
                    case LocationStatusEnum.PickingStation1_6_90:
                    case LocationStatusEnum.PickingStation7_13:
                        break;
                    case LocationStatusEnum.RollTainer1_4:
                        {
                            var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment];
                            if (eq != null)
                            {
                                if (eq is PlcPickingEquipment pickingEq)
                                {
                                    TowerLampEnum towerLampEnum = TowerLampEnum.OFF;
                                    switch (rollTainter.RollTainterTowerLamp)
                                    {
                                        case RollTainterTowerLampEnum.A:
                                            towerLampEnum = TowerLampEnum.GREEN_On;
                                            break;
                                        case RollTainterTowerLampEnum.D:
                                            towerLampEnum = TowerLampEnum.YEllOW_On;
                                            break;
                                        case RollTainterTowerLampEnum.N:
                                            towerLampEnum = TowerLampEnum.OFF;
                                            break;
                                    }
                                    pickingEq.RolltainerTowerLamapSetValue(towerLampEnum);
                                }
                            }
                        }
                        break;
                    case LocationStatusEnum.RollTainer5_6:
                        {
                            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcWeightInvoiceEquipment>();
                            if (eq != null)
                            {
                                TowerLampEnum towerLampEnum = TowerLampEnum.OFF;
                                switch (rollTainter.RollTainterTowerLamp)
                                {
                                    case RollTainterTowerLampEnum.A:
                                        towerLampEnum = TowerLampEnum.GREEN_On;
                                        break;
                                    case RollTainterTowerLampEnum.D:
                                        towerLampEnum = TowerLampEnum.YEllOW_On;
                                        break;
                                    case RollTainterTowerLampEnum.N:
                                        towerLampEnum = TowerLampEnum.OFF;
                                        break;
                                }

                                eq.RolltainerTowerLamapSetValue(towerLampEnum);
                            }
                        }
                        break;
                }
            }

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(RolltainerScheduleSetting), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<RicpResponse> PickingResultsImportPostAsync(Stream body)
        {
            RicpResponse response = new RicpResponse().SetBadRequset();

            try
            {
                var pickingResultsImport = await this.ParseBody<PickingResultsImport>(body);

                if (pickingResultsImport == null)
                {
                    this.WrtieLog("[Send]", typeof(PickingResultsImport), response.ToJson());
                    return response;
                }

                DataTable dataTable = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertPicking(pickingResultsImport);
                if (dataTable == null)
                {
                    this.WrtieLog("[Send]", typeof(PickingResultsImport), response.ToJson());
                    return response;
                }

                EcsServerAppManager.Instance.Cache.ProductInfoLoadAsync(dataTable);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(PickingResultsImport), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<RicpResponse> RmsStatusSettingCallbackPostAsync(Stream body)
        {
            RicpResponse response = new RicpResponse().SetBadRequset();

            var rmsStatusSettingCallback = await this.ParseBody<RmsStatusSettingCallback>(body);
            if (rmsStatusSettingCallback == null)
            {
                this.WrtieLog("[Send]", typeof(RmsStatusSettingCallback), response.ToJson());
                return response;
            }

            var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
            if (manager == null)
            {
                response.errorCode = ErrorCode.InternalServerError.ToString();
                response.errorMsg = $"RestfulRequsetRicpManager is not Exist";
                this.WrtieLog("[Send]", typeof(RmsStatusSettingCallback), response.ToJson());
                return response;
            }

            var plcEqs = EcsServerAppManager.Instance.Equipments.GetPlcEquipments();

            #region SYSTEM_STOP
            if (rmsStatusSettingCallback.callbackType == $"{InstructionEnum.SYSTEM_STOP}")
            {
                if (rmsStatusSettingCallback.systemCode == $"{SystemCodeEnum.ROBOT_PICKING_RMS}")
                {
                    if (rmsStatusSettingCallback.source == $"{SourceEnum.INTERFACE}")
                    {
                        if (manager.P800DoorOpenRequseted)
                        {
                            foreach (var eq in plcEqs)
                            {
                                eq.P800DoorsOpenEnableExcute();
                                this.Logger?.Write($"{eq.Name} P800 Doors Open Enable");
                            }

                            this.PickingTowerLampSet(TowerLampEnum.RED_On);
                            this.P800DoorOpenTowerLamp = true;
                        }
                    }
                }
                else if (rmsStatusSettingCallback.systemCode == $"{SystemCodeEnum.BOX_MOVING_RMS}")
                {
                    if (rmsStatusSettingCallback.source == $"{SourceEnum.INTERFACE}")
                    {
                        if (manager.P500DoorOpenRequseted)
                        {
                            foreach (var eq in plcEqs)
                            {
                                eq.P500DoorsOpenEnableExcute();
                                this.Logger?.Write($"{eq.Name} P500 Doors Open Enable");
                            }

                            this.PickingTowerLampSet(TowerLampEnum.RED_On);
                            this.P500DoorOpenTowerLamp = true;
                        }
                        if (manager.P500EmgRequested)
                        {
                            {
                                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment];
                                if (eq != null)
                                {
                                    if (eq is PlcPickingEquipment plcPickingEq)
                                    {
                                        plcPickingEq.AgvSystemStopSetValue(AgvTypeEnum.P500);
                                        this.Logger?.Write($"{HubServiceName.PlcPicking1Equipment} P500 System Stop");
                                    }
                                }
                            }

                            {
                                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment];
                                if (eq != null)
                                {
                                    if (eq is PlcPickingEquipment plcPickingEq)
                                    {
                                        plcPickingEq.AgvSystemStopSetValue(AgvTypeEnum.P500);
                                        this.Logger?.Write($"{HubServiceName.PlcPicking2Equipment} P500 System Stop");
                                    }
                                }
                            }

                            this.PickingTowerLampSet(TowerLampEnum.RED_On);
                            this.P500EmgTowerLamp = true;
                        }
                    }
                }
            }
            #endregion

            #region SYSTEM_RECOVER
            else if (rmsStatusSettingCallback.callbackType == $"{InstructionEnum.SYSTEM_RECOVER}")
            {
                if (rmsStatusSettingCallback.systemCode == $"{SystemCodeEnum.ROBOT_PICKING_RMS}")
                {
                    if (rmsStatusSettingCallback.source == $"{SourceEnum.INTERFACE}")
                    {
                        if (manager.P800DoorOpenRequseted && manager.P800DoorCloseRequested)
                        {
                            foreach (var eq in plcEqs)
                            {
                                eq.P800DoorsCloseEnableExcute();
                                this.Logger?.Write($"{eq.Name} P800 Doors Close Enable");
                            }

                            manager.P800DoorOpenRequseted = false;
                            manager.P800DoorCloseRequested = false;

                            this.P800DoorOpenTowerLamp = false;
                        }
                    }
                }
                else if (rmsStatusSettingCallback.systemCode == $"{SystemCodeEnum.BOX_MOVING_RMS}")
                {
                    if (rmsStatusSettingCallback.source == $"{SourceEnum.INTERFACE}")
                    {
                        if (manager.P500DoorOpenRequseted && manager.P500DoorCloseRequested)
                        {
                            foreach (var eq in plcEqs)
                            {
                                eq.P500DoorsCloseEnableExcute();
                                this.Logger?.Write($"{eq.Name} P500 Doors Close Enable");
                            }

                            manager.P500DoorOpenRequseted = false;
                            manager.P500DoorCloseRequested = false;
                            this.P500DoorOpenTowerLamp = false;
                        }
                        if (manager.P500EmgRequested)
                        {
                            {
                                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment];
                                if (eq != null)
                                {
                                    if (eq is PlcPickingEquipment plcPickingEq)
                                    {
                                        plcPickingEq.AgvSystemResetSetValue(AgvTypeEnum.P500);
                                        this.Logger?.Write($"{HubServiceName.PlcPicking1Equipment} P500 System Reset");
                                    }
                                }
                            }

                            {
                                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment];
                                if (eq != null)
                                {
                                    if (eq is PlcPickingEquipment plcPickingEq)
                                    {
                                        plcPickingEq.AgvSystemResetSetValue(AgvTypeEnum.P500);
                                        this.Logger?.Write($"{HubServiceName.PlcPicking2Equipment} P500 System Reset");
                                    }
                                }
                            }

                            manager.P500EmgRequested = false;
                            this.P500EmgTowerLamp = false;
                        }
                    }
                }
            }
            #endregion

            if (this.P800DoorOpenTowerLamp == false &&
                this.P500DoorOpenTowerLamp == false &&
                this.P500EmgTowerLamp == false)
            {
                this.PickingTowerLampSet(TowerLampEnum.OFF);
            }

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(RmsStatusSettingCallback), response.ToJson());
            return response;
        }

        #region Utility
        private void PickingTowerLampSet(TowerLampEnum towerLampEnum)
        {
            {
                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment];
                if (eq != null)
                {
                    if (eq is PlcPickingEquipment plcPickingEq)
                        plcPickingEq.AllTowerLampsOnSetValue(towerLampEnum);
                }
            }

            {
                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment];
                if (eq != null)
                {
                    if (eq is PlcPickingEquipment plcPickingEq)
                        plcPickingEq.AllTowerLampsOnSetValue(towerLampEnum);
                }
            }
        }
        #endregion
    }
}
