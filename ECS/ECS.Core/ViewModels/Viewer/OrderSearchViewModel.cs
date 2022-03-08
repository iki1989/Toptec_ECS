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
    public class OrderSearchViewModel : SearchPageViewModel
    {
        #region field
        private Func<OrderSearchData, bool> m_OutPred = d => true;
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
                    case "출고":
                        m_OutPred = d => d.OUT_TIME != "";
                        break;
                    case "미출고":
                        m_OutPred = d => d.OUT_TIME == "" && d.ORDER_CANCEL == "";
                        break;
                    case "주문취소":
                        m_OutPred = d => d.ORDER_CANCEL == "O";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }

        private List<OrderSearchData> m_OrderSearchList = new List<OrderSearchData>();
        private IEnumerable<OrderSearchData> FilteredList
            => this.m_OrderSearchList.Where(this.m_OutPred);
        public IEnumerable<OrderSearchData> OrderSearchList
            => this.GetPagedList(this.FilteredList);
        public int DataCount =>
            this.m_OrderSearchList.Count;
        public int OutCount =>
            this.m_OrderSearchList.Where(d => d.OUT_TIME != "").Count();
        public int NonOutCount =>
            this.m_OrderSearchList.Where(d => d.OUT_TIME == "" && d.ORDER_CANCEL == "").Count();
        public int CanelCount =>
            this.m_OrderSearchList.Where(d => d.ORDER_CANCEL == "O").Count();

        #endregion

        #region constructor
        public OrderSearchViewModel() : base()
        {
            this.PageSizeChanged += () =>
            {
                this.SetMaxPage(this.FilteredList.Count());
            };
            this.PageChanged += () =>
            {
                OnPropertyChanged(nameof(this.OrderSearchList));
            };
        }
        #endregion

        #region method

        #region private
        private void FilterUpdated()
        {
            this.SetMaxPage(this.FilteredList.Count());
            this.OnPropertyChanged(nameof(this.OrderSearchList));
            if (this.Page > this.MaxPage)
                this.Page = this.MaxPage;
        }
        private void InitOrderSearchList(List<OrderSearchData> list)
        {
            this.Logger.Write($"InitOrderSearchList");
            this.m_OrderSearchList.Clear();
            this.m_OrderSearchList = list;
        }
        private void CountChanged()
        {
            this.OnPropertyChanged(nameof(this.DataCount));
            this.OnPropertyChanged(nameof(this.OutCount));
            this.OnPropertyChanged(nameof(this.NonOutCount));
            this.OnPropertyChanged(nameof(this.CanelCount));

        }
        #endregion

        #region public
        public void Search()
        {
            var list = dbm.SelectOrderQuery(this.m_Param);
            this.InitOrderSearchList(list);
            this.SetMaxPage(this.FilteredList.Count());
            this.Page = 1;
            this.CountChanged();
        }
        public void Export()
        {
            if (this.m_OrderSearchList.Count == 0)
            {
                MessageBox.Show("Order(주문정보) data does not exist.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = "Order_" + DateTime.Now.ToString("yyyyMMdd_HHmm"); // Default file name
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
            this.m_OrderSearchList.Clear();
            this.SetMaxPage(0);
            this.CountChanged();
        }
        #endregion

        #endregion

    }
}
