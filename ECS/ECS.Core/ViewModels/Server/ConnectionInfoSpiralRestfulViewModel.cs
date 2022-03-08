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
    public class ConnectionInfoSpiralRestfulViewModel : Notifier
    {
        [DisplayName("Spiral Restful Url"), ReadOnly(true)]
        public RestfulRequsetSpiralManager Manager => (RestfulRequsetSpiralManager)EcsServerAppManager.Instance.RestfulRequsetManagers[HubServiceName.SpiralPost];
    }
}
