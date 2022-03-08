using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ECS.Core.Restful;
using ECS.Model.Restfuls;
using System.IO;
using System.Linq;
using System.Text;

namespace TestWindowsFormsApp
{
    public partial class UserControlRESTfulWcs : UserControl
    {
        public UserControlRESTfulWcs()
        {
            InitializeComponent();

            #region Order Test
            //var json = File.ReadAllText("D:\\order.txt");
            //var deserialized = JsonConvert.DeserializeObject<Order>(json);


            //StringBuilder sb = new StringBuilder();
            ////{

            ////    List<string> list = new List<string>();
            ////    var fields = deserialized.meta.GetType().GetFields();
            ////    foreach (var field in fields)
            ////        list.Add($"{field.Name}");

            ////    var metaNames = string.Join(",", list);
            ////    sb.AppendLine(metaNames);
            ////}

            //{
            //    List<string> list = new List<string>();
            //    var fields = deserialized.meta.GetType().GetFields();
            //    foreach (var field in fields)
            //        list.Add($"{field.GetValue(deserialized.meta)}");

            //    var metaValues = string.Join(",", list);
            //    sb.Append(metaValues);
            //}
            //File.WriteAllText("D:\\order_meta.csv", sb.ToString());

            //sb.Clear();

            ////{
            ////    List<string> list = new List<string>();
            ////    var fields = deserialized.data[0].GetType().GetFields();
            ////    foreach (var field in fields)
            ////        list.Add($"{field.Name}");

            ////    var dataNames = string.Join(",", list);
            ////    sb.AppendLine(dataNames);
            ////}

            //{
            //    foreach (var d in deserialized.data)
            //    {
            //        var fields = d.GetType().GetFields();
            //        List<string> valuelist = new List<string>();
            //        foreach (var field in fields)
            //        {
            //            if (field.Name != "INVOICE_ZPL_300DPI")
            //            {
            //                valuelist.Add($"{field.GetValue(d)}");
            //            }
            //        }

            //        string value = string.Join(",", valuelist);
            //        sb.AppendLine(value);
            //    }

            //}
            //File.WriteAllText("D:\\order_data.csv", sb.ToString());

            //Directory.CreateDirectory("D:\\Zpl");
            //foreach (var d in deserialized.data)
            //    File.WriteAllText($"D:\\Zpl\\{d.INVOICE_ID}.txt", d.INVOICE_ZPL_300DPI);
            #endregion

            #region Picking Test
            //for (int i = 0; i < 5; i++)
            //{
            //    var json = File.ReadAllText($"D:\\Picking{i + 1}.txt");
            //    var deserialized = JsonConvert.DeserializeObject<PickingResultsImport>(json);


            //    StringBuilder sb = new StringBuilder();
            //    //{
            //    //    List<string> list = new List<string>();
            //    //    var fields = deserialized.data[0].GetType().GetFields();
            //    //    foreach (var field in fields)
            //    //        list.Add($"{field.Name}");

            //    //    var dataNames = string.Join(",", list);
            //    //    sb.AppendLine(dataNames);
            //    //}

            //    {
            //        List<string> list = new List<string>();

            //        foreach (var d in deserialized.data)
            //        {
            //            var fields = d.GetType().GetFields();
            //            foreach (var field in fields)
            //            {
            //                if (field.GetValue(d) == null)
            //                    list.Add($" ");
            //                else
            //                    list.Add($"{field.GetValue(d)}");
            //            }


            //            var dataValues = string.Join(",", list);
            //            sb.AppendLine(dataValues);
            //        }

            //    }
            //    File.WriteAllText($"D:\\PickingResultsImport{i}.csv", sb.ToString());
            //}
            #endregion
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


        private async void buttonboxIDSampleData_Click(object sender, EventArgs e)
        {
            RestfulWcsRequester restful = new RestfulWcsRequester(this.textBoxUrl.Text, "api/wcs/boxID");

            BoxID boxID = new BoxID();
            //boxID.meta.RSTR_ID = "RSTR_ID";
            //boxID.meta.UPD_DT = DateTime.Now;
            //boxID.meta.UPDR_ID = "UPDR_ID";
            //boxID.meta.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL";
            //boxID.meta.IF_TXN_DATE = DateTime.Now;
            //boxID.meta.META_FROM = "GP_ECS_022";
            //boxID.meta.META_TO = "GP_WCS_001";
            //boxID.meta.META_GROUP_CD = "META_GROUP_CD";
            //boxID.meta.META_SEQ = "1";
            //boxID.meta.META_TOTAL = "1";
            //boxID.meta.META_COMPLETE_YN = "Y";
            //boxID.meta.META_NO = 1;

            boxID.meta.meta_from = "GP_ECS_022";
            boxID.meta.meta_to = "GP_WCS_001";
            boxID.meta.meta_group_cd = "WCS20211007000027528";
            boxID.meta.meta_seq = "1";
            boxID.meta.meta_total = "1";
            boxID.meta.meta_complete_yn = "Y";


            {
                BoxID.DataClass data = new BoxID.DataClass();
                data.box_id = "5D20003744";
                data.box_type = "D";
                data.floor = "2";
                data.eqp_id = "GB21";
                boxID.data.Add(data);
            }

            //{
            //    BoxID.DataClass data = new BoxID.DataClass();
            //    data.BOX_ID = "BOX_ID1";
            //    data.BOX_TYPE = "BOX_TYPE1";
            //    data.FLOOR = "FLOOR1";
            //    data.EQP_ID = "GB21";
            //    boxID.data.Add(data);
            //}

            //{
            //    BoxID.DataClass data = new BoxID.DataClass();
            //    data.BOX_ID = "BOX_ID2";
            //    data.BOX_TYPE = "BOX_TYPE2";
            //    data.FLOOR = "FLOOR2";
            //    data.EQP_ID = "GB22";
            //    boxID.data.Add(data);
            //}

            var json = JsonConvert.SerializeObject(boxID);
            this.WriteLog(json);

            var response = await restful.PostHttpAsync<BoxID>(boxID) as WcsResponse;
        }

        private async void buttonWeight_Click(object sender, EventArgs e)
        {
            RestfulWcsRequester restful = new RestfulWcsRequester(this.textBoxUrl.Text, "api/wcs/rsltWgt");

            WeightAndInvoice weightInvoice = new WeightAndInvoice();

            weightInvoice.meta.meta_from = "GP_ECS_022";
            weightInvoice.meta.meta_to = "GP_WCS_001";
            weightInvoice.meta.meta_group_cd = "WCS20211007000027528";
            weightInvoice.meta.meta_seq = "1";
            weightInvoice.meta.meta_total = "1";
            weightInvoice.meta.meta_complete_yn = "Y";

            {
                WeightAndInvoice.DataClass data = new WeightAndInvoice.DataClass();
                data.wh_id = "GPS01";
                data.cst_cd = "90001763";
                data.wave_no = "9019975";
                data.wave_line_no = "4";
                data.ord_no = "211202789370";
                data.ord_line_no = "1";
                data.box_id = "2A10000606";
                data.box_no = "GPS01211202341597";
                data.store_loc_cd = "0001405227";
                data.box_type = "A";
                data.floor = "2";
                data.invoice_id = "552595414253";
                data.status = "66";
                data.eqp_id = "GI21";
                data.box_wt = 2.4900;
                data.result_cd = "01";
                data.result_text = "정상통과 실적";

                weightInvoice.data.Add(data);
            }

            {
                WeightAndInvoice.DataClass data = new WeightAndInvoice.DataClass();
                data.wh_id = "GPS01";
                data.cst_cd = "90001769";
                data.wave_no = "9019975";
                data.wave_line_no = "1";
                data.ord_no = "211202789415";
                data.ord_line_no = "1";
                data.box_id = "2A10000419";
                data.box_no = "GPS01211202348398";
                data.store_loc_cd = "0001428168";
                data.box_type = "A";
                data.floor = "2";
                data.invoice_id = "553970212912";
                data.status = "66";
                data.eqp_id = "GI21";
                data.box_wt = 2.4900;
                data.result_cd = "01";
                data.result_text = "정상통과 실적";

                weightInvoice.data.Add(data);
            }

            {
                WeightAndInvoice.DataClass data = new WeightAndInvoice.DataClass();
                data.wh_id = "GPS01";
                data.cst_cd = "90001769";
                data.wave_no = "9019975";
                data.wave_line_no = "1";
                data.ord_no = "211202789471";
                data.ord_line_no = "1";
                data.box_id = "2A10000408";
                data.box_no = "GPS01211202341822";
                data.store_loc_cd = "0001428166";
                data.box_type = "A";
                data.floor = "2";
                data.invoice_id = "553970213402";
                data.status = "66";
                data.eqp_id = "GI21";
                data.box_wt = 2.4900;
                data.result_cd = "01";
                data.result_text = "정상통과 실적";

                weightInvoice.data.Add(data);
            }

            {
                WeightAndInvoice.DataClass data = new WeightAndInvoice.DataClass();
                data.wh_id = "GPS01";
                data.cst_cd = "90001769";
                data.wave_no = "9019975";
                data.wave_line_no = "1";
                data.ord_no = "211202789486";
                data.ord_line_no = "1";
                data.box_id = "2A10000570";
                data.box_no = "GPS01211202341731";
                data.store_loc_cd = "0001428168";
                data.box_type = "A";
                data.floor = "2";
                data.invoice_id = "553970213531";
                data.status = "66";
                data.eqp_id = "GI21";
                data.box_wt = 2.4900;
                data.result_cd = "01";
                data.result_text = "정상통과 실적";

                weightInvoice.data.Add(data);
            }

            var json = JsonConvert.SerializeObject(weightInvoice);
            this.WriteLog(json);

            var response = await restful.PostHttpAsync<WeightAndInvoice>(weightInvoice) as WcsResponse;
        }

        private async void buttonInvoice_Click(object sender, EventArgs e)
        {
            RestfulWcsRequester restful = new RestfulWcsRequester(this.textBoxUrl.Text, "api/wcs/rsltWaybill");

            WeightAndInvoice weightInvoice = new WeightAndInvoice();
            weightInvoice.meta.meta_from = "GP_ECS_022";
            weightInvoice.meta.meta_to = "GP_WCS_001";
            weightInvoice.meta.meta_group_cd = "WCS20211213001432203";
            weightInvoice.meta.meta_seq = "1";
            weightInvoice.meta.meta_total = "1";
            weightInvoice.meta.meta_complete_yn = "Y";

            {
                WeightAndInvoice.DataClass data = new WeightAndInvoice.DataClass();
                data.wh_id = "GPS01";
                data.cst_cd = "90001771";
                data.wave_no = "9553817";
                data.wave_line_no = "10";
                data.ord_no = "211212290990";
                data.ord_line_no = "1";
                data.box_id = "2B20000761";
                data.box_no = "GPS01211212803505";
                data.store_loc_cd = "0001460193";
                data.box_type = "B";
                data.floor = "2";
                data.invoice_id = "552703349025";
                data.status = "75";
                data.eqp_id = "GUB21";
                data.box_wt = 1.49;
                data.result_cd = "E000";
                data.result_text = "정상처리";

                weightInvoice.data.Add(data);
            }


            var json = JsonConvert.SerializeObject(weightInvoice);
            this.WriteLog(json);

            var response = await restful.PostHttpAsync<WeightAndInvoice>(weightInvoice) as WcsResponse;
        }

        private async void buttonAlive_Click(object sender, EventArgs e)
        {
            RestfulRequester restful = new RestfulRequester(this.textBoxUrl.Text);
            string json = await restful.GetHttpAsync<string>() as string;
            this.WriteLog(json);
        }

    }
}
