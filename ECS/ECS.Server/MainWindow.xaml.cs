using ECS.Core.Managers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Urcis.SmartCode.Wpf;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using ECS.Model;
using Urcis.SmartCode;
using ECS.Core.Equipments;
using ECS.Core.Util;

namespace ECS.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Uri AppSettingScreen = new Uri("/Views/AppSettingScreen.xaml", UriKind.Relative);

        private Uri connectionInfoDatabaseScreen = new Uri("/Views/ConnectionInfoDatabaseScreen.xaml", UriKind.Relative);
        private Uri ConnectionInfoEquipmentsScreen = new Uri("/Views/ConnectionInfoEquipmentsScreen.xaml", UriKind.Relative);
        private Uri ConnectionInfoWebServiceScreen = new Uri("/Views/ConnectionInfoWebServiceScreen.xaml", UriKind.Relative);
        private Uri ConnectionInfoWcsRestfulScreen = new Uri("/Views/ConnectionInfoWcsRestfulScreen.xaml", UriKind.Relative);
        private Uri ConnectionInfoRicpRestfulScreen = new Uri("/Views/ConnectionInfoRicpRestfulScreen.xaml", UriKind.Relative);
        private Uri ConnectionInfoSpiralRestfulScreen = new Uri("/Views/ConnectionInfoSpiralRestfulScreen.xaml", UriKind.Relative);

        private Uri PlcIoMonitoringScreen = new Uri("/Views/MonitoringPlcIoScreen.xaml", UriKind.Relative);

        private Uri WcsForcePostScreen = new Uri("/Views/WcsForcePostScreen.xaml", UriKind.Relative);

        public MainWindow()
        {
            InitializeComponent();

            this.Title += $" v{ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
            this.TitleBlock.Text = this.Title;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Uri url = null;

            if (e.OriginalSource == this.AppSettingMenuItem)
                url = this.AppSettingScreen;
            else if (e.OriginalSource == this.SaveMenuItem)
            {
                EcsServerAppManager.Instance.SaveSetting();
                Process.Start(AppDirectory.Instance.Setting);
            }
            else if (e.OriginalSource == this.CloseMenuItem)
                this.Close();

            else if (e.OriginalSource == this.DatabaseMenuItem)
                url = this.connectionInfoDatabaseScreen;
            else if (e.OriginalSource == this.EquipmentsMenuItem)
                url = this.ConnectionInfoEquipmentsScreen;
            else if (e.OriginalSource == this.WebServiceMenuItem)
                url = this.ConnectionInfoWebServiceScreen;
            else if (e.OriginalSource == this.WcsRestfulMenuItem)
                url = this.ConnectionInfoWcsRestfulScreen;
            else if (e.OriginalSource == this.RicpRestfulMenuItem)
                url = this.ConnectionInfoRicpRestfulScreen;
            else if (e.OriginalSource == this.SpiralRestfulMenuItem)
                url = this.ConnectionInfoSpiralRestfulScreen;

            else if (e.OriginalSource == this.PlcIoMenuItem)
                url = this.PlcIoMonitoringScreen;

            else if (e.OriginalSource == this.MemoryReloadMenuItem)
            {
                if (MsgBox.ShowOkCancel("메모리 리로드를 하시겠습니까?") == MessageBoxResult.OK)
                    EcsServerAppManager.Instance.Cache.ProductInfoReLoadAsync();
            }
            else if (e.OriginalSource == this.DatabaseBackupMenuItem)
            {
                if (MsgBox.ShowOkCancel("데이터베이스 백업을 하시겠습니까?") == MessageBoxResult.OK)
                {
                    EcsServerAppManager.Instance.DataBaseManagerForServer.BackupDatabaseAsync();
                    Process.Start(EcsAppDirectory.MssqlBackup);
                }
            }
            else if (e.OriginalSource == this.LogFolderOpenMenuItem)
                Process.Start(AppDirectory.Instance.Log);

            else if (e.OriginalSource == this.WcsForcePostMenuItem)
                    url = this.WcsForcePostScreen;

            if (url != null)
                this.frame.Source = url;
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (MsgBox.ShowOkCancel("정말 종료하시겠습니까?") != MessageBoxResult.OK)
                e.Cancel = true;

            base.OnClosing(e);
        }

        private void ButtonMinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonRestoreWindow_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else if(this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
        }
    }
}
