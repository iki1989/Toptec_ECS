using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using ECS.Core.Restful;
using ECS.Model.Restfuls;
using ECS.Model.Plc;

namespace TestWindowsFormsApp
{
    public partial class UserControlRESTfulRicp : UserControl
    {
       public UserControlRESTfulRicp()
        {
            InitializeComponent();
        }

        private void WriteLog(string text)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.WriteLog(text);
                }));
            }
            else
            {
                this.richTextBoxLog.AppendText($"{DateTime.Now.ToString("HH:mm:ss.fff")} > {text}{Environment.NewLine}");
                this.richTextBoxLog.ScrollToCaret();
            }
        }

        private async void buttonDeviceStatus_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ricp/api/ecs/device/status");

            DeviceStatus deviceStatus = new DeviceStatus();

            {
                DeviceInfo deviceInfo = new DeviceInfo
                {
                    deviceId = "Id1",
                    deviceName = "Name1",
                    deviceTypeId = "TypeId1",
                    deviceTypeName = "TypeName1",
                    deviceStatusCd = (int)EquipmentStateEnum.Stop,
                    deviceErrorMsg = "ErrorMsg1",
                };
                deviceStatus.deviceList.Add(deviceInfo);
            }

            {
                DeviceInfo deviceInfo = new DeviceInfo
                {
                    deviceId = "Id2",
                    deviceName = "Name2",
                    deviceTypeId = "TypeId2",
                    deviceTypeName = "TypeName2",
                    deviceStatusCd = (int)EquipmentStateEnum.Run,
                    deviceErrorMsg = "ErrorMsg2",
                };
                deviceStatus.deviceList.Add(deviceInfo);
            }

            var json = JsonConvert.SerializeObject(deviceStatus);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<DeviceInfo>(deviceStatus) as RicpResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonContainerScan_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ricp/api/ecs/container/scan");

            ContainerScan containerScan = new ContainerScan()
            {
                containerCode = "containerCode1",
                containerTypeCd = "containerTypeCd1",
            };

            var json = JsonConvert.SerializeObject(containerScan);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<ContainerScan>(containerScan) as RicpResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonInvoiceScan_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ricp/api/ecs/invoice/scan");

            InvoiceScan invoiceScan = new InvoiceScan()
            {
                invoiceNo = "invoiceScan"
            };

            var json = JsonConvert.SerializeObject(invoiceScan);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<InvoiceScan>(invoiceScan) as RicpResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonOutInvoiceScan_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ricp/api/ecs/outinvoice/scan");

            OutInvoiceScan outInvoiceScan = new OutInvoiceScan()
            {
                invoiceNo = "invoiceScan"
            };

            var json = JsonConvert.SerializeObject(outInvoiceScan);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<OutInvoiceScan>(outInvoiceScan) as RicpResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonLocationPointPush_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ricp/api/ecs/location/point/push");

            LocationPointPush locationPointPush = new LocationPointPush()
            {
                locationPointCode = "locationPointPush1"
            };

            var json = JsonConvert.SerializeObject(locationPointPush);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<LocationPointPush>(locationPointPush) as RicpLocationPointPushResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonLocationPointStatus_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ricp/api/ecs/location/point/status");

            LocationPointStatus locationPointStatus = new LocationPointStatus()
            {
                pushWorkId = "pushWorkId1",
                locationPointCode = "locationPointCode1"
            };

            var json = JsonConvert.SerializeObject(locationPointStatus);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<LocationPointStatus>(locationPointStatus) as RicpLocationPointStatusResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonRolltainerSensorStatus_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ricp/api/ecs/rolltainer/sensor/status");

            RolltainerSensorStatus rolltainerSensorStatus = new RolltainerSensorStatus()
            {
                locationPointCode = "locationPointCode1",
                sensorStatus = "ON"
            };

            var json = JsonConvert.SerializeObject(rolltainerSensorStatus);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<RolltainerSensorStatus>(rolltainerSensorStatus) as RicpResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonRmsStatusSetting_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ricp/api/ecs/rms/status/setting");

            RmsStatusSetting rmsStatusSetting = new RmsStatusSetting()
            {
                //requestId = "20211208",
                systemCode = $"{SystemCodeEnum.BOX_MOVING_RMS}",
                instruction = $"{InstructionEnum.FIRE_STOP}"
            };

            var json = JsonConvert.SerializeObject(rmsStatusSetting);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<RmsStatusSetting>(rmsStatusSetting) as RicpRmsStatusSettingResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonAlive_Click(object sender, EventArgs e)
        {
            RestfulRequester restful = new RestfulRequester(this.textBoxUrl.Text);
            //restful.UrlOption = "weatherforecast";
            string json = await restful.GetHttpAsync<string>() as string;
            this.WriteLog(json);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.richTextBoxLog.Clear();
        }

       
    }
}
