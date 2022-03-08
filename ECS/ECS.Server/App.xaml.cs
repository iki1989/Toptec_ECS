using ECS.Core.Managers;
using ECS.Core.Util;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Wpf;

namespace ECS.Server
{
    public partial class App : Application
    {
        public EcsServerAppManager appManager;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EcsSplashScreen.Show();

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
            EcsSplashScreen.Show();
            this.TerminateApp();

            base.OnExit(e);
        }

        private bool PrepareApp()
        {
            appManager = new EcsServerAppManager(this);

            try
            {
                #region Load Settings
                FileSettingManager settingManager = FileSettingManager.Instance;

                EcsServerAppManagerSetting appManagerSetting;
                #region Load App Manager Setting
                try
                {
                    if (settingManager.ExistsByName(appManager.SettingFileName))
                    {
                        appManagerSetting = settingManager.LoadByName<EcsServerAppManagerSetting>(appManager.SettingFileName);
                    }
                    else
                    {
                        appManagerSetting = new EcsServerAppManagerSetting();
                        settingManager.SaveByName(appManagerSetting, appManager.SettingFileName);
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

                    appManagerSetting = new EcsServerAppManagerSetting();
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
