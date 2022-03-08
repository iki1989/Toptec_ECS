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
    public class ConnectionInfoRicpRestfulViewModel : Notifier
    {
        [DisplayName("RICP Restful Url"), ReadOnly(true)]
        public RestfulRequsetRicpManager Manager => (RestfulRequsetRicpManager)EcsServerAppManager.Instance.RestfulRequsetManagers[HubServiceName.RicpPost];
    }
}
