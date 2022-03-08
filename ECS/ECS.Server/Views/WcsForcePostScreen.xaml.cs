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
using Urcis.SmartCode.Wpf;

namespace ECS.Server.Views
{
    /// <summary>
    /// WcsForcePostScreen.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WcsForcePostScreen : Page
    {
        private WcsForcePostViewModel viewModel = null;
        public WcsForcePostScreen()
        {
            InitializeComponent();

            this.propertyGrid.SelectedObject = new WcsForcePostViewModel.DataClass();

            if (this.DataContext is WcsForcePostViewModel vm)
                this.viewModel = vm;
        }

        private async void BoxIdButton_Click(object sender, RoutedEventArgs e)
        {
            this.propertyGrid.SelectedObject = await this.viewModel.GetOrderDataAsync(this.BoxId.Text, this.InvoiceId.Text);
        }

        private async void InvoiceIdButton_Click(object sender, RoutedEventArgs e)
        {
            this.propertyGrid.SelectedObject = await this.viewModel.GetOrderDataAsync(this.BoxId.Text, this.InvoiceId.Text);
        }

        private void RsltWgtPostButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.propertyGrid.SelectedObject is WcsForcePostViewModel.DataClass data)
            {
                if (MsgBox.ShowOkCancel("중량 검수 보고를 하시겠습니까?", "WCS 강제 보고") == MessageBoxResult.OK)
                {
                    this.viewModel.WeightPostAsync(data);
                    MsgBox.ShowInfo($"Boxid = {data.box_id} 중량 검수 실적이 전송되었습니다.", "WCS 강제 보고");
                }
            }
        }

        private void RsltWaybillPostButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.propertyGrid.SelectedObject is WcsForcePostViewModel.DataClass data)
            {
                if (MsgBox.ShowOkCancel("송장 검증 보고를 하시겠습니까?", "WCS 강제 보고") == MessageBoxResult.OK)
                {
                    this.viewModel.InvoicePostAsync(data);
                    MsgBox.ShowInfo($"Boxid = {data.box_id} 송장 검증 실적이 전송되었습니다.", "WCS 강제 보고");
                }
            }
        }
    }

}
