using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode.Threading;

using ECS.Model.Hub;
using ECS.Model.Restfuls;
using ECS.Model;
using System.Globalization;
using Newtonsoft.Json;
using Urcis.SmartCode;
using ECS.Model.Databases;
using ECS.Model.Domain.Touch;

namespace ECS.Core.Managers
{
    public class DataBaseManagerForServer : DataBaseManager
    {
        #region field
        private SqlCommand m_SelectAfterPickingBoxIdCmd;

        private SqlCommand m_SelectAfterPickingCmd;
        private SqlCommand m_DeleteByDayAfterCmd;
        private SqlCommand m_SelectCanceledInvoiceByIdCmd;
        //wcs
        private SqlCommand m_InsertMetaDataCmd;
        private SqlCommand m_InsertOrderDataCmd;
        private SqlCommand m_InsertInvoiceCmd;
        private SqlCommand m_InsertOrderedSkuCmd;
        private SqlCommand m_InsertOrderCancelCmd;
        private SqlCommand m_UpdateOrderCancelCmd;
        private SqlCommand m_InsertOrderDeleteCmd;
        private SqlCommand m_InsertOrderDeleteListCmd;
        private SqlCommand m_DeleteOrderByIdCmd;
        private SqlCommand m_DeleteOrderByWaveCmd;
        private SqlCommand m_InsertSkuMasterCmd;
        private SqlCommand m_InsertManualWeightCheckCmd;
        //ricp
        private SqlCommand m_InsertRicpLogCmd;
        private SqlCommand m_InsertPickingCmd;
        private SqlCommand m_UpdateBoxInvoiceCmd;
        //InsertBox
        private SqlCommand m_InsertBoxCmd;
        //WeightCheck
        private SqlCommand m_InsertWeightCheckCmd;
        //invoice bcr
        private SqlCommand m_InsertRouteCmd;
        private SqlCommand m_InsertPrintCmd;
        private SqlCommand m_InsertTopCmd;
        private SqlCommand m_InsertOutCmd;
        //smart packing
        private SqlCommand m_SelectManualPackingCmd;
        private SqlCommand m_InsertSmartPackingCmd;
        private SqlCommand m_UpdateSmartPackingCmd;
        //location status
        private SqlCommand m_UpdatetLocationStatusCmd;
        #endregion

        #region constructor
        public DataBaseManagerForServer(DataBaseManagerSetting setting) : base(setting)
        {
            this.Logger.Setting.KeepingDays = EcsServerAppManager.Instance.Logger.Setting.KeepingDays;
            this.InitSqlCommands();
        }
        public DataBaseManagerForServer() : this(new DataBaseManagerSetting()) { }
        #endregion

        #region method

        #region private
        private void InitSqlCommands()
        {
            this.m_SelectAfterPickingBoxIdCmd = new SqlCommand("SELECT_AFTER_PICKING_BOXID") { CommandType = CommandType.StoredProcedure };
            this.m_SelectAfterPickingBoxIdCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);

            this.m_SelectAfterPickingCmd = new SqlCommand("SELECT_AFTER_PICKING") { CommandType = CommandType.StoredProcedure };

            this.m_DeleteByDayAfterCmd = new SqlCommand("DELETE_BY_DAY_AFTER") { CommandType = CommandType.StoredProcedure };
            this.m_DeleteByDayAfterCmd.Parameters.Add("@DATE", SqlDbType.DateTime);

            this.m_SelectCanceledInvoiceByIdCmd = new SqlCommand("SELECT_CANCELED_INVOICE_BY_ID") { CommandType = CommandType.StoredProcedure };
            this.m_SelectCanceledInvoiceByIdCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);

            this.InitWcsCommand();
            this.InitRicpCommand();
            // Insert Box
            this.InitInsertBox();
            // Mapping Invoice
            // Weight Check
            this.InitInsertWeightCheck();
            // Invoice Bcr
            this.InitInsertRoute();
            this.InitInsertPrint();
            this.InitInsertTop();
            this.InitInsertOut();
            //smart packing
            this.InitSmartPackingCommand();
            //location status
            this.InitUpdatelocationstatus();

        }
        private DataTable SelectAfterPickingBoxId(SqlConnection connection, string boxId)
        {
            var methodName = nameof(SelectAfterPickingBoxId);
            this.Logger.Write($"{methodName} Request");
            var table = new DataTable();

            lock (this.m_SelectAfterPickingBoxIdCmd)
            {
                this.m_SelectAfterPickingBoxIdCmd.Connection = connection;
                this.m_SelectAfterPickingBoxIdCmd.Parameters["@BOX_ID"].Value = boxId;
                try
                {
                    using (var reader = this.m_SelectAfterPickingBoxIdCmd.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {this.DataTableToString(table)}");
                return table;
            }
        }
        private DataTable SelectAfterPicking(SqlConnection connection)
        {
            var methodName = nameof(SelectAfterPicking);
            this.Logger.Write($"{methodName} Request");
            var table = new DataTable();

            lock (this.m_SelectAfterPickingCmd)
            {
                this.m_SelectAfterPickingCmd.Connection = connection;
                try
                {
                    using (var reader = this.m_SelectAfterPickingCmd.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {this.DataTableToString(table)}");
                return table;
            }
        }
        private bool DeleteByDayAfter(SqlConnection connection, DateTime date)
        {
            var methodName = nameof(DeleteByDayAfter);
            this.Logger.Write($"{methodName} Request : date = {date}");
            var res = false;

            lock (this.m_DeleteByDayAfterCmd)
            {
                this.m_DeleteByDayAfterCmd.Connection = connection;
                this.m_DeleteByDayAfterCmd.Parameters["@DATE"].Value = date;
                try
                {
                    using (var reader = this.m_DeleteByDayAfterCmd.ExecuteReader())
                    {
                        if (reader.Read() && reader[0].ToString() == "Y")
                            res = true;
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {res}");
                return res;
            }
        }
        private CanceledInvoice SelectCanceledInvoiceById(SqlConnection connection, string boxId)
        {
            var methodName = nameof(SelectCanceledInvoiceById);
            this.Logger.Write($"{methodName} Request : boxId = {boxId}");
            CanceledInvoice res = null;

            lock (this.m_SelectCanceledInvoiceByIdCmd)
            {
                this.m_SelectCanceledInvoiceByIdCmd.Connection = connection;
                this.m_SelectCanceledInvoiceByIdCmd.Parameters["@BOX_ID"].Value = boxId;
                try
                {
                    using (var reader = this.m_SelectCanceledInvoiceByIdCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res = new CanceledInvoice();
                            res.WH_ID = reader["WH_ID"].ToString();
                            res.CST_CD = reader["CST_CD"].ToString();
                            res.WAVE_NO = reader["WAVE_NO"].ToString();
                            res.WAVE_LINE_NO = reader["WAVE_LINE_NO"].ToString();
                            res.ORD_NO = reader["ORD_NO"].ToString();
                            res.ORD_LINE_NO = reader["ORD_LINE_NO"].ToString();
                            res.BOX_NO = reader["BOX_NO"].ToString();
                            res.STORE_LOC_CD = reader["STORE_LOC_CD"].ToString();
                            res.BOX_TYPE = reader["BOX_TYPE"].ToString();
                            res.INVOICE_ID = reader["INVOICE_ID"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }

                if (res != null)
                    this.Logger.Write($"{methodName} Response : {JsonConvert.SerializeObject(res)}");

                return res;
            }
        }

        #region wcs
        private void InitWcsCommand()
        {
            this.InitInsertMetaData();
            this.InitInsertOrderData();
            this.InitInsertInvoice();
            this.InitInsertOrderedSku();
            this.InitOrderCancelCommand();
            this.InitOrderDeleteCommand();
            this.InitSkuMasterCommand();
            this.InitManualWeightCheckCommand();
        }

        #region InsertOrder
        private void InitInsertMetaData()
        {
            this.m_InsertMetaDataCmd = new SqlCommand("INSERT_META_DATA") { CommandType = CommandType.StoredProcedure };
            this.m_InsertMetaDataCmd.Parameters.Add("@META_NO", SqlDbType.BigInt);
            this.m_InsertMetaDataCmd.Parameters.Add("@META_FROM", SqlDbType.VarChar);
            this.m_InsertMetaDataCmd.Parameters.Add("@META_TO", SqlDbType.VarChar);
            this.m_InsertMetaDataCmd.Parameters.Add("@META_GROUP_CD", SqlDbType.VarChar);
            this.m_InsertMetaDataCmd.Parameters.Add("@META_SEQ", SqlDbType.VarChar);
            this.m_InsertMetaDataCmd.Parameters.Add("@META_TOTAL", SqlDbType.VarChar);
            this.m_InsertMetaDataCmd.Parameters.Add("@META_COMPLETE_YN", SqlDbType.VarChar);
        }
        private void InitInsertOrderData()
        {
            this.m_InsertOrderDataCmd = new SqlCommand("INSERT_ORDER_DATA") { CommandType = CommandType.StoredProcedure };
            this.m_InsertOrderDataCmd.Parameters.Add("@WH_ID", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@CST_CD", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@SND_NM", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@WAVE_NO", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@WAVE_LINE_NO", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@ORD_NO", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@ORD_LINE_NO", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@BOX_NO", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@STORE_LOC_CD", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@STANDARD_WHT", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@BOX_TYPE_CD", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@STATUS", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@EQP_ID", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@JOB_DATE", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@ORDER_DATE", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@CST_ORD_NO", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@CST_ORD_LINE_NO", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@ORDER_DETAIL_ID", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@ORDER_CLASS", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@MULTI_BOX", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@FRE_DRY_QTY", SqlDbType.Decimal);
            this.m_InsertOrderDataCmd.Parameters.Add("@FRE_PACK_QTY", SqlDbType.Decimal);
            this.m_InsertOrderDataCmd.Parameters.Add("@REF_DRY_QTY", SqlDbType.Decimal);
            this.m_InsertOrderDataCmd.Parameters.Add("@REF_PACK_QTY", SqlDbType.Decimal);
            this.m_InsertOrderDataCmd.Parameters.Add("@DIVIDER", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@MNL_PACKING_FLAG", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@EMPTY_RATE", SqlDbType.Int);
            this.m_InsertOrderDataCmd.Parameters.Add("@PACK_TYPE", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@BOX_IN_QTY", SqlDbType.Decimal);
            this.m_InsertOrderDataCmd.Parameters.Add("@SKU_BARCD", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@SKU_CD", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@SKU_NM", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@QTY_UNIT", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@SKU_QTY", SqlDbType.Decimal);
            this.m_InsertOrderDataCmd.Parameters.Add("@STORE_ZONE_CD", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@INVOICE_ZPL_200DPI", SqlDbType.Text);
            this.m_InsertOrderDataCmd.Parameters.Add("@INVOICE_ZPL_300DPI", SqlDbType.Text);
            this.m_InsertOrderDataCmd.Parameters.Add("@ALLOC_NO", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@PASS_STOP_CD", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@PASS_STOP_NM", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@PAYMENT_TYPE", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@SND_CUST_NO", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@B2C_CUST_ID", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@B2C_CUST_MGR_ID", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@RESV_TYPE", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@ORDER_SKU_STATUS", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@BUYER_PO_CD", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@DLV_CLS_CD", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@DLV_SUB_CLS_CD", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@DELIVERY_TYPE", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@REG_DT", SqlDbType.Text);
            this.m_InsertOrderDataCmd.Parameters.Add("@RSTR_ID", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@UPD_DT", SqlDbType.Text);
            this.m_InsertOrderDataCmd.Parameters.Add("@UPDR_ID", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@IF_TXN_TYPE_FL", SqlDbType.VarChar);
            this.m_InsertOrderDataCmd.Parameters.Add("@IF_TXN_DATE", SqlDbType.Text);
            this.m_InsertOrderDataCmd.Parameters.Add("@META_INDEX", SqlDbType.BigInt);
        }
        private void InitInsertInvoice()
        {
            this.m_InsertInvoiceCmd = new SqlCommand("INSERT_INVOICE") { CommandType = CommandType.StoredProcedure };
            this.m_InsertInvoiceCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@WH_ID", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@CST_CD", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@WAVE_NO", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@WAVE_LINE_NO", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@ORD_NO", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@ORD_LINE_NO", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@BOX_NO", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@STORE_LOC_CD", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@BOX_TYPE_CD", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@EQP_ID", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@CST_ORD_NO", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@MNL_PACKING_FLAG", SqlDbType.VarChar);
            this.m_InsertInvoiceCmd.Parameters.Add("@INVOICE_ZPL_300DPI", SqlDbType.Text);
        }
        private void InitInsertOrderedSku()
        {
            this.m_InsertOrderedSkuCmd = new SqlCommand("INSERT_ORDERED_SKU") { CommandType = CommandType.StoredProcedure };
            this.m_InsertOrderedSkuCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            this.m_InsertOrderedSkuCmd.Parameters.Add("@SKU_CD", SqlDbType.VarChar);
            this.m_InsertOrderedSkuCmd.Parameters.Add("@SKU_QTY", SqlDbType.VarChar);
            this.m_InsertOrderedSkuCmd.Parameters.Add("@STANDARD_WHT", SqlDbType.VarChar);
        }
        private DataTable GetInvoices(List<Order.DataClass> data)
        {
            var dataType = typeof(Order.DataClass);
            using (var table = new DataTable())
            {
                foreach (SqlParameter param in m_InsertInvoiceCmd.Parameters)
                    table.Columns.Add(param.ParameterName.Substring(1));
                foreach (var d in data)
                {
                    var row = table.NewRow();
                    foreach (DataColumn col in table.Columns)
                    {
                        var val = dataType.GetField(col.ColumnName)?.GetValue(d) ?? DBNull.Value;
                        row[col.ColumnName] = val;
                    }
                    table.Rows.Add(row);
                }
                return table.DefaultView.ToTable(true);
            }
        }
        private DataTable GetOrderedSkus(List<Order.DataClass> data)
        {
            var dataType = typeof(Order.DataClass);
            var table = new DataTable();
            foreach (SqlParameter param in m_InsertOrderedSkuCmd.Parameters)
                table.Columns.Add(param.ParameterName.Substring(1));
            foreach (var d in data)
            {
                var row = table.NewRow();
                foreach (DataColumn col in table.Columns)
                {
                    var val = dataType.GetField(col.ColumnName)?.GetValue(d) ?? DBNull.Value;
                    row[col.ColumnName] = val;
                }
                table.Rows.Add(row);
            }
            return table;
        }
        private long InsertMetaData(SqlConnection connection, WcsFormat.MetaClass meta)
        {
            lock (this.m_InsertMetaDataCmd)
            {
                var methodName = nameof(InsertMetaData);
                this.Logger.Write($"{methodName} Request : {JsonConvert.SerializeObject(meta)}");
                long result = -1;
                var metaType = typeof(Order.MetaClass);
                this.m_InsertMetaDataCmd.Connection = connection;
                try
                {
                    foreach (SqlParameter param in this.m_InsertMetaDataCmd.Parameters)
                    {
                        param.Value = metaType.GetField(param.ParameterName.Substring(1).ToLower())?.GetValue(meta) ?? DBNull.Value;
                    }
                    using (var reader = this.m_InsertMetaDataCmd.ExecuteReader())
                    {
                        if (reader.Read())
                            result = long.Parse(reader[0].ToString());
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {result}");
                return result;
            }
        }
        private void InsertOrderData(SqlConnection connection, List<Order.DataClass> data, long metaIndex)
        {
            lock (this.m_InsertOrderDataCmd)
            {
                var methodName = nameof(InsertOrderData);
                this.Logger.Write($"{methodName} Request : {JsonConvert.SerializeObject(data)}, {metaIndex}");
                var dataType = typeof(Order.DataClass);
                this.m_InsertOrderDataCmd.Connection = connection;

                foreach (var d in data)
                {
                    try
                    {
                        foreach (SqlParameter param in this.m_InsertOrderDataCmd.Parameters)
                        {
                            param.Value = dataType.GetField(param.ParameterName.Substring(1))?.GetValue(d) ?? DBNull.Value;
                        }
                        this.m_InsertOrderDataCmd.Parameters["@META_INDEX"].Value = metaIndex;
                        this.m_InsertOrderDataCmd.ExecuteNonQuery();
                        this.Logger.Write($"{methodName} : {JsonConvert.SerializeObject(d)}");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Write($"{methodName} Exception In Process : {JsonConvert.SerializeObject(d)}");
                        this.ErrorLog(ex);
                    }
                }
                this.Logger.Write($"{methodName} Response");
            }
        }
        private void InsertInvoice(SqlConnection connection, DataTable invoices)
        {
            var methodName = nameof(InsertInvoice);
            this.Logger.Write($"{methodName} Request : {JsonConvert.SerializeObject(invoices)}");
            lock (this.m_InsertInvoiceCmd)
            {
                this.m_InsertInvoiceCmd.Connection = connection;

                foreach (var invoice in invoices.AsEnumerable())
                {
                    try
                    {
                        foreach (var col in invoices.Columns)
                        {
                            this.m_InsertInvoiceCmd.Parameters[$"@{col}"].Value = invoice[col.ToString()];
                        }
                        this.m_InsertInvoiceCmd.ExecuteNonQuery();
                        this.Logger.Write($"{methodName} : {JsonConvert.SerializeObject(invoice)}");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Write($"{methodName} Exception In Process : {JsonConvert.SerializeObject(invoice)}");
                        this.ErrorLog(ex);
                    }
                }
                invoices?.Dispose();
                this.Logger.Write($"{methodName} Response");
            }
        }
        private void InsertOrderedSku(SqlConnection connection, DataTable orderedSkus)
        {
            var methodName = nameof(InsertOrderedSku);
            this.Logger.Write($"{methodName} Request : {JsonConvert.SerializeObject(orderedSkus)}");

            lock (this.m_InsertOrderedSkuCmd)
            {
                this.m_InsertOrderedSkuCmd.Connection = connection;

                foreach (var sku in orderedSkus.AsEnumerable())
                {
                    try
                    {
                        foreach (var col in orderedSkus.Columns)
                        {
                            this.m_InsertOrderedSkuCmd.Parameters[$"@{col}"].Value = sku[col.ToString()];
                        }
                        this.m_InsertOrderedSkuCmd.ExecuteNonQuery();
                        this.Logger.Write($"{methodName} : {JsonConvert.SerializeObject(sku)}");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Write($"{methodName} Exception In Process : {JsonConvert.SerializeObject(sku)}");
                        this.ErrorLog(ex);
                    }
                }

                orderedSkus?.Dispose();
                this.Logger.Write($"{methodName} Response");
            }
        }
        #endregion
        private DataTable InsertPicking(SqlConnection connection, Picking picking)
        {
            var methodName = nameof(InsertPicking);
            this.Logger.Write($"{methodName} By Wcs Request : {JsonConvert.SerializeObject(picking)}");
            var metaIndex = this.InsertMetaData(connection, picking.meta);
            var dataType = typeof(Picking.DataClass);

            lock (this.m_InsertPickingCmd)
            {
                this.m_InsertPickingCmd.Connection = connection;
                foreach (var d in picking.data)
                {
                    try
                    {
                        foreach (SqlParameter param in this.m_InsertPickingCmd.Parameters)
                        {
                            param.Value = dataType.GetField(param.ParameterName.Substring(1))?.GetValue(d) ?? DBNull.Value;
                        }
                        this.m_InsertPickingCmd.Parameters["@META_INDEX"].Value = metaIndex;
                        this.m_InsertPickingCmd.ExecuteNonQuery();
                        this.Logger.Write($"{methodName} : {JsonConvert.SerializeObject(d)}");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Write($"{methodName} Exception In Process : {JsonConvert.SerializeObject(d)}");
                        this.ErrorLog(ex);
                    }
                }
            }

            return this.UpdateBoxInvoice(connection, (from d in picking.data
                                                      select (d.BOX_ID, d.INVOICE_ID, d.WT_CHECK_FLAG)).Distinct(), "Y");
        }
        private void InitOrderCancelCommand()
        {
            this.m_InsertOrderCancelCmd = new SqlCommand("INSERT_ORDER_CANCEL") { CommandType = CommandType.StoredProcedure };
            this.m_InsertOrderCancelCmd.Parameters.Add("@WH_ID", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@CST_CD", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@WAVE_NO", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@WAVE_LINE_NO", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ORD_NO", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ORD_LINE_NO", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@BOX_NO", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@STORE_LOC_CD", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@STANDARD_WHT", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@CST_ORD_NO", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@CST_ORD_LINE_NO", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@SKU_CD", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@SKU_QTY", SqlDbType.Decimal);
            this.m_InsertOrderCancelCmd.Parameters.Add("@BOX_TYPE_CD", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@STATUS", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@EQP_ID", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ATTRIBUTE01", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ATTRIBUTE02", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ATTRIBUTE03", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ATTRIBUTE04", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ATTRIBUTE05", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ATTRIBUTE06", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ATTRIBUTE07", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ATTRIBUTE08", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ATTRIBUTE09", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@ATTRIBUTE10", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@REG_DT", SqlDbType.Text);
            this.m_InsertOrderCancelCmd.Parameters.Add("@RSTR_ID", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@UPD_DT", SqlDbType.Text);
            this.m_InsertOrderCancelCmd.Parameters.Add("@UPDR_ID", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@IF_TXN_TYPE_FL", SqlDbType.VarChar);
            this.m_InsertOrderCancelCmd.Parameters.Add("@IF_TXN_DATE", SqlDbType.Text);
            this.m_InsertOrderCancelCmd.Parameters.Add("@META_INDEX", SqlDbType.BigInt);

            this.m_UpdateOrderCancelCmd = new SqlCommand("UPDATE_ORDER_CANCEL") { CommandType = CommandType.StoredProcedure };
            this.m_UpdateOrderCancelCmd.Parameters.Add("@BOX_NO", SqlDbType.VarChar);
        }
        private bool InsertOrderCancel(SqlConnection connection, OrderCancel cancel)
        {
            lock (this.m_InsertOrderCancelCmd)
            {
                var methodName = nameof(InsertOrderCancel);
                this.Logger.Write($"{methodName} Request : {JsonConvert.SerializeObject(cancel)}");
                var res = true;
                var metaIndex = this.InsertMetaData(connection, cancel.meta);
                var dataType = typeof(OrderCancel.DataClass);
                this.m_InsertOrderCancelCmd.Connection = connection;

                foreach (var d in cancel.data)
                {
                    try
                    {
                        foreach (SqlParameter param in this.m_InsertOrderCancelCmd.Parameters)
                        {
                            param.Value = dataType.GetField(param.ParameterName.Substring(1))?.GetValue(d) ?? DBNull.Value;
                        }
                        this.m_InsertOrderCancelCmd.Parameters["@META_INDEX"].Value = metaIndex;
                        using (var reader = this.m_InsertOrderCancelCmd.ExecuteReader())
                        {
                            if (reader.Read() && reader[0].ToString() == "N")
                                res = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Write($"{methodName} Exception In Process : {JsonConvert.SerializeObject(d)}");
                        this.ErrorLog(ex);
                        res = false;
                    }
                }
                this.Logger.Write($"{methodName} Response : {res}");
                return res;
            }
        }
        private bool UpdateOrderCancel(SqlConnection connection, string boxNo)
        {
            lock (this.m_UpdateOrderCancelCmd)
            {
                var methodName = nameof(UpdateOrderCancel);
                var res = false;
                this.Logger.Write($"{methodName} Request : invoiceId = {boxNo}");
                this.m_UpdateOrderCancelCmd.Connection = connection;
                try
                {
                    this.m_UpdateOrderCancelCmd.Parameters["@BOX_NO"].Value = boxNo;
                    using (var reader = this.m_UpdateOrderCancelCmd.ExecuteReader())
                    {
                        if (reader.Read())
                            res = reader[0].ToString() == "Y";
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }

                this.Logger.Write($"{methodName} Response : {res}");
                return res;
            }
        }
        private void InitOrderDeleteCommand()
        {
            this.m_InsertOrderDeleteCmd = new SqlCommand("INSERT_ORDER_DELETE") { CommandType = CommandType.StoredProcedure };
            this.m_InsertOrderDeleteCmd.Parameters.Add("@WAVE_NO", SqlDbType.VarChar);
            this.m_InsertOrderDeleteCmd.Parameters.Add("@WAVE_REMOV_FL", SqlDbType.VarChar);
            this.m_InsertOrderDeleteCmd.Parameters.Add("@REG_DT", SqlDbType.Text);
            this.m_InsertOrderDeleteCmd.Parameters.Add("@RSTR_ID", SqlDbType.VarChar);
            this.m_InsertOrderDeleteCmd.Parameters.Add("@UPD_DT", SqlDbType.Text);
            this.m_InsertOrderDeleteCmd.Parameters.Add("@UPDR_ID", SqlDbType.VarChar);
            this.m_InsertOrderDeleteCmd.Parameters.Add("@IF_TXN_TYP_FL", SqlDbType.VarChar);
            this.m_InsertOrderDeleteCmd.Parameters.Add("@IF_TXN_DAT", SqlDbType.Text);
            this.m_InsertOrderDeleteCmd.Parameters.Add("@META_INDEX", SqlDbType.BigInt);


            this.m_InsertOrderDeleteListCmd = new SqlCommand("INSERT_ORDER_DELETE_LIST") { CommandType = CommandType.StoredProcedure };
            this.m_InsertOrderDeleteListCmd.Parameters.Add("@ORD_NO", SqlDbType.VarChar);
            this.m_InsertOrderDeleteListCmd.Parameters.Add("@DELETE_INDEX", SqlDbType.BigInt);

            this.m_DeleteOrderByIdCmd = new SqlCommand("DELETE_ORDER_BY_ID") { CommandType = CommandType.StoredProcedure };
            this.m_DeleteOrderByIdCmd.Parameters.Add("@ORD_NO", SqlDbType.VarChar);

            this.m_DeleteOrderByWaveCmd = new SqlCommand("DELETE_ORDER_BY_WAVE") { CommandType = CommandType.StoredProcedure };
            this.m_DeleteOrderByWaveCmd.Parameters.Add("@WAVE_NO", SqlDbType.VarChar);
        }
        private bool InsertOrderDelete(SqlConnection connection, OrderDelete delete)
        {
            var methodName = nameof(InsertOrderDelete);
            this.Logger.Write($"{methodName} Request : {JsonConvert.SerializeObject(delete)}");
            bool result = false;
            var metaIndex = this.InsertMetaData(connection, delete.meta);
            var dataType = typeof(OrderDelete.DataClass);

            this.m_InsertOrderDeleteCmd.Connection = connection;

            foreach (var d in delete.data)
            {
                long deleteIndex = -1;
                try
                {
                    foreach (SqlParameter param in this.m_InsertOrderDeleteCmd.Parameters)
                    {
                        param.Value = dataType.GetField(param.ParameterName.Substring(1))?.GetValue(d) ?? DBNull.Value;
                    }
                    this.m_InsertOrderDeleteCmd.Parameters["@META_INDEX"].Value = metaIndex;
                    using (var reader = this.m_InsertOrderDeleteCmd.ExecuteReader())
                    {
                        if (reader.Read())
                            deleteIndex = long.Parse(reader[0].ToString());
                    }
                    this.Logger.Write($"{methodName} : {JsonConvert.SerializeObject(d)}");
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception In Process : {JsonConvert.SerializeObject(d)}");
                    this.ErrorLog(ex);
                    continue;
                }
                if (d.WAVE_REMOVE_FL == "Y")
                {
                    try
                    {
                        this.m_DeleteOrderByWaveCmd.Parameters["@WAVE_NO"].Value = d.WAVE_NO;
                        using (var reader = this.m_DeleteOrderByWaveCmd.ExecuteReader())
                        {
                            if (reader.Read())
                                result = reader[0].ToString() == "Y";
                        }
                        this.Logger.Write($"DeleteOrderByWave : {d.WAVE_NO}");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Write($"DeleteOrderByWave Exception In Process :  {d.WAVE_NO}");
                        this.ErrorLog(ex);
                    }
                }
                else
                {
                    result = true;
                    foreach (var ordNo in d.ORD_NO_LIST)
                    {
                        try
                        {
                            this.m_InsertOrderDeleteListCmd.Parameters["@ORD_NO"].Value = ordNo.ORD_NO;
                            this.m_InsertOrderDeleteListCmd.Parameters["@DELETE_INDEX"].Value = deleteIndex;
                            using (var reader = this.m_InsertOrderDeleteListCmd.ExecuteReader())
                            {
                                if (reader.Read() && reader[0].ToString() == "N")
                                    result = false;
                            }
                            this.Logger.Write($"InsertOrderDelete : {ordNo}");
                        }
                        catch (Exception ex)
                        {
                            this.Logger.Write($"InsertOrderDelete Exception In Process :  {ordNo}");
                            this.ErrorLog(ex);
                        }
                    }
                    if (result)
                    {
                        foreach (var ordNo in d.ORD_NO_LIST)
                        {
                            try
                            {
                                this.m_DeleteOrderByIdCmd.Parameters["@ORD_NO"].Value = ordNo.ORD_NO;
                                this.m_DeleteOrderByIdCmd.ExecuteNonQuery();
                                this.Logger.Write($"DeleteOrderById : {ordNo}");
                            }
                            catch (Exception ex)
                            {
                                this.Logger.Write($"DeleteOrderById Exception In Process :  {ordNo}");
                                this.ErrorLog(ex);
                            }
                        }
                    }
                }
            }
            this.Logger.Write($"{methodName} Response : {result}");
            return result;
        }
        private void InitSkuMasterCommand()
        {
            this.m_InsertSkuMasterCmd = new SqlCommand("INSERT_SKU_MASTER") { CommandType = CommandType.StoredProcedure };
            this.m_InsertSkuMasterCmd.Parameters.Add("@WH_ID", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@CST_CD", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_CD", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_CLASS", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_NM", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_NM_ENG", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_NM_LOC", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@DELETE_FLAG", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@PACKING_FLAG", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ITEM_TEMP", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ORD_TYPE", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@WGT_UOM_CD", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@PUR_VENDOR_CD", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@PUR_VENDOR_NM", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@PUR_VENDOR_NM_ENG", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@PUR_VENDOR_NM_LOC", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@LEN_UOM_CD", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@CBM_UOM_CD", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_PRICE", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@STYL_CD", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@STYL_NM", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SIZ_CD", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SIZ_NM", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@CLR_CD", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@CLR_NM", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@BRND_CD", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@BRND_NM", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_TMPT_TYPE_CD", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@EXPIRA_DATE_TERM", SqlDbType.SmallInt);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_BCR_NO1", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_BCR_NO2", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_BCR_NO3", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@BOX_BCR_NO", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@CASE_BCR_NO", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@CASE_YN", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@BOX_IN_QTY", SqlDbType.BigInt);
            this.m_InsertSkuMasterCmd.Parameters.Add("@PLT_BOX_QTY", SqlDbType.BigInt);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_WGT", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@CASE_WGT", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@BOX_WGT", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_VERT_LEN", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_WTH_LEN", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_HGT_LEN", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@SKU_CBM", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@CASE_WTH_LEN", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@CASE_VERT_LEN", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@CASE_HGT_LEN", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@CASE_CBM", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@BOX_VERT_LEN", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@BOX_WTH_LEN", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@BOX_HGT_LEN", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@BOX_CBM", SqlDbType.Decimal);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ATTRIBUTE01", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ATTRIBUTE02", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ATTRIBUTE03", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ATTRIBUTE04", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ATTRIBUTE05", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ATTRIBUTE06", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ATTRIBUTE07", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ATTRIBUTE08", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ATTRIBUTE09", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@ATTRIBUTE10", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@REG_DT", SqlDbType.Text);
            this.m_InsertSkuMasterCmd.Parameters.Add("@RSTR_ID", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@UPD_DT", SqlDbType.Text);
            this.m_InsertSkuMasterCmd.Parameters.Add("@UPDR_ID", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@IF_TXN_TYPE_FL", SqlDbType.VarChar);
            this.m_InsertSkuMasterCmd.Parameters.Add("@IF_TXN_DATE", SqlDbType.Text);
            this.m_InsertSkuMasterCmd.Parameters.Add("@META_INDEX", SqlDbType.BigInt);
        }
        private void InsertSkuMaster(SqlConnection connection, SkuMaster skuMaster)
        {
            var methodName = nameof(InsertSkuMaster);
            this.Logger.Write($"{methodName} Request : {JsonConvert.SerializeObject(skuMaster)}");
            var metaIndex = this.InsertMetaData(connection, skuMaster.meta);
            var dataType = typeof(SkuMaster.DataClass);

            lock (this.m_InsertSkuMasterCmd)
            {
                this.m_InsertSkuMasterCmd.Connection = connection;

                foreach (var d in skuMaster.data)
                {
                    try
                    {
                        foreach (SqlParameter param in this.m_InsertSkuMasterCmd.Parameters)
                        {
                            param.Value = dataType.GetField(param.ParameterName.Substring(1))?.GetValue(d) ?? DBNull.Value;
                        }
                        this.m_InsertSkuMasterCmd.Parameters["@META_INDEX"].Value = metaIndex;
                        this.m_InsertSkuMasterCmd.ExecuteNonQuery();
                        this.Logger.Write($"{methodName} : {JsonConvert.SerializeObject(d)}");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Write($"{methodName} Exception In Process : {JsonConvert.SerializeObject(d)}");
                        this.ErrorLog(ex);
                    }
                }
                this.Logger.Write($"{methodName} Response");
            }
        }
        private void InitManualWeightCheckCommand()
        {
            this.m_InsertManualWeightCheckCmd = new SqlCommand("INSERT_MANUAL_WEIGHT_CHECK") { CommandType = CommandType.StoredProcedure };
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@WH_ID", SqlDbType.VarChar);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@CST_CD", SqlDbType.VarChar);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@ORD_NO", SqlDbType.VarChar);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@WT_CHECK_FLAG", SqlDbType.VarChar);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@EQP_ID", SqlDbType.VarChar);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@REG_DT", SqlDbType.Text);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@RSTR_ID", SqlDbType.VarChar);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@UPD_DT", SqlDbType.Text);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@UPDR_ID", SqlDbType.VarChar);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@IF_TXN_TYPE_FL", SqlDbType.VarChar);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@IF_TXN_DATE", SqlDbType.Text);
            this.m_InsertManualWeightCheckCmd.Parameters.Add("@META_INDEX", SqlDbType.BigInt);
        }
        private void InsertManualWeightCheck(SqlConnection connection, OperatorWeightResult manualWeightCheck)
        {
            var methodName = nameof(InsertManualWeightCheck);
            this.Logger.Write($"{methodName} Request : {JsonConvert.SerializeObject(manualWeightCheck)}");
            var metaIndex = this.InsertMetaData(connection, manualWeightCheck.meta);
            var dataType = typeof(OperatorWeightResult.DataClass);

            lock (this.m_InsertManualWeightCheckCmd)
            {
                this.m_InsertManualWeightCheckCmd.Connection = connection;

                foreach (var d in manualWeightCheck.data)
                {
                    try
                    {
                        foreach (SqlParameter param in this.m_InsertManualWeightCheckCmd.Parameters)
                        {
                            param.Value = dataType.GetField(param.ParameterName.Substring(1))?.GetValue(d) ?? DBNull.Value;
                        }
                        this.m_InsertManualWeightCheckCmd.Parameters["@META_INDEX"].Value = metaIndex;
                        this.m_InsertManualWeightCheckCmd.ExecuteNonQuery();
                        this.Logger.Write($"{methodName} : {JsonConvert.SerializeObject(d)}");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Write($"{methodName} Exception In Process : {JsonConvert.SerializeObject(d)}");
                        this.ErrorLog(ex);
                    }
                }
                this.Logger.Write($"{methodName} Response");
            }
        }

        #endregion

        #region ricp
        private void InitRicpCommand()
        {
            this.m_InsertRicpLogCmd = new SqlCommand("INSERT_RICP_LOG") { CommandType = CommandType.StoredProcedure };

            this.m_InsertPickingCmd = new SqlCommand("INSERT_PICKING") { CommandType = CommandType.StoredProcedure };
            this.m_InsertPickingCmd.Parameters.Add("@WH_ID", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@CST_CD", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@WAVE_NO", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@WAVE_LINE_NO", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ORD_NO", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ORD_LINE_NO", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@BOX_NO", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@STORE_LOC_CD", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@BOX_TYPE_CD", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ORDER_CLASS", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@STATUS", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@EQP_ID", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@CST_ORD_NO", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@CST_ORD_LINE_NO", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@WT_CHECK_FLAG", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@SKU_CD", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@SKU_NM", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@SKU_QTY", SqlDbType.Decimal);
            this.m_InsertPickingCmd.Parameters.Add("@DLV_CLS_CD", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@DLV_SUB_CLS_CD", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@DELIVERY_TYPE", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ATTRIBUTE01", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ATTRIBUTE02", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ATTRIBUTE03", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ATTRIBUTE04", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ATTRIBUTE05", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ATTRIBUTE06", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ATTRIBUTE07", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ATTRIBUTE08", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ATTRIBUTE09", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@ATTRIBUTE10", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@REG_DT", SqlDbType.Text);
            this.m_InsertPickingCmd.Parameters.Add("@RSTR_ID", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@UPD_DT", SqlDbType.Text);
            this.m_InsertPickingCmd.Parameters.Add("@UPDR_ID", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@IF_TXN_TYPE_FL", SqlDbType.VarChar);
            this.m_InsertPickingCmd.Parameters.Add("@IF_TXN_DATE", SqlDbType.Text);
            this.m_InsertPickingCmd.Parameters.Add("@META_INDEX", SqlDbType.BigInt);
            this.m_InsertPickingCmd.Parameters.Add("@RICP_INDEX", SqlDbType.BigInt);

            this.m_UpdateBoxInvoiceCmd = new SqlCommand("UPDATE_BOX_INVOICE") { CommandType = CommandType.StoredProcedure };
            this.m_UpdateBoxInvoiceCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_UpdateBoxInvoiceCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            this.m_UpdateBoxInvoiceCmd.Parameters.Add("@WT_CHECK_FLAG", SqlDbType.VarChar);
            this.m_UpdateBoxInvoiceCmd.Parameters.Add("@IS_WCS", SqlDbType.VarChar);
        }
        private long InsertRicpLog(SqlConnection connection)
        {
            var methodName = nameof(InsertRicpLog);
            this.Logger.Write($"{methodName} Request");
            long result = -1;

            lock (this.m_InsertRicpLogCmd)
            {
                this.m_InsertRicpLogCmd.Connection = connection;
                try
                {
                    using (var reader = this.m_InsertRicpLogCmd.ExecuteReader())
                    {
                        if (reader.Read())
                            result = long.Parse(reader[0].ToString());
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {result}");
                return result;
            }
        }
        private DataTable UpdateBoxInvoice(SqlConnection connection, IEnumerable<(string, string, string)> mappingInfos, string isWcs)
        {
            var methodName = nameof(UpdateBoxInvoice);
            this.Logger.Write($"{methodName} Request : {JsonConvert.SerializeObject(mappingInfos)}, isWcs = {isWcs}");
            var table = new DataTable();

            lock (this.m_UpdateBoxInvoiceCmd)
            {
                this.m_UpdateBoxInvoiceCmd.Connection = connection;
                foreach (var mapping in mappingInfos)
                {
                    try
                    {
                        this.m_UpdateBoxInvoiceCmd.Parameters["@BOX_ID"].Value = mapping.Item1;
                        this.m_UpdateBoxInvoiceCmd.Parameters["@INVOICE_ID"].Value = mapping.Item2;
                        this.m_UpdateBoxInvoiceCmd.Parameters["@WT_CHECK_FLAG"].Value = mapping.Item3;
                        this.m_UpdateBoxInvoiceCmd.Parameters["@IS_WCS"].Value = isWcs;
                        using (var reader = this.m_UpdateBoxInvoiceCmd.ExecuteReader())
                        {
                            table.Load(reader);
                        }
                        this.Logger.Write($"{methodName} : {JsonConvert.SerializeObject(mapping)}");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Write($"{methodName} Exception In Process : {JsonConvert.SerializeObject(mapping)}");
                        this.ErrorLog(ex);
                    }
                }
                this.Logger.Write($"{methodName} Response : {JsonConvert.SerializeObject(table)}");
                return table;
            }
        }
        private DataTable InsertPicking(SqlConnection connection, PickingResultsImport picking)
        {
            var methodName = nameof(InsertPicking);
            this.Logger.Write($"{methodName} Request : {JsonConvert.SerializeObject(picking)}");
            var ricpIndex = this.InsertRicpLog(connection);
            var dataType = typeof(Picking.DataClass);

            lock (this.m_InsertPickingCmd)
            {
                this.m_InsertPickingCmd.Connection = connection;
                foreach (var d in picking.data)
                {
                    try
                    {
                        foreach (SqlParameter param in this.m_InsertPickingCmd.Parameters)
                        {
                            param.Value = dataType.GetField(param.ParameterName.Substring(1))?.GetValue(d) ?? DBNull.Value;
                        }
                        this.m_InsertPickingCmd.Parameters["@RICP_INDEX"].Value = ricpIndex;
                        var reader = this.m_InsertPickingCmd.ExecuteNonQuery();
                        this.Logger.Write($"{methodName} : {JsonConvert.SerializeObject(d)}");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Write($"{methodName} Exception In Process : {JsonConvert.SerializeObject(d)}");
                        this.ErrorLog(ex);
                    }
                }
            }

            return this.UpdateBoxInvoice(connection, (from d in picking.data
                                                      select (d.BOX_ID, d.INVOICE_ID, d.WT_CHECK_FLAG)).Distinct(), "N");
        }
        #endregion

        #region InsertBox
        private void InitInsertBox()
        {
            this.m_InsertBoxCmd = new SqlCommand("INSERT_BOX") { CommandType = CommandType.StoredProcedure };
            this.m_InsertBoxCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_InsertBoxCmd.Parameters.Add("@ERECTOR_TYPE", SqlDbType.VarChar);
        }
        private (string verification, long bcrIndex) InsertBox(SqlConnection connection, string boxId, string erectorType)
        {
            var methodName = nameof(InitInsertBox);
            this.Logger.Write($"{methodName} Request : boxId = {boxId}, erectorType = {erectorType}");
            var result = ("NOREAD", -1L);

            lock (this.m_InsertBoxCmd)
            {
                this.m_InsertBoxCmd.Connection = connection;
                try
                {
                    this.m_InsertBoxCmd.Parameters["@BOX_ID"].Value = boxId;
                    this.m_InsertBoxCmd.Parameters["@ERECTOR_TYPE"].Value = erectorType;
                    using (var reader = this.m_InsertBoxCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = (reader[0].ToString(), long.Parse(reader[1].ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {result}");
                return result;
            }
        }
        #endregion

        #region Weight Check
        private void InitInsertWeightCheck()
        {
            this.m_InsertWeightCheckCmd = new SqlCommand("INSERT_WEIGHT_CHECK") { CommandType = CommandType.StoredProcedure };
            this.m_InsertWeightCheckCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_InsertWeightCheckCmd.Parameters.Add("@WEIGHT", SqlDbType.Float);
            this.m_InsertWeightCheckCmd.Parameters.Add("@VERIFICATION", SqlDbType.VarChar);
        }
        private long InsertWeightCheck(SqlConnection connection, string boxId, double weight, string verification)
        {
            var methodName = nameof(InsertWeightCheck);
            this.Logger.Write($"{methodName} Request : boxId = {boxId}, weight = {weight}, verification = {verification}");
            long result = -1;

            lock (this.m_InsertWeightCheckCmd)
            {
                this.m_InsertWeightCheckCmd.Connection = connection;
                try
                {
                    this.m_InsertWeightCheckCmd.Parameters["@BOX_ID"].Value = boxId;
                    this.m_InsertWeightCheckCmd.Parameters["@WEIGHT"].Value = weight;
                    this.m_InsertWeightCheckCmd.Parameters["@VERIFICATION"].Value = verification;
                    using (var reader = this.m_InsertWeightCheckCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = long.Parse(reader[0].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {result}");
                return result;
            }
        }
        #endregion

        #region Invoice Bcr
        private void InitInsertRoute()
        {
            this.m_InsertRouteCmd = new SqlCommand("INSERT_ROUTE") { CommandType = CommandType.StoredProcedure };
            this.m_InsertRouteCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_InsertRouteCmd.Parameters.Add("@LINE", SqlDbType.VarChar);
        }
        private void InitInsertPrint()
        {
            this.m_InsertPrintCmd = new SqlCommand("INSERT_PRINT") { CommandType = CommandType.StoredProcedure };
            this.m_InsertPrintCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_InsertPrintCmd.Parameters.Add("@ISNORMAL", SqlDbType.VarChar);
            this.m_InsertPrintCmd.Parameters.Add("@RESULT", SqlDbType.VarChar);
        }
        private void InitInsertTop()
        {
            this.m_InsertTopCmd = new SqlCommand("INSERT_TOP") { CommandType = CommandType.StoredProcedure };
            this.m_InsertTopCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_InsertTopCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            this.m_InsertTopCmd.Parameters.Add("@VERIFICATION", SqlDbType.VarChar);
            this.m_InsertTopCmd.Parameters.Add("@IS_REPRINT", SqlDbType.VarChar);
        }
        private void InitInsertOut()
        {
            this.m_InsertOutCmd = new SqlCommand("INSERT_OUT") { CommandType = CommandType.StoredProcedure };
            this.m_InsertOutCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
        }
        private long InsertRoute(SqlConnection connection, string boxId, string line)
        {
            var methodName = nameof(InsertRoute);
            this.Logger.Write($"{methodName} Request : boxId = {boxId}, line = {line}");
            long result = -1;

            lock (this.m_InsertRouteCmd)
            {
                this.m_InsertRouteCmd.Connection = connection;
                try
                {
                    this.m_InsertRouteCmd.Parameters["@BOX_ID"].Value = boxId;
                    this.m_InsertRouteCmd.Parameters["@LINE"].Value = line;
                    using (var reader = this.m_InsertRouteCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = long.Parse(reader["BCR_INDEX"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {result}");
                return result;
            }
        }
        private long InsertPrint(SqlConnection connection, string boxId, bool isNormal, string result)
        {
            var methodName = nameof(InsertPrint);
            this.Logger.Write($"{methodName} Request : boxId = {boxId}, isNormal = {isNormal}, result = {result}");
            long res = -1;

            lock (this.m_InsertPrintCmd)
            {
                this.m_InsertPrintCmd.Connection = connection;
                try
                {
                    this.m_InsertPrintCmd.Parameters["@BOX_ID"].Value = boxId;
                    this.m_InsertPrintCmd.Parameters["@ISNORMAL"].Value = isNormal ? "Y" : "N";
                    this.m_InsertPrintCmd.Parameters["@RESULT"].Value = result;
                    using (var reader = this.m_InsertPrintCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res = long.Parse(reader["BCR_INDEX"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {res}");
                return res;
            }
        }
        private long InsertTop(SqlConnection connection, string boxId, string invoiceId, string verification, bool isReprint)
        {
            var methodName = nameof(InsertTop);
            this.Logger.Write($"{methodName} Request : boxId = {boxId}, invoiceId = {invoiceId}, verification = {verification}, isReprint = {isReprint}");
            long res = -1;

            lock (this.m_InsertTopCmd)
            {
                this.m_InsertTopCmd.Connection = connection;
                try
                {
                    this.m_InsertTopCmd.Parameters["@BOX_ID"].Value = boxId;
                    this.m_InsertTopCmd.Parameters["@INVOICE_ID"].Value = invoiceId;
                    this.m_InsertTopCmd.Parameters["@VERIFICATION"].Value = verification;
                    this.m_InsertTopCmd.Parameters["@IS_REPRINT"].Value = isReprint ? "Y" : "N";
                    using (var reader = this.m_InsertTopCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res = long.Parse(reader["BCR_INDEX"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {res}");
                return res;
            }

        }
        private long InsertOut(SqlConnection connection, string boxId)
        {
            var methodName = nameof(InsertOut);
            this.Logger.Write($"{methodName} Request : boxId = {boxId}");
            long res = -1;

            lock (this.m_InsertOutCmd)
            {
                this.m_InsertOutCmd.Connection = connection;
                try
                {
                    this.m_InsertOutCmd.Parameters["@BOX_ID"].Value = boxId;
                    using (var reader = this.m_InsertOutCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res = long.Parse(reader["BCR_INDEX"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {res}");
                return res;
            }
        }
        #endregion

        #region smart packing
        private void InitSmartPackingCommand()
        {
            this.m_SelectManualPackingCmd = new SqlCommand("SELECT_MANUAL_PACKING") { CommandType = CommandType.StoredProcedure };
            this.m_SelectManualPackingCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);

            this.m_InsertSmartPackingCmd = new SqlCommand("INSERT_SMART_PACKING") { CommandType = CommandType.StoredProcedure };
            this.m_InsertSmartPackingCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_InsertSmartPackingCmd.Parameters.Add("@RESULT", SqlDbType.VarChar);
            this.m_InsertSmartPackingCmd.Parameters.Add("@VOLUME", SqlDbType.Float);
            this.m_InsertSmartPackingCmd.Parameters.Add("@HEIGHT", SqlDbType.Float);

            this.m_UpdateSmartPackingCmd = new SqlCommand("UPDATE_SMART_PACKING") { CommandType = CommandType.StoredProcedure };
            this.m_UpdateSmartPackingCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_UpdateSmartPackingCmd.Parameters.Add("@IS_MANUAL", SqlDbType.Bit);
            this.m_UpdateSmartPackingCmd.Parameters.Add("@PACKING_AMOUNT", SqlDbType.Int);
        }
        private bool SelectManualPacking(SqlConnection connection, string boxId)
        {
            var methodName = nameof(SelectManualPacking);
            var cmd = this.m_SelectManualPackingCmd;
            this.Logger.Write($"{methodName} Request : boxId = {boxId}");
            bool res = false;

            lock (cmd)
            {
                cmd.Connection = connection;
                try
                {
                    if (boxId != null)
                        cmd.Parameters["@BOX_ID"].Value = boxId;
                    else
                        cmd.Parameters["@BOX_ID"].Value = DBNull.Value;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res = reader[0].ToString() == "Y";
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {res}");
                return res;
            }
        }
        private long InsertSmartPacking(SqlConnection connection, string boxId, string result, double? volume, double? height)
        {
            var methodName = nameof(InsertSmartPacking);
            var cmd = this.m_InsertSmartPackingCmd;
            this.Logger.Write($"{methodName} Request : boxId = {boxId}, result = {result}, volume = {volume}, height = {height}");
            long res = -1;

            lock (cmd)
            {
                cmd.Connection = connection;
                try
                {
                    if (boxId != null)
                        cmd.Parameters["@BOX_ID"].Value = boxId;
                    else
                        cmd.Parameters["@BOX_ID"].Value = DBNull.Value;

                    if (result != null)
                        cmd.Parameters["@RESULT"].Value = result;
                    else
                        cmd.Parameters["@RESULT"].Value = DBNull.Value;

                    if (volume != null)
                        cmd.Parameters["@VOLUME"].Value = volume;
                    else
                        cmd.Parameters["@VOLUME"].Value = DBNull.Value;

                    if (height != null)
                        cmd.Parameters["@HEIGHT"].Value = height;
                    else
                        cmd.Parameters["@HEIGHT"].Value = DBNull.Value;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res = long.Parse(reader[0].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {res}");
                return res;
            }
        }
        private SmartPackingData UpdateSmartPacking(SqlConnection connection, string boxId, bool isManual, int packingAmount)
        {
            var methodName = nameof(UpdateSmartPacking);
            var cmd = this.m_UpdateSmartPackingCmd;
            this.Logger.Write($"{methodName} Request : boxId = {boxId}, isManual = {isManual}, packingAmount = {packingAmount}");
            var res = SmartPackingData.None;

            lock (cmd)
            {
                cmd.Connection = connection;
                try
                {
                    if (boxId != null)
                        cmd.Parameters["@BOX_ID"].Value = boxId;
                    else
                        cmd.Parameters["@BOX_ID"].Value = DBNull.Value;
                    cmd.Parameters["@IS_MANUAL"].Value = isManual ? 1 : 0;
                    cmd.Parameters["@PACKING_AMOUNT"].Value = packingAmount;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res.Convert(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Write($"{methodName} Exception");
                    this.ErrorLog(ex);
                }
                this.Logger.Write($"{methodName} Response : {res}");
                return res;
            }
        }
        #endregion

        #region location status
        private void InitUpdatelocationstatus()
        {
            this.m_UpdatetLocationStatusCmd = new SqlCommand("UPDATE_LOCATIONSTATUS") { CommandType = CommandType.StoredProcedure };
            this.m_UpdatetLocationStatusCmd.Parameters.Add("@SHELLCODE", SqlDbType.NChar);
            this.m_UpdatetLocationStatusCmd.Parameters.Add("@PUSHWORKID", SqlDbType.NChar);
            this.m_UpdatetLocationStatusCmd.Parameters.Add("@PUSHWORKSTATUSCD", SqlDbType.NChar);
        }

        private bool Updatelocationstatus(SqlConnection connection, string shellCode, string pushworkid, PushWorkStatusCdEnum pushworkstatuscd)
        {
            lock (this.m_UpdatetLocationStatusCmd)
            {
                this.m_UpdatetLocationStatusCmd.Connection = connection;
                try
                {
                    this.m_UpdatetLocationStatusCmd.Parameters["@SHELLCODE"].Value = shellCode;

                    if (pushworkid == null)
                        this.m_UpdatetLocationStatusCmd.Parameters["@PUSHWORKID"].Value = DBNull.Value;
                    else
                        this.m_UpdatetLocationStatusCmd.Parameters["@PUSHWORKID"].Value = pushworkid;

                    this.m_UpdatetLocationStatusCmd.Parameters["@PUSHWORKSTATUSCD"].Value = pushworkstatuscd;

                    using (var reader = this.m_UpdatetLocationStatusCmd.ExecuteReader())
                    {
                        return reader?.Read() ?? false;
                    }
                }
                catch (Exception ex)
                {
                    this.ErrorLog(ex);
                    return false;
                }
            }
        }
        #endregion

        private void BackupDatabase(string filePath)
        {
            var methodName = nameof(BackupDatabase);
            this.Logger.Write($"{methodName} Request");

            using (SqlCommand backupDatabaseCmd = new SqlCommand("BACKUP_DATABASE") { CommandType = CommandType.StoredProcedure })
            {
                backupDatabaseCmd.Parameters.Add("@FILE_PATH", SqlDbType.VarChar);
                using (var conn = this.GetConnection())
                {
                    backupDatabaseCmd.Connection = conn;

                    try
                    {
                        backupDatabaseCmd.Parameters["@FILE_PATH"].Value = filePath;
                        conn.Open();

                        using (var reader = backupDatabaseCmd.ExecuteReader()) { }
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Write($"{methodName} Exception");
                        this.ErrorLog(ex);
                    }
                    this.Logger.Write($"{methodName} Response");
                }
            }
        }
        #endregion

        #region public
        public DataTable SelectAfterPickingBoxId(string boxId)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectAfterPickingBoxId(conn, boxId);
            }
        }
        public DataTable SelectAfterPicking()
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectAfterPicking(conn);
            }
        }
        public bool DeleteByDayAfter(DateTime date)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.DeleteByDayAfter(conn, date);
            }
        }
        public CanceledInvoice SelectCanceledInvoiceById(string boxId)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectCanceledInvoiceById(conn, boxId);
            }
        }
        //wcs
        public void InsertOrder(Order order)
        {
            var invoices = this.GetInvoices(order.data);
            var orderedSkus = this.GetOrderedSkus(order.data);

            try
            {
                using (var conn = this.GetConnection())
                {
                    conn.Open();
                    long metaIndex = this.InsertMetaData(conn, order.meta);
                    if (metaIndex != -1)
                    {
                        this.InsertOrderData(conn, order.data, metaIndex);
                        this.InsertInvoice(conn, invoices);
                        this.InsertOrderedSku(conn, orderedSkus);
                    }
                }
            }
            finally
            {
                invoices.Dispose();
                orderedSkus.Dispose();
            }
        }

        public void InsertOrderAsync(Order order)
        {
            ScTask.Run(() => this.InsertOrder(order));
        }

        public DataTable InsertPicking(Picking picking)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertPicking(conn, picking);
            }
        }
        public bool InsertOrderCancel(OrderCancel cancel)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertOrderCancel(conn, cancel);
            }
        }
        public bool[] UpdateOrderCancel(string[] boxNos)
        {
            var res = new bool[boxNos.Length];
            using (var conn = this.GetConnection())
            {
                conn.Open();
                for (int i = 0; i < boxNos.Length; ++i)
                {
                    res[i] = this.UpdateOrderCancel(conn, boxNos[i]);
                }
            }
            return res;
        }
        public bool InsertOrderDelete(OrderDelete delete)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertOrderDelete(conn, delete);
            }
        }
        public void InsertSkuMaster(SkuMaster skuMaster)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                this.InsertSkuMaster(conn, skuMaster);
            }
        }
        public void InsertManualWeightCheck(OperatorWeightResult manualWeightCheck)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                this.InsertManualWeightCheck(conn, manualWeightCheck);
            }
        }
        //ricp
        public DataTable InsertPicking(PickingResultsImport picking)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertPicking(conn, picking);
            }
        }
        //제함 bcr 리드
        public (string verification, long bcrIndex) InsertBox(string boxId, string erectorType)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertBox(conn, boxId, erectorType);
            }
        }
        //중량검수
        public long InsertWeightCheck(string boxId, double weight, string verification)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertWeightCheck(conn, boxId, weight, verification);
            }
        }
        //BCR
        public long InsertRoute(string boxId, string line)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertRoute(conn, boxId, line);
            }
        }
        public long InsertPrint(string boxId, bool isNormal, string result)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertPrint(conn, boxId, isNormal, result);
            }
        }
        public long InsertTop(string boxId, string invoiceId, string verification, bool isReprint = false)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertTop(conn, boxId, invoiceId, verification, isReprint);
            }
        }
        public long InsertOut(string boxId)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertOut(conn, boxId);
            }
        }
        //smart packing

        /// <summary>
        /// bypass 확인
        /// </summary>
        /// <param name="boxId"></param>
        /// <returns>bypass일 경우 true, 아닐경우 false</returns>
        public bool SelectManualPacking(string boxId)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectManualPacking(conn, boxId);
            }
        }
        /// <summary>
        /// 스마트 패킹 데이터 추가
        /// </summary>
        /// <param name="boxId">박스ID</param>
        /// <param name="result">결과, OK, BYPASS, NOREAD, NOWEIGHT, WING_FAIL, NOSKU, HEIGHT_OVER, VOLUME_OVER, WEIGHT_FAIL, MULTIERROR</param>
        /// <param name="volume">부피</param>
        /// <param name="height">높이</param>
        /// <returns>추가된 데이터 인덱스</returns>
        public long InsertSmartPacking(string boxId, string result, double? volume, double? height)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertSmartPacking(conn, boxId, result, volume, height);
            }
        }
        /// <summary>
        /// 스마트 패킹 데이터 업데이트
        /// </summary>
        /// <param name="boxId">박스ID</param>
        /// <param name="isManual">수동처리 여부</param>
        /// <param name="packingAmount">충진 수량</param>
        /// <returns>수정된 데이터 인덱스</returns>
        public SmartPackingData UpdateSmartPacking(string boxId, bool isManual, int packingAmount)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.UpdateSmartPacking(conn, boxId, isManual, packingAmount);
            }
        }
        //location status
        public DataTable GetSelectLocationStatusPickingStation(LocationStatusEnum locationStatus)
        {
            DataTable table = null;
            using (var con = this.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT_LOCATIONSTATUS") { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddWithValue("@LocationStatus", (int)locationStatus);

                    try
                    {
                        cmd.Connection = con;
                        using (var reader = cmd.ExecuteReader())
                        {
                            table = new DataTable();
                            table.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ErrorLog(ex);
                    }
                }
            }
            return table;
        }

        public bool Updatelocationstatus(LocationStatus buttonStatus)
        {
            if (buttonStatus == null) return false;

            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.Updatelocationstatus(conn, buttonStatus.ShellCode, buttonStatus.WorkId, buttonStatus.StatusCd);
            }
        }

        public void BackupDatabaseAsync()
        {
            ScTask.Run(() => this.BackupDatabase());
        }

        public void BackupDatabase()
        {
            if (Directory.Exists(EcsAppDirectory.MssqlBackup) == false)
                Directory.CreateDirectory(EcsAppDirectory.MssqlBackup);

            string fileName = $"{this.Setting.SqlConnectionStringBuilder.InitialCatalog} {LocalTime.Now.ToString("yyyy-MM-dd HHmmss")}.bak";
            string filePath = Path.Combine(EcsAppDirectory.MssqlBackup, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Directory.Exists == false)
                fileInfo.Directory.Create();

            this.BackupDatabase(filePath);
        }

        public void DeleteBackupDatabase(int keepingDays)
        {
            SortedList<DateTime, FileInfo> fileInfos = new SortedList<DateTime, FileInfo>();

            string searchPattern = string.Format($"{this.Setting.SqlConnectionStringBuilder.InitialCatalog}*.bak");
            string[] filePathes = Directory.GetFiles(EcsAppDirectory.MssqlBackup, searchPattern, SearchOption.TopDirectoryOnly);

            DateTime latestTime = DateTime.MinValue;
            foreach (string filePath in filePathes)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                TimeSpan span = DateTime.Now - fileInfo.CreationTime;
                if (span.TotalDays > keepingDays)
                    fileInfo.Delete();
            }
        }
        #endregion

        #endregion
    }
}
