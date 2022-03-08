using ECS.Model.Controls;
using System.Windows.Controls;

namespace ECS.Viewer.Views.Pages
{
    /// <summary>
    /// Interaction logic for MainScreen.xaml
    /// </summary>
    public partial class MainScreen : Page
    {
        public MainScreen()
        {
            InitializeComponent();
        }

        private void Menu_SubMenuClicked(EcsViewerMenuSubItem subItem)
        {
            this.ViewModel.OnSubMenuClicked(subItem);
        }
    }
}
