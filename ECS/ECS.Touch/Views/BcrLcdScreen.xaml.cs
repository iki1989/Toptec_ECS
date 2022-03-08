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
    public partial class BcrLcdScreen : Page
    {
        private SharpTimer midNightTimer = new SharpTimer(0, 0, 0);

        public BcrLcdScreen()
        {
            InitializeComponent();

            this.InitDataPickier();

            this.midNightTimer.Elapsed += MidNightTimer_Elapsed;
            this.midNightTimer.Start();
        }
        private void InitDataPickier()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                this.OutBcrBegin.DisplayDateStart = DateTime.Now.AddDays(-30);
                this.OutBcrBegin.DisplayDateEnd = DateTime.Now;
                this.OutBcrEnd.DisplayDateStart = DateTime.Now.AddDays(-30);
                this.OutBcrEnd.DisplayDateEnd = DateTime.Now;
            }));
        }

        private void SmartSummaryPrintBcrUpClick(object sender, RoutedEventArgs e) => ViewModel.SmartSummaryPrintBcrMove(true);
        private void SmartSummaryPrintBcrDownClick(object sender, RoutedEventArgs e) => ViewModel.SmartSummaryPrintBcrMove(false);
        private void SmartSummaryPrintBcrTopClick(object sender, RoutedEventArgs e) => ViewModel.SmartSummaryPrintBcrMoveTop();
        private void SmartSummaryPrintBcrBottomClick(object sender, RoutedEventArgs e) => ViewModel.SmartSummaryPrintBcrMoveBottom();
        private void SmartTopBcrUpClick(object sender, RoutedEventArgs e) => ViewModel.SmartTopBcrMove(true);
        private void SmartTopBcrDownClick(object sender, RoutedEventArgs e) => ViewModel.SmartTopBcrMove(false);
        private void SmartTopBcrTopClick(object sender, RoutedEventArgs e) => ViewModel.SmartTopBcrMoveTop();
        private void SmartTopBcrBottomClick(object sender, RoutedEventArgs e) => ViewModel.SmartTopBcrMoveBottom();
        private void NormalSummaryPrintBcrUpClick(object sender, RoutedEventArgs e) => ViewModel.NormalSummaryPrintBcrMove(true);
        private void NormalSummaryPrintBcrDownClick(object sender, RoutedEventArgs e) => ViewModel.NormalSummaryPrintBcrMove(false);
        private void NormalSummaryPrintBcrTopClick(object sender, RoutedEventArgs e) => ViewModel.NormalSummaryPrintBcrMoveTop();
        private void NormalSummaryPrintBcrBottomClick(object sender, RoutedEventArgs e) => ViewModel.NormalSummaryPrintBcrMoveBottom();
        private void NormalTopBcrUpClick(object sender, RoutedEventArgs e) => ViewModel.NormalTopBcrMove(true);
        private void NormalTopBcrDownClick(object sender, RoutedEventArgs e) => ViewModel.NormalTopBcrMove(false);
        private void NormalTopBcrTopClick(object sender, RoutedEventArgs e) => ViewModel.NormalTopBcrMoveTop();
        private void NormalTopBcrBottomClick(object sender, RoutedEventArgs e) => ViewModel.NormalTopBcrMoveBottom();
        private void SmartRouteBcrUpClick(object sender, RoutedEventArgs e) => ViewModel.SmartRouteBcrMove(true);
        private void SmartRouteBcrDownClick(object sender, RoutedEventArgs e) => ViewModel.SmartRouteBcrMove(false);
        private void SmartRouteBcrTopClick(object sender, RoutedEventArgs e) => ViewModel.SmartRouteBcrMoveTop();
        private void SmartRouteBcrBottomClick(object sender, RoutedEventArgs e) => ViewModel.SmartRouteBcrMoveBottom();
        private void NormalRouteBcrUpClick(object sender, RoutedEventArgs e) => ViewModel.NormalRouteBcrMove(true);
        private void NormalRouteBcrDownClick(object sender, RoutedEventArgs e) => ViewModel.NormalRouteBcrMove(false);
        private void NormalRouteBcrTopClick(object sender, RoutedEventArgs e) => ViewModel.NormalRouteBcrMoveTop();
        private void NormalRouteBcrBottomClick(object sender, RoutedEventArgs e) => ViewModel.NormalRouteBcrMoveBottom();
        private void SmartPrintBcrUpClick(object sender, RoutedEventArgs e) => ViewModel.SmartPrintBcrMove(true);
        private void SmartPrintBcrDownClick(object sender, RoutedEventArgs e) => ViewModel.SmartPrintBcrMove(false);
        private void SmartPrintBcrTopClick(object sender, RoutedEventArgs e) => ViewModel.SmartPrintBcrMoveTop();
        private void SmartPrintBcrBottomClick(object sender, RoutedEventArgs e) => ViewModel.SmartPrintBcrMoveBottom();
        private void NormalPrintBcrUpClick(object sender, RoutedEventArgs e) => ViewModel.NormalPrintBcrMove(true);
        private void NormalPrintBcrDownClick(object sender, RoutedEventArgs e) => ViewModel.NormalPrintBcrMove(false);
        private void NormalPrintBcrTopClick(object sender, RoutedEventArgs e) => ViewModel.NormalPrintBcrMoveTop();
        private void NormalPrintBcrBottomClick(object sender, RoutedEventArgs e) => ViewModel.NormalPrintBcrMoveBottom();
        private void TopBcrUpClick(object sender, RoutedEventArgs e) => ViewModel.TopBcrMove(true);
        private void TopBcrDownClick(object sender, RoutedEventArgs e) => ViewModel.TopBcrMove(false);
        private void TopBcrTopClick(object sender, RoutedEventArgs e) => ViewModel.TopBcrMoveTop();
        private void TopBcrBottomClick(object sender, RoutedEventArgs e) => ViewModel.TopBcrMoveBottom();
        private void OutBcrUpClick(object sender, RoutedEventArgs e) => ViewModel.OutBcrMove(true);
        private void OutBcrDownClick(object sender, RoutedEventArgs e) => ViewModel.OutBcrMove(false);
        private void OutBcrTopClick(object sender, RoutedEventArgs e) => ViewModel.OutBcrMoveTop();
        private void OutBcrBottomClick(object sender, RoutedEventArgs e) => ViewModel.OutBcrMoveBottom();

        private void OutBcrSearchClick(object sender, RoutedEventArgs e)
            => ViewModel.OutBcrSearch();
        private void OutBcrRefresh()
        {
            this.OutBcrId.Text = "";
            this.OutBcrBegin.SelectedDate = DateTime.Now;
            this.OutBcrEnd.SelectedDate = DateTime.Now;
        }
        private void OutBcrRefreshClick(object sender, RoutedEventArgs e)
        {
            this.OutBcrRefresh();
            ViewModel.OutBcrRefresh();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                this.OutBcrRefresh();
                ViewModel.ScrollReset();
            }
        }

        private void MidNightTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.midNightTimer.SetSarpInterval();

            this.InitDataPickier();
        }

        private void ErrorMessageCloseClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.TopBcrConnection == false) return;

            BcrAlarmSetReset bcrAlarmSetReset = new BcrAlarmSetReset();

            ViewModel.BcrAlarmReset(bcrAlarmSetReset);
            ViewModel.ShowErrorMessage = false;
        }

        private void OutBcrId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.OutBcrId = this.OutBcrId.Text;
                ViewModel.OutBcrSearch();
            }
            else
            {
                //BoxId 10글자
                if (this.OutBcrId.Text.Length == 10)
                    this.OutBcrId.Text = string.Empty;
            }
        }
    }
}
