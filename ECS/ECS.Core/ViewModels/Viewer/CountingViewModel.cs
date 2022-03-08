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
using System.Timers;
using System.Windows;

namespace ECS.Core.ViewModels.Viewer
{
    public class CountingViewModel : EcsViewerViewModel
    {
        #region field
        public event Action DataChanged;
        private Dictionary<string, HourlyCountingContentData> m_HourlyCountingContentMap = new Dictionary<string, HourlyCountingContentData>();
        private Dictionary<string, DailyCountingData> m_DailyCountingMap = new Dictionary<string, DailyCountingData>();
        private Timer m_Timer;
        #endregion

        #region property

        #region monitoring
        public (string key, int count)[] OrderData
        {
            get
            {
                var data = new (string key, int count)[5];
                string key;
                for (int i = 0; i < 4; ++i)
                {
                    var date = DateTime.Now.AddDays(i - 4);
                    key = date.ToString("yyyy-MM-dd");
                    data[i] = (date.ToString("MM/dd"), 0);
                    if (m_DailyCountingMap.ContainsKey(key))
                        data[i].count = m_DailyCountingMap[key].DATA.ORDER_COUNT;
                }
                data[4] = (DateTime.Now.ToString("MM/dd"), 0);
                key = DateTime.Now.ToString("yyyy-MM-dd");
                if (m_HourlyCountingContentMap.ContainsKey(key))
                    data[4].count = m_HourlyCountingContentMap[key].ORDER_COUNTS.Sum();
                return data;
            }
        }
        public (string key, int count)[] NonOutData
        {
            get
            {
                var data = new (string key, int count)[5];
                string key;
                for (int i = 0; i < 5; ++i)
                {
                    var date = DateTime.Now.AddDays(i - 5);
                    key = date.ToString("yyyy-MM-dd");
                    data[i] = (date.ToString("MM/dd"), 0);
                    if (m_DailyCountingMap.ContainsKey(key))
                        data[i].count = m_DailyCountingMap[key].NON_OUT_COUNT;
                }
                return data;
            }
        }
        public (double orderData, double outData) TodayOutData
        {
            get
            {
                var data = (0, 0);
                var key = DateTime.Now.ToString("yyyy-MM-dd");
                if (m_HourlyCountingContentMap.ContainsKey(key))
                {
                    data.Item1 = m_HourlyCountingContentMap[key].ORDER_COUNTS.Sum();
                    data.Item2 = m_HourlyCountingContentMap[key].REAL_OUT_COUNTS.Sum();
                }
                return data;
            }
        }
        public (string key, int count)[] TotalOutData
        {
            get
            {
                var data = new (string key, int count)[5];
                string key;
                for (int i = 0; i < 4; ++i)
                {
                    var date = DateTime.Now.AddDays(i - 4);
                    key = date.ToString("yyyy-MM-dd");
                    data[i] = (date.ToString("MM/dd"), 0);
                    if (m_DailyCountingMap.ContainsKey(key))
                        data[i].count = m_DailyCountingMap[key].DATA.REAL_OUT_COUNT;
                }
                data[4] = (DateTime.Now.ToString("MM/dd"), 0);
                key = DateTime.Now.ToString("yyyy-MM-dd");
                if (m_HourlyCountingContentMap.ContainsKey(key))
                    data[4].count = m_HourlyCountingContentMap[key].REAL_OUT_COUNTS.Sum();
                return data;
            }
        }
        public long TotalOutCount
        {
            get
            {
                long sum = 0;
                sum += this.m_DailyCountingMap.Sum(d => (long)d.Value.DATA.REAL_OUT_COUNT);
                var key = DateTime.Now.ToString("yyyy-MM-dd");
                if (this.m_HourlyCountingContentMap.ContainsKey(key))
                    sum += m_HourlyCountingContentMap[key].REAL_OUT_COUNTS.Sum(d => (long)d);
                return sum;
            }
        }
        public int TodayCaseErectRejectCount
        {
            get
            {
                var key = DateTime.Now.ToString("yyyy-MM-dd");
                if (this.m_HourlyCountingContentMap.ContainsKey(key))
                    return m_HourlyCountingContentMap[key].CASE_ERECT_REJECT_COUNTS.Sum();
                return 0;
            }
        }
        public int TodayWeightRejectCount
        {
            get
            {
                var key = DateTime.Now.ToString("yyyy-MM-dd");
                if (this.m_HourlyCountingContentMap.ContainsKey(key))
                    return m_HourlyCountingContentMap[key].WEIGHT_REJECT_COUNTS.Sum();
                return 0;
            }
        }
        public int TodaySmartPackingRejectCount
        {
            get
            {
                var key = DateTime.Now.ToString("yyyy-MM-dd");
                if (this.m_HourlyCountingContentMap.ContainsKey(key))
                    return m_HourlyCountingContentMap[key].PACKING_REJECT_COUNTS.Sum();
                return 0;
            }
        }
        public int TodayTopRejectCount
        {
            get
            {
                var key = DateTime.Now.ToString("yyyy-MM-dd");
                if (this.m_HourlyCountingContentMap.ContainsKey(key))
                    return m_HourlyCountingContentMap[key].TOP_REJECT_COUNTS.Sum();
                return 0;
            }
        }
        public int HourlyOutCount
        {
            get
            {
                var key = DateTime.Now.ToString("yyyy-MM-dd");
                if (this.m_HourlyCountingContentMap.ContainsKey(key))
                    return m_HourlyCountingContentMap[key].REAL_OUT_COUNTS[DateTime.Now.Hour];
                return 0;
            }
        }


        #endregion

        #region out chart

        private string m_ChartType = "제함기";
        public string ChartType
        {
            get => this.m_ChartType;
            set
            {
                this.m_ChartType = value;
                this.OnPropertyChanged(nameof(this.ChartType));
                this.DataChanged?.Invoke();
            }
        }

        private DateTime m_OutChartDate = DateTime.Now;
        public DateTime OutChartDate
        {
            get => this.m_OutChartDate;
            set
            {
                this.m_OutChartDate = value;
                this.OnPropertyChanged(nameof(this.OutChartDate));
                this.DataChanged?.Invoke();
            }
        }

        public int[] OutChartData
        {
            get
            {
                var key = OutChartDate.ToString("yyyy-MM-dd");
                if (!this.m_HourlyCountingContentMap.ContainsKey(key))
                    return new int[24];
                var content = this.m_HourlyCountingContentMap[key];
                switch (ChartType)
                {
                    case "제함기": return content.CASE_ERECT_COUNTS;
                    case "중량검수기": return content.WEIGHT_COUNTS;
                    case "스마트 완충재": return content.PACKING_COUNTS;
                    case "오토라벨러(스마트)": return content.SMART_PRINT_COUNTS;
                    case "오토라벨러(일반)": return content.NORMAL_PRINT_COUNTS;
                    case "상면 검증": return content.TOP_COUNTS;
                    case "2층 출고": return content.REAL_OUT_COUNTS;
                    default: return new int[24];
                }
            }
        }
        #endregion

        #endregion

        #region constructor
        public CountingViewModel() : base()
        {
            var list = dbm.SelectHourlyCounts(DateTime.Now.AddDays(-90), DateTime.Now);
            this.SetHourlyCountingContents(list);
            var daily = dbm.SelectDailyCounts(null, DateTime.Now.AddDays(-1));
            this.SetDailyCounts(daily);
            this.DataChanged?.Invoke();
            this.InitTimer();
        }
        #endregion

        #region method

        #region private
        private void InitTimer()
        {
            this.m_Timer = new Timer(2000);
            this.m_Timer.Elapsed += this.OnTimerEvent;
            this.m_Timer.Start();
        }
        private void SetDailyCounts(IEnumerable<DailyCountingData> list)
        {
            foreach (var data in list)
                this.m_DailyCountingMap[data.DATE] = data;
        }
        private void SetHourlyCountingContents(IEnumerable<HourlyCountingData> list)
        {
            foreach (var data in from d in list
                                 group d by d.DATE into g
                                 select new HourlyCountingContentData(g))
                this.m_HourlyCountingContentMap[data.DATE] = data;
        }
        private void OnTimerEvent(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                var hourly = dbm.SelectHourlyCounts(DateTime.Now.AddDays(-7), DateTime.Now);
                this.SetHourlyCountingContents(hourly);
                var daily = dbm.SelectDailyCounts(DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-1));
                this.SetDailyCounts(daily);
                this.DataChanged?.Invoke();
            });
        }
        #endregion

        #endregion

    }
}
