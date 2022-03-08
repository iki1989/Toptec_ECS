using ECS.Model.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.ViewModels.Viewer
{
    public class MainViewModel : Notifier
    {
        #region property
        private EcsViewerMenuSubItem m_SelectedItem;
        public EcsViewerMenuSubItem SelectedItem
        {
            get => this.m_SelectedItem;
            set
            {
                this.m_SelectedItem = value;
                this.OnPropertyChanged(nameof(this.SelectedItem));
            }
        }
        #endregion

        #region event handler
        public void OnSubMenuClicked(EcsViewerMenuSubItem subItem)
        {
            this.SelectedItem = subItem;
        }
        #endregion
    }
}
