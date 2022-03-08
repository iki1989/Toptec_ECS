using ECS.Model;
using ECS.Model.Domain;
using ECS.Model.Domain.Touch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;

namespace ECS.Core.Managers
{
    public class DataBaseManager : IHaveLogger
    {
        #region field
        private SqlCommand m_GetBoxNumberCmd;
        private SqlCommand m_PrintBoxCmd;
        private SqlCommand m_SelectBoxInfosCmd;
        #endregion

        #region property
        public Logger Logger { get; protected set; }
        public DataBaseManagerSetting Setting { get; set; }
        #endregion

        #region constructor
        public DataBaseManager(DataBaseManagerSetting setting)
        {
            this.Setting = setting;
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo("ECS_DataBase", EcsAppDirectory.SqlServerLog));
            this.Logger.Write("");
            this.Logger.Write("ECS_DataBase Start");
            this.InitCommands();
        }
        public DataBaseManager() : this(new DataBaseManagerSetting()) { }
        #endregion

        #region method
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(this.Setting.SqlConnectionStringBuilder.ConnectionString);
        }

        #region util
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

        #region private
        private void InitCommands()
        {
            this.m_GetBoxNumberCmd = new SqlCommand("GET_BOX_NUMBER") { CommandType = CommandType.StoredProcedure };
            this.m_GetBoxNumberCmd.Parameters.Add("@BOX_TYPE", SqlDbType.VarChar);
            this.m_GetBoxNumberCmd.Parameters.Add("@ERECTOR_TYPE", SqlDbType.VarChar);

            this.m_PrintBoxCmd = new SqlCommand("PRINT_BOX") { CommandType = CommandType.StoredProcedure };
            this.m_PrintBoxCmd.Parameters.Add("@BOX_ID", SqlDbType.VarChar);

            this.m_SelectBoxInfosCmd = new SqlCommand("SELECT_BOX_INFOS") { CommandType = CommandType.StoredProcedure };
        }
        protected string DataTableToString(DataTable table)
        {
            return JsonConvert.SerializeObject(table);
        }
        protected int GetBoxNumber(SqlConnection connection, string boxType, string erectorType)
            => this.SelectQueryResult<int>(
                 connection,
                 nameof(GetBoxNumber),
                 this.m_GetBoxNumberCmd,
                 -1,
                 reader => int.Parse(reader[0].ToString()),
                 $" : boxType = {boxType}, erectorType = {erectorType}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_TYPE"].Value = boxType;
                     cmd.Parameters["@ERECTOR_TYPE"].Value = erectorType;
                 });
        protected void PrintBox(SqlConnection connection, string boxId)
            => this.ExecuteNonQuery(
                 connection,
                 nameof(PrintBox),
                 this.m_PrintBoxCmd,
                 $" : boxId = {boxId}",
                 cmd =>
                 {
                     cmd.Parameters["@BOX_ID"].Value = boxId;
                 });
        private List<BoxInfoData> SelectBoxInfos(SqlConnection connection)
            => this.SelectDataList<BoxInfoData>(
                connection,
                nameof(SelectBoxInfos),
                this.m_SelectBoxInfosCmd);
        //채번
        #endregion

        #region public
        /// <summary>
        /// 박스 타입과 제함기 타입에 해당하는 번호를 반환
        /// </summary>
        /// <param name="boxType">박스 타입</param>
        /// <param name="erectorType">제함기 타입</param>
        /// <returns></returns>
        public int GetBoxNumber(string boxType, string erectorType)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.GetBoxNumber(conn, boxType, erectorType);
            }
        }
        /// <summary>
        /// 박스ID를 가진 box를 DB에 생성
        /// </summary>
        /// <param name="boxId">박스ID</param>
        public void PrintBox(string boxId)
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                this.PrintBox(conn, boxId);
            }
        }
        /// <summary>
        /// 박스 정보들 반환
        /// </summary>
        /// <returns></returns>
        public List<BoxInfoData> SelectBoxInfos()
        {
            using (var conn = this.GetConnection())
            {
                conn.Open();
                return this.SelectBoxInfos(conn);
            }
        }
        #endregion

        #endregion
    }
    [Serializable]
    public class DataBaseManagerSetting : Setting
    {
        public SqlConnectionStringBuilder SqlConnectionStringBuilder { get; set; }

        public DataBaseManagerSetting()
        {
            this.SqlConnectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = "127.0.0.1",
                InitialCatalog = "ECS",
                UserID = "ecsuser",
                Password = "1",
                IntegratedSecurity = false,
            };
        }
    }
}
