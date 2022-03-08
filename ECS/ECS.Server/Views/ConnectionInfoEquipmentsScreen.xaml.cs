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
using System.Windows.Threading;

namespace ECS.Server.Views
{
    /// <summary>
    /// Interaction logic for ConnectionInfoEqupmentsScreen.xaml
    /// </summary>
    public partial class ConnectionInfoEquipmentsScreen : Page
    {
        #region event
        private EventHandler tickEventHandler;
        #endregion

        #region field
        private DispatcherTimer timer = new DispatcherTimer();
        private ConnectionInfoEquipmentsModel viewModel;
        private bool isLoaded;
        #endregion

        public ConnectionInfoEquipmentsScreen()
        {
            InitializeComponent();

            this.viewModel = this.DataContext as ConnectionInfoEquipmentsModel;


            this.tickEventHandler = new EventHandler(timer_Tick);
            this.timer.Interval = TimeSpan.FromMilliseconds(1000);
        }

        #region Event Handler
        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.isLoaded && this.IsVisible)
                this.viewModel.UpdateEquipmentConnectonInfos();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.isLoaded = true;
            this.timer.Tick += this.tickEventHandler;
            this.timer.Start();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.isLoaded = false;
            this.timer.Stop();
            this.timer.Tick -= this.tickEventHandler;
        }
        #endregion
    }
}
