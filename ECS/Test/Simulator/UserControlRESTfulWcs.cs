using System;
using System.IO;
using System.Windows.Forms;
using ECS.Core.Restful;
using ECS.Model.Restfuls;
using Newtonsoft.Json;

namespace Simulator
{
    public partial class UserControlRESTfulWcs : UserControl
    {
        public UserControlRESTfulWcs()
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

        private async void buttonOrder_Click(object sender, EventArgs e)
        {
            RestfulWcsRequester restful = new RestfulWcsRequester(this.textBoxUrl.Text, "api/v1/ecs/order");

            Order order = new Order();
          

            order.meta.meta_from = "GP_WCS_001";
            order.meta.meta_to = "GP_ECS_011";
            order.meta.meta_group_cd = "WCS2021111203241800049";
            order.meta.meta_seq = "1";
            order.meta.meta_total = "1";
            order.meta.meta_complete_yn = "Y";

            {
                for (int i = 0; i < 1000; i++)
                {
                    Order.DataClass data = new Order.DataClass();
                    data.WH_ID = "GPS01";
                    data.CST_CD = "90001763";
                    data.WAVE_NO = "356467";
                    data.WAVE_LINE_NO = "1";
                    data.ORD_NO = "211209628370";
                    data.ORD_LINE_NO = "1";
                    data.BOX_NO = "GPS01211109097376";
                    data.STORE_LOC_CD = "A0100101";
                    data.ORD_TYPE = "11";
                    data.ORD_CLASS = null;
                    data.INVOICE_ID = "552595403705";
                    data.BOX_ID = null;
                    data.CST_ORD_NO = "10007";
                    data.CST_ORD_LINE_NO = null;
                    data.SKU_CD = "4293654264-1";
                    data.SKU_QTY = 1.0000;
                    data.BOX_TYPE_CD = "A";
                    data.BOX_TYPE = null;
                    data.FLOOR = null;
                    data.INVOICE_NO = null;
                    data.BOX_WT = null;
                    data.STATUS = "10";
                    data.EQP_ID = "GI21";
                    data.INVOICE_ZPL = null;
                    data.INVOICE_ZPL_300DPI = "^XA\r\n^PON\r\n^PW1300\r\n^LH30,0\r\n^SEE:UHANGUL.DAT^FS\r\n^CWQ,E:KFONT3.FNT^FS\r\n^CI26\r\n\r\n^FT30,1300^FR^AQB,50,50^FD5525-9540-3705^FS\r\n^FT30,800^FR^AQB,50,50^FD2021-11-09^FS\r\n^FT30,300^FR^AQB,50,50^FD ^FS\r\n^FO50,1060^BY5^BCB,220,N,N,N,A^FD5E51^FS\r\n\r\n^FT250,1050^FR^AQB,160,160^FD5^FS\r\n^FT260,1040^GB0,90,6^FS\r\n\r\n^FT250,900^FR^AQB,240,220^FDE51^FS\r\n\r\n^FT250,500^FR^AQB,180,120^FD-^FS\r\n^FT250,400^FR^AQB,160,120^FD0^FS\r\n\r\n^FT320,1420^FR^AQB,40,40^FD홍길동 010-1111-1111/^FS\r\n\r\n^FO280,100^BY5^BCB,50,N,N,N,A^FD552595403705^FS\r\n\r\n^FT360,1420^FR^AQB,40,40^FD서울특별시 중구 세종대로9길 53^FS\r\n^FT400,1420^FR^AQB,40,40^FD대한통운^FS\r\n^FT440,1420^FR^AQB,40,40^FD ^FS\r\n^FT500,1420^FR^AQB,70,70^FD서소문 58-12 대한통운^FS\r\n\r\n^FT540,1420^FR^AQB,40,40^FD헬스밸런스 (070-8233-2213)^FS\r\n^FT540,550^FR^AQB,40,40^FD1^FS\r\n^FT540,300^FR^AQB,40,40^FD0^FS\r\n^FT540,120^FR^AQB,40,40^FD신용^FS \r\n\r\n^FT580,1420^FR^AQB,40,40^FD경기도 군포시 번영로 82^FS\r\n\r\n^FT625,1420^FR^AQB,40,40^FD주문자명 / ^FS\r\n^FT665,1420^FR^AQB,40,40^FD주문번호 ^FS\r\n\r\n\r\n^FT725,1420^FR^AQB,40,40^FD[4293654264-1] 스키니랩 가벼워지는 시서스 다이어트 2g x 14포 x 1박스 600% 농축 시서스추출물 시서스 가루 분말-1^FS\r\n^FT725,450^FR^AQB,40,40^FD[수량 : 1]^FS\r\n\r\n\r\n^FT925,1420^FR^AQB,40,40^FD ^FS \r\n^FT925,1140^FR^AQB,40,40^FD 상품 파손시 4B98로 발송해주세요^FS\r\n^FT925,450^FR^AQB,40,40^FD[총 수량 : 1 ]^FS\r\n^FT965,1420^FR^AQB,40,40^FD28370^FS\r\n\r\n^FT1010,1440^FR^AQB,40,40^FD서소문 58-12 대한통운^FS\r\n\r\n\r\n^FT1090,1440^AQB,40,40^FB1015,2,,^FD문앞^FS\r\n^FT1112,1440^AQB,40,40^FB1015,2,,^FD^FS\r\n\r\n^FT1135,1440^FR^AQB,40,40^FD중구Bsub(송종훈)_송종훈_Z02^FS\r\n^FT1135,900^FR^AQB,40,40^FD0^FS \r\n^FT1135,650^FR^AQB,40,40^FD신용^FS\r\n^FO990,80^BY4^BCB,120,Y,N,N^FD>;552595403705^FS \r\n^XZ";
                    data.WT_CHECK_FLAG = "Y";
                    data.DIV_CLS_CD = "5E51";
                    data.DIV_SUB_CLS_CD = "0";
                    data.VARIANT_SKU_FLAG = "N";
                    data.DELIVERY_TYPE = "17";
                    data.ATTRIBUTE01 = "";
                    data.ATTRIBUTE02 = "";
                    data.ATTRIBUTE03 = "";
                    data.ATTRIBUTE04 = "";
                    data.ATTRIBUTE05 = "";
                    data.ATTRIBUTE06 = "";
                    data.ATTRIBUTE07 = "";
                    data.ATTRIBUTE08 = "";
                    data.ATTRIBUTE09 = "";
                    data.ATTRIBUTE10 = "";
                    data.MNL_PACKING_FLAG = "N";
                    data.EMPTY_RATE = null;
                    data.RESULT_CD = null;
                    data.RESULT_TEXT = null;
                    data.STANDARD_WHT = "0.000";
                    data.JOB_DATE = "20211109";
                    data.ORDER_DATE = "20211109";
                    data.ORDER_DETAIL_ID = "4293654264-1";
                    data.MULTI_BOX = null;
                    data.FRE_DRY_QTY = null;
                    data.FRE_PACK_QTY = null;
                    data.REF_DRY_QTY = null;
                    data.REF_PACK_QTY = null;
                    data.DIVIDER = null;
                    data.PACK_TYPE = "BOX";
                    data.BOX_IN_QTY = 14.0000;
                    data.SKU_BARCD = "4293654264-1";
                    data.SKU_NM = "스키니랩 가벼워지는 시서스 다이어트 2g x 14포 x 1박스 600% 농축 시서스추출물 시서스 가루 분말-1";
                    data.QTY_UNIT = "EA";
                    data.STORE_ZONE_CD = "A";
                    data.ALLOC_NO = "1";
                    data.PASS_STOP_CD = "30421980";
                    data.PASS_STOP_NM = null;
                    data.PAYMENT_TYPE = null;
                    data.SND_CUST_NO = "30421980";
                    data.B2C_CUST_ID = null;
                    data.B2C_CUST_MGR_ID = null;
                    data.RESV_TYPE = null;
                    data.ORDER_SKU_STATUS = null;
                    data.BUYER_PO_CD = null;
                    data.SND_NM = "헬스밸런스";

                    order.data.Add(data);
                }
            }

            var json = JsonConvert.SerializeObject(order);
            this.WriteLog(json);

            var response = await restful.PostHttpAsync<Order>(order) as WcsResponse;

            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonOrderCancel_Click(object sender, EventArgs e)
        {
            RestfulWcsRequester restful = new RestfulWcsRequester(this.textBoxUrl.Text, "api/v1/ecs/orderCancel");

            OrderCancel orderCancel = new OrderCancel();
            orderCancel.meta.meta_from = "IF_TXN_DATE";
            orderCancel.meta.meta_to = "META_TO";
            orderCancel.meta.meta_group_cd = "META_GROUP_CD";
            orderCancel.meta.meta_seq = "META_SEQ";
            orderCancel.meta.meta_total = "META_TOTAL";
            orderCancel.meta.meta_complete_yn = "META_COMPLETE_YN";

            {
                OrderCancel.DataClass data = new OrderCancel.DataClass();
                data.WH_ID = "WH_ID1";
                data.CST_CD = "CST_CD1";
                data.WAVE_NO = "WAVE_NO1";
                data.WAVE_LINE_NO = "WAVE_LINE_NO1";
                data.ORD_NO = "ORD_NO1";
                data.ORD_LINE_NO = "ORD_LINE_NO1";
                data.BOX_NO = "BOX_NO1";
                data.STORE_LOC_CD = "STORE_LOC_CD1";
                data.STANDARD_WHT = "STANDARD_WHT1";
                data.INVOICE_ID = "INVOICE_ID1";
                data.BOX_ID = "BOX_ID1";
                data.CST_ORD_NO = "CST_ORD_NO1";
                data.CST_ORD_LINE_NO = "CST_ORD_LINE_NO1";
                data.SKU_CD = "SKU_CD1";
                data.SKU_QTY = 1;
                data.BOX_TYPE_CD = "BOX_TYPE_CD1";
                data.STATUS = "STATUS1";
                data.EQP_ID = "EQP_ID1";
                data.ATTRIBUTE01 = "ATTRIBUTE011";
                data.ATTRIBUTE02 = "ATTRIBUTE021";
                data.ATTRIBUTE03 = "ATTRIBUTE031";
                data.ATTRIBUTE04 = "ATTRIBUTE041";
                data.ATTRIBUTE05 = 2;
                data.ATTRIBUTE06 = 3;
                data.ATTRIBUTE07 = 4;
                data.ATTRIBUTE08 = 5;
                data.ATTRIBUTE09 = "ATTRIBUTE091";
                data.ATTRIBUTE10 = "ATTRIBUTE101";

                data.REG_DT = DateTime.Now.ToString();
                data.RSTR_ID = "RSTR_ID";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL";
                data.IF_TXN_DATE = DateTime.Now.ToString();

                orderCancel.data.Add(data);
            }

            {
                OrderCancel.DataClass data = new OrderCancel.DataClass();
                data.WH_ID = "WH_ID2";
                data.CST_CD = "CST_CD2";
                data.WAVE_NO = "WAVE_NO2";
                data.WAVE_LINE_NO = "WAVE_LINE_NO2";
                data.ORD_NO = "ORD_NO2";
                data.ORD_LINE_NO = "ORD_LINE_NO2";
                data.BOX_NO = "BOX_NO2";
                data.STORE_LOC_CD = "STORE_LOC_CD2";
                data.STANDARD_WHT = "STANDARD_WHT2";
                data.INVOICE_ID = "INVOICE_ID2";
                data.BOX_ID = "BOX_ID2";
                data.CST_ORD_NO = "CST_ORD_NO2";
                data.CST_ORD_LINE_NO = "CST_ORD_LINE_NO2";
                data.SKU_CD = "SKU_CD2";
                data.SKU_QTY = 1;
                data.BOX_TYPE_CD = "BOX_TYPE_CD2";
                data.STATUS = "STATUS2";
                data.EQP_ID = "EQP_ID2";
                data.ATTRIBUTE01 = "ATTRIBUTE012";
                data.ATTRIBUTE02 = "ATTRIBUTE022";
                data.ATTRIBUTE03 = "ATTRIBUTE032";
                data.ATTRIBUTE04 = "ATTRIBUTE042";
                data.ATTRIBUTE05 = 2;
                data.ATTRIBUTE06 = 3;
                data.ATTRIBUTE07 = 4;
                data.ATTRIBUTE08 = 5;
                data.ATTRIBUTE09 = "ATTRIBUTE092";
                data.ATTRIBUTE10 = "ATTRIBUTE102";

                data.REG_DT = DateTime.Now.ToString();
                data.RSTR_ID = "RSTR_ID";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL";
                data.IF_TXN_DATE = DateTime.Now.ToString();

                orderCancel.data.Add(data);
            }

            var json = JsonConvert.SerializeObject(orderCancel);
            this.WriteLog(json);

            var response = await restful.PostHttpAsync<OrderCancel>(orderCancel) as WcsResponse;

            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonOrderDelete_Click(object sender, EventArgs e)
        {
            RestfulWcsRequester restful = new RestfulWcsRequester(this.textBoxUrl.Text, "api/v1/ecs/orderDelete");

            OrderDelete orderDelete = new OrderDelete();
            orderDelete.meta.meta_from = "IF_TXN_DATE";
            orderDelete.meta.meta_to = "META_TO";
            orderDelete.meta.meta_group_cd = "META_GROUP_CD";
            orderDelete.meta.meta_seq = "META_SEQ";
            orderDelete.meta.meta_total = "META_TOTAL";
            orderDelete.meta.meta_complete_yn = "META_COMPLETE_YN";

            {
                OrderDelete.DataClass data = new OrderDelete.DataClass();
                data.WAVE_NO = "WAVE_NO1";
                data.WAVE_REMOVE_FL = "WAVE_REMOVE_FL1";

                data.REG_DT = DateTime.Now.ToString();
                data.RSTR_ID = "RSTR_ID";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL";
                data.IF_TXN_DATE = DateTime.Now.ToString();

                orderDelete.data.Add(data);
            }

            {
                OrderDelete.DataClass data = new OrderDelete.DataClass();
                data.WAVE_NO = "WAVE_NO2";
                data.WAVE_REMOVE_FL = "WAVE_REMOVE_FL2";

                data.REG_DT = DateTime.Now.ToString();
                data.RSTR_ID = "RSTR_ID";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL";
                data.IF_TXN_DATE = DateTime.Now.ToString();

                orderDelete.data.Add(data);
            }

            var json = JsonConvert.SerializeObject(orderDelete);
            this.WriteLog(json);

            var response = await restful.PostHttpAsync<OrderDelete>(orderDelete) as WcsResponse;

            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonOperatorWeightResult_Click(object sender, EventArgs e)
        {
            RestfulWcsRequester restful = new RestfulWcsRequester(this.textBoxUrl.Text, "api/v1/ecs/operatorWeightResult");

            OperatorWeightResult operatorWeightResult = new OperatorWeightResult();
           
            operatorWeightResult.meta.meta_from = "IF_TXN_DATE";
            operatorWeightResult.meta.meta_to = "META_TO";
            operatorWeightResult.meta.meta_group_cd = "META_GROUP_CD";
            operatorWeightResult.meta.meta_seq = "META_SEQ";
            operatorWeightResult.meta.meta_total = "META_TOTAL";
            operatorWeightResult.meta.meta_complete_yn = "META_COMPLETE_YN";

            {
                OperatorWeightResult.DataClass data = new OperatorWeightResult.DataClass();
                data.WH_ID = "WH_ID1";
                data.CST_CD = "CST_CD1";
                data.ORD_NO = "ORD_NO1";
                data.BOX_ID = "BOX_ID1";
                data.WT_CHECK_FLAG = "WT_CHECK_FLAG1";
                data.EQP_ID = "EQP_ID1";
                data.INVOICE_ID = "INVOICE_ID1";

                data.REG_DT = DateTime.Now.ToString();
                data.RSTR_ID = "RSTR_ID";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL";
                data.IF_TXN_DATE = DateTime.Now.ToString();

                operatorWeightResult.data.Add(data);
            }

            {
                OperatorWeightResult.DataClass data = new OperatorWeightResult.DataClass();
                data.WH_ID = "WH_ID2";
                data.CST_CD = "CST_CD2";
                data.ORD_NO = "ORD_NO2";
                data.BOX_ID = "BOX_ID2";
                data.WT_CHECK_FLAG = "WT_CHECK_FLAG2";
                data.EQP_ID = "EQP_ID2";
                data.INVOICE_ID = "INVOICE_ID2";

                data.REG_DT = DateTime.Now.ToString();
                data.RSTR_ID = "RSTR_ID";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL";
                data.IF_TXN_DATE = DateTime.Now.ToString();

                operatorWeightResult.data.Add(data);
            }

            var json = JsonConvert.SerializeObject(operatorWeightResult);
            this.WriteLog(json);

            var response = await restful.PostHttpAsync<OperatorWeightResult>(operatorWeightResult) as WcsResponse;

            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonSkuMaster_Click(object sender, EventArgs e)
        {
            RestfulWcsRequester restful = new RestfulWcsRequester(this.textBoxUrl.Text, "api/v1/ecs/skuMaster");

            SkuMaster skuMaster = new SkuMaster();
            skuMaster.meta.meta_from = "IF_TXN_DATE";
            skuMaster.meta.meta_to = "META_TO";
            skuMaster.meta.meta_group_cd = "META_GROUP_CD";
            skuMaster.meta.meta_seq = "META_SEQ";
            skuMaster.meta.meta_total = "META_TOTAL";
            skuMaster.meta.meta_complete_yn = "META_COMPLETE_YN";

            {
                SkuMaster.DataClass data = new SkuMaster.DataClass();
                data.WH_ID = "WH_ID1";
                data.CST_CD = "CST_CD1";
                data.SKU_CD = "SKU_CD1";
                data.SKU_CLASS = "SKU_CLASS1";
                data.SKU_NM = "SKU_NM1";
                data.SKU_NM_ENG = "SKU_NM_ENG1";
                data.SKU_NM_LOC = "SKU_NM_LOC1";
                data.DELETE_FLAG = "DELETE_FLAG1";
                data.PACKING_FLAG = "PACKING_FLAG1";
                data.ITEM_TEMP = "ITEM_TEMP1";
                data.ORD_TYPE = "ORD_TYPE1";
                data.WGT_UOM_CD = "WGT_UOM_CD1";
                data.PUR_VENDOR_CD = "PUR_VENDOR_CD1";
                data.PUR_VENDOR_NM = "PUR_VENDOR_NM1";
                data.PUR_VENDOR_NM_ENG = "PUR_VENDOR_NM_ENG1";
                data.PUR_VENDOR_NM_LOC = "PUR_VENDOR_NM_LOC1";
                data.LEN_UOM_CD = "LEN_UOM_CD1";
                data.CBM_UOM_CD = "CBM_UOM_CD1";
                data.SKU_PRICE = 1;
                data.STYL_CD = "STYL_CD1";
                data.STYL_NM = "STYL_NM1";
                data.SIZ_CD = "SIZ_CD1";
                data.SIZ_NM = 2;
                data.CLR_CD = 3;
                data.CLR_NM = 4;
                data.BRND_CD = 5;
                data.BRND_NM = "BRND_NM1";
                data.SKU_TMPT_TYPE_CD = "SKU_TMPT_TYPE_CD1";
                data.EXPIRA_DATE_TERM = 6;
                data.SKU_BCR_NO1 = "SKU_BCR_NO11";
                data.SKU_BCR_NO2 = "SKU_BCR_NO21";
                data.SKU_BCR_NO3 = "SKU_BCR_NO31";
                data.BOX_BCR_NO = "BOX_BCR_NO1";
                data.CASE_BCR_NO = "CASE_BCR_NO1";
                data.CASE_YN = "CASE_YN1";
                data.BOX_IN_QTY = 7;
                data.PLT_BOX_QTY = 8;
                data.SKU_WGT = 9;
                data.CASE_WGT = 10;
                data.BOX_WGT = 11;
                data.SKU_VERT_LEN = 12;
                data.SKU_WTH_LEN = 13;
                data.SKU_HGT_LEN = 14;
                data.SKU_CBM = 15;
                data.CASE_WTH_LEN = 16;
                data.CASE_VERT_LEN = 17;
                data.CASE_HGT_LEN = 18;
                data.CASE_CBM = 19;
                data.BOX_VERT_LEN = 20;
                data.BOX_WTH_LEN = 21;
                data.BOX_HGT_LEN = 22;
                data.BOX_CBM = 23;
                data.ATTRIBUTE01 = "ATTRIBUTE011";
                data.ATTRIBUTE02 = "ATTRIBUTE021";
                data.ATTRIBUTE03 = "ATTRIBUTE031";
                data.ATTRIBUTE04 = "ATTRIBUTE041";
                data.ATTRIBUTE05 = "ATTRIBUTE051";
                data.ATTRIBUTE06 = "ATTRIBUTE061";
                data.ATTRIBUTE07 = "ATTRIBUTE071";
                data.ATTRIBUTE08 = "ATTRIBUTE081";
                data.ATTRIBUTE09 = "ATTRIBUTE091";
                data.ATTRIBUTE10 = "ATTRIBUTE101";

                data.RSTR_ID = "RSTR_ID";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL";
                data.IF_TXN_DATE = DateTime.Now.ToString();

                skuMaster.data.Add(data);
            }

            {
                SkuMaster.DataClass data = new SkuMaster.DataClass();
                data.WH_ID = "WH_ID2";
                data.CST_CD = "CST_CD2";
                data.SKU_CD = "SKU_CD2";
                data.SKU_CLASS = "SKU_CLASS2";
                data.SKU_NM = "SKU_NM2";
                data.SKU_NM_ENG = "SKU_NM_ENG2";
                data.SKU_NM_LOC = "SKU_NM_LOC2";
                data.DELETE_FLAG = "DELETE_FLAG2";
                data.PACKING_FLAG = "PACKING_FLAG2";
                data.ITEM_TEMP = "ITEM_TEMP2";
                data.ORD_TYPE = "ORD_TYPE2";
                data.WGT_UOM_CD = "WGT_UOM_CD2";
                data.PUR_VENDOR_CD = "PUR_VENDOR_CD2";
                data.PUR_VENDOR_NM = "PUR_VENDOR_NM2";
                data.PUR_VENDOR_NM_ENG = "PUR_VENDOR_NM_ENG2";
                data.PUR_VENDOR_NM_LOC = "PUR_VENDOR_NM_LOC2";
                data.LEN_UOM_CD = "LEN_UOM_CD2";
                data.CBM_UOM_CD = "CBM_UOM_CD2";
                data.SKU_PRICE = 1;
                data.STYL_CD = "STYL_CD2";
                data.STYL_NM = "STYL_NM2";
                data.SIZ_CD = "SIZ_CD2";
                data.SIZ_NM = 2;
                data.CLR_CD = 3;
                data.CLR_NM = 4;
                data.BRND_CD = 5;
                data.BRND_NM = "BRND_NM2";
                data.SKU_TMPT_TYPE_CD = "SKU_TMPT_TYPE_CD2";
                data.EXPIRA_DATE_TERM = 6;
                data.SKU_BCR_NO1 = "SKU_BCR_NO12";
                data.SKU_BCR_NO2 = "SKU_BCR_NO22";
                data.SKU_BCR_NO3 = "SKU_BCR_NO32";
                data.BOX_BCR_NO = "BOX_BCR_NO2";
                data.CASE_BCR_NO = "CASE_BCR_NO2";
                data.CASE_YN = "CASE_YN2";
                data.BOX_IN_QTY = 7;
                data.PLT_BOX_QTY = 8;
                data.SKU_WGT = 9;
                data.CASE_WGT = 10;
                data.BOX_WGT = 11;
                data.SKU_VERT_LEN = 12;
                data.SKU_WTH_LEN = 13;
                data.SKU_HGT_LEN = 14;
                data.SKU_CBM = 15;
                data.CASE_WTH_LEN = 16;
                data.CASE_VERT_LEN = 17;
                data.CASE_HGT_LEN = 18;
                data.CASE_CBM = 19;
                data.BOX_VERT_LEN = 20;
                data.BOX_WTH_LEN = 21;
                data.BOX_HGT_LEN = 22;
                data.BOX_CBM = 23;
                data.ATTRIBUTE01 = "ATTRIBUTE012";
                data.ATTRIBUTE02 = "ATTRIBUTE022";
                data.ATTRIBUTE03 = "ATTRIBUTE032";
                data.ATTRIBUTE04 = "ATTRIBUTE042";
                data.ATTRIBUTE05 = "ATTRIBUTE052";
                data.ATTRIBUTE06 = "ATTRIBUTE062";
                data.ATTRIBUTE07 = "ATTRIBUTE072";
                data.ATTRIBUTE08 = "ATTRIBUTE082";
                data.ATTRIBUTE09 = "ATTRIBUTE092";
                data.ATTRIBUTE10 = "ATTRIBUTE102";

                data.RSTR_ID = "RSTR_ID";
                data.UPD_DT = DateTime.Now.ToString();
                data.UPDR_ID = "UPDR_ID";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL";
                data.IF_TXN_DATE = DateTime.Now.ToString();

                skuMaster.data.Add(data);
            }

            var json = JsonConvert.SerializeObject(skuMaster);
            this.WriteLog(json);

            var response = await restful.PostHttpAsync<SkuMaster>(skuMaster) as WcsResponse;

            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");
        }

        private async void buttonPickingResultsImport_Click(object sender, EventArgs e)
        {
            RestfulWcsRequester restful = new RestfulWcsRequester(this.textBoxUrl.Text, "ecs/api/ricp/pickingResultsImport");

            PickingResultsImport pickingResultsImport = new PickingResultsImport();

            {
                var data = new Picking.DataClass();
                data.WH_ID = "WH_ID";
                data.CST_CD = "CST_CD";
                data.WAVE_NO = "WAVE_NO";
                data.WAVE_LINE_NO = "WAVE_LINE_NO";
                data.ORD_NO = "ORD_NO";
                data.ORD_LINE_NO = "ORD_LINE_NO";
                data.BOX_NO = "BOX_NO";
                data.STORE_LOC_CD = "STORE_LOC_CD";
                data.INVOICE_ID = "INVOICE_ID";
                data.BOX_ID = "BOX_ID";
                data.BOX_TYPE_CD = "BOX_TYPE_CD";
                data.ORDER_CLASS = "ORDER_CLASS";
                data.STATUS = "STATUS";
                data.EQP_ID = "EQP_ID";
                data.CST_ORD_NO = "CST_ORD_NO";
                data.CST_ORD_LINE_NO = "CST_ORD_LINE_NO";
                data.WT_CHECK_FLAG = "WT_CHECK_FLAG";
                data.SKU_CD = "SKU_CD";
                data.SKU_NM = "SKU_NM";
                data.SKU_QTY = 10;
                data.DLV_CLS_CD = "DLV_CLS_CD";
                data.DLV_SUB_CLS_CD = "DLV_SUB_CLS_CD";
                data.DELIVERY_TYPE = "DELIVERY_TYPE";
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
                data.REG_DT = "REG_DT";
                data.RSTR_ID = "RSTR_ID";
                data.UPD_DT = "UPD_DT";
                data.UPDR_ID = "UPDR_ID";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL";
                data.IF_TXN_DATE = "IF_TXN_DATE";


                pickingResultsImport.data.Add(new Picking.DataClass());
            }
            {
                var data = new Picking.DataClass();
                data.WH_ID = "WH_ID";
                data.CST_CD = "CST_CD";
                data.WAVE_NO = "WAVE_NO";
                data.WAVE_LINE_NO = "WAVE_LINE_NO";
                data.ORD_NO = "ORD_NO";
                data.ORD_LINE_NO = "ORD_LINE_NO";
                data.BOX_NO = "BOX_NO";
                data.STORE_LOC_CD = "STORE_LOC_CD";
                data.INVOICE_ID = "INVOICE_ID";
                data.BOX_ID = "BOX_ID";
                data.BOX_TYPE_CD = "BOX_TYPE_CD";
                data.ORDER_CLASS = "ORDER_CLASS";
                data.STATUS = "STATUS";
                data.EQP_ID = "EQP_ID";
                data.CST_ORD_NO = "CST_ORD_NO";
                data.CST_ORD_LINE_NO = "CST_ORD_LINE_NO";
                data.WT_CHECK_FLAG = "WT_CHECK_FLAG";
                data.SKU_CD = "SKU_CD";
                data.SKU_NM = "SKU_NM";
                data.SKU_QTY = 10;
                data.DLV_CLS_CD = "DLV_CLS_CD";
                data.DLV_SUB_CLS_CD = "DLV_SUB_CLS_CD";
                data.DELIVERY_TYPE = "DELIVERY_TYPE";
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
                data.REG_DT = "REG_DT";
                data.RSTR_ID = "RSTR_ID";
                data.UPD_DT = "UPD_DT";
                data.UPDR_ID = "UPDR_ID";
                data.IF_TXN_TYPE_FL = "IF_TXN_TYPE_FL";
                data.IF_TXN_DATE = "IF_TXN_DATE";


                pickingResultsImport.data.Add(new Picking.DataClass());
            }

            var json = JsonConvert.SerializeObject(pickingResultsImport);
            this.WriteLog(json);

            var response = await restful.PostHttpAsync<PickingResultsImport>(pickingResultsImport) as WcsResponse;

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

        private async void buttonTestOrder_Click(object sender, EventArgs e)
        {
            string metafile = $"D:\\Test\\order_meta.csv";
            string datafile = $"D:\\Test\\order_data.csv";

            if (File.Exists(metafile) == false) return;
            if (File.Exists(datafile) == false) return;


            var metaCsv = File.ReadAllText(metafile);
            var dataCsv = File.ReadAllText(datafile);

            Order order = new Order();
            {
                var splited = metaCsv.Split(',');
                var fields = order.meta.GetType().GetFields();

                for (int i = 0; i < splited.Length; i++)
                    fields[i].SetValue(order.meta, splited[i]);
            }

            {
                var splitedLine = dataCsv.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                for (int i = 0; i < splitedLine.Length; i++)
                {
                    Order.DataClass data = new Order.DataClass();
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
                    order.data.Add(data);
                }

            }

            foreach (var d in order.data)
            {
                string zplFile = $"D:\\Test\\Zpl\\{d.INVOICE_ID}.txt";
                if (File.Exists(zplFile))
                    d.INVOICE_ZPL_300DPI = File.ReadAllText(zplFile);
            }

            RestfulWcsRequester restful = new RestfulWcsRequester(this.textBoxUrl.Text, "api/v1/ecs/order");

            var response = await restful.PostHttpAsync<Order>(order) as WcsResponse;

            if (response != null)
                this.WriteLog($"Recived : {response.ToJson()}");

        }
    }
}
