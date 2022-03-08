using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ECS.Model.Plc;

namespace ECS.Touch.Views
{
    public class TestObject2
    {
    }
    /// <summary>
    /// Interaction logic for BcrLcdScreen.xaml
    /// </summary>
    public partial class ConveyorScreen : Page
    {
        public ConveyorScreen()
        {
            InitializeComponent();
        }
        private void ErectorSpeed1_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.CaseErector_Speed1, "잉크젯 벨트 컨베이어#1  하부 속도/BM_003");
        private void ErectorSpeed2_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.CaseErector_Speed2, "BCR 벨트 컨베이어#1 하부 속도/BM_009");
        private void ErectorSpeed3_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.CaseErector_Speed3, "잉크젯 벨트 컨베이어#2 상부 속도/BM_014");
        private void ErectorSpeed4_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.CaseErector_Speed4, "BCR 벨트 컨베이어#2 상부 속도/BM_019");
        private void ErectorSpeed5_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.CaseErector_Speed5, "경사 컨베이어 /BM_015");
        private void WeightInvoice1_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.WeightInvoice_Speed1, "중량 전 벨트 컨베이어 속도/BM_006");
        private void WeightInvoice2_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.WeightInvoice_Speed2, "경사 벨트 컨베이어#1 속도/BM_022");
        private void WeightInvoice3_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.WeightInvoice_Speed3, "경사 벨트 컨베이어#2 속도/BM_36");
        private void WeightInvoice4_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.WeightInvoice_Speed4, "경사#2 후 벨트 컨베이어  속도/BM_037");
        private void WeightInvoice5_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.WeightInvoice_Speed5, "분기 BCR 벨트 컨베이어 속도/BM_072");
        private void WeightInvoice6_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.WeightInvoice_Speed6, "오토라벨러 벨트 컨베이어#1 속도/BM_080~83");
        private void WeightInvoice10_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.WeightInvoice_Speed10, "오토라벨러 벨트 컨베이어#5 속도/BM_091~94");
        private void WeightInvoice14_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.WeightInvoice_Speed14, "상면 BCR 벨트 컨베이어#1 속도/BM_098~101");
        private void WeightInvoice18_Click(object sender, RoutedEventArgs e) => ViewModel.ConveyorClick(ConveyorSpeedEnum.WeightInvoice_Speed18, "리프트 배출 벨트 컨베이어#1 속도/BM_107~109");
        private void ConveyorSpeedClose_Click(object sender, RoutedEventArgs e) => ViewModel.ShowConveyorSpeedPopup = false;
        private void ConveyorSpeedPopopSubmitClick(object sender, RoutedEventArgs e) => ViewModel.ConveyorSubmit();
        private void ErrorMessageCloseClick(object sender, RoutedEventArgs e) => ViewModel.ShowErrorMessage = false;
        private void ToggleClick(object sender, RoutedEventArgs e) => ViewModel.ToggleClick();
        private void RouteModeSubmitClick(object sender, RoutedEventArgs e) => ViewModel.RouteModeSubmit();

        private void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TabItem && ViewModel.TabIndex != 2)
            {
                ViewModel.ShowPasswordBox = true;
                e.Handled = true;
            }
        }

        private void PasswordBoxCloseClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowPasswordBox = false;
        }

        private void PasswordBoxConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CheckPassword(Password.Password))
            {
                ViewModel.ShowPasswordBox = false;
                ViewModel.RouteModeClick();
            }
            else
            {
                ViewModel.ShowErrorMessageBox("비밀 번호가 일치하지 않습니다.");
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(((Key.D0 <= e.Key) && (e.Key <= Key.D9))
               || ((Key.NumPad0 <= e.Key) && (e.Key <= Key.NumPad9))
               || e.Key == Key.Back))
            {
                e.Handled = true;
            }
        }

        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var a = e.NewValue;
        }
    }
}
