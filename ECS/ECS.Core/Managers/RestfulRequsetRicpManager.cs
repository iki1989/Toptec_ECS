using ECS.Core.Equipments;
using ECS.Core.Restful;
using ECS.Core.Util;
using ECS.Model;
using ECS.Model.Databases;
using ECS.Model.Domain.Touch;
using ECS.Model.Hub;
using ECS.Model.Inkject;
using ECS.Model.Plc;
using ECS.Model.Restfuls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Urcis.SmartCode;

namespace ECS.Core.Managers
{
    public class RestfulRequsetRicpManager : RestfulRequsetManager
    {
        #region Field
        private RestfulRicpRequester DeviceStatusRequester;
        private RestfulRicpRequester ContainerCodeRequester;
        private RestfulRicpRequester WeightContainerScanRequester;
        private RestfulRicpRequester WeightResultRequester;
        private RestfulRicpRequester InvoiceScanRequester;
        private RestfulRicpRequester OutInvoiceScanRequester;

        private RestfulRicpRequester LocationPointPushRequester;
        private RestfulRicpRequester LocationPointStatusRequester;

        private RestfulRicpRequester RolltainerSensingStatusRequester;

        private RestfulRicpRequester RmsStatusSettingRequester;

        private RestfulRicpRequester PackageResultRequester;
        #endregion

        #region Prop
        public new RestfulRequsetRicpManagerSetting Setting
        {
            get => base.Setting as RestfulRequsetRicpManagerSetting;
            set => base.Setting = value;
        }

        public bool P500DoorOpenRequseted { get; set; }
        public bool P500DoorCloseRequested { get; set; }
        public bool P500EmgRequested { get; set; }

        public bool P800DoorOpenRequseted { get; set; }
        public bool P800DoorCloseRequested { get; set; }
        #endregion

        #region Ctor
        public RestfulRequsetRicpManager(RestfulRequsetRicpManagerSetting setting)
        {
            this.Setting = setting ?? new RestfulRequsetRicpManagerSetting();
            this.Name = HubServiceName.RicpPost;

            this.ContainerCodeRequester = new RestfulRicpRequester(setting.DomainName, setting.ContainerScan);
            this.WeightContainerScanRequester = new RestfulRicpRequester(setting.DomainName, setting.WeightContainerScan);
            this.WeightResultRequester = new RestfulRicpRequester(setting.DomainName, setting.WeightResult);
            this.InvoiceScanRequester = new RestfulRicpRequester(setting.DomainName, setting.InvoiceScan);
            this.OutInvoiceScanRequester = new RestfulRicpRequester(setting.DomainName, setting.OutinvoiceScan);

            string logName = string.Empty;
            logName = "RICP DeviceStatus";
            this.DeviceStatusRequester = new RestfulRicpRequester(setting.DomainName, setting.DeviceStatus, logName);

            logName = "RICP LocationPoint";
            this.LocationPointPushRequester = new RestfulRicpRequester(setting.DomainName, setting.LocationPointPush, logName);
            this.LocationPointStatusRequester = new RestfulRicpRequester(setting.DomainName, setting.LocationPointStatus, logName);

            logName = "RICP Rolltainer";
            this.RolltainerSensingStatusRequester = new RestfulRicpRequester(setting.DomainName, setting.RolltainerSensorStatus, logName);

            logName = "RICP RMS";
            this.RmsStatusSettingRequester = new RestfulRicpRequester(setting.DomainName, setting.RmsStatusSetting, logName);

            logName = "RICP SmartPacking";
            this.PackageResultRequester = new RestfulRicpRequester(setting.DomainName, setting.PackageResult, logName);
        }
        #endregion

        #region Method
        public LocationStatus FindlocationStatusByIoName(Dictionary<string, LocationStatus> station, string ioName)
        {
            return station.Where(d => d.Value.IoName == ioName).FirstOrDefault().Value;
        }

        #region Post
        public async void DeviceStatusPostAsync(DeviceStatus deviceStatus)
        {
            if (Debugger.IsAttached == false)
                await this.DeviceStatusRequester.PostHttpAsync<DeviceStatus>(deviceStatus);
        }

        public async void CaseErectContainerCodePostAsync(CaseErectBcrResultArgs caseErectBcrResult)
        {
            if (caseErectBcrResult == null) return;

            ContainerScan containerScan = new ContainerScan();
            containerScan.containerCode = caseErectBcrResult.BoxId;
            containerScan.containerTypeCd = caseErectBcrResult.BoxType;

            await this.ContainerCodeRequester.PostHttpAsync<ContainerScan>(containerScan);
        }

        public async void DynamicScaleContainerCodePostAsync(DynamicScaleBcrOnReadDataArgs dynamicScaleBcrOnReadDataArgs)
        {
            if (dynamicScaleBcrOnReadDataArgs == null) return;

            WeightInvoiceContainerScan containerScan = new WeightInvoiceContainerScan();
            containerScan.containerCode = dynamicScaleBcrOnReadDataArgs.BoxID;

            await this.WeightContainerScanRequester.PostHttpAsync<WeightInvoiceContainerScan>(containerScan);
        }

        public async void DynamicScaleWeightResultPostAsync(WeightOrInvoiceResultArgs weightInspectionResultArgs)
        {
            if (weightInspectionResultArgs == null
                || weightInspectionResultArgs.WeightAndInvoice.data.Count < 1) 
                return;

            WeightResultContaierScan weightResultContaierScan = new WeightResultContaierScan();
            weightResultContaierScan.containerCode = weightInspectionResultArgs.WeightAndInvoice.data[0].box_id;
            weightResultContaierScan.containerTypeCd = weightInspectionResultArgs.WeightAndInvoice.data[0].box_type;
            weightResultContaierScan.checkResult = weightInspectionResultArgs.WeightAndInvoice.data[0].status == $"{(int)statusEnum.inspect_pass_scale}" ? true : false;
            weightResultContaierScan.checkValue = $"{weightInspectionResultArgs.WeightAndInvoice.data[0].box_wt}";

            await this.WeightResultRequester.PostHttpAsync<WeightResultContaierScan>(weightResultContaierScan);
        }

        public async void TopBcrInvoiceScanPostAsync(TopBcrResultArgs topBcrResultArgs)
        {
            if (topBcrResultArgs == null) return;

            InvoiceScan invoiceScan = new InvoiceScan();
            invoiceScan.containerCode = topBcrResultArgs.BoxId;
            invoiceScan.invoiceNo = topBcrResultArgs.Invoice;
            invoiceScan.scanResult = topBcrResultArgs.Result == BcrResult.OK ? true : false;

            await this.InvoiceScanRequester.PostHttpAsync<InvoiceScan>(invoiceScan);
        }

        public async void OutLogicalBcrInvoiceScanPostAsync(OutLogicalBcrOnReadDataArgs outLogicalBcrOnReadDataArgs)
        {
            if (outLogicalBcrOnReadDataArgs == null) return;
;
            OutInvoiceScan outInvoiceScan = new OutInvoiceScan();
            outInvoiceScan.containerCode = outLogicalBcrOnReadDataArgs.BoxId;
            outInvoiceScan.invoiceNo = outLogicalBcrOnReadDataArgs.InvoiceNo;

            await this.OutInvoiceScanRequester.PostHttpAsync<OutInvoiceScan>(outInvoiceScan);
        }

        public async void LocationPointPushPostAsync(string ioName)
        {
            if (string.IsNullOrEmpty(ioName)) return;

            foreach (var station in EcsServerAppManager.Instance.Cache.LocationStatusPikcingStations)
            {
                var status = this.FindlocationStatusByIoName(station, ioName);
                if (status != null)
                {
                    LocationPointPush locationPointPush = new LocationPointPush();
                    locationPointPush.locationPointCode = status.ShellCode;

                    RicpLocationPointPushResponse response = await this.LocationPointPushRequester.PostHttpAsync<LocationPointPush>(locationPointPush) as RicpLocationPointPushResponse;
                    if (response == null) return;

                    if (response.success)
                    {
                        foreach (var item in EcsServerAppManager.Instance.Cache.LocationStatusPikcingStations)
                        {
                            if (response.resultData != null)
                            {
                                var shellCode = response.resultData.locationPointCode;
                                if (item.ContainsKey(shellCode))
                                {
                                    LocationStatus buttonStatus = item[shellCode];
                                    buttonStatus.WorkId = response.resultData.pushWorkId;
                                    buttonStatus.StatusCd = PushWorkStatusCdEnum.READY;
                                    buttonStatus.UpdateTime = DateTime.Now;

                                    EcsServerAppManager.Instance.DataBaseManagerForServer.Updatelocationstatus(buttonStatus);
                                    this.LocationPointStatusPostAsync(buttonStatus);

                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }

        public async void LocationPointStatusPostAsync(LocationStatus buttonStatusArg)
        {
            if (buttonStatusArg == null) return;
            LocationPointStatus locationPointStatus = new LocationPointStatus();
            locationPointStatus.pushWorkId = buttonStatusArg.WorkId;

            //locationPointCode -> ShellCode 변경
            locationPointStatus.locationPointCode = buttonStatusArg.ShellCode;

            RicpLocationPointStatusResponse response = await this.LocationPointStatusRequester.PostHttpAsync<LocationPointStatus>(locationPointStatus) as RicpLocationPointStatusResponse;
            if (response == null) return;

            if (response.success)
            {
                LocationStatus button = null;
                var shellCode = response.resultData.locationPointCode;

                if (response.resultData != null)
                {
                    for (int i = 0; i < EcsServerAppManager.Instance.Cache.LocationStatusPikcingStations.Length; i++)
                    {
                        if (EcsServerAppManager.Instance.Cache.LocationStatusPikcingStations[i].ContainsKey(shellCode))
                        {
                            button = EcsServerAppManager.Instance.Cache.LocationStatusPikcingStations[i][shellCode];
                            button.WorkId = response.resultData.pushWorkId;
                            button.StatusCd = response.resultData.pushWorkStatusCd;
                            button.UpdateTime = DateTime.Now;
                            EcsServerAppManager.Instance.DataBaseManagerForServer.Updatelocationstatus(button);

                            var statusEnum = EcsServerAppManager.Instance.Cache.GetLocationStatusPickingStationEnum(button.ShellCode);
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
                        }
                    }
                }
            }
        }

        public async void RolltainerSensingStatusPostAsync(string ioName, sensorStatusEnum sensorStatusEnum)
        {
            if (string.IsNullOrEmpty(ioName)) return;

            foreach (var rollTainer in EcsServerAppManager.Instance.Cache.LocationStatusRollTainers)
            {
                var status = this.FindlocationStatusByIoName(rollTainer, ioName);
                if (status != null)
                {
                    RolltainerSensorStatus rolltainerSensorStatus = new RolltainerSensorStatus();
                    rolltainerSensorStatus.sensorStatus = $"{sensorStatusEnum}";
                    rolltainerSensorStatus.locationPointCode = status.ShellCode;

                    await this.RolltainerSensingStatusRequester.PostHttpAsync<RolltainerSensorStatus>(rolltainerSensorStatus);

                    switch (sensorStatusEnum)
                    {
                        case sensorStatusEnum.ON:
                            status.RollTainterTowerLamp = RollTainterTowerLampEnum.A;
                            break;
                        case sensorStatusEnum.OFF:
                            status.RollTainterTowerLamp = RollTainterTowerLampEnum.D;
                            break;
                        default:
                            status.RollTainterTowerLamp = RollTainterTowerLampEnum.N;
                            break;
                    }
                    break;
                }
            }
        }

        public async void RmsStatusSettingPostAsync(SystemCodeEnum systemCodeEnum, InstructionEnum InstructionEnum)
        {
            RmsStatusSetting rmsStatusSetting = new RmsStatusSetting();
            {
                rmsStatusSetting.systemCode = $"{systemCodeEnum}";
                rmsStatusSetting.instruction = $"{InstructionEnum}";
            }

             await this.RmsStatusSettingRequester.PostHttpAsync<RmsStatusSetting>(rmsStatusSetting);
        }

        public async void PackageResultPostAsync(PackageResult packageResult)
        {
            try
            {
                await this.PackageResultRequester.PostHttpAsync<PackageResult>(packageResult);
            }
            catch { }
        }
        #endregion
        #endregion 

        #region Event Handler
        public override void OnHub_Recived(EventArgs e) 
        {
            if (e is CaseErectBcrResultArgs caseErectBcrResultArgs)
            {
                this.CaseErectContainerCodePostAsync(caseErectBcrResultArgs);
            }
            else if (e is DynamicScaleBcrOnReadDataArgs dynamicScaleBcrOnReadDataArgs)
            {
                this.DynamicScaleContainerCodePostAsync(dynamicScaleBcrOnReadDataArgs);
            }
            else if (e is WeightOrInvoiceResultArgs weightInspectionResultArgs)
            {
                this.DynamicScaleWeightResultPostAsync(weightInspectionResultArgs);
            }
            else if (e is TopBcrResultArgs topBcrResultArgs)
            {
                this.TopBcrInvoiceScanPostAsync(topBcrResultArgs);
            }
            else if (e is OutLogicalBcrOnReadDataArgs outLogicalBcrOnReadDataArgs)
            {
                this.OutLogicalBcrInvoiceScanPostAsync(outLogicalBcrOnReadDataArgs);
            }
        }
        #endregion
    }
   
    [Serializable]
    public class RestfulRequsetRicpManagerSetting : RestfulRequsetManagerSetting
    {
        [DisplayName("설비가동 상태")]
        public string DeviceStatus { get; set; } = "ricp/api/ecs/device/status";

        [DisplayName("제함 박스 아이디 스캔")]
        public string ContainerScan { get; set; } = "ricp/api/ecs/container/scan";

        [DisplayName("중량 검수기 앞 박스 아이디 스캔")]
        public string WeightContainerScan { get; set; } = "ricp/api/ecs/weight/container/scan";

        [DisplayName("중량 검수 결과 전송")]
        public string WeightResult { get; set; } = "ricp/api/ecs/weight/result";

        [DisplayName("송장번호 스캔")]
        public string InvoiceScan { get; set; } = "ricp/api/ecs/invoice/scan";

        [DisplayName("출고 송장 스캔")]
        public string OutinvoiceScan { get; set; } = "ricp/api/ecs/outinvoice/scan";

        [DisplayName("지점 버튼 푸시")]
        public string LocationPointPush { get; set; } = "ricp/api/ecs/location/point/push";

        [DisplayName("지점 버튼 상태")]
        public string LocationPointStatus { get; set; } = "ricp/api/ecs/location/point/status";

        [DisplayName("지점 롤테이너 센서 상태")]
        public string RolltainerSensorStatus { get; set; } = "ricp/api/ecs/rolltainer/sensor/status";

        [DisplayName("RMS 상태 설정")]
        public string RmsStatusSetting { get; set; } = "ricp/api/ecs/rms/status/setting";

        [DisplayName("친환경충진 결과전송")]
        public string PackageResult { get; set; } = "ricp/api/ecs/package/result";
    }
}
