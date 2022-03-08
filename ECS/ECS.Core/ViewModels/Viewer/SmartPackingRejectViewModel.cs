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
    public class SmartPackingRejectViewModel : SearchPageViewModel
    {
        #region field
        private Func<SmartPackingRejectData, bool> m_OutPred = d => true;
        #endregion

        #region property
        private string m_OutFilter = "전체";
        public string OutFilter
        {
            get => this.m_OutFilter;
            set
            {
                this.m_OutFilter = value;
                this.OnPropertyChanged(nameof(this.OutFilter));
                switch (value)
                {
                    case "전체":
                        m_OutPred = d => true;
                        break;
                    case "미출고":
                        m_OutPred = d =>  d.OUT_TIME == null;
                        break;
                    case "출고":
                        m_OutPred = d =>  d.OUT_TIME != null;
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }
        private List<SmartPackingRejectData> m_SmartPackingRejectSearchList = new List<SmartPackingRejectData>();
        private IEnumerable<SmartPackingRejectData> FilteredList
            => this.m_SmartPackingRejectSearchList.Where(this.m_OutPred);
        public IEnumerable<SmartPackingRejectData> SmartPackingRejectSearchList
            => this.GetPagedList(this.FilteredList);

        #endregion

        #region constructor
        public SmartPackingRejectViewModel() : base()
        {
            this.PageSizeChanged += () =>
            {
                this.SetMaxPage(this.FilteredList.Count());
            };
            this.PageChanged += () =>
            {
                OnPropertyChanged(nameof(this.SmartPackingRejectSearchList));
            };
        }
        #endregion

        #region method

        #region private
        private void FilterUpdated()
        {
            this.SetMaxPage(this.FilteredList.Count());
            this.OnPropertyChanged(nameof(this.SmartPackingRejectSearchList));
            if (this.Page > this.MaxPage)
                this.Page = this.MaxPage;
        }
        private void InitSearchList(List<SmartPackingRejectData> list)
        {
            this.Logger.Write($"InitOrderSearchList : {JsonConvert.SerializeObject(list)}");
            this.m_SmartPackingRejectSearchList.Clear();
            this.m_SmartPackingRejectSearchList = list;

        }
        #endregion

        #region public
        public void Search()
        {
            var list = dbm.SelectSmartPackingRejectQuery(this.m_Param);
            this.InitSearchList(list);
            this.SetMaxPage(this.FilteredList.Count());
            this.Page = 1;
        }
        public void Export()
        {
            if (this.m_SmartPackingRejectSearchList.Count == 0)
            {
                MessageBox.Show("Smart Packing Reject(스마트 완충재 리젝) data does not exist.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = "Smart_Packing_Reject_" + DateTime.Now.ToString("yyyyMMdd_HHmm"); // Default file name
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
            this.m_SmartPackingRejectSearchList.Clear();
            this.SetMaxPage(0);
        }
        #endregion

        #endregion
    }
}
