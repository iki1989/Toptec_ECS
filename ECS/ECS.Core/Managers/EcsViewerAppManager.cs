using System;
using System.Windows;
using System.Data.SqlClient;
using Urcis.SmartCode;
using Urcis.SmartCode.Wpf;
using Urcis.SmartCode.Database.SqlServer;
using Newtonsoft.Json;
using ECS.Model;
using ECS.Core.Util;
using System.Net.NetworkInformation;
using System.Net;

namespace ECS.Core.Managers
{
    public class EcsViewerAppManager : WpfAppManager
    {
        public new static EcsViewerAppManager Instance => AppManager.Instance as EcsViewerAppManager;

        public EcsViewerAppManager(Application app) : base(app)
        {
        }

        public new EcsViewerAppManagerSetting Setting => base.Setting as EcsViewerAppManagerSetting;

        private SqlServerDbExecutor GetDbExecutor(SqlServerConnectionInfo connectionInfo)
        {
            try
            {
                SqlServerDbExecutor dbExecutor = new SqlServerDbExecutor(connectionInfo);
                dbExecutor.CheckConnection();
                return dbExecutor;
            }
            catch
            {
                return null;
            }
        }

        protected override void OnAfterApplySetting()
        {
            base.OnAfterApplySetting();

            SqlServerDbExecutor dbExecutor = this.GetDbExecutor(this.Setting.InternalConnection);
            if (dbExecutor == null)
            {
                dbExecutor = this.GetDbExecutor(this.Setting.ExternalConnection);
                if (dbExecutor == null)
                    throw new ApplicationException($"DB Connection Error");
            }
            DataBaseManagerForViewer.Instance = new DataBaseManagerForViewer(dbExecutor);
        }
    }

    #region EcsViewerAppManagerSetting
    [Serializable]
    public class EcsViewerAppManagerSetting : WpfAppManagerSetting
    {
        public EcsViewerAppManagerSetting()
        {
            SqlServerConnectionInfo info = new SqlServerConnectionInfo();
            info.ServerName = "127.0.0.1";
            info.DatabaseName = "ECS";
            info.UserId = "ecsuser";
            info.Password = "1";
            info.WindowsAuthentication = false;
            info.ConnectTimeoutSeconds = 3;
            this.InternalConnection = info;

            info = new SqlServerConnectionInfo();
            info.ServerName = "10.211.77.40";
            info.DatabaseName = "ECS";
            info.UserId = "ecsuser";
            info.Password = "1";
            info.WindowsAuthentication = false;
            info.ConnectTimeoutSeconds = 3;
            this.ExternalConnection = info;
        }

        public SqlServerConnectionInfo InternalConnection { get; set; }

        public SqlServerConnectionInfo ExternalConnection { get; set; }
    }
    #endregion
}
