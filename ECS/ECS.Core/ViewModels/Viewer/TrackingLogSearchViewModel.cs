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

    public class TrackingLogSearchViewModel : SearchPageViewModel
    {
        #region field
        private Func<TrackingData, bool> m_RestPred = d => true;
        private Func<TrackingData, bool> m_WeightPred = d => false;
        private Func<TrackingData, bool> m_BcrPred = d => false;
        private Func<TrackingData, bool> m_PackingPred = d => false;
        private Func<TrackingData, bool> m_TopPred = d => false;
        private List<TrackingData> m_TrackingList = new List<TrackingData>();
        #endregion

        #region property
        private string m_RestFilter = "전체";
        public string RestFilter
        {
            get => this.m_RestFilter;
            set
            {
                this.m_RestFilter = value;
                this.OnPropertyChanged(nameof(this.RestFilter));
                switch (value)
                {
                    case "전체":
                        this.m_RestPred = d => true;
                        break;
                    case "X":
                        this.m_RestPred = d => false;
                        break;
                    case "주문 수신":
                        this.m_RestPred = d => d.TASK_TYPE == "주문 수신";
                        break;
                    case "피킹 완료":
                        this.m_RestPred = d => d.TASK_TYPE == "피킹 완료";
                        break;
                    case "WCS 피킹 완료":
                        this.m_RestPred = d => d.TASK_TYPE == "WCS 피킹 완료";
                        break;
                    case "주문 취소":
                        this.m_RestPred = d => d.TASK_TYPE == "주문 취소";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }

        private string m_WeightFilter = "X";
        public string WeightFilter
        {
            get => this.m_WeightFilter;
            set
            {
                this.m_WeightFilter = value;
                this.OnPropertyChanged(nameof(this.WeightFilter));
                switch (value)
                {
                    case "X":
                        this.m_WeightPred = d => false;
                        break;
                    case "중량 검수 실패":
                        this.m_WeightPred = d => d.TASK_TYPE == "중량 검수 실패";
                        break;
                    case "중량 수기 검수":
                        this.m_WeightPred = d => d.TASK_TYPE == "중량 수기 검수";
                        break;
                    case "중량 검수 성공":
                        this.m_WeightPred = d => d.TASK_TYPE == "중량 검수 성공";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }

        private string m_BcrFilter = "X";
        public string BcrFilter
        {
            get => this.m_BcrFilter;
            set
            {
                this.m_BcrFilter = value;
                this.OnPropertyChanged(nameof(this.BcrFilter));
                switch (value)
                {
                    case "X":
                        this.m_BcrPred = d => false;
                        break;
                    case "분기 BCR 통과":
                        this.m_BcrPred = d => d.TASK_TYPE == "분기 BCR 통과";
                        break;
                    case "스마트 송장 발행":
                        this.m_BcrPred = d => d.TASK_TYPE == "스마트 송장 발행";
                        break;
                    case "일반 송장 발행":
                        this.m_BcrPred = d => d.TASK_TYPE == "일반 송장 발행";
                        break;
                    case "2층 출고 완료":
                        this.m_BcrPred = d => d.TASK_TYPE == "2층 출고 완료";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }

        private string m_PackingFilter = "X";
        public string PackingFilter
        {
            get => this.m_PackingFilter;
            set
            {
                this.m_PackingFilter = value;
                this.OnPropertyChanged(nameof(this.PackingFilter));
                switch (value)
                {
                    case "X":
                        this.m_PackingPred = d => false;
                        break;
                    case "충진 실패":
                        this.m_PackingPred = d => d.TASK_TYPE == "충진 실패";
                        break;
                    case "충진 수동 처리":
                        this.m_PackingPred = d => d.TASK_TYPE == "충진 수동 처리";
                        break;
                    case "충진 통과":
                        this.m_PackingPred = d => d.TASK_TYPE == "충진 통과";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }

        private string m_TopFilter = "X";
        public string TopFilter
        {
            get => this.m_TopFilter;
            set
            {
                this.m_TopFilter = value;
                this.OnPropertyChanged(nameof(this.TopFilter));
                switch (value)
                {
                    case "X":
                        this.m_TopPred = d => false;
                        break;
                    case "상면 검증 실패":
                        this.m_TopPred = d => d.TASK_TYPE == "상면 검증 실패";
                        break;
                    case "상면 검증 성공":
                        this.m_TopPred = d => d.TASK_TYPE == "상면 검증 성공";
                        break;
                    case "상면 수동 검증":
                        this.m_TopPred = d => d.TASK_TYPE == "상면 수동 검증";
                        break;
                    default: break;
                }
                this.FilterUpdated();
            }
        }
        private IEnumerable<TrackingData> FilteredList
            => this.m_TrackingList.Where(d => this.m_RestPred(d) || this.m_WeightPred(d) || this.m_BcrPred(d) || this.m_PackingPred(d) || this.m_TopPred(d));
        public IEnumerable<TrackingData> TrackingList
            => this.GetPagedList(this.FilteredList);
        #endregion

        #region constructor
        public TrackingLogSearchViewModel() : base()
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
            this.m_TrackingList.Clear();
            this.m_TrackingList = list;
        }
        #endregion

        #region public
        public void Search()
        {
            var list = dbm.SelectTrackingQuery(this.m_Param);
            this.InitSearchList(list);
            this.SetMaxPage(this.FilteredList.Count());
            this.Page = 1;
        }
        public void Export()
        {
            if (this.m_TrackingList.Count == 0)
            {
                MessageBox.Show("Tracking Log(트래킹 이력) data does not exist.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = "Tracking_Log_" + DateTime.Now.ToString("yyyyMMdd_HHmm"); // Default file name
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
            this.m_TrackingList.Clear();
            this.SetMaxPage(0);
        }
        #endregion

        #endregion

    }
}
