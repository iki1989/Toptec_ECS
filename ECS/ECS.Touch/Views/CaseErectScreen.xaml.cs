using ECS.Core.Util;
using ECS.Core.ViewModels.Touch;
using ECS.Model.Domain.Touch;
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
using System.Windows.Threading;

namespace ECS.Touch.Views
{
    /// <summary>
    /// Interaction logic for BcrLcdScreen.xaml
    /// </summary>
    public partial class CaseErectScreen : Page
    {
        private SharpTimer midNightTimer = new SharpTimer(0, 0, 0);

        public CaseErectScreen()
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
                this.CaseErectBegin.SelectedDate = DateTime.Now;
                this.CaseErectBegin.DisplayDateStart = DateTime.Now.AddDays(-30);
                this.CaseErectBegin.DisplayDateEnd = DateTime.Now;
                this.CaseErectEnd.SelectedDate = DateTime.Now;
                this.CaseErectEnd.DisplayDateStart = DateTime.Now.AddDays(-30);
                this.CaseErectEnd.DisplayDateEnd = DateTime.Now;
            }));
        }

        private void CaseErectDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CaseErectBegin != null && this.CaseErectEnd != null &&
                this.CaseErectEnd.SelectedDate < this.CaseErectBegin.SelectedDate)
            {
                this.CaseErectEnd.SelectedDate = this.CaseErectBegin.SelectedDate;
            }
        }

        private void CaseErect1UpClick(object sender, RoutedEventArgs e) => ViewModel.CaseErect1Move(true);
        private void CaseErect1DownClick(object sender, RoutedEventArgs e) => ViewModel.CaseErect1Move(false);
        private void CaseErect1TopClick(object sender, RoutedEventArgs e) => ViewModel.CaseErect1MoveTop();
        private void CaseErect1BottomClick(object sender, RoutedEventArgs e) => ViewModel.CaseErect1MoveBottom();
        private void CaseErect2UpClick(object sender, RoutedEventArgs e) => ViewModel.CaseErect2Move(true);
        private void CaseErect2DownClick(object sender, RoutedEventArgs e) => ViewModel.CaseErect2Move(false);
        private void CaseErect2TopClick(object sender, RoutedEventArgs e) => ViewModel.CaseErect2MoveTop();
        private void CaseErect2BottomClick(object sender, RoutedEventArgs e) => ViewModel.CaseErect2MoveBottom();
        private void SearchClick(object sender, RoutedEventArgs e) => ViewModel.Search();
        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Refresh();
            this.CaseErectCount.Focus();
        }
        private void CaseErectSearchUpClick(object sender, RoutedEventArgs e) => ViewModel.CaseErectSearchMove(true);
        private void CaseErectSearchDownClick(object sender, RoutedEventArgs e) => ViewModel.CaseErectSearchMove(false);
        private void CaseErectSearchTopClick(object sender, RoutedEventArgs e) => ViewModel.CaseErectSearchMoveTop();
        private void CaseErectSearchBottomClick(object sender, RoutedEventArgs e) => ViewModel.CaseErectSearchMoveBottom();

        private void NumberingPopupOpenClick(object sender, RoutedEventArgs e)
        {
            Password.Password = "";
            ViewModel.ShowPasswordBox = true;
        }
        private void NumberingPopupCloseClick(object sender, RoutedEventArgs e) => ViewModel.ShowNumberingPopup = false;
        private void NumberingInsertClick(object sender, RoutedEventArgs e) => ViewModel.ShowMessageBox(this.ErectorType.SelectedIndex, true);
        private void NumberingUpdateClick(object sender, RoutedEventArgs e) => ViewModel.ShowMessageBox(this.ErectorType.SelectedIndex, false);
        private void NumberingDeleteClick(object sender, RoutedEventArgs e) => ViewModel.DeleteBoxInfo();
        private void NumberingPopupRefreshClick(object sender, RoutedEventArgs e) => ViewModel.NumberingPopupRefresh();
        private void NumberingUpClick(object sender, RoutedEventArgs e) => ViewModel.NumberingPopupMove(true);
        private void NumberingDownClick(object sender, RoutedEventArgs e) => ViewModel.NumberingPopupMove(false);
        private void NumberingTopClick(object sender, RoutedEventArgs e) => ViewModel.NumberingPopupMoveTop();
        private void NumberingBottomClick(object sender, RoutedEventArgs e) => ViewModel.NumberingPopupMoveBottom();
        private void NumberingMessageBoxCloseClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowNumberingMessageBox = false;

            if (this.ManualVerificationBoxId.Visibility == Visibility.Visible)
                this.BoxId.Focus();
            else
                this.CaseErectCount.Focus();


        }
        private void NumberingMessageBoxSubmitClick(object sender, RoutedEventArgs e) => ViewModel.SubmitMessageBox(this.ErectorType.SelectedIndex);
        private void BoxSettingGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                ViewModel.SelectedBoxInfo = (BoxInfoData)e.AddedItems[0];
            else
                ViewModel.SelectedBoxInfo = BoxInfoData.None;
        }

        private void BoxSettingGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.BoxSettingGrid.CurrentCell == null
                || this.BoxSettingGrid.CurrentCell.Item == null) return;
       
            if (this.BoxSettingGrid.CurrentCell.Item is BoxInfoData info)
                ViewModel.SelectedBoxInfo = info;
        }

        private void Resume1Click(object sender, RoutedEventArgs e) => ViewModel.InkjetResume(1);
        private void Resume2Click(object sender, RoutedEventArgs e) => ViewModel.InkjetResume(2);
        private void OpenNewPrint(object sender, RoutedEventArgs e)
        {
            this.newPrintCount.Text = "1";
            ViewModel.OpenNewPrint();
            this.newPrintCount.Focus();
        }

        private void NewPrintClick(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(this.newPrintCount.Text, out int count))
            {
                ViewModel.NewPrint(count);
                ViewModel.ShowNewPrint = false;
                this.CaseErectCount.Focus();
            }
                
        }

        private void NewPrintCloseClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowNewPrint = false;
            this.CaseErectCount.Focus();
        }

        private void newPrintCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(((Key.D0 <= e.Key) && (e.Key <= Key.D9))
               || ((Key.NumPad0 <= e.Key) && (e.Key <= Key.NumPad9))
               || e.Key == Key.Back))
            {
                e.Handled = true;
            }
        }

        private void ReprintClick(object sender, RoutedEventArgs e)
        {
            if (this.SearchGrid.CurrentCell == null) return;

            if (this.SearchGrid.SelectedCells.Count <= 0) return;

            var cellInfo = this.SearchGrid.SelectedCells[0];

            if (cellInfo.Item is CaseErectData data)
                ViewModel.Reprint(data.BoxId);

            //if (this.SearchGrid.SelectedItem != null)
            //{
            //    var data = (CaseErectData)this.SearchGrid.SelectedItem;
            //    ViewModel.Reprint(data.BoxId);
            //}
        }
        private void ManualVerificationOpenClick(object sender, RoutedEventArgs e)
        {
            BoxId.Text = "";
            ViewModel.OpenManualVerification();
            this.BoxId.Focus();
        }
        private void ManualVerificationCloseClick(object sender, RoutedEventArgs e) => ViewModel.ShowManualVerification = false;
        private void ManualVerificationUpClick(object sender, RoutedEventArgs e) => ViewModel.ManualVerificationMove(true);
        private void ManualVerificationDownClick(object sender, RoutedEventArgs e) => ViewModel.ManualVerificationMove(false);
        private void ManualVerificationKeyDown(object sender, KeyEventArgs e)
        {
            //BoxId 10글자
            if (this.BoxId.Text.Length >= 10)
            {
                if (e.Key == Key.Enter)
                    ViewModel.SearchNonVerifiedBox(this.BoxId.Text);
                else
                    this.BoxId.Text = string.Empty;
            }
        }
        private void BoxId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.BoxId.Text.Length == 10)
                ViewModel.SearchNonVerifiedBox(this.BoxId.Text);
        }
        private void ManualBoxIdConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.SearchNonVerifiedBox(this.BoxId.Text);
        }

        private void ManualVerificationClick(object sender, RoutedEventArgs e)
        {
            if (this.NonVerifiedBoxGird.CurrentCell == null) return;

            if (this.NonVerifiedBoxGird.SelectedCells.Count <= 0) return;

            var cellInfo = this.NonVerifiedBoxGird.SelectedCells[0];

            if (cellInfo.Item is CaseErectData data)
                ViewModel.ManualVerification(data.BoxId, data.BoxType);

            //if (this.NonVerifiedBoxGird.SelectedItem != null)
            //{
            //    var data = (CaseErectData)this.NonVerifiedBoxGird.SelectedItem;
            //    ViewModel.ManualVerification(data.BoxId, data.BoxType);
            //}
        }
        private void ErrorMessageCloseClick(object sender, RoutedEventArgs e) => ViewModel.ShowErrorMessage = false;

        private void MidNightTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.midNightTimer.SetSarpInterval();

            this.InitDataPickier();
        }

        private void CaseErectCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.CaseErectCount.Text.Length >= 10)
            {
                if (e.Key == Key.Enter)
                    ViewModel.Search();
                else
                    this.CaseErectCount.Text = string.Empty;
            }
        }

        private void PasswordBoxCloseClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowPasswordBox = false;
            this.CaseErectCount.Focus();
        }

        private void PasswordBoxConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CheckPassword(Password.Password))
            {
                ViewModel.ShowPasswordBox = false;
                ViewModel.ShowNumberingPopup = true;
            }
            else
            {
                ViewModel.ShowErrorMessageBox("비밀 번호가 일치하지 않습니다.");
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as TabControl).SelectedIndex == 1)
                this.CaseErectCount.Focus();
        }

        private void CaseErectBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CaseErectBoxType.SelectedIndex > 0)
                this.CaseErectCount.Focus();
        }

        private void CaseErectCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.CaseErectCount.Text.Length == 10)
            {
                ViewModel.BoxNumber = this.CaseErectCount.Text;
                ViewModel.Search();
            }
        }
    }
}
