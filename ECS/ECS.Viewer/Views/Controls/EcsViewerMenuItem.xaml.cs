using ECS.Model.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ECS.Viewer.Views.Controls
{
    [ContentProperty("Items")]
    public partial class EcsViewerMenuItem : UserControl
    {
        #region field
        public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(typeof(EcsViewerMenuItem));
        public static readonly DependencyProperty HeaderProperty = HeaderedItemsControl.HeaderProperty.AddOwner(typeof(EcsViewerMenuItem));
        #endregion

        #region callback
        public event Action<EcsViewerMenuSubItem> SubMenuClicked;
        #endregion

        #region property
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }
        public string Header
        {
            get
            {
                return (string)GetValue(HeaderProperty);
            }
            set
            {
                SetValue(HeaderProperty, value);
            }
        }
        public string Title
        {
            get
            {
                return _Header.Text;
            }
        }
        public ItemCollection Items
        {
            get
            {
                return ListViewMenu.Items;
            }
        }
        #endregion


        #region constructor
        public EcsViewerMenuItem()
        {
            InitializeComponent();
        }
        #endregion

        #region event handler
        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            var textMenu = sender as ListView;
            if (textMenu == null) return;

            var subItem = textMenu.SelectedItem as EcsViewerMenuSubItem;
            if (subItem == null) return;

            this.SubMenuClicked?.Invoke(subItem);
        }
        #endregion
    }
}
