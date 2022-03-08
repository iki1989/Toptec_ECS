using ECS.Core.Managers;
using ECS.Model.Domain.Touch;
using ECS.Model.Domain.Viewer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ECS.Core.ViewModels.Viewer
{
    public class OutManageViewModel : SearchPageViewModel
    {
        #region field
        private Func<OutBcrData, bool> m_BcrPred = d => true;
        #endregion

        #region property
        private string m_BcrFilter = "전체";
        public string BcrFilter
        {
            get => this.m_BcrFilter;
            set
            {
                this.m_BcrFilter = value;
                this.OnPropertyChanged(nameof(this.BcrFilter));
                switch (value)
                {
                    case "전체":
                        m_BcrPred = d => true;
                        break;
                    case "미식별":
                        m_BcrPred = d => d.BOX_ID == "";
                        break;
                    case "식별":
                        m_BcrPred = d => d.BOX_ID != "";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }

        private List<OutBcrData> m_OutBcrList = new List<OutBcrData>();
        private IEnumerable<OutBcrData> FilteredList
            => this.m_OutBcrList.Where(this.m_BcrPred);
        public IEnumerable<OutBcrData> OutBcrList
            => this.GetPagedList(this.FilteredList);
        #endregion

        #region constructor
        public OutManageViewModel() : base()
        {
            this.PageSizeChanged += () =>
            {
                this.SetMaxPage(this.FilteredList.Count());
            };
            this.PageChanged += () =>
            {
                OnPropertyChanged(nameof(this.OutBcrList));
            };
        }
        #endregion

        #region method

        #region private
        private void FilterUpdated()
        {
            this.SetMaxPage(this.FilteredList.Count());
            this.OnPropertyChanged(nameof(this.OutBcrList));
            if (this.Page > this.MaxPage)
                this.Page = this.MaxPage;
        }
        private void InitSearchList(List<OutBcrData> list)
        {
            this.Logger.Write($"InitSearchList");
            this.m_OutBcrList.Clear();
            this.m_OutBcrList = list;
        }
        #endregion

        #region public
        public void Search()
        {
            var list = dbm.SelectOutManageQuery(this.m_Param);
            this.InitSearchList(list);
            this.SetMaxPage(this.FilteredList.Count());
            this.Page = 1;
        }
        public void Export()
        {
            if (this.m_OutBcrList.Count == 0)
            {
                MessageBox.Show("Out Bcr(2층 출고 BCR) data does not exist.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = "Out_Bcr_" + DateTime.Now.ToString("yyyyMMdd_HHmm"); // Default file name
            dlg.DefaultExt = "csv"; // Default file extension
            dlg.Filter = "CSV files (*.csv)|*.csv"; // Filter files by extension
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                Export(filename, this.FilteredList);
            }
        }
        public override void Clear()
        {
            base.Clear();
            this.m_OutBcrList.Clear();
            this.SetMaxPage(0);
        }
        #endregion

        #endregion

    }
}
