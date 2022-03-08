using ECS.Model.Domain.Touch;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode.Threading;

namespace ECS.Core.Managers
{
    public class DataBaseManagerForTouch : DataBaseManager
    {
        #region field
        //bcr lcd
        private SqlCommand m_SelectTodayInvoiceBcrCmd;
        private SqlCommand m_SelectInvoiceBcrByIndexCmd;
        private SqlCommand m_SelectOutBcrQueryCmd;
        //invoice reject
        private SqlCommand m_SelectInvoicesByOrderCmd;
        private SqlCommand m_SelectBcrInfoByIdCmd;
        private SqlCommand m_SelectInvoiceReprintCmd;
        private SqlCommand m_InsertInvoiceReprintCmd;
        //weight check
        private SqlCommand m_SelectTodayWeightCmd;
        private SqlCommand m_SelectWeightByIndexCmd;
        private SqlCommand m_SelectWieghtSearchCmd;
        private SqlCommand m_SelectSkuCmd;
        //case erect
        private SqlCommand m_SelectTodayErectCmd;
        private SqlCommand m_SelectErectByIndexCmd;
        private SqlCommand m_SelectCaseErectQueryCmd;
        private SqlCommand m_InsertBoxInfoCmd;
        private SqlCommand m_UpdateBoxInfoCmd;
        private SqlCommand m_UpdateNumberingCmd;
        private SqlCommand m_DeleteBoxInfoCmd;
        private SqlCommand m_ReprintBoxCmd;
        private SqlCommand m_SelectNonVerifiedBoxCmd;
        //smart packing
        private SqlCommand m_SelectTodaySmartPackingCmd;
        private SqlCommand m_SelectSmartPackingByIndexCmd;
        private SqlCommand m_SelectSmartPackingQueryCmd;
        #endregion

        #region constructor
        public DataBaseManagerForTouch(DataBaseManagerSetting setting) : base(setting)
        {
            this.Logger.Setting.KeepingDays = EcsTouchAppManager.Instance.Logger.Setting.KeepingDays;
            this.InitSqlCommands();
        }
        public DataBaseManagerForTouch() : this(new DataBaseManagerSetting()) { }
        #endregion

        #region method

        #region private
        private void InitSqlCommands()
        {
            this.InitBcrLcdCommand();
            this.InitInvoiceRejectCommand();
            this.InitWeightCheckCommand();
            this.InitCaseErectCommand();
            this.InitSmartPackingCommand();
        }

        #region bcr lcd
        private void InitBcrLcdCommand()
        {
            this.m_SelectTodayInvoiceBcrCmd = new SqlCommand("SELECT_TODAY_INVOICE_BCR") { CommandType = CommandType.StoredProcedure };

            this.m_SelectInvoiceBcrByIndexCmd = new SqlCommand("SELECT_INVOICE_BCR_BY_INDEX") { CommandType = CommandType.StoredProcedure };
            this.m_SelectInvoiceBcrByIndexCmd.Parameters.Add("@BCR_INDEX", SqlDbType.BigInt);

            this.m_SelectOutBcrQueryCmd = new SqlCommand("SELECT_OUT_BCR_QUERY") { CommandType = CommandType.StoredProcedure };
            this.m_SelectOutBcrQueryCmd.Parameters.Add("@ID", SqlDbType.VarChar);
            this.m_SelectOutBcrQueryCmd.Parameters.Add("@BEGIN", SqlDbType.Date);
            this.m_SelectOutBcrQueryCmd.Parameters.Add("@END", SqlDbType.Date);
        }

        private List<BcrReadData> SelectTodayInvoiceBcr(SqlConnection connection)
            => this.SelectDataList<BcrReadData>(
                connection,
                nameof(SelectTodayInvoiceBcr),
                this.m_SelectTodayInvoiceBcrCmd);
        private List<BcrReadData> SelectInvoiceBcrByIndex(SqlConnection connection, TouchParam param)
            => this.SelectDataList<BcrReadData>(
                connection,
                nameof(SelectInvoiceBcrByIndex),
                this.m_SelectInvoiceBcrByIndexCmd,
                $" : param = {param}",
                cmd =>
                {
                    cmd.Parameters["@BCR_INDEX"].Value = param.BcrIndex ?? -1;
                });
        private List<BcrReadData> SelectOutBcrQuery(SqlConnection connection, TouchParam param)
            => this.SelectDataList<BcrReadData>(
                connection,
                nameof(SelectOutBcrQuery),
                this.m_SelectOutBcrQueryCmd,
                $" : param = {param}",
                cmd =>
                {
                    cmd.Parameters["@ID"].Value = param.BoxId;
                    cmd.Parameters["@BEGIN"].Value = param.Begin;
                    cmd.Parameters["@END"].Value = param.End;
                });
        #endregion

        #region invoice reject
        private void InitInvoiceRejectCommand()
        {
            this.m_SelectInvoicesByOrderCmd = new SqlCommand("SELECT_INVOICES_BY_ORDER") { CommandType = CommandType.StoredProcedure };
            this.m_SelectInvoicesByOrderCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);

            this.m_SelectBcrInfoByIdCmd = new SqlCommand("SELECT_BCR_INFO_BY_ID") { CommandType = CommandType.StoredProcedure };
            this.m_SelectBcrInfoByIdCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);

            this.m_SelectInvoiceReprintCmd = new SqlCommand("SELECT_INVOICE_REPRINT") { CommandType = CommandType.StoredProcedure };
            this.m_SelectInvoiceReprintCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);

            this.m_InsertInvoiceReprintCmd = new SqlCommand("INSERT_INVOICE_REPRINT") { CommandType = CommandType.StoredProcedure };
            this.m_InsertInvoiceReprintCmd.Parameters.Add("@BCR_INDEX", SqlDbType.BigInt);
            this.m_InsertInvoiceReprintCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
        }
        private List<InvoiceData> SelectInvoicesByOrder(SqlConnection connection, TouchParam param)
            => this.SelectDataList<InvoiceData>(
                 connection,
                 nameof(SelectInvoicesByOrder),
                 this.m_SelectInvoicesByOrderCmd,
                 $" : param = {param}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_ID"].Value = param.BoxId;
                 });
        private List<InvoicePrintData> SelectBcrInfoById(SqlConnection connection, TouchParam param)
            => this.SelectDataList<InvoicePrintData>(
                 connection,
                 nameof(SelectBcrInfoById),
                 this.m_SelectBcrInfoByIdCmd,
                 $" : param = {param}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_ID"].Value = param.BoxId;
                 });
        private List<InvoiceReprintData> SelectInvoiceReprint(SqlConnection connection, TouchParam param)
            => this.SelectDataList<InvoiceReprintData>(
                 connection,
                 nameof(SelectInvoiceReprint),
                 this.m_SelectInvoiceReprintCmd,
                 $" : param = {param}",
                 cmd =>
                 {
                     cmd.Parameters["@INVOICE_ID"].Value = param.InvoiceId;
                 });
        private void InsertInvoiceReprint(SqlConnection connection, TouchParam param)
            => this.ExecuteNonQuery(
                connection,
                nameof(InsertInvoiceReprint),
                 this.m_InsertInvoiceReprintCmd,
                 $" : param = {param}",
                 cmd =>
                 {
                     if (param.BcrIndex == null)
                         cmd.Parameters["@BCR_INDEX"].Value = DBNull.Value;
                     else
                         cmd.Parameters["@BCR_INDEX"].Value = param.BcrIndex;
                     cmd.Parameters["@INVOICE_ID"].Value = param.InvoiceId;
                 });
        #endregion

        #region weight check
        private void InitWeightCheckCommand()
        {
            this.m_SelectTodayWeightCmd = new SqlCommand("SELECT_TODAY_WEIGHT") { CommandType = CommandType.StoredProcedure };

            this.m_SelectWeightByIndexCmd = new SqlCommand("SELECT_WEIGHT_BY_INDEX") { CommandType = CommandType.StoredProcedure };
            this.m_SelectWeightByIndexCmd.Parameters.Add("@WEIGHT_INDEX", SqlDbType.BigInt);

            this.m_SelectWieghtSearchCmd = new SqlCommand("SELECT_WEIGHT_SEARCH") { CommandType = CommandType.StoredProcedure };
            this.m_SelectWieghtSearchCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_SelectWieghtSearchCmd.Parameters.Add("@CST_ORD_NO", SqlDbType.VarChar);

            this.m_SelectSkuCmd = new SqlCommand("SELECT_SKU") { CommandType = CommandType.StoredProcedure };
            this.m_SelectSkuCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
        }
        private List<WeightCheckData> SelectTodayWeight(SqlConnection connection)
            => this.SelectDataList<WeightCheckData>(
                connection,
                nameof(SelectTodayWeight),
                this.m_SelectTodayWeightCmd);
        private List<WeightCheckData> SelectWeightByIndex(SqlConnection connection, TouchParam param)
            => this.SelectDataList<WeightCheckData>(
                 connection,
                 nameof(SelectWeightByIndex),
                 this.m_SelectWeightByIndexCmd,
                 $" : param = {param}",
                 cmd =>
                 {
                     cmd.Parameters["@WEIGHT_INDEX"].Value = param.BcrIndex;
                 });
        private List<WeightCheckData> SelectWieghtSearch(SqlConnection connection, TouchParam param)
            => this.SelectDataList<WeightCheckData>(
                 connection,
                 nameof(SelectWieghtSearch),
                 this.m_SelectWieghtSearchCmd,
                 $" : param = {param}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_ID"].Value = param.BoxId;
                     cmd.Parameters["@CST_ORD_NO"].Value = param.CstOrdNo;
                 });
        private List<SkuData> SelectSku(SqlConnection connection, TouchParam param)
            => this.SelectDataList<SkuData>(
                 connection,
                 nameof(SelectSku),
                 this.m_SelectSkuCmd,
                 $" : param = {param}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_ID"].Value = param.BoxId;
                 });
        #endregion

        #region case erect
        private void InitCaseErectCommand()
        {
            this.m_SelectTodayErectCmd = new SqlCommand("SELECT_TODAY_ERECT") { CommandType = CommandType.StoredProcedure };

            this.m_SelectErectByIndexCmd = new SqlCommand("SELECT_ERECT_BY_INDEX") { CommandType = CommandType.StoredProcedure };
            this.m_SelectErectByIndexCmd.Parameters.Add("@BCR_INDEX", SqlDbType.BigInt);

            this.m_SelectCaseErectQueryCmd = new SqlCommand("SELECT_CASE_ERECT_QUERY") { CommandType = CommandType.StoredProcedure };
            this.m_SelectCaseErectQueryCmd.Parameters.Add("@BOX_TYPE", SqlDbType.VarChar);
            this.m_SelectCaseErectQueryCmd.Parameters.Add("@COUNT", SqlDbType.VarChar);
            this.m_SelectCaseErectQueryCmd.Parameters.Add("@BEGIN", SqlDbType.Date);
            this.m_SelectCaseErectQueryCmd.Parameters.Add("@END", SqlDbType.Date);

            this.m_InsertBoxInfoCmd = new SqlCommand("INSERT_BOX_INFO") { CommandType = CommandType.StoredProcedure };
            this.m_InsertBoxInfoCmd.Parameters.Add("@BOX_TYPE", SqlDbType.VarChar);
            this.m_InsertBoxInfoCmd.Parameters.Add("@NAME", SqlDbType.VarChar);
            this.m_InsertBoxInfoCmd.Parameters.Add("@WEIGHT", SqlDbType.Float);
            this.m_InsertBoxInfoCmd.Parameters.Add("@LENGTH", SqlDbType.Float);
            this.m_InsertBoxInfoCmd.Parameters.Add("@WIDTH", SqlDbType.Float);
            this.m_InsertBoxInfoCmd.Parameters.Add("@HEIGHT", SqlDbType.Float);
            this.m_InsertBoxInfoCmd.Parameters.Add("@H_SENSOR", SqlDbType.Int);
            this.m_InsertBoxInfoCmd.Parameters.Add("@NORMAL_FROM", SqlDbType.Int);
            this.m_InsertBoxInfoCmd.Parameters.Add("@NORMAL_TO", SqlDbType.Int);
            this.m_InsertBoxInfoCmd.Parameters.Add("@MANUAL_FROM", SqlDbType.Int);
            this.m_InsertBoxInfoCmd.Parameters.Add("@MANUAL_TO", SqlDbType.Int);

            this.m_UpdateBoxInfoCmd = new SqlCommand("UPDATE_BOX_INFO") { CommandType = CommandType.StoredProcedure };
            this.m_UpdateBoxInfoCmd.Parameters.Add("@PREV_BOX_TYPE", SqlDbType.VarChar);
            this.m_UpdateBoxInfoCmd.Parameters.Add("@NEXT_BOX_TYPE", SqlDbType.VarChar);
            this.m_UpdateBoxInfoCmd.Parameters.Add("@NAME", SqlDbType.VarChar);
            this.m_UpdateBoxInfoCmd.Parameters.Add("@WEIGHT", SqlDbType.Float);
            this.m_UpdateBoxInfoCmd.Parameters.Add("@LENGTH", SqlDbType.Float);
            this.m_UpdateBoxInfoCmd.Parameters.Add("@WIDTH", SqlDbType.Float);
            this.m_UpdateBoxInfoCmd.Parameters.Add("@HEIGHT", SqlDbType.Float);
            this.m_UpdateBoxInfoCmd.Parameters.Add("@H_SENSOR", SqlDbType.Int);

            this.m_UpdateNumberingCmd = new SqlCommand("UPDATE_NUMBERING") { CommandType = CommandType.StoredProcedure };
            this.m_UpdateNumberingCmd.Parameters.Add("@BOX_TYPE", SqlDbType.VarChar);
            this.m_UpdateNumberingCmd.Parameters.Add("@ERECTOR_TYPE", SqlDbType.VarChar);
            this.m_UpdateNumberingCmd.Parameters.Add("@FROM", SqlDbType.Int);
            this.m_UpdateNumberingCmd.Parameters.Add("@TO", SqlDbType.Int);

            this.m_DeleteBoxInfoCmd = new SqlCommand("DELETE_BOX_INFO") { CommandType = CommandType.StoredProcedure };
            this.m_DeleteBoxInfoCmd.Parameters.Add("@BOX_TYPE", SqlDbType.VarChar);

            this.m_ReprintBoxCmd = new SqlCommand("REPRINT_BOX") { CommandType = CommandType.StoredProcedure };
            this.m_ReprintBoxCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);

            this.m_SelectNonVerifiedBoxCmd = new SqlCommand("SELECT_NON_VERIFIED_BOX") { CommandType = CommandType.StoredProcedure };
            this.m_SelectNonVerifiedBoxCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);

        }
        private List<CaseErectData> SelectTodayErect(SqlConnection connection)
            => this.SelectDataList<CaseErectData>(
                 connection,
                 nameof(SelectTodayErect),
                 this.m_SelectTodayErectCmd);
        private List<CaseErectData> SelectErectByIndex(SqlConnection connection, TouchParam param)
            => this.SelectDataList<CaseErectData>(
                 connection,
                 nameof(SelectErectByIndex),
                 this.m_SelectErectByIndexCmd,
                 $" : param = {param}",
                 cmd =>
                 {
                     cmd.Parameters["@BCR_INDEX"].Value = param.BcrIndex;
                 });
        private List<CaseErectData> SelectCaseErectQuery(SqlConnection connection, TouchParam param, string boxType)
            => this.SelectDataList<CaseErectData>(
                 connection,
                 nameof(SelectCaseErectQuery),
                 this.m_SelectCaseErectQueryCmd,
                 $" : param = {param}, boxType = {boxType}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_TYPE"].Value = boxType;
                     cmd.Parameters["@COUNT"].Value = param.BoxId;
                     cmd.Parameters["@BEGIN"].Value = param.Begin;
                     cmd.Parameters["@END"].Value = param.End;
                 });
        private List<CaseErectData> SelectNonVerifiedBox(SqlConnection connection, string boxId)
            => this.SelectDataList<CaseErectData>(
                 connection,
                 nameof(SelectNonVerifiedBox),
                 this.m_SelectNonVerifiedBoxCmd,
                 $" : boxId = {boxId}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_ID"].Value = boxId;
                 });
        private void ReprintBox(SqlConnection connection, string boxId)
            => this.ExecuteNonQuery(
                connection,
                nameof(ReprintBox),
                this.m_ReprintBoxCmd,
                $" : boxId = {boxId}",
                cmd =>
                {
                    cmd.Parameters["@BOX_ID"].Value = boxId;
                });
        #endregion

        #region numbering
        private bool InsertBoxInfo(SqlConnection connection, BoxInfoData data)
            => this.SelectQueryResult<bool>(
                 connection,
                 nameof(InsertBoxInfo),
                 this.m_InsertBoxInfoCmd,
                 false,
                 reader => reader[0].ToString() == "Y",
                 $" : data = {data}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_TYPE"].Value = data.BoxTypeCd;
                     cmd.Parameters["@NAME"].Value = data.Name;
                     cmd.Parameters["@LENGTH"].Value = data.Length;
                     cmd.Parameters["@WEIGHT"].Value = data.Weight;
                     cmd.Parameters["@WIDTH"].Value = data.Width;
                     cmd.Parameters["@HEIGHT"].Value = data.Height;
                     cmd.Parameters["@H_SENSOR"].Value = data.HeightSensor;
                     cmd.Parameters["@NORMAL_FROM"].Value = data.FirstNormalFrom;
                     cmd.Parameters["@NORMAL_TO"].Value = data.FirstNormalTo;
                     cmd.Parameters["@MANUAL_FROM"].Value = data.ManualFrom;
                     cmd.Parameters["@MANUAL_TO"].Value = data.ManualTo;
                 });
        private bool UpdateBoxInfo(SqlConnection connection, string prevBoxType, BoxInfoData data)
            => this.SelectQueryResult<bool>(
                 connection,
                 nameof(UpdateBoxInfo),
                 this.m_UpdateBoxInfoCmd,
                 false,
                 reader => reader[0].ToString() == "Y",
                 $" : prevBoxType = {prevBoxType}, data = {data}",
                 cmd =>
                 {
                     cmd.Parameters["@PREV_BOX_TYPE"].Value = prevBoxType;
                     cmd.Parameters["@NEXT_BOX_TYPE"].Value = data.BoxTypeCd;
                     cmd.Parameters["@NAME"].Value = data.Name;
                     cmd.Parameters["@WEIGHT"].Value = data.Weight;
                     cmd.Parameters["@LENGTH"].Value = data.Length;
                     cmd.Parameters["@WIDTH"].Value = data.Width;
                     cmd.Parameters["@HEIGHT"].Value = data.Height;
                     cmd.Parameters["@H_SENSOR"].Value = data.HeightSensor;
                 });
        private bool UpdateNumbering(SqlConnection connection, string boxType, string erectorType, int from, int to)
            => this.SelectQueryResult<bool>(
                 connection,
                 nameof(UpdateNumbering),
                 this.m_UpdateNumberingCmd,
                 false,
                 reader => reader[0].ToString() == "Y",
                 $" : boxType = {boxType}, erectorType = {erectorType}, from = {from}, to = {to}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_TYPE"].Value = boxType;
                     cmd.Parameters["@ERECTOR_TYPE"].Value = erectorType;
                     cmd.Parameters["@FROM"].Value = from;
                     cmd.Parameters["@TO"].Value = to;
                 });
        private void DeleteBoxInfo(SqlConnection connection, string boxType)
            => this.ExecuteNonQuery(
                 connection,
                 nameof(DeleteBoxInfo),
                 this.m_DeleteBoxInfoCmd,
                 $" : boxType = {boxType}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_TYPE"].Value = boxType;
                 });
        #endregion

        #region smart packing
        private void InitSmartPackingCommand()
        {
            this.m_SelectTodaySmartPackingCmd = new SqlCommand("SELECT_TODAY_SMART_PACKING") { CommandType = CommandType.StoredProcedure };

            this.m_SelectSmartPackingByIndexCmd = new SqlCommand("SELECT_SMART_PACKING_BY_INDEX") { CommandType = CommandType.StoredProcedure };
            this.m_SelectSmartPackingByIndexCmd.Parameters.Add("@INDEX", SqlDbType.BigInt);

            this.m_SelectSmartPackingQueryCmd = new SqlCommand("SELECT_SMART_PACKING_QUERY") { CommandType = CommandType.StoredProcedure };
            this.m_SelectSmartPackingQueryCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);

        }
        private List<SmartPackingData> SelectTodaySmartPacking(SqlConnection connection)
            => this.SelectDataList<SmartPackingData>(
                 connection,
                 nameof(SelectTodaySmartPacking),
                 this.m_SelectTodaySmartPackingCmd);
        private List<SmartPackingData> SelectSmartPackingByIndex(SqlConnection connection, long index)
            => this.SelectDataList<SmartPackingData>(
                 connection,
                 nameof(SelectSmartPackingByIndex),
                 this.m_SelectSmartPackingByIndexCmd,
                 $" : index = {index}",
                 cmd =>
                 {
                     cmd.Parameters["@INDEX"].Value = index;
                 });
        private List<SmartPackingData> SelectSmartPackingQuery(SqlConnection connection, string boxId)
            => this.SelectDataList<SmartPackingData>(
                 connection,
                 nameof(SelectSmartPackingQuery),
                 this.m_SelectSmartPackingQueryCmd,
                 $" : boxId = {boxId}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_ID"].Value = boxId;
                 });
        #endregion

        #endregion

        #region public

        #region bcr lcd
        /// <summary>
        /// 당일 07시 ~ 익일 07시에 해당하는 route, print, top, out bcr 정보들을 반환
        /// </summary>
        /// <returns></returns>
        public List<BcrReadData> SelectTodayInvoiceBcr()
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectTodayInvoiceBcr(conn);
            }
        }
        /// <summary>
        /// bcrIndex에 해당하는 정보를 반환
        /// </summary>
        /// <param name="param">BcrIndex</param>
        /// <returns></returns>
        public List<BcrReadData> SelectInvoiceBcrByIndex(TouchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectInvoiceBcrByIndex(conn, param);
            }
        }
        /// <summary>
        /// begin, end, id 조건에 해당하는 out bcr 정보들을 반환
        /// </summary>
        /// <param name="param">Begin, End, BoxId(=Id)</param>
        /// <returns></returns>
        public List<BcrReadData> SelectOutBcrQuery(TouchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectOutBcrQuery(conn, param);
            }
        }
        #endregion

        #region invoice reject
        /// <summary>
        /// BoxId가 일치하는 송장과 고객주문번호가 같은 송장들을 반환
        /// </summary>
        /// <param name="param">BoxId</param>
        /// <returns></returns>
        public List<InvoiceData> SelectInvoicesByOrder(TouchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectInvoicesByOrder(conn, param);
            }
        }
        /// <summary>
        /// BoxId가 일치하는 상면검증 기록과 출력 기록을 반환
        /// </summary>
        /// <param name="param">BoxId</param>
        /// <returns></returns>
        public List<InvoicePrintData> SelectBcrInfoById(TouchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectBcrInfoById(conn, param);
            }
        }
        /// <summary>
        /// 송장번호가 InvoiceId인 송장의 재발행 이력 반환
        /// </summary>
        /// <param name="param">InvoiceId</param>
        /// <returns></returns>
        public List<InvoiceReprintData> SelectInvoiceReprint(TouchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectInvoiceReprint(conn, param);
            }
        }
        /// <summary>
        /// InvoiceId, BcrIndex 재발행 기록
        /// </summary>
        /// <param name="param">InvoiceId, BcrIndex</param>
        public void InsertInvoiceReprint(TouchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                this.InsertInvoiceReprint(conn, param);
            }
        }
        #endregion

        #region weight check
        /// <summary>
        /// 당일 07시 ~ 익일 07시에 해당하는 중량 검수 기록들을 반환
        /// </summary>
        /// <returns></returns>
        public List<WeightCheckData> SelectTodayWeight()
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectTodayWeight(conn);
            }
        }
        /// <summary>
        /// WeightIndex가 일치하는 정보 반환
        /// </summary>
        /// <param name="param">BcrIndex(=WeightIndex)</param>
        /// <returns></returns>
        public List<WeightCheckData> SelectWeightByIndex(TouchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectWeightByIndex(conn, param);
            }
        }
        /// <summary>
        /// BoxId나 CstOrdNo가 일치하는 중량기록들 반환
        /// </summary>
        /// <param name="param">BoxId, CstOrdNo</param>
        /// <returns></returns>
        public List<WeightCheckData> SelectWieghtSearch(TouchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectWieghtSearch(conn, param);
            }
        }
        /// <summary>
        /// BoxId가 일치하는 상품들 반환
        /// </summary>
        /// <param name="param">BoxId</param>
        /// <returns></returns>
        public List<SkuData> SelectSku(TouchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectSku(conn, param);
            }
        }
        #endregion

        #region case erect
        /// <summary>
        /// 당일 07시 ~ 익일 07에 해당하는 제함 검증 기록들을 반환
        /// </summary>
        /// <returns></returns>
        public List<CaseErectData> SelectTodayErect()
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectTodayErect(conn);
            }
        }
        /// <summary>
        /// BcrIndex가 일치하는 제함 검증 기록을 반환
        /// </summary>
        /// <param name="param">BcrIndex</param>
        /// <returns></returns>
        public List<CaseErectData> SelectErectByIndex(TouchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectErectByIndex(conn, param);
            }
        }
        /// <summary>
        /// 박스 타입이 일치하고 boxnumber를 포함하는 시간 범위 내의 데이터들 반환
        /// </summary>
        /// <param name="param">Begin,End,BoxNumber(=BoxId)</param>
        /// <param name="boxType">박스 타입</param>
        /// <returns></returns>
        public List<CaseErectData> SelectCaseErectQuery(TouchParam param, string boxType)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectCaseErectQuery(conn, param, boxType);
            }
        }
        /// <summary>
        /// 검증 안된 박스 정보들 반환
        /// </summary>
        /// <param name="boxId"></param>
        /// <returns></returns>
        public List<CaseErectData> SelectNonVerifiedBox(string boxId)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectNonVerifiedBox(conn, boxId);
            }
        }
        /// <summary>
        /// 재발행 이력 추가
        /// </summary>
        /// <param name="boxId"></param>
        public void ReprintBox(string boxId)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                this.ReprintBox(conn, boxId);
            }
        }
        #endregion

        #region numbering
        /// <summary>
        /// 새로운 박스 정보를 추가
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertBoxInfo(BoxInfoData data)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.InsertBoxInfo(conn, data);
            }
        }
        /// <summary>
        /// 박스 정보를 갱신
        /// </summary>
        /// <param name="prevBoxType">변경할 대상</param>
        /// <param name="data">변경될 정보</param>
        /// <param name="isFirst">1호기 여부</param>
        /// <returns></returns>
        public bool UpdateBoxInfo(string prevBoxType, BoxInfoData data, bool isFirst)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                if (this.UpdateBoxInfo(conn, prevBoxType, data))
                {
                    this.UpdateNumbering(conn, data.BoxTypeCd, "0", data.ManualFrom, data.ManualTo);
                    this.UpdateNumbering(conn, data.BoxTypeCd, isFirst ? "1" : "2", data.FirstNormalFrom, data.FirstNormalTo);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 박스 정보 제거
        /// </summary>
        /// <param name="boxType"></param>
        public void DeleteBoxInfo(string boxType)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                this.DeleteBoxInfo(conn, boxType);
            }
        }
        #endregion

        #region smart packing
        /// <summary>
        /// 당일 07시 ~ 익일 07에 해당하는 스마트 충진 기록들을 반환
        /// </summary>
        /// <returns></returns>
        public List<SmartPackingData> SelectTodaySmartPacking()
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectTodaySmartPacking(conn);
            }
        }
        /// <summary>
        /// index를 기준으로 데이터 조회
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<SmartPackingData> SelectSmartPackingByIndex(long index)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectSmartPackingByIndex(conn, index);
            }
        }
        /// <summary>
        /// BoxId를 포함한 데이터들 조회
        /// </summary>
        /// <param name="boxId"></param>
        /// <returns></returns>
        public List<SmartPackingData> SelectSmartPackingQuery(string boxId)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectSmartPackingQuery(conn, boxId);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
