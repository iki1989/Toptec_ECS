using ECS.Core.Equipments;
using ECS.Core.Managers;
using ECS.Model.Databases;
using ECS.Model.Restfuls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Urcis.SmartCode.Threading;

namespace ECS.Core.ViewModels.Server
{
    public class WcsForcePostViewModel : Notifier
    {
        private RestfulRequsetWcsManager restful = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetWcsManager>();
        private DataBaseManagerForServer db = EcsServerAppManager.Instance.DataBaseManagerForServer;

        #region Nested
        public class DataClass
        {
            #region Prop
            public string wh_id { get; set; }

            public string cst_cd { get; set; }

            public string wave_no { get; set; }

            public string wave_line_no { get; set; }

            public string ord_no { get; set; }

            public string ord_line_no { get; set; }

            public string box_id { get; set; }

            public string box_no { get; set; }

            public string store_loc_cd { get; set; }

            public string box_type { get; set; }

            public string invoice_id { get; set; }

            public double box_wt { get; set; }
            #endregion

            #region Ctor
            public DataClass(SqlDataReader reader)
            {
                this.wh_id = reader["WH_ID"]?.ToString() ?? "";
                this.cst_cd = reader["CST_CD"]?.ToString() ?? "";
                this.wave_no = reader["WAVE_NO"]?.ToString() ?? "";
                this.wave_line_no = reader["WAVE_LINE_NO"]?.ToString() ?? "";
                this.ord_no = reader["ORD_NO"]?.ToString() ?? "";
                this.ord_line_no = reader["ORD_LINE_NO"]?.ToString() ?? "";
                this.box_id = reader["BOX_ID"]?.ToString() ?? "";
                this.box_no = reader["BOX_NO"]?.ToString() ?? "";
                this.store_loc_cd = reader["STORE_LOC_CD"]?.ToString() ?? "";
                this.box_type = reader["BOX_TYPE_CD"]?.ToString() ?? "";
                this.invoice_id = reader["INVOICE_ID"]?.ToString() ?? "";

                if (double.TryParse(reader["WEIGHT_SUM"]?.ToString(), out double weight_sum))
                    this.box_wt = weight_sum;
            }

            public DataClass() { }
            #endregion
        }
        #endregion

        private WeightAndInvoice.DataClass GetWcsDataClass(DataClass data)
        {
            WeightAndInvoice.DataClass d = new WeightAndInvoice.DataClass();
            d.wh_id = data.wh_id;
            d.cst_cd = data.cst_cd;
            d.wave_no = data.wave_no;
            d.wave_line_no = data.wave_line_no;
            d.ord_no = data.ord_no;
            d.ord_line_no = data.ord_line_no;
            d.box_id = data.box_id;
            d.box_no = data.box_no;
            d.store_loc_cd = data.store_loc_cd;
            d.box_type = data.box_type;
            d.invoice_id = data.invoice_id;
            d.box_wt = data.box_wt;

            return d;
        }

        public void WeightPostAsync(DataClass data)
        {
            ScTask.Run(() => this.WeightPost(data));
        }

        public void WeightPost(DataClass data)
        {
            WeightAndInvoice weightInvoice = new WeightAndInvoice();

            WeightAndInvoice.DataClass d = this.GetWcsDataClass(data);
            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<DynamicScaleEquipment>();
            if (eq != null)
                d.eqp_id = eq.Id;
            
            d.floor = "2";
            d.status = ((int)statusEnum.inspect_pass_scale).ToString();
            d.result_cd = "E000";
            d.result_text = "정상처리";

            weightInvoice.data.Add(d);

            if (this.restful != null)
                this.restful.WeightPostAsync(weightInvoice);
           
        }

        public void InvoicePostAsync(DataClass data)
        {
            ScTask.Run(() => this.InvoicePost(data));
        }

        public void InvoicePost(DataClass data)
        {
            WeightAndInvoice weightInvoice = new WeightAndInvoice();

            WeightAndInvoice.DataClass d = this.GetWcsDataClass(data);
            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TopBcrEquipment>();
            if (eq != null)
                d.eqp_id = eq.Id;

            d.floor = "2";
            d.status = ((int)statusEnum.top_invoice).ToString();
            d.result_cd = "E000";
            d.result_text = "정상처리";
            weightInvoice.data.Add(d);

            if (this.restful != null)
                this.restful.InvoicePostAsync(weightInvoice);

        }
        public async Task<DataClass> GetOrderDataAsync(string boxId, string invoiceId)
        {
            return await Task.Run(() => this.GetOrderData(boxId, invoiceId));
        }

        private DataClass GetOrderData(string boxId, string invoiceId)
        {
            //비정상 Flow이기 때문에 프로시저 등록 안함!

            using (var connection = new SqlConnection(this.db.Setting.SqlConnectionStringBuilder.ConnectionString))
            {
                string sql = $@"SELECT TOP 1
       h_invoice_info.WH_ID
      ,h_invoice_info.CST_CD
      ,h_invoice_info.WAVE_NO
      ,h_invoice_info.WAVE_LINE_NO
      ,h_invoice_info.ORD_NO
      ,h_invoice_info.ORD_LINE_NO
	  ,box.BOX_ID
	  ,h_invoice_info.INVOICE_ID
      ,h_invoice_info.BOX_NO
      ,h_invoice_info.STORE_LOC_CD
      ,h_invoice_info.BOX_TYPE_CD
	  ,invoice.WEIGHT_SUM
  FROM (h_invoice_info
		INNER JOIN invoice
		ON	h_invoice_info.INVOICE_ID = invoice.INVOICE_ID)
  INNER JOIN box
  ON invoice.INVOICE_ID = box.INVOICE_ID
  WHERE h_invoice_info.INVOICE_ID = '{invoiceId}'
  OR BOX_ID = '{boxId}'";

                using (var cmd = new SqlCommand(sql))
                {
                    cmd.Connection = connection;

                    try
                    {
                        connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var data = new DataClass(reader);
                                connection.Close();

                                return data;
                            }
                        }
                    }
                    catch { }
                }
            }
            return null;
        }
    }
}
