using ECS.Core.Restful;
using ECS.Model.Restfuls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvoiceResultSender
{
    public partial class Form1 : Form
    {
        private RestfulWcsRequester restful = new RestfulWcsRequester("http://128.11.3.65", "api/wcs/rsltWaybill");
       
        private SqlConnectionStringBuilder buidler = new SqlConnectionStringBuilder()
        {
            DataSource = "192.168.12.20",
            InitialCatalog = "ECS",
            UserID = "ecsuser",
            Password = "1",
            IntegratedSecurity = false,
        };

        private SqlConnection connection = null;
        private SqlCommand cmd = null;

        public Form1()
        {
            InitializeComponent();

            this.RefreshDatagrid();
        }

        private void RefreshDatagrid()
        {
            this.dataGridView1.Rows.Clear();

            var dt = this.GetTracking();

            int i = 1;
            foreach (var row in dt.AsEnumerable())
            {
                this.dataGridView1.Rows.Add(i, row.ItemArray[4], row.ItemArray[2], row.ItemArray[0]);
                i++;
            }

            this.dataGridView1.Refresh();
        }

        private DataTable GetTracking()
        {
            DataTable table = null;

            using (this.connection = new SqlConnection(buidler.ConnectionString))
            {

                string sql = $@"SELECT * 
FROM(
	SELECT 
		INV.INVOICE_ID,
		ORDER_TIME = INV.CREATED_AT,
		BOX.BOX_ID,
		TOP_TIME = (
			SELECT TOP 1 CREATED_AT 
			FROM h_tracking 
			WHERE 
				INVOICE_ID = INV.INVOICE_ID 
				AND TASK_ID in (61,62)
			ORDER BY [INDEX] DESC),
		OUT_TIME = (
			SELECT TOP 1 CREATED_AT 
			FROM h_tracking 
			WHERE 
				INVOICE_ID = INV.INVOICE_ID 
				AND TASK_ID = 90
			ORDER BY [INDEX] DESC)
	FROM invoice AS INV
	LEFT OUTER JOIN box
		ON box.INVOICE_ID = INV.INVOICE_ID
) AS TR
WHERE ORDER_TIME >= '{DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss")}'
    AND BOX_ID IS NOT NULL
	AND TOP_TIME IS NULL
	AND OUT_TIME IS NOT NULL
ORDER BY ORDER_TIME desc";
                using (this.cmd = new SqlCommand(sql))
                {
                    this.cmd.Connection = this.connection;

                    try
                    {
                        this.connection.Open();

                        using (var reader = this.cmd.ExecuteReader())
                        {
                            table = new DataTable();
                            table.Load(reader);
                            this.connection.Close();
                            return table;
                        }
                    }
                    catch { }
                }
            }
            return table;
        }

        private DataTable GetOrder(string invoiceid)
        {
            DataTable table = null;

            using (this.connection = new SqlConnection(buidler.ConnectionString))
            {

                string sql = $@"SELECT 
       [WH_ID]
      ,[CST_CD]
      ,[WAVE_NO]
      ,[WAVE_LINE_NO]
      ,[ORD_NO]
      ,[ORD_LINE_NO]
      ,[BOX_NO]
      ,[STORE_LOC_CD]
      ,[BOX_TYPE_CD]
	  ,invoice.WEIGHT_SUM
  FROM [ECS].[dbo].[h_order_info]
  INNER JOIN invoice
  on [h_order_info].INVOICE_ID = invoice.INVOICE_ID

  where invoice.INVOICE_ID = '{invoiceid}'";

                using (this.cmd = new SqlCommand(sql))
                {
                    this.cmd.Connection = this.connection;

                    try
                    {
                        this.connection.Open();

                        using (var reader = this.cmd.ExecuteReader())
                        {
                            table = new DataTable();
                            table.Load(reader);
                            this.connection.Close();
                            return table;
                        }
                    }
                    catch { }
                }
            }
            return table;
        }

        private void InsertTracking(string invoiceid)
        {
            using (this.connection = new SqlConnection(buidler.ConnectionString))
            {

                string sql = $@"INSERT INTO [dbo].[h_tracking]
           ([INVOICE_ID] ,[STATUS],[TASK_ID]) VALUES ('{invoiceid}','75','61')";

                using (this.cmd = new SqlCommand(sql))
                {
                    this.cmd.Connection = this.connection;

                    try
                    {
                        this.connection.Open();

                        using (var reader = this.cmd.ExecuteReader())
                        {
                        }
                    }
                    catch { }
                }
            }
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 4) return;

            string invoice = this.dataGridView1.Rows[e.RowIndex].Cells["ColumnInvoiceId"].Value?.ToString();
            if (string.IsNullOrEmpty(invoice))
            {
                this.WriteLog("Invoice is null");
                return;
            }

            var order = this.GetOrder(invoice);
            if (order == null)
            {
                this.WriteLog("Order is null");
                return;
            }

            foreach (var row in order.AsEnumerable())
            {
                WeightAndInvoice weightInvoice = new WeightAndInvoice();
                weightInvoice.meta.meta_from = "GP_ECS_022";
                weightInvoice.meta.meta_to = "GP_WCS_001";
                weightInvoice.meta.meta_group_cd = $"WCS{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                weightInvoice.meta.meta_seq = "1";
                weightInvoice.meta.meta_total = "1";
                weightInvoice.meta.meta_complete_yn = "Y";

                WeightAndInvoice.DataClass data = new WeightAndInvoice.DataClass();
                data.wh_id = row.ItemArray[0].ToString();
                data.cst_cd = row.ItemArray[1].ToString();
                data.wave_no = row.ItemArray[2].ToString();
                data.wave_line_no = row.ItemArray[3].ToString();
                data.ord_no = row.ItemArray[4].ToString();
                data.ord_line_no = row.ItemArray[5].ToString();
                data.box_id = this.dataGridView1.Rows[e.RowIndex].Cells["ColumnBoxId"].Value.ToString();
                data.box_no = row.ItemArray[6].ToString();
                data.store_loc_cd = row.ItemArray[7].ToString();
                data.box_type = row.ItemArray[8].ToString();
                data.floor = "2";
                data.invoice_id = $"{invoice}";
                data.status = "75";
                data.eqp_id = "GUB21";

                if (double.TryParse(row.ItemArray[9].ToString(), out double result))
                    data.box_wt = result;
                
                data.result_cd = "E000";
                data.result_text = "정상처리";

                weightInvoice.data.Add(data);

                string json = JsonConvert.SerializeObject(weightInvoice);
                this.WriteLog(json);

                var response = await this.restful.PostHttpAsync<WeightAndInvoice>(weightInvoice) as WcsResponse;
                if (response != null)
                {
                    this.WriteLog(response.ToJson());
                    if (response.result_cd == (int)ErrorCode.Success)
                    {
                        this.InsertTracking(invoice);
                    }
                }
            }

            this.RefreshDatagrid();
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
    }
}

    
