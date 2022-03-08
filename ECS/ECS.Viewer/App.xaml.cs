using ECS.Core.Managers;
using ECS.Core.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Serialization;
using Urcis.SmartCode.Wpf;

namespace ECS.Viewer
{
    public partial class App : Application
    {
        public new MainWindow MainWindow => base.MainWindow as MainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EcsSplashScreen.Show();

            ShutdownMode originalMode = this.ShutdownMode;
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            if (this.PrepareApplication() == false)
            {
                if (this.StartupUri != null)
                    this.StartupUri = WpfHelper.GetHiddenWindowUri();
                this.Shutdown();
                return;
            }

            this.ShutdownMode = originalMode;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            EcsSplashScreen.Show();
            this.TerminateApplication();

            base.OnExit(e);
        }

        private bool PrepareApplication()
        {
            try
            {
                EcsViewerAppManager appManager = new EcsViewerAppManager(this);

                EcsViewerAppManagerSetting setting = null;

                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    AppDirectory.Instance = new AppDirectory(@"C:\URCIS\ECS Viewer");

                    #region Download App Manager Setting
                    string uri = $"http://{ApplicationDeployment.CurrentDeployment.UpdateLocation.Host}:{ApplicationDeployment.CurrentDeployment.UpdateLocation.Port}/Setting/App Manager.txt";

                    HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                    request.Method = "GET";

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            setting = new ScJsonSerializer().Deserialize(typeof(EcsViewerAppManagerSetting), sr.ReadToEnd()) as EcsViewerAppManagerSetting;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Load App Manager Setting
                    FileSettingManager settingManager = FileSettingManager.Instance;
                    try
                    {
                        if (settingManager.ExistsByName(appManager.SettingFileName))
                        {
                            setting = settingManager.LoadByName<EcsViewerAppManagerSetting>(appManager.SettingFileName);
                        }
                        else
                        {
                            setting = new EcsViewerAppManagerSetting();
                            settingManager.SaveByName(setting, appManager.SettingFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"App Manager Setting Load Error:");
                        sb.AppendLine($"File: {appManager.SettingFileName}");
                        sb.AppendLine($"Exception: {ex.Message}");
                        sb.AppendLine();
                        sb.Append($"Do you want to continue with the default setting?");
                        if (MsgBox.ShowOkCancel(sb.ToString()) != MessageBoxResult.OK) return false;

                        setting = new EcsViewerAppManagerSetting();
                    }
                    #endregion
                }

                setting.ThreadCultureName = "ko-KR";
                setting.UiCultureName = setting.ThreadCultureName;
                appManager.ApplySetting(setting);

                if (appManager.Initialize() == false) return false;

                appManager.Create();
                appManager.Prepare();

                return true;
            }
            catch (Exception ex)
            {
                string text = $"Prepare Application Error: {ex}";
                AppManager.Instance?.Logger?.Write(text);
                MsgBox.ShowError(text);
                return false;
            }
        }

        private void TerminateApplication()
        {
            try
            {
                // Terminate App Manager
                AppManager.Instance?.Terminate();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Terminate Application Error: {ex}");
            }
            finally
            {
                Log.Terminate();
            }
        }
    }
}
