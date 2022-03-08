using ECS.Core.Managers;
using ECS.Core.Util;
using ECS.Model.Databases;
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

    public class TrackingSearchViewModel : SearchPageViewModel
    {
        #region field
        private Func<TrackingContentData, bool> m_CancelPred = d => true;
        private Func<TrackingContentData, bool> m_PicingPred = d => true;
        private Func<TrackingContentData, bool> m_WeightPred = d => true;
        private Func<TrackingContentData, bool> m_PrintPred = d => true;
        private Func<TrackingContentData, bool> m_TopPred = d => true;

        private List<TrackingData> m_TrackingLogList = new List<TrackingData>();
        #endregion

        #region property
        private string m_CancelFilter = "전체";
        public string CancelFilter
        {
            get => this.m_CancelFilter;
            set
            {
                this.m_CancelFilter = value;
                this.OnPropertyChanged(nameof(this.CancelFilter));
                switch (value)
                {
                    case "전체":
                        m_CancelPred = d => true;
                        break;
                    case "정상":
                        m_CancelPred = d => d.ORDER_CANCEL_TIME == "";
                        break;
                    case "주문 취소":
                        m_CancelPred = d => d.ORDER_CANCEL_TIME != "";
                        break;
                    case "미출고":
                        m_CancelPred = d => d.ORDER_CANCEL_TIME == "" && d.OUT_TIME == "";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }

        private string m_PickingFilter = "전체";
        public string PickingFilter
        {
            get => this.m_PickingFilter;
            set
            {
                this.m_PickingFilter = value;
                this.OnPropertyChanged(nameof(this.PickingFilter));
                switch (value)
                {
                    case "전체":
                        m_PicingPred = d => true;
                        break;
                    case "피킹 없음":
                        m_PicingPred = d => d.PICKING_TIME == "";
                        break;
                    case "피킹 완료":
                        m_PicingPred = d => d.PICKING_TIME != "";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }

        private string m_WeightFilter = "전체";
        public string WeightFilter
        {
            get => this.m_WeightFilter;
            set
            {
                this.m_WeightFilter = value;
                this.OnPropertyChanged(nameof(this.WeightFilter));
                switch (value)
                {
                    case "전체":
                        m_WeightPred = d => true;
                        break;
                    case "중량 없음":
                        m_WeightPred = d => d.WEIGHT_TIME == "";
                        break;
                    case "중량 실패":
                        m_WeightPred = d => d.WEIGHT_REJECT_TIME != "";
                        break;
                    case "중량 성공":
                        m_WeightPred = d => d.WEIGHT_TIME != "";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }

        private string m_PrintFilter = "전체";
        public string PrintFilter
        {
            get => this.m_PrintFilter;
            set
            {
                this.m_PrintFilter = value;
                this.OnPropertyChanged(nameof(this.PrintFilter));
                switch (value)
                {
                    case "전체":
                        m_PrintPred = d => true;
                        break;
                    case "발행 없음":
                        m_PrintPred = d => d.NORMAL_PRINT_TIME == "" && d.SMART_PRINT_TIME == "";
                        break;
                    case "일반":
                        m_PrintPred = d => d.NORMAL_PRINT_TIME != "";
                        break;
                    case "스마트":
                        m_PrintPred = d => d.SMART_PRINT_TIME != "";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }

        private string m_TopFilter = "전체";
        public string TopFilter
        {
            get => this.m_TopFilter;
            set
            {
                this.m_TopFilter = value;
                this.OnPropertyChanged(nameof(this.TopFilter));
                switch (value)
                {
                    case "전체":
                        m_PrintPred = d => true;
                        break;
                    case "검증 없음":
                        m_PrintPred = d => d.TOP_TIME == "";
                        break;
                    case "검증 실패":
                        m_PrintPred = d => d.TOP_REJECT_TIME != "";
                        break;
                    case "검증 성공":
                        m_PrintPred = d => d.TOP_TIME != "";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }

        private List<TrackingContentData> m_TrackingList = new List<TrackingContentData>();
        private IEnumerable<TrackingContentData> FilteredList
            => this.m_TrackingList.Where(this.m_CancelPred).Where(this.m_PicingPred).Where(this.m_WeightPred).Where(this.m_PrintPred).Where(this.m_TopPred);
        public IEnumerable<TrackingContentData> TrackingList
            => this.GetPagedList(this.FilteredList);
        #endregion

        #region constructor
        public TrackingSearchViewModel() : base()
        {
            this.PageSizeChanged += () =>
            {
                this.SetMaxPage(this.FilteredList.Count());
            };
            this.PageChanged += () =>
            {
                OnPropertyChanged(nameof(this.TrackingList));
            };
        }
        #endregion

        #region method

        #region private
        private void FilterUpdated()
        {
            this.SetMaxPage(this.FilteredList.Count());
            this.OnPropertyChanged(nameof(this.TrackingList));
            if (this.Page > this.MaxPage)
                this.Page = this.MaxPage;
        }
        private void InitSearchList(List<TrackingData> list)
        {
            this.Logger.Write($"InitSearchList : {JsonConvert.SerializeObject(list)}");
            this.m_TrackingLogList.Clear();
            this.m_TrackingLogList = list;
        }
        #endregion

        #region public
        public void Search()
        {
            var list = dbm.SelectTrackingQuery(this.m_Param);
            this.InitSearchList(list);
            this.m_TrackingList = (from tr in this.m_TrackingLogList
                                   group tr by tr.INVOICE_ID into g
                                   select new TrackingContentData(g)).ToList();
            this.SetMaxPage(this.FilteredList.Count());
            this.Page = 1;
        }
        public void Export()
        {
            if (this.m_TrackingList.Count == 0)
            {
                MessageBox.Show("Tracking(트래킹) data does not exist.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = "Tracking_" + DateTime.Now.ToString("yyyyMMdd_HHmm"); // Default file name
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
            this.m_TrackingLogList.Clear();
            this.m_TrackingList.Clear();
            this.SetMaxPage(0);
        }
        #endregion

        #endregion

    }
}
