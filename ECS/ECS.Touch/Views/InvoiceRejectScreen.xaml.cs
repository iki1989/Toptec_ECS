using ECS.Core.ViewModels.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    public partial class InvoiceRejectScreen : Page
    {
        public InvoiceRejectScreen()
        {
            InitializeComponent();
            this.BoxIdTextBox.Focus();
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Search())
                this.ReprintClick(this, null);
            else
                this.BoxIdTextBox.Focus();
        }
        private void ReprintClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Reprint())
            {
                ViewModel.ShowManualVerification = true;
                this.ManualVerificationBoxId.Focus();
            }
            else
                this.BoxIdTextBox.Focus();
        }
        private void InvoiceReprintUpClick(object sender, RoutedEventArgs e) => ViewModel.InvoiceReprintMove(true);
        private void InvoiceReprintDownClick(object sender, RoutedEventArgs e) => ViewModel.InvoiceReprintMove(false);
        private void InvoiceReprintTopClick(object sender, RoutedEventArgs e) => ViewModel.InvoiceReprintMoveTop();
        private void InvoiceReprintBottomClick(object sender, RoutedEventArgs e) => ViewModel.InvoiceReprintMoveBottom();
        private void SameOrderInvoiceUpClick(object sender, RoutedEventArgs e) => ViewModel.SameOrderInvoiceMove(true);
        private void SameOrderInvoiceDownClick(object sender, RoutedEventArgs e) => ViewModel.SameOrderInvoiceMove(false);

        private void ErrorMessageCloseClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowErrorMessage = false;

            if (ViewModel.ShowManualVerification)
                this.ManualVerificationBoxId.Focus();
            else
                this.BoxIdTextBox.Focus();

            //if (this.ManualVerificationBoxId.Visibility == Visibility.Visible)
            //    this.ManualVerificationBoxId.Focus();
            //else
            //    this.BoxIdTextBox.Focus();
        }

        private void BoxId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.Search();
                //if (ViewModel.Search())
                //    ViewModel.Reprint();
            } 
            else
            {
                if (this.BoxIdTextBox.Text.Length >= 10)
                    this.BoxIdTextBox.Text = string.Empty;
            }
        }

        private void BoxIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //BoxId 10글자
            if (this.BoxIdTextBox.Text.Length == 10)
            {
                ViewModel.BoxId = this.BoxIdTextBox.Text;
                this.SearchClick(this, null);
            }

            if (ViewModel.ShowErrorMessage && this.BoxIdTextBox.Text.Length > 0)
                ViewModel.ShowErrorMessage = false;
        }

        #region 미사용
        private void ManualVerificationKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ViewModel.ManualVerification())
                    this.BoxIdTextBox.Focus();
                else
                    this.ManualVerificationBoxId.Focus();
            }
        }

        private void ManualVerificationClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.ManualVerification())
                this.BoxIdTextBox.Focus();
            else
                this.ManualVerificationInvoiceId.Focus();
        }
        private void ManualVerificationCloseClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowManualVerification = false;

            this.ManualVerificationBoxId.Text = string.Empty;
            this.ManualVerificationInvoiceId.Text = string.Empty;

            this.BoxIdTextBox.Focus();
        }

        private void ManualVerificationBoxId_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.ManualVerificationBoxId.Visibility == Visibility.Visible)
                this.ManualVerificationBoxId.Focus();
        }

        private void ManualVerificationBoxId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.ManualVerificationInvoiceId.Focus();
        }

        private void ManualVerificationBoxId_TextChanged(object sender, TextChangedEventArgs e)
        {
            //BoxId 10글자
            if (this.ManualVerificationBoxId.Text.Length == 10)
                this.ManualVerificationInvoiceId.Focus();
        }

        private void ManualVerificationInvoiceId_TextChanged(object sender, TextChangedEventArgs e)
        {
            //invoiceId 12글자
            if (this.ManualVerificationInvoiceId.Text.Length == 12)
            {
                ViewModel.VerificationInvoiceId = this.ManualVerificationInvoiceId.Text;
                if (ViewModel.ManualVerification())
                    this.BoxIdTextBox.Focus();
                else
                    this.ManualVerificationInvoiceId.Focus();
            }
        }

        private void ManualClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowManualVerification = true;
        }
        #endregion

        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ViewModel.ShowErrorMessage)
            {
                if (ViewModel.ShowManualVerification)
                    this.ManualVerificationBoxId.Focus();
                else
                    this.BoxIdTextBox.Focus();
            }
        }
    }
}
