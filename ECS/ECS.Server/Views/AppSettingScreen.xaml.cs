using ECS.Core.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ECS.Server.Views
{
    /// <summary>
    /// AppSettingScreen.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AppSettingScreen : Page
    {
        public AppSettingScreen()
        {
            InitializeComponent();

            if (this.DataContext is AppSettingViewModel viewModel)
                this.propertyGrid.SelectedObject = viewModel.SettingApp;
        }
    }
}