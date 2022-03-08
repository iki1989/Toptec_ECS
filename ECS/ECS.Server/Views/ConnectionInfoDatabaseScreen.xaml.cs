using System;
using System.Windows;
using System.Collections;
using System.Security.Cryptography;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using ECS.Core.ViewModels.Server;
using ECS.Core.Util;

namespace ECS.Server.Views
{
    /// <summary>
    /// Interaction logic for ConnectionInfoDatabaseScreen.xaml
    /// </summary>
    public partial class ConnectionInfoDatabaseScreen : Page
    {
        public ConnectionInfoDatabaseScreen()
        {
            InitializeComponent();

            if (this.DataContext is ConnectionInfoDatabaseViewModel viewModel)
            {
                DataBaseConnectioninfo setting = new DataBaseConnectioninfo();

                var con = viewModel.Manager.Setting.SqlConnectionStringBuilder;
                setting.DataSource = con.DataSource;
                setting.InitialCatalog = con.InitialCatalog;
                setting.UserID = con.UserID;
                //setting.Password = con.Password;
                //setting.Password = SecurityHelper.MD5_Encrypt(con.Password);
                setting.Password = "******";
                this.propertyGrid.SelectedObject = setting; 
            }
        }

    }

    public struct DataBaseConnectioninfo
    {
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
    }
}
