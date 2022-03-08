using System.ServiceModel;

namespace ECS.Core.WebServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DefaultRestfulServerService : BaseService, IDefaultRestfulServerService
    {
        public string GetServiceAlive() => "I am Alive";
    }
}
