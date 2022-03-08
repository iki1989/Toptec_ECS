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
    public class AppSettingViewModel : Notifier
    {
        [DisplayName("앱 설정")]
        public EcsServerAppManagerSetting SettingApp => EcsServerAppManager.Instance.Setting;
    }
}