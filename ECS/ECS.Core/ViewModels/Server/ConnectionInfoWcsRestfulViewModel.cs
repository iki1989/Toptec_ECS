using ECS.Core.Managers;
using ECS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.ViewModels.Server
{
    public class ConnectionInfoWcsRestfulViewModel : Notifier
    {
        [DisplayName("WCS Restful Url"), ReadOnly(true)]
        public RestfulRequsetWcsManager Manager => (RestfulRequsetWcsManager)EcsServerAppManager.Instance.RestfulRequsetManagers[HubServiceName.WcsPost];
    }
}
