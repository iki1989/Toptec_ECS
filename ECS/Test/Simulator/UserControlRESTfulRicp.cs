using System;
using System.IO;
using System.Windows.Forms;
using ECS.Core.Restful;
using ECS.Model.Restfuls;
using Newtonsoft.Json;

namespace Simulator
{
    public partial class UserControlRESTfulRicp : UserControl
    {
        public UserControlRESTfulRicp()
        {
            InitializeComponent();
        }

        public void WriteLog(string text)
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

        private async void buttonContainerMapping_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ecs/api/ricp/container/mapping");

            ContainerMapping containerMapping = new ContainerMapping();
            containerMapping.outOrderCode = "outOrderCode";

            {
                Mapping mapping = new Mapping
                {
                    containerCode = "containerCode1",
                    invoiceNo = "invoiceNo1"
                };
                containerMapping.mappingList.Add(mapping);
            }

            {
                Mapping mapping = new Mapping
                {
                    containerCode = "containerCode2",
                    invoiceNo = "invoiceNo2"
                };
                containerMapping.mappingList.Add(mapping);
            }

            var json = JsonConvert.SerializeObject(containerMapping);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<ContainerMapping>(containerMapping) as RicpResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonLocationPointPushCallback_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ecs/api/ricp/location/point/push/callback");

            LocationPointPushCallback locationPointPushCallback = new LocationPointPushCallback()
            {
                pushWorkId = "pushWorkId1",
                locationPointCode = "16351085",
            };

            var json = JsonConvert.SerializeObject(locationPointPushCallback);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<RestfulRicpRequester>(locationPointPushCallback) as RicpResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonRolltainerScheduleSetting_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ecs/api/ricp/rolltainer/schedule/setting");

            RolltainerScheduleSetting rolltainerScheduleSetting = new RolltainerScheduleSetting();

            {
                RolltainerPoint rolltainerPoint = new RolltainerPoint()
                {
                    locationPointCode = "locationPointCode1",
                    upcomingTypeCode = "upcomingTypeCode1"
                };
                rolltainerScheduleSetting.locationPointList.Add(rolltainerPoint);
            }

            {
                RolltainerPoint rolltainerPoint = new RolltainerPoint()
                {
                    locationPointCode = "locationPointCode2",
                    upcomingTypeCode = "upcomingTypeCode2"
                };
                rolltainerScheduleSetting.locationPointList.Add(rolltainerPoint);
            }

            var json = JsonConvert.SerializeObject(rolltainerScheduleSetting);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<RolltainerScheduleSetting>(rolltainerScheduleSetting) as RicpResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonPickingResultsImport_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ecs/api/ricp/pickingResultsImport");

            PickingResultsImport pickingResultsImport = new PickingResultsImport();

            {
                var data = new Picking.DataClass();
                data.WH_ID = "WH_ID1";
                data.CST_CD = "CST_CD1";
                data.WAVE_NO = "WAVE_NO1";
                data.WAVE_LINE_NO = "WAVE_LINE_NO1";
                data.ORD_NO = "ORD_NO1";
                data.ORD_LINE_NO = "ORD_LINE_NO1";
                data.BOX_NO = "BOX_NO1";
                data.STORE_LOC_CD = "STORE_LOC_CD1";
                data.INVOICE_ID = "INVOICE_ID1";
                data.BOX_ID = "BOX_ID1";
                data.BOX_TYPE_CD = "BOX_TYPE_CD1";
                data.ORDER_CLASS = "ORDER_CLASS1";
                data.STATUS = "STATUS1";
                data.EQP_ID = "EQP_ID1";
                data.CST_ORD_NO = "CST_ORD_NO1";
                data.CST_ORD_LINE_NO = "CST_ORD_LINE_NO1";
                data.WT_CHECK_FLAG = "Y";
                data.SKU_CD = "SKU_CD1";
                data.SKU_NM = "SKU_NM1";
                data.SKU_QTY = 10;
                data.DLV_CLS_CD = "DLV_CLS_CD1";
                data.DLV_SUB_CLS_CD = "DLV_SUB_CLS_CD1";
                data.DELIVERY_TYPE = "DELIVERY_TYPE1";
                data.ATTRIBUTE01 = "ATTRIBUTE01";
                data.ATTRIBUTE02 = "ATTRIBUTE02";
                data.ATTRIBUTE03 = "ATTRIBUTE03";
                data.ATTRIBUTE04 = "ATTRIBUTE04";
                data.ATTRIBUTE05 = "ATTRIBUTE05";
                data.ATTRIBUTE06 = "ATTRIBUTE06";
                data.ATTRIBUTE07 = "ATTRIBUTE07";
                data.ATTRIBUTE08 = "ATTRIBUTE08";
                data.ATTRIBUTE09 = "ATTRIBUTE09";
                data.ATTRIBUTE10 = "ATTRIBUTE10";

                data.REG_DT = DateTime.Now.ToString();
                data.RSTR_ID = "RSTR_ID1";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID1";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL1";
                data.IF_TXN_DATE = "IF_TXN_DATE1";

                pickingResultsImport.data.Add(data);
            }
            {
                var data = new Picking.DataClass();
                data.WH_ID = "WH_ID2";
                data.CST_CD = "CST_CD2";
                data.WAVE_NO = "WAVE_NO2";
                data.WAVE_LINE_NO = "WAVE_LINE_NO2";
                data.ORD_NO = "ORD_NO2";
                data.ORD_LINE_NO = "ORD_LINE_NO2";
                data.BOX_NO = "BOX_NO2";
                data.STORE_LOC_CD = "STORE_LOC_CD2";
                data.INVOICE_ID = "INVOICE_ID2";
                data.BOX_ID = "BOX_ID2";
                data.BOX_TYPE_CD = "BOX_TYPE_CD2";
                data.ORDER_CLASS = "ORDER_CLASS2";
                data.STATUS = "STATUS2";
                data.EQP_ID = "EQP_ID2";
                data.CST_ORD_NO = "CST_ORD_NO2";
                data.CST_ORD_LINE_NO = "CST_ORD_LINE_NO2";
                data.WT_CHECK_FLAG = "N";
                data.SKU_CD = "SKU_CD2";
                data.SKU_NM = "SKU_NM2";
                data.SKU_QTY = 10;
                data.DLV_CLS_CD = "DLV_CLS_CD2";
                data.DLV_SUB_CLS_CD = "DLV_SUB_CLS_CD2";
                data.DELIVERY_TYPE = "DELIVERY_TYPE2";
                data.ATTRIBUTE01 = "ATTRIBUTE01";
                data.ATTRIBUTE02 = "ATTRIBUTE02";
                data.ATTRIBUTE03 = "ATTRIBUTE03";
                data.ATTRIBUTE04 = "ATTRIBUTE04";
                data.ATTRIBUTE05 = "ATTRIBUTE05";
                data.ATTRIBUTE06 = "ATTRIBUTE06";
                data.ATTRIBUTE07 = "ATTRIBUTE07";
                data.ATTRIBUTE08 = "ATTRIBUTE08";
                data.ATTRIBUTE09 = "ATTRIBUTE09";
                data.ATTRIBUTE10 = "ATTRIBUTE10";
                data.REG_DT = DateTime.Now.ToString();
                data.RSTR_ID = "RSTR_ID2";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID2";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL2";
                data.IF_TXN_DATE = "IF_TXN_DATE2";

                pickingResultsImport.data.Add(data);
            }
            {
                var data = new Picking.DataClass();
                data.WH_ID = "WH_ID3";
                data.CST_CD = "CST_CD3";
                data.WAVE_NO = "WAVE_NO3";
                data.WAVE_LINE_NO = "WAVE_LINE_NO3";
                data.ORD_NO = "ORD_NO3";
                data.ORD_LINE_NO = "ORD_LINE_NO3";
                data.BOX_NO = "BOX_NO3";
                data.STORE_LOC_CD = "STORE_LOC_CD3";
                data.INVOICE_ID = "INVOICE_ID3";
                data.BOX_ID = "BOX_ID3";
                data.BOX_TYPE_CD = "BOX_TYPE_CD3";
                data.ORDER_CLASS = "ORDER_CLASS3";
                data.STATUS = "STATUS3";
                data.EQP_ID = "EQP_ID3";
                data.CST_ORD_NO = "CST_ORD_NO3";
                data.CST_ORD_LINE_NO = "CST_ORD_LINE_NO3";
                data.WT_CHECK_FLAG = "M";
                data.SKU_CD = "SKU_CD3";
                data.SKU_NM = "SKU_NM3";
                data.SKU_QTY = 10;
                data.DLV_CLS_CD = "DLV_CLS_CD3";
                data.DLV_SUB_CLS_CD = "DLV_SUB_CLS_CD3";
                data.DELIVERY_TYPE = "DELIVERY_TYPE3";
                data.ATTRIBUTE01 = "ATTRIBUTE01";
                data.ATTRIBUTE02 = "ATTRIBUTE02";
                data.ATTRIBUTE03 = "ATTRIBUTE03";
                data.ATTRIBUTE04 = "ATTRIBUTE04";
                data.ATTRIBUTE05 = "ATTRIBUTE05";
                data.ATTRIBUTE06 = "ATTRIBUTE06";
                data.ATTRIBUTE07 = "ATTRIBUTE07";
                data.ATTRIBUTE08 = "ATTRIBUTE08";
                data.ATTRIBUTE09 = "ATTRIBUTE09";
                data.ATTRIBUTE10 = "ATTRIBUTE10";
                data.REG_DT = DateTime.Now.ToString();
                data.RSTR_ID = "RSTR_ID3";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID3";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL3";
                data.IF_TXN_DATE = "IF_TXN_DATE3";

                pickingResultsImport.data.Add(data);
            }
            {
                var data = new Picking.DataClass();
                data.WH_ID = "WH_ID4";
                data.CST_CD = "CST_CD4";
                data.WAVE_NO = "WAVE_NO4";
                data.WAVE_LINE_NO = "WAVE_LINE_NO4";
                data.ORD_NO = "ORD_NO4";
                data.ORD_LINE_NO = "ORD_LINE_NO4";
                data.BOX_NO = "BOX_NO4";
                data.STORE_LOC_CD = "STORE_LOC_CD4";
                data.INVOICE_ID = "INVOICE_ID4";
                data.BOX_ID = "BOX_ID4";
                data.BOX_TYPE_CD = "BOX_TYPE_CD4";
                data.ORDER_CLASS = "ORDER_CLASS4";
                data.STATUS = "STATUS4";
                data.EQP_ID = "EQP_ID4";
                data.CST_ORD_NO = "CST_ORD_NO4";
                data.CST_ORD_LINE_NO = "CST_ORD_LINE_NO4";
                data.WT_CHECK_FLAG = "W";
                data.SKU_CD = "SKU_CD4";
                data.SKU_NM = "SKU_NM4";
                data.SKU_QTY = 10;
                data.DLV_CLS_CD = "DLV_CLS_CD4";
                data.DLV_SUB_CLS_CD = "DLV_SUB_CLS_CD4";
                data.DELIVERY_TYPE = "DELIVERY_TYPE4";
                data.ATTRIBUTE01 = "ATTRIBUTE01";
                data.ATTRIBUTE02 = "ATTRIBUTE02";
                data.ATTRIBUTE03 = "ATTRIBUTE03";
                data.ATTRIBUTE04 = "ATTRIBUTE04";
                data.ATTRIBUTE05 = "ATTRIBUTE05";
                data.ATTRIBUTE06 = "ATTRIBUTE06";
                data.ATTRIBUTE07 = "ATTRIBUTE07";
                data.ATTRIBUTE08 = "ATTRIBUTE08";
                data.ATTRIBUTE09 = "ATTRIBUTE09";
                data.ATTRIBUTE10 = "ATTRIBUTE10";
                data.REG_DT = DateTime.Now.ToString();
                data.RSTR_ID = "RSTR_ID4";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID4";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL4";
                data.IF_TXN_DATE = "IF_TXN_DATE4";

                pickingResultsImport.data.Add(data);
            }
            {
                var data = new Picking.DataClass();
                data.WH_ID = "WH_ID5";
                data.CST_CD = "CST_CD5";
                data.WAVE_NO = "WAVE_NO5";
                data.WAVE_LINE_NO = "WAVE_LINE_NO5";
                data.ORD_NO = "ORD_NO5";
                data.ORD_LINE_NO = "ORD_LINE_NO5";
                data.BOX_NO = "BOX_NO5";
                data.STORE_LOC_CD = "STORE_LOC_CD5";
                data.INVOICE_ID = "INVOICE_ID5";
                data.BOX_ID = "BOX_ID5";
                data.BOX_TYPE_CD = "BOX_TYPE_CD5";
                data.ORDER_CLASS = "ORDER_CLASS5";
                data.STATUS = "STATUS5";
                data.EQP_ID = "EQP_ID5";
                data.CST_ORD_NO = "CST_ORD_NO5";
                data.CST_ORD_LINE_NO = "CST_ORD_LINE_NO5";
                data.WT_CHECK_FLAG = "B";
                data.SKU_CD = "SKU_CD5";
                data.SKU_NM = "SKU_NM5";
                data.SKU_QTY = 10;
                data.DLV_CLS_CD = "DLV_CLS_CD5";
                data.DLV_SUB_CLS_CD = "DLV_SUB_CLS_CD5";
                data.DELIVERY_TYPE = "DELIVERY_TYPE5";
                data.ATTRIBUTE01 = "ATTRIBUTE01";
                data.ATTRIBUTE02 = "ATTRIBUTE02";
                data.ATTRIBUTE03 = "ATTRIBUTE03";
                data.ATTRIBUTE04 = "ATTRIBUTE04";
                data.ATTRIBUTE05 = "ATTRIBUTE05";
                data.ATTRIBUTE06 = "ATTRIBUTE06";
                data.ATTRIBUTE07 = "ATTRIBUTE07";
                data.ATTRIBUTE08 = "ATTRIBUTE08";
                data.ATTRIBUTE09 = "ATTRIBUTE09";
                data.ATTRIBUTE10 = "ATTRIBUTE10";
                data.REG_DT = DateTime.Now.ToString();
                data.RSTR_ID = "RSTR_ID5";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID5";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL5";
                data.IF_TXN_DATE = "IF_TXN_DATE5";

                pickingResultsImport.data.Add(data);
            }

            var json = JsonConvert.SerializeObject(pickingResultsImport);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<PickingResultsImport>(pickingResultsImport) as RicpResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonRmsStatusSettingCallback_Click(object sender, EventArgs e)
        {
            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ecs/api/ricp/rms/status/setting/callback");

            RmsStatusSettingCallback RmsStatusSettingCallback = new RmsStatusSettingCallback();
            RmsStatusSettingCallback.requestId = "20211208";
            RmsStatusSettingCallback.callbackType = $"{InstructionEnum.FIRE_STOP}";


            var json = JsonConvert.SerializeObject(RmsStatusSettingCallback);
            this.WriteLog($"Send : {json}");

            var response = await restful.PostHttpAsync<RmsStatusSettingCallback>(RmsStatusSettingCallback) as RicpResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonAlive_Click(object sender, EventArgs e)
        {
            RestfulRequester restful = new RestfulRequester(this.textBoxUrl.Text);
            string json = await restful.GetHttpAsync<string>() as string;
            this.WriteLog(json);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.richTextBoxLog.Clear();
        }

        private async void buttonTestPicking_Click(object sender, EventArgs e)
        {
            string file = $"D:\\Test\\PickingResultsImport.csv";
            if (File.Exists(file) == false) return;
        
            var PickingCsv = File.ReadAllText(file);

            PickingResultsImport picking = new PickingResultsImport();
            {
                var splitedLine = PickingCsv.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                for (int i = 0; i < splitedLine.Length; i++)
                {
                    Picking.DataClass data = new Picking.DataClass();
                    var splited = splitedLine[i].Split(',');
                    var fields = data.GetType().GetFields();

                    for (int j = 0; j < splited.Length; j++)
                    {
                        var typeName = fields[j].FieldType.Name;

                        if (typeName.Contains("Null"))
                        {
                            if (double.TryParse(splited[j], out double d))
                                fields[j].SetValue(data, d);
                            else if (long.TryParse(splited[j], out long l))
                                fields[j].SetValue(data, l);
                        }
                        else
                        {
                            fields[j].SetValue(data, splited[j]);
                        }
                    }
                    picking.data.Add(data);
                }

            }

            RestfulRicpRequester restful = new RestfulRicpRequester(this.textBoxUrl.Text, "ecs/api/ricp/pickingResultsImport");

            var response = await restful.PostHttpAsync<PickingResultsImport>(picking) as RicpResponse;
            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

      
    }
}
