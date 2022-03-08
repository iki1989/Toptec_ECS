using ECS.Core.ViewModels.Touch;
using ECS.Model.Domain.Touch;
using ECS.Model.Pcs;
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

namespace ECS.Touch.Views
{
    /// <summary>
    /// Interaction logic for BcrLcdScreen.xaml
    /// </summary>
    public partial class WeightInspectorScreen : Page
    {
        public WeightInspectorScreen()
        {
            InitializeComponent();
        }

        private void WeightUpClick(object sender, RoutedEventArgs e) => this.ViewModel.WeightMove(true);
        private void WeightDownClick(object sender, RoutedEventArgs e) => this.ViewModel.WeightMove(false);
        private void WeightTopClick(object sender, RoutedEventArgs e) => this.ViewModel.WeightMoveTop();
        private void WeightBottomClick(object sender, RoutedEventArgs e) => this.ViewModel.WeightMoveBottom();

        private void RefreshClick(object sender, RoutedEventArgs e) => this.ViewModel.Refresh();

        private void PopupCloseClick(object sender, RoutedEventArgs e) => this.ViewModel.ShowPopup = false;
        private void SearchClick(object sender, RoutedEventArgs e) => this.ViewModel.Search();

        private void WeightSearchUpClick(object sender, RoutedEventArgs e) => this.ViewModel.WeightSearchMove(true);
        private void WeightSearchDownClick(object sender, RoutedEventArgs e) => this.ViewModel.WeightSearchMove(false);
        private void WeightSearchTopClick(object sender, RoutedEventArgs e) => this.ViewModel.WeightSearchMoveTop();
        private void WeightSearchBottomClick(object sender, RoutedEventArgs e) => this.ViewModel.WeightSearchMoveBottom();

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid.SelectedItem != null)
            {
                this.ViewModel.SelectBox((WeightCheckData)grid.SelectedItem);
            }
        }

        private void ErrorMessageCloseClick(object sender, RoutedEventArgs e)
        {
            BcrAlarmSetReset bcrAlarmSetReset = new BcrAlarmSetReset();

            ViewModel.BcrAlarmReset(bcrAlarmSetReset);
            ViewModel.ShowErrorMessage = false;
        }

        private void BoxId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.ViewModel.BoxId = this.BoxId.Text;
                this.ViewModel.Search();
            }
            else
            {
                //BoxId 10글자
                if (this.BoxId.Text.Length == 10)
                    this.BoxId.Text = string.Empty;
            }
        }

        private void CstOrdNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.ViewModel.Search();
            }
        }
    }
}
