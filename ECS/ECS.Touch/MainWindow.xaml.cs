using ECS.Core.Managers;
using ECS.Touch.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace ECS.Touch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Uri intro = new Uri("/Views/IntroScreen.xaml", UriKind.Relative);
        private Uri invoiceScreen = new Uri("/Views/BcrLcdScreen.xaml", UriKind.Relative);
        private Uri invoiceRejectScreen = new Uri("/Views/InvoiceRejectScreen.xaml", UriKind.Relative);
        private Uri weightInspectorScreen = new Uri("/Views/WeightInspectorScreen.xaml", UriKind.Relative);
        private Uri caseErectScreen = new Uri("/Views/CaseErectScreen.xaml", UriKind.Relative);
        private Uri cvScreen = new Uri("/Views/ConveyorScreen.xaml", UriKind.Relative);
        private Uri smartScreen = new Uri("/Views/SmartPackingScreen.xaml", UriKind.Relative);
        public MainWindow()
        {
            InitializeComponent();
            switch (EcsTouchAppManager.Instance.Setting.TouchType)
            {
                case EcsTouchType.BcrLcd:
                    frame.NavigationService.Navigate(invoiceScreen);
                    break;
                case EcsTouchType.InvoiceReject:
                    frame.NavigationService.Navigate(invoiceRejectScreen);
                    break;
                case EcsTouchType.WeightCheck:
                    frame.NavigationService.Navigate(weightInspectorScreen);
                    break;
                case EcsTouchType.CaseErect:
                    frame.NavigationService.Navigate(caseErectScreen);
                    break;
                case EcsTouchType.Conveyor:
                    frame.NavigationService.Navigate(cvScreen);
                    break;
                case EcsTouchType.SmartPacking:
                    frame.NavigationService.Navigate(smartScreen);
                    break;
                default:
                    frame.NavigationService.Navigate(intro);
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(intro);
        }
    }
}
