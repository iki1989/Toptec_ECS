using ECS.Core.Util;
using ECS.Core.ViewModels.Viewer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ECS.Viewer.Views.Pages
{
    /// <summary>
    /// Interaction logic for TrackingSearchScreen.xaml
    /// </summary>
    public partial class TrackingSearchScreen : Page
    {
        private TrackingSearchViewModel ViewModel => (TrackingSearchViewModel)DataContext;
        private SharpTimer midNightTimer = new SharpTimer(0, 0, 0);

        public TrackingSearchScreen()
        {
            InitializeComponent();
            this.ViewModel.Clear();
            this.InitDataPickier();

            this.midNightTimer.Elapsed += MidNightTimer_Elapsed;
            this.midNightTimer.Start();
        }

        private void InitDataPickier()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                this.Begin.SelectedDate = DateTime.Today;
                this.Begin.DisplayDateEnd = DateTime.Today;
                this.End.SelectedDate = DateTime.Today;
                this.End.DisplayDateEnd = DateTime.Today;
            }));
        }
        private void MidNightTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.midNightTimer.SetSarpInterval();

        }


        private void SearchClick(object sender, RoutedEventArgs e)
        {
            EcsSplashScreen.Show();
            ViewModel.Search();
        }
        private void ExportClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Export();
        }

        private void PageLeftButtonClick(object sender, RoutedEventArgs e)
        {
            EcsSplashScreen.Show();
            --ViewModel.Page;
        }
        private void PageRightButtonClick(object sender, RoutedEventArgs e)
        {
            EcsSplashScreen.Show();
            ++ViewModel.Page;
        }

        private void PageKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EcsSplashScreen.Show();
                ViewModel.Page = int.Parse(((TextBox)sender).Text);
            }
        }

        private void SearchKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EcsSplashScreen.Show();
                ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
                ViewModel.Search();
            }
        }
    }
}
