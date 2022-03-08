using ECS.Core.Restful;
using ECS.Model;
using ECS.Model.Databases;
using ECS.Model.Hub;
using ECS.Model.Inkject;
using ECS.Model.Plc;
using ECS.Model.Restfuls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Urcis.SmartCode;

namespace ECS.Core.Managers
{
    public class RestfulRequsetSpiralManager : RestfulRequsetManager
    {
        #region Field
        public RestfulSpiralRequester SpiralBuffer2fRequester;
        #endregion

        #region Prop
        public new RestfulRequsetSpiralManagerSetting Setting
        {
            get => base.Setting as RestfulRequsetSpiralManagerSetting;
            set => base.Setting = value;
        }
        #endregion

        #region Ctor
        public RestfulRequsetSpiralManager(RestfulRequsetSpiralManagerSetting setting)
        {
            this.Setting = setting ?? new RestfulRequsetSpiralManagerSetting();
            this.Name = HubServiceName.SpiralPost;

            this.SpiralBuffer2fRequester = new RestfulSpiralRequester(this.Setting.DomainName, this.Setting.SpiralBuffer2f);
        }
        #endregion
    }

    [Serializable]
    public class RestfulRequsetSpiralManagerSetting : RestfulRequsetManagerSetting
    {
        [DisplayName("설비가동 상태")]
        public string SpiralBuffer2f { get; set; } = "api/spiral/buffer2f";
    }
}
