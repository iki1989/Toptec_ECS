using ECS.Core.Util;
using ECS.Core.ViewModels.Touch;
using ECS.Model.Pcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ECS.Touch.Views
{
    /// <summary>
    /// Interaction logic for BcrLcdScreen.xaml
    /// </summary>
    public partial class SmartPackingScreen : Page
    {
        private SharpTimer midNightTimer = new SharpTimer(0, 0, 0);

        public SmartPackingScreen()
        {
            InitializeComponent();


            this.midNightTimer.Elapsed += MidNightTimer_Elapsed;
            this.midNightTimer.Start();
        }

        private void SmartUpClick(object sender, RoutedEventArgs e) => ViewModel.SmartMove(true);
        private void SmartDownClick(object sender, RoutedEventArgs e) => ViewModel.SmartMove(false);
        private void SmartTopClick(object sender, RoutedEventArgs e) => ViewModel.SmartMoveTop();
        private void SmartBottomClick(object sender, RoutedEventArgs e) => ViewModel.SmartMoveBottom();

        private void SmartSearchClick(object sender, RoutedEventArgs e) =>
            ViewModel.SmartSearch();
        private void SmartRefreshClick(object sender, RoutedEventArgs e) =>
            ViewModel.SmartRefresh();

        private void ManualPopupOpenClick(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenManualPopup();
        }


        private void MidNightTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.midNightTimer.SetSarpInterval();
        }

        private void ErrorMessageCloseClick(object sender, RoutedEventArgs e)
        {
            BcrAlarmSetReset bcrAlarmSetReset = new BcrAlarmSetReset();

            ViewModel.BcrAlarmReset(bcrAlarmSetReset);
            ViewModel.ShowErrorMessage = false;
        }

        private void Smart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.BoxId = this.BoxId.Text;
                ViewModel.SmartSearch();
            }
            else
            {
                //BoxId 10글자
                if (this.BoxId.Text.Length == 10)
                    this.BoxId.Text = string.Empty;
            }
        }

        private void ManualPopupCloseClick(object sender, RoutedEventArgs e) => ViewModel.ShowManualPopup = false;

        private void ManualAmountUp(object sender, RoutedEventArgs e) => ++ViewModel.ManualAmount;
        private void ManualAmountDown(object sender, RoutedEventArgs e) => --ViewModel.ManualAmount;

        private void ManualProcessClick(object sender, RoutedEventArgs e) => ViewModel.ManualProcess();
    }
}
