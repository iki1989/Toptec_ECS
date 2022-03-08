using ECS.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.ViewModels.Server
{
    public class ConnectionInfoDatabaseViewModel : Notifier
    {
        [DisplayName("데이터베이스 정보"), ReadOnly(true)]
        public DataBaseManagerForServer Manager => EcsServerAppManager.Instance.DataBaseManagerForServer;
    }
}
