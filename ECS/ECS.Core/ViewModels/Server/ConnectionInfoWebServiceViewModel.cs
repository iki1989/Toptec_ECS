using ECS.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.ViewModels.Server
{
    public class ConnectionInfoWebServiceViewModel : Notifier
    {
        [DisplayName("웹서비스 정보"), ReadOnly(true)]
        public WebServiceManager Manager => EcsServerAppManager.Instance.WebManager;
    }
}
