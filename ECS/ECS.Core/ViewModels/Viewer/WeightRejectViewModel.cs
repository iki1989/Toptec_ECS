using ECS.Core.Managers;
using ECS.Core.Util;
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
    public class WeightRejectViewModel : SearchPageViewModel
    {
        #region field
        private Func<WeightRejectData, bool> m_OutPred = d => true;
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
                        m_OutPred = d => d.OUT_TIME == "";
                        break;
                    case "출고":
                        m_OutPred = d => d.OUT_TIME != "";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }
        private List<WeightRejectData> m_WeightRejectSearchList = new List<WeightRejectData>();
        private IEnumerable<WeightRejectData> FilteredList
            => this.m_WeightRejectSearchList.Where(this.m_OutPred);
        public IEnumerable<WeightRejectData> WeightRejectSearchList
            => this.GetPagedList(this.FilteredList);

        #endregion

        #region constructor
        public WeightRejectViewModel() : base()
        {
            this.PageSizeChanged += () =>
            {
                this.SetMaxPage(this.FilteredList.Count());
            };
            this.PageChanged += () =>
            {
                OnPropertyChanged(nameof(this.WeightRejectSearchList));
            };
        }
        #endregion

        #region method

        #region private
        private void FilterUpdated()
        {
            this.SetMaxPage(this.FilteredList.Count());
            this.OnPropertyChanged(nameof(this.WeightRejectSearchList));
            if (this.Page > this.MaxPage)
                this.Page = this.MaxPage;
        }
        private void InitOrderSearchList(List<WeightRejectData> list)
        {
            this.Logger.Write($"InitOrderSearchList : {list.Count}");
            this.m_WeightRejectSearchList = list;

        }
        #endregion

        #region public
        public void Search()
        {
            var list = dbm.SelectWeightCheckRejectQuery(this.m_Param);
            this.InitOrderSearchList(list);
            this.SetMaxPage(this.FilteredList.Count());
            this.Page = 1;
        }
        public void Export()
        {
            if (this.m_WeightRejectSearchList.Count == 0)
            {
                MessageBox.Show("Weight Reject(중량 리젝) data does not exist.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = "Weight_Reject_" + DateTime.Now.ToString("yyyyMMdd_HHmm"); // Default file name
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
            this.m_WeightRejectSearchList.Clear();
            this.SetMaxPage(0);
        }
        #endregion

        #endregion

    }
}
