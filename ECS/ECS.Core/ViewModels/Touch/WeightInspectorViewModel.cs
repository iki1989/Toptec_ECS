using ECS.Core.Managers;
using ECS.Model.Domain.Touch;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ECS.Model.Pcs;
using ECS.Core.Equipments;
using ECS.Core.Util;
using System;

namespace ECS.Core.ViewModels.Touch
{
    public class WeightInspectorViewModel : EcsTouchViewModel
    {
        #region field
        private PagingList<WeightCheckData> m_WeightList;
        private PagingList<WeightCheckData> m_WeightSearchList;
        private PagingList<WeightCheckData> m_SearchDisplayList;

        private readonly int m_WeightWindowSize = 10;
        private readonly int m_WeightSearchWindowSize = 19;

        private TouchParam m_WeightParam =
            new TouchParam() { Begin = DateTime.Now, End = DateTime.Now, InvoiceId = "", CstOrdNo = "", BoxId = "", BcrIndex = -1 };
        #endregion

        #region property

        private bool m_BcrConnection;
        public bool BcrConnection
        {
            get => this.m_BcrConnection;
            set
            {
                this.Logger.Write($"BcrConnection Changed : {value}");
                this.m_BcrConnection = value;
                this.OnPropertyChanged(nameof(this.BcrConnection));
            }
        }

        #region weight list
        private WeightCheckData m_LastWeightCheckData = WeightCheckData.None;
        public WeightCheckData LastWeightCheckData
        {
            get => this.m_LastWeightCheckData;
            set
            {
                this.Logger.Write($"LastWeightCheckData Changed : {value}");
                this.m_LastWeightCheckData = value;
                this.OnPropertyChanged(nameof(this.LastWeightCheckData));
            }
        }
        public IEnumerable<WeightCheckData> WeightList
            => this.m_WeightList.GetPagedList("");
        #endregion

        #region weight search
        public string BoxId
        {
            get => this.m_WeightParam.BoxId;
            set
            {
                this.m_WeightParam.BoxId = value;
                this.OnPropertyChanged(nameof(this.BoxId));
            }
        }

        public string CstOrdNo
        {
            get => this.m_WeightParam.CstOrdNo;
            set
            {
                this.m_WeightParam.CstOrdNo = value;
                this.OnPropertyChanged(nameof(this.CstOrdNo));
            }
        }


        private bool m_ShowPopup = false;
        public bool ShowPopup
        {
            get => this.m_ShowPopup;
            set
            {
                this.Logger.Write($"ShowPopup Changed : {value}");
                this.m_ShowPopup = value;
                this.OnPropertyChanged(nameof(this.ShowPopup));
            }
        }


        private WeightCheckData m_SelectedBox = WeightCheckData.None;
        public WeightCheckData SelectedBox
        {
            get => this.m_SelectedBox;
            set
            {
                this.Logger.Write($"SelectedBox Changed : {value}");
                this.m_SelectedBox = value;
                this.OnPropertyChanged(nameof(this.SelectedBox));
            }
        }

        public IEnumerable<WeightCheckData> WeightSearchList
            => this.m_WeightSearchList.GetPagedList("");
        public IEnumerable<WeightCheckData> SearchDisplayList
            => this.m_SearchDisplayList.GetPagedList("");
        #endregion

        #endregion


        #region constructor
        public WeightInspectorViewModel() : base()
        {
            this.Logger.Write($"WeightInspectorViewModel Start");
            this.InitPagingLists();
            this.EnrollEventHandler();
            this.SelectTodayData();
        }
        #endregion

        #region method
        #region private
        private void InitPagingLists()
        {
            this.Logger.Write($"InitPagingLists");
            this.m_WeightList = new PagingList<WeightCheckData>(() => { });
            this.m_WeightList.AddPaging("", m_WeightWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.WeightList));
            });
            this.m_WeightSearchList = new PagingList<WeightCheckData>(() => { });
            this.m_WeightSearchList.AddPaging("", m_WeightSearchWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.WeightSearchList));
            }, false);
            this.m_SearchDisplayList = new PagingList<WeightCheckData>(() => { });
            this.m_SearchDisplayList.AddPaging("", 1, () =>
            {
                this.OnPropertyChanged(nameof(this.SearchDisplayList));
            }, false);
        }
        private void SelectTodayData()
        {
            this.Logger.Write($"SelectTodayData");
            var list = dbm.SelectTodayWeight();
            this.m_WeightList.Clear();
            this.AddWeightCheckDatas(list);
        }
        private void AddWeightCheckDatas(List<WeightCheckData> list)
        {
            this.Logger.Write($"AddWeightCheckDatas");
            this.m_WeightList.AddRange(list);
            this.LastWeightCheckData = list.Count() > 0 ? list.Last() : WeightCheckData.None;
        }
        private void AddWeightSearchDatas(List<WeightCheckData> list)
        {
            this.Logger.Write($"AddWeightSearchDatas");
            this.m_WeightSearchList.AddRange(list);
        }
        #endregion

        #region public
        public void Refresh()
        {
            this.Logger.Write($"Refresh");
            this.m_WeightList.Clear();
            this.LastWeightCheckData = WeightCheckData.None;
            var list = dbm.SelectTodayWeight();
            this.AddWeightCheckDatas(list);
        }
        public void WeightMove(bool isUp) => this.m_WeightList.PageMove("", isUp);
        public void WeightMoveTop() => this.m_WeightList.PageMoveTop("");
        public void WeightMoveBottom() => this.m_WeightList.PageMoveBottom("");

        public void Search()
        {
            this.Logger.Write($"Search : param = {this.m_WeightParam}");
            this.m_WeightSearchList.Clear();
            var list = dbm.SelectWieghtSearch(this.m_WeightParam);
            this.AddWeightSearchDatas(list);
            this.ShowPopup = true;
        }

        public void WeightSearchMove(bool isUp) => this.m_WeightSearchList.PageMove("", isUp);
        public void WeightSearchMoveTop() => this.m_WeightSearchList.PageMoveTop("");
        public void WeightSearchMoveBottom() => this.m_WeightSearchList.PageMoveBottom("");

        public void SelectBox(WeightCheckData box)
        {
            this.Logger.Write($"SelectBox : {box}");
            this.m_SearchDisplayList.Clear();
            this.m_SearchDisplayList.Add(box);
            this.SelectedBox = box;
            this.ShowPopup = false;
        }
        public void BcrAlarmReset(BcrAlarmSetReset bcrAlarmSetReset)
        {
            this.Logger.Write($"BcrAlarmReset : {bcrAlarmSetReset}");
            this.server.BcrAlarmResetRequest(bcrAlarmSetReset.Reason, bcrAlarmSetReset.AlarmResult);
        }
        #endregion

        #region event handler 
        protected override void OnTimeSyncronizeReceived(TimeSyncronize data)
        {
            base.OnTimeSyncronizeReceived(data);
            this.Dispatcher.Invoke(() =>
            {
                this.SelectTodayData();
            });
        }
        protected override void OnBcrAlarmSetResetReceived(BcrAlarmSetReset data)
        {
            base.OnBcrAlarmSetResetReceived(data);
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnBcrAlarmSetResetReceived : {data}");
                this.ShowErrorMessageBox($"{data.Reason}");
            });
        }
        private void EnrollEventHandler()
        {
            this.Logger.Write($"EnrollEventHandler");
            server.WeightCheckReceived += OnWeightCheckReceived;
            server.WeightCheckBcrStateReceived += OnWeightCheckBcrStateReceived;
        }
        private void OnWeightCheckReceived(WeightCheck data)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnWeightCheckReceived : {data}");
                var list = dbm.SelectWeightByIndex(new TouchParam() { BcrIndex = data.WeightCheckIndex });
                this.AddWeightCheckDatas(list);
            });
        }
        private void OnWeightCheckBcrStateReceived(WeightCheckBcrState data)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnWeightCheckReceived : {data}");
                this.BcrConnection = data.WeightCheckBcrConnection;
            });
        }
        #endregion

        #endregion
    }
}
