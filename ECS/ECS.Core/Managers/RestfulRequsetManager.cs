using ECS.Core.Restful;
using ECS.Model;
using ECS.Model.Hub;
using ECS.Model.Inkject;
using ECS.Model.Plc;
using ECS.Model.Restfuls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Urcis.SmartCode;
using Urcis.SmartCode.Threading;

namespace ECS.Core.Managers
{
    public abstract class RestfulRequsetManager : INameable
    {
        #region Prop
        public string Name { get; protected set; }

        public RestfulRequsetManagerSetting Setting { get; set; }

        public LifeCycleStateEnum LifeState { get; protected set; }
        #endregion

        #region Method
        public virtual void OnHub_Recived(EventArgs e) { }
        #endregion
    }

    [Serializable]
    public class RestfulRequsetManagerSetting : Setting
    {
        public string DomainName { get; set; } = "localhost";

        public RestfulRequsetManagerSetting()
        {
            this.DomainName = "http://127.0.0.1:8081";
        }
    }

    public class RestfulRequsetCollection : Dictionary<string, RestfulRequsetManager>
    {
        public T GetByRestfulRequsetManagerType<T>()
        {
            foreach (var item in this.Values)
            {
                if (item.GetType() == typeof(T))
                    return (T)Convert.ChangeType(item, typeof(T));
            }

            return default;
        }
    }
}
