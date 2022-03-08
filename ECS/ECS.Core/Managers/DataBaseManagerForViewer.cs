using ECS.Model.Domain;
using ECS.Model.Domain.Viewer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode.Database.SqlServer;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Threading;

namespace ECS.Core.Managers
{
    public class DataBaseManagerForViewer
    {
        #region Static
        public static DataBaseManagerForViewer Instance { get; set; }
        #endregion

        #region Field
        private readonly SqlServerDbExecutor DbExecutor;
        private readonly Logger Logger;

        private SqlCommand m_SelectViewerOrderQueryCmd = null;
        private SqlCommand m_SelectWeightCheckRejectCmd = null;
        private SqlCommand m_SelectViewerInvoiceRejectQueryCmd = null;
        private SqlCommand m_SelectSmartPackingRejectQuerytCmd = null;
        private SqlCommand m_SelectViewerTrackingQueryCmd = null;
        private SqlCommand m_SelectViewerOutManageQueryCmd = null;
        private SqlCommand m_SelectHourlyCountsCmd = null;
        private SqlCommand m_SelectDailyCountsCmd = null;
        #endregion

        #region Constructor
        public DataBaseManagerForViewer(SqlServerDbExecutor dbExecutor)
        {
            this.DbExecutor = dbExecutor;
            this.Logger = new Logger("Database Manager");

            this.InitSqlCommands();
        }
        #endregion

        #region Method

        #region Private
        private void InitSqlCommands()
        {
            #region 주문정보
            this.m_SelectViewerOrderQueryCmd = new SqlCommand("SELECT_VIEWER_ORDER_QUERY") { CommandType = CommandType.StoredProcedure };
            this.m_SelectViewerOrderQueryCmd.Parameters.Add("@BEGIN", SqlDbType.DateTime);
            this.m_SelectViewerOrderQueryCmd.Parameters.Add("@END", SqlDbType.DateTime);
            this.m_SelectViewerOrderQueryCmd.Parameters.Add("@WAVE_NO", SqlDbType.VarChar);
            this.m_SelectViewerOrderQueryCmd.Parameters.Add("@CST_CD", SqlDbType.VarChar);
            this.m_SelectViewerOrderQueryCmd.Parameters.Add("@CST_ORD_NO", SqlDbType.VarChar);
            this.m_SelectViewerOrderQueryCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_SelectViewerOrderQueryCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            #endregion
            #region 중량리젝
            this.m_SelectWeightCheckRejectCmd = new SqlCommand("SELECT_VIEWER_WEIGHT_CHECK_REJECT_QUERY") { CommandType = CommandType.StoredProcedure };
            this.m_SelectWeightCheckRejectCmd.Parameters.Add("@BEGIN", SqlDbType.DateTime);
            this.m_SelectWeightCheckRejectCmd.Parameters.Add("@END", SqlDbType.DateTime);
            this.m_SelectWeightCheckRejectCmd.Parameters.Add("@WAVE_NO", SqlDbType.VarChar);
            this.m_SelectWeightCheckRejectCmd.Parameters.Add("@CST_CD", SqlDbType.VarChar);
            this.m_SelectWeightCheckRejectCmd.Parameters.Add("@CST_ORD_NO", SqlDbType.VarChar);
            this.m_SelectWeightCheckRejectCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_SelectWeightCheckRejectCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            #endregion
            #region 상면 리젝
            this.m_SelectViewerInvoiceRejectQueryCmd = new SqlCommand("SELECT_VIEWER_INVOICE_REJECT_QUERY") { CommandType = CommandType.StoredProcedure };
            this.m_SelectViewerInvoiceRejectQueryCmd.Parameters.Add("@BEGIN", SqlDbType.DateTime);
            this.m_SelectViewerInvoiceRejectQueryCmd.Parameters.Add("@END", SqlDbType.DateTime);
            this.m_SelectViewerInvoiceRejectQueryCmd.Parameters.Add("@WAVE_NO", SqlDbType.VarChar);
            this.m_SelectViewerInvoiceRejectQueryCmd.Parameters.Add("@CST_CD", SqlDbType.VarChar);
            this.m_SelectViewerInvoiceRejectQueryCmd.Parameters.Add("@CST_ORD_NO", SqlDbType.VarChar);
            this.m_SelectViewerInvoiceRejectQueryCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_SelectViewerInvoiceRejectQueryCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            #endregion
            #region 스마트 완충재
            this.m_SelectSmartPackingRejectQuerytCmd = new SqlCommand("SELECT_SMART_PACKING_REJECT_QUERY") { CommandType = CommandType.StoredProcedure };
            this.m_SelectSmartPackingRejectQuerytCmd.Parameters.Add("@BEGIN", SqlDbType.DateTime);
            this.m_SelectSmartPackingRejectQuerytCmd.Parameters.Add("@END", SqlDbType.DateTime);
            this.m_SelectSmartPackingRejectQuerytCmd.Parameters.Add("@WAVE_NO", SqlDbType.VarChar);
            this.m_SelectSmartPackingRejectQuerytCmd.Parameters.Add("@CST_CD", SqlDbType.VarChar);
            this.m_SelectSmartPackingRejectQuerytCmd.Parameters.Add("@CST_ORD_NO", SqlDbType.VarChar);
            this.m_SelectSmartPackingRejectQuerytCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_SelectSmartPackingRejectQuerytCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            #endregion
            #region 출고
            this.m_SelectViewerOutManageQueryCmd = new SqlCommand("SELECT_VIEWER_OUT_MANAGE_QUERY") { CommandType = CommandType.StoredProcedure };
            this.m_SelectViewerOutManageQueryCmd.Parameters.Add("@BEGIN", SqlDbType.DateTime);
            this.m_SelectViewerOutManageQueryCmd.Parameters.Add("@END", SqlDbType.DateTime);
            this.m_SelectViewerOutManageQueryCmd.Parameters.Add("@WAVE_NO", SqlDbType.VarChar);
            this.m_SelectViewerOutManageQueryCmd.Parameters.Add("@CST_CD", SqlDbType.VarChar);
            this.m_SelectViewerOutManageQueryCmd.Parameters.Add("@CST_ORD_NO", SqlDbType.VarChar);
            this.m_SelectViewerOutManageQueryCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_SelectViewerOutManageQueryCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            #endregion
            #region 트래킹
            this.m_SelectViewerTrackingQueryCmd = new SqlCommand("SELECT_VIEWER_TRACKING_QUERY") { CommandType = CommandType.StoredProcedure };
            this.m_SelectViewerTrackingQueryCmd.Parameters.Add("@BEGIN", SqlDbType.DateTime);
            this.m_SelectViewerTrackingQueryCmd.Parameters.Add("@END", SqlDbType.DateTime);
            this.m_SelectViewerTrackingQueryCmd.Parameters.Add("@CST_ORD_NO", SqlDbType.VarChar);
            this.m_SelectViewerTrackingQueryCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);
            this.m_SelectViewerTrackingQueryCmd.Parameters.Add("@INVOICE_ID", SqlDbType.VarChar);
            #endregion
            #region 시간별 집계
            this.m_SelectHourlyCountsCmd = new SqlCommand("SELECT_HOURLY_COUNTS") { CommandType = CommandType.StoredProcedure };
            this.m_SelectHourlyCountsCmd.Parameters.Add("@BEGIN", SqlDbType.DateTime);
            this.m_SelectHourlyCountsCmd.Parameters.Add("@END", SqlDbType.DateTime);
            #endregion
            #region 일자별 집계
            this.m_SelectDailyCountsCmd = new SqlCommand("SELECT_DAILY_COUNTS") { CommandType = CommandType.StoredProcedure };
            this.m_SelectDailyCountsCmd.Parameters.Add("@BEGIN", SqlDbType.DateTime);
            this.m_SelectDailyCountsCmd.Parameters.Add("@END", SqlDbType.DateTime);
            #endregion
        }

        #region util
        private SqlConnection GetConnection()
        {
            return this.DbExecutor.CreateConnection();
        }
        /// <summary>
        /// 에러 발생 시 메시지와 콜스택을 출력
        /// </summary>
        /// <param name="ex"></param>
        protected void ErrorLog(Exception ex)
        {
            this.Logger?.Write(ex.Message);
            this.Logger?.Write(ex.StackTrace);
        }
        /// <summary>
        /// 결과를 반환하지 않는 쿼리 실행
        /// </summary>
        /// <param name="connection">db 커넥션</param>
        /// <param name="methodName">로그에 저장될 메소드 이름</param>
        /// <param name="cmd">사용할 SqlCommand</param>
        /// <param name="paramStr">로그에 저장될 매개변수</param>
        /// <param name="setParam">cmd에 매개변수를 저장하는 함수</param>
        protected void ExecuteNonQuery(SqlConnection connection, string methodName, SqlCommand cmd,
            string paramStr = "", Action<SqlCommand> setParam = null)
        {
            this.Logger.Write($"{methodName} Request" + paramStr);
            cmd.Connection = connection;
            try
            {
                setParam?.Invoke(cmd);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.Logger.Write($"{methodName} Exception");
                this.ErrorLog(ex);
            }
            this.Logger.Write($"{methodName} Response");
        }
        /// <summary>
        /// 결과를 T 리스트로 반환하는 쿼리 실행
        /// </summary>
        /// <typeparam name="T">IReaderConvetable을 구현하여 SqlDataReader를 사용해 변환할 수 있는 구조체</typeparam>
        /// <param name="connection">db 커넥션</param>
        /// <param name="methodName">로그에 저장될 메소드 이름</param>
        /// <param name="cmd">사용할 SqlCommand</param>
        /// <param name="paramStr">로그에 저장될 매개변수</param>
        /// <param name="setParam">cmd에 매개변수를 저장하는 함수</param>
        /// <returns>T 리스트</returns>
        protected List<T> SelectDataList<T>(SqlConnection connection, string methodName, SqlCommand cmd,
            string paramStr = "", Action<SqlCommand> setParam = null) where T : struct, IReaderConvertable
        {
            this.Logger.Write($"{methodName} Request" + paramStr);
            var list = new List<T>();
            cmd.Connection = connection;
            try
            {
                setParam?.Invoke(cmd);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var data = new T();
                        data.Convert(reader);
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger.Write($"{methodName} Exception");
                this.ErrorLog(ex);
            }
            this.Logger.Write($"{methodName} Response : {list.Count}");
            return list;
        }
        /// <summary>
        /// 쿼리를 실행한 뒤 결과값 T를 반환
        /// </summary>
        /// <typeparam name="T">값타입</typeparam>
        /// <param name="connection">db 커넥션</param>
        /// <param name="methodName">로그에 저장될 메소드 이름</param>
        /// <param name="cmd">사용할 SqlCommand</param>
        /// <param name="defaultResult">실패시 반환될 기본값</param>
        /// <param name="getResult">SqlDataReader를 T로 변환하는 함수</param>
        /// <param name="paramStr">로그에 저장될 매개변수</param>
        /// <param name="setParam">cmd에 매개변수를 저장하는 함수</param>
        /// <returns></returns>
        protected T SelectQueryResult<T>(SqlConnection connection, string methodName, SqlCommand cmd,
             T defaultResult, Func<SqlDataReader, T> getResult, string paramStr = "", Action<SqlCommand> setParam = null) where T : struct
        {
            this.Logger.Write($"{methodName} Request" + paramStr);
            T res = defaultResult;
            cmd.Connection = connection;
            try
            {
                setParam?.Invoke(cmd);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        res = getResult.Invoke(reader);
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
        #endregion
        private List<OrderSearchData> SelectOrderQuery(SqlConnection connection, SearchParam param)
            => this.SelectDataList<OrderSearchData>(
                connection,
                nameof(SelectOrderQuery),
                this.m_SelectViewerOrderQueryCmd,
                $" : param = {param}",
                cmd =>
                {
                    cmd.Parameters["@BEGIN"].Value = param.Begin;
                    cmd.Parameters["@END"].Value = param.End;
                    cmd.Parameters["@WAVE_NO"].Value = param.WaveNo;
                    cmd.Parameters["@CST_CD"].Value = param.CstCd;
                    cmd.Parameters["@CST_ORD_NO"].Value = param.CstOrdNo;
                    cmd.Parameters["@BOX_ID"].Value = param.BoxId;
                    cmd.Parameters["@INVOICE_ID"].Value = param.InvoiceId;
                });
        private List<WeightRejectData> SelectWeightCheckRejectQuery(SqlConnection connection, SearchParam param)
            => this.SelectDataList<WeightRejectData>(
                connection,
                nameof(SelectWeightCheckRejectQuery),
                this.m_SelectWeightCheckRejectCmd,
                $" : param = {param}",
                cmd =>
                {
                    cmd.Parameters["@BEGIN"].Value = param.Begin;
                    cmd.Parameters["@END"].Value = param.End;
                    cmd.Parameters["@WAVE_NO"].Value = param.WaveNo;
                    cmd.Parameters["@CST_CD"].Value = param.CstCd;
                    cmd.Parameters["@CST_ORD_NO"].Value = param.CstOrdNo;
                    cmd.Parameters["@BOX_ID"].Value = param.BoxId;
                    cmd.Parameters["@INVOICE_ID"].Value = param.InvoiceId;
                });
        private List<InvoiceRejectData> SelectInvoiceRejectQuery(SqlConnection connection, SearchParam param)
            => this.SelectDataList<InvoiceRejectData>(
                connection,
                nameof(SelectInvoiceRejectQuery),
                this.m_SelectViewerInvoiceRejectQueryCmd,
                $" : param = {param}",
                cmd =>
                {
                    cmd.Parameters["@BEGIN"].Value = param.Begin;
                    cmd.Parameters["@END"].Value = param.End;
                    cmd.Parameters["@WAVE_NO"].Value = param.WaveNo;
                    cmd.Parameters["@CST_CD"].Value = param.CstCd;
                    cmd.Parameters["@CST_ORD_NO"].Value = param.CstOrdNo;
                    cmd.Parameters["@BOX_ID"].Value = param.BoxId;
                    cmd.Parameters["@INVOICE_ID"].Value = param.InvoiceId;
                });
        private List<SmartPackingRejectData> SelectSmartPackingRejectQuery(SqlConnection connection, SearchParam param)
            => this.SelectDataList<SmartPackingRejectData>(
                connection,
                nameof(SelectSmartPackingRejectQuery),
                this.m_SelectSmartPackingRejectQuerytCmd,
                $" : param = {param}",
                cmd =>
                {
                    cmd.Parameters["@BEGIN"].Value = param.Begin;
                    cmd.Parameters["@END"].Value = param.End;
                    cmd.Parameters["@WAVE_NO"].Value = param.WaveNo;
                    cmd.Parameters["@CST_CD"].Value = param.CstCd;
                    cmd.Parameters["@CST_ORD_NO"].Value = param.CstOrdNo;
                    cmd.Parameters["@BOX_ID"].Value = param.BoxId;
                    cmd.Parameters["@INVOICE_ID"].Value = param.InvoiceId;
                });
        private List<OutBcrData> SelectOutManageQuery(SqlConnection connection, SearchParam param)
            => this.SelectDataList<OutBcrData>(
                connection,
                nameof(SelectOutManageQuery),
                this.m_SelectViewerOutManageQueryCmd,
                $" : param = {param}",
                cmd =>
                {
                    cmd.Parameters["@BEGIN"].Value = param.Begin;
                    cmd.Parameters["@END"].Value = param.End;
                    cmd.Parameters["@WAVE_NO"].Value = param.WaveNo;
                    cmd.Parameters["@CST_CD"].Value = param.CstCd;
                    cmd.Parameters["@CST_ORD_NO"].Value = param.CstOrdNo;
                    cmd.Parameters["@BOX_ID"].Value = param.BoxId;
                    cmd.Parameters["@INVOICE_ID"].Value = param.InvoiceId;
                });
        private List<TrackingData> SelectTrackingQuery(SqlConnection connection, SearchParam param)
            => this.SelectDataList<TrackingData>(
                connection,
                nameof(SelectTrackingQuery),
                this.m_SelectViewerTrackingQueryCmd,
                $" : param = {param}",
                cmd =>
                {
                    cmd.Parameters["@BEGIN"].Value = param.Begin;
                    cmd.Parameters["@END"].Value = param.End;
                    cmd.Parameters["@CST_ORD_NO"].Value = param.CstOrdNo;
                    cmd.Parameters["@BOX_ID"].Value = param.BoxId;
                    cmd.Parameters["@INVOICE_ID"].Value = param.InvoiceId;
                });
        private List<HourlyCountingData> SelectHourlyCounts(SqlConnection connection, DateTime begin, DateTime end)
            => this.SelectDataList<HourlyCountingData>(
                connection,
                nameof(SelectHourlyCounts),
                this.m_SelectHourlyCountsCmd,
                $" : begin = {begin}, end = {end}",
                cmd =>
                {
                    cmd.Parameters["@BEGIN"].Value = begin;
                    cmd.Parameters["@END"].Value = end;
                });
        private List<DailyCountingData> SelectDailyCounts(SqlConnection connection, DateTime? begin, DateTime? end)
            => this.SelectDataList<DailyCountingData>(
                connection,
                nameof(SelectDailyCounts),
                this.m_SelectDailyCountsCmd,
                $" : begin = {begin}, end = {end}",
                cmd =>
                {
                    if (begin != null)
                        cmd.Parameters["@BEGIN"].Value = begin;
                    else
                        cmd.Parameters["@BEGIN"].Value = DBNull.Value;
                    if (end != null)
                        cmd.Parameters["@END"].Value = end;
                    else
                        cmd.Parameters["@END"].Value = DBNull.Value;
                });
        #endregion

        #region public
        /// <summary>
        /// 주문정보 쿼리
        /// </summary>
        /// <param name="param">Begin, End, WaveNo, CstCd, CstOrdNo, BoxId, InvoiceId</param>
        /// <returns></returns>
        public List<OrderSearchData> SelectOrderQuery(SearchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectOrderQuery(conn, param);
            }
        }
        /// <summary>
        /// 중량 리젝 쿼리
        /// </summary>
        /// <param name="param">Begin, End, WaveNo, CstCd, CstOrdNo, BoxId, InvoiceId</param>
        /// <returns></returns>
        public List<WeightRejectData> SelectWeightCheckRejectQuery(SearchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectWeightCheckRejectQuery(conn, param);
            }
        }
        /// <summary>
        /// 송장 리젝 쿼리
        /// </summary>
        /// <param name="param">Begin, End, WaveNo, CstCd, CstOrdNo, BoxId, InvoiceId</param>
        /// <returns></returns>
        public List<InvoiceRejectData> SelectInvoiceRejectQuery(SearchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectInvoiceRejectQuery(conn, param);
            }
        }
        /// <summary>
        /// 스마트 완충재 리젝 쿼리
        /// </summary>
        /// <param name="param">Begin, End, WaveNo, CstCd, CstOrdNo, BoxId, InvoiceId</param>
        /// <returns></returns>
        public List<SmartPackingRejectData> SelectSmartPackingRejectQuery(SearchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectSmartPackingRejectQuery(conn, param);
            }
        }
        /// <summary>
        /// 출고 조회
        /// </summary>
        /// <param name="param">Begin, End, WaveNo, CstCd, CstOrdNo, BoxId, InvoiceId</param>
        /// <returns></returns>
        public List<OutBcrData> SelectOutManageQuery(SearchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectOutManageQuery(conn, param);
            }
        }
        /// <summary>
        /// 이력 조회
        /// </summary>
        /// <param name="param">>Begin, End, CstOrdNo, BoxId, InvoiceId</param>
        /// <returns></returns>
        public List<TrackingData> SelectTrackingQuery(SearchParam param)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectTrackingQuery(conn, param);
            }
        }
        /// <summary>
        /// 시간별 집계 테이블 조회
        /// </summary>
        /// <param name="begin">시작</param>
        /// <param name="end">종료</param>
        /// <returns></returns>
        public List<HourlyCountingData> SelectHourlyCounts(DateTime begin, DateTime end)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectHourlyCounts(conn, begin, end);
            }
        }
        /// <summary>
        /// 시간별 집계 테이블 조회
        /// </summary>
        /// <param name="begin">시작, null이면 datetime.min</param>
        /// <param name="end">종료, null이면 오늘</param>
        /// <returns></returns>
        public List<DailyCountingData> SelectDailyCounts(DateTime? begin = null, DateTime? end = null)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectDailyCounts(conn, begin, end);
            }
        }
        #endregion

        #endregion
    }
}
