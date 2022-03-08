using ECS.Core.Managers;
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
    public partial class IntroScreen : Page
    {
        private Uri invoiceScreen = new Uri("/Views/BcrLcdScreen.xaml", UriKind.Relative);
        private Uri invoiceRejectScreen = new Uri("/Views/InvoiceRejectScreen.xaml", UriKind.Relative);
        private Uri weightInspectorScreen = new Uri("/Views/WeightInspectorScreen.xaml", UriKind.Relative);
        private Uri caseErectScreen = new Uri("/Views/CaseErectScreen.xaml", UriKind.Relative);
        private Uri cvScreen = new Uri("/Views/ConveyorScreen.xaml", UriKind.Relative);
        public IntroScreen()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(invoiceScreen);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(invoiceRejectScreen);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(weightInspectorScreen);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(caseErectScreen);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(cvScreen);
        }
    }
}
