using ECS.Core.Managers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Wpf;

namespace ECS.Touch
{
    public partial class App : Application
    {
        private EcsTouchAppManager appManager;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode originalMode = this.ShutdownMode;
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            if (this.PrepareApp() == false)
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
            this.TerminateApp();

            base.OnExit(e);
        }
        private bool PrepareApp()
        {
            appManager = new EcsTouchAppManager(this);

            try
            {
                #region Load Settings
                FileSettingManager settingManager = FileSettingManager.Instance;

                EcsTouchAppManagerSetting appManagerSetting;
                #region Load App Manager Setting
                try
                {
                    if (settingManager.ExistsByName(appManager.SettingFileName))
                    {
                        appManagerSetting = settingManager.LoadByName<EcsTouchAppManagerSetting>(appManager.SettingFileName);
                    }
                    else
                    {
                        appManagerSetting = new EcsTouchAppManagerSetting();
                        settingManager.SaveByName(appManagerSetting, appManager.SettingFileName);
                    }
                }
                catch (Exception ex)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"App Manager Setting Load Error:");
                    sb.AppendLine($"File: {appManager.SettingFileName}");
                    sb.AppendLine($"Exception: {ex.Message}");
                    sb.AppendLine();
                    sb.Append($"Do you want to continue with the default setting?");
                    if (MsgBox.ShowOkCancel(sb.ToString()) != MessageBoxResult.OK) return false;

                    appManagerSetting = new EcsTouchAppManagerSetting();
                }
                #endregion
                #endregion

                appManager.ApplySetting(appManagerSetting);

                if (appManager.Initialize() == false) return false;

                appManager.Create();
                appManager.Prepare();

                return true;
            }
            catch (Exception ex)
            {
                string text = $"Prepare Application Error: {ex}";
                appManager.Logger?.Write(text);
                MsgBox.ShowError(text);
                return false;
            }
        }
        private void TerminateApp()
        {
            try
            {
                appManager.Terminate();
            }
            catch (Exception ex)
            {
                string text = $"Terminate Application Error: {ex}";
                MsgBox.ShowError(text);
                appManager.Logger?.Write(text);
            }
            finally
            {
                Log.Terminate();
            }
        }
    }
}
