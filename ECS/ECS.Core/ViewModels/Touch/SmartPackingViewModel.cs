using ECS.Core.Equipments;
using ECS.Core.Managers;
using ECS.Core.Util;
using ECS.Model.Domain.Touch;
using ECS.Model.Pcs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.ViewModels.Touch
{
    public class SmartPackingViewModel : EcsTouchViewModel
    {
        #region field
        private PagingList<SmartPackingData> m_SmartPackingList;
        private readonly int m_SmartWindowSize = 20;
        #endregion

        #region property

        #region connection
        private bool m_BcrConnection;
        public bool BcrConnection
        {
            get => this.m_BcrConnection;
            set
            {
                this.m_BcrConnection = value;
                this.OnPropertyChanged(nameof(this.BcrConnection));
            }
        }

        private bool m_PlcConnection;
        public bool PlcConnection
        {
            get => this.m_PlcConnection;
            set
            {
                this.m_PlcConnection = value;
                this.OnPropertyChanged(nameof(this.PlcConnection));
            }
        }
        #endregion

        private string m_BoxId = "";
        public string BoxId
        {
            get => this.m_BoxId;
            set
            {
                this.m_BoxId = value;
                this.OnPropertyChanged(nameof(this.BoxId));
            }
        }


        private bool m_BeSearching = false;
        private bool BeSearching
        {
            get => m_BeSearching;
            set
            {
                this.m_BeSearching = value;
                this.OnPropertyChanged(nameof(this.SmartPackingList));
            }
        }

        public IEnumerable<SmartPackingData> SmartPackingList =>
            this.m_SmartPackingList.GetPagedList("");


        private SmartPackingData? m_SelectedData;
        public SmartPackingData? SelectedData
        {
            get => this.m_SelectedData;
            set
            {
                this.m_SelectedData = value;
                this.OnPropertyChanged(nameof(this.SelectedData));
            }
        }


        private int m_ManualAmount = 0;
        public int ManualAmount
        {
            get => this.m_ManualAmount;
            set
            {
                if (value < 0)
                    value = 0;
                if (value > 10)
                    value = 10;
                this.m_ManualAmount = value;
                this.OnPropertyChanged(nameof(this.ManualAmount));
            }
        }


        private bool m_ShowManualPopup;
        public bool ShowManualPopup
        {
            get => this.m_ShowManualPopup;
            set
            {
                this.m_ShowManualPopup = value;
                this.OnPropertyChanged(nameof(this.ShowManualPopup));
            }
        }
        #endregion


        #region constructor
        public SmartPackingViewModel() : base()
        {
            this.Logger.Write($"SmartPackingViewModel Start");
            this.InitPagingLists();
            this.EnrollEventHandler();
            this.SelectTodayDatas();
        }
        #endregion

        #region method

        #region private
        private void InitPagingLists()
        {
            this.Logger.Write($"InitPagingLists");
            this.m_SmartPackingList = new PagingList<SmartPackingData>(() =>
            {
                this.OnPropertyChanged(nameof(this.SmartPackingList));
            });
            this.m_SmartPackingList.AddPaging("", m_SmartWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.SmartPackingList));
            });
        }
        private void SelectTodayDatas()
        {
            this.Logger.Write($"SelectTodayDatas");
            this.m_SmartPackingList.Clear();
            var list = dbm.SelectTodaySmartPacking();
            this.m_SmartPackingList.AddRange(list);
        }
        public void BcrAlarmReset(BcrAlarmSetReset bcrAlarmSetReset)
        {
            this.Logger.Write($"BcrAlarmReset : {bcrAlarmSetReset}");
            this.server.BcrAlarmResetRequest(bcrAlarmSetReset.Reason, bcrAlarmSetReset.AlarmResult);
        }
        #endregion

        #region public
        public void SmartMove(bool isUp) => this.m_SmartPackingList.PageMove("", isUp);
        public void SmartMoveTop() => this.m_SmartPackingList.PageMoveTop("");
        public void SmartMoveBottom() => this.m_SmartPackingList.PageMoveBottom("");

        public void SmartSearch()
        {
            if (this.BoxId == "")
            {
                this.BeSearching = false;
                this.SelectTodayDatas();
                return;
            }
            this.BeSearching = true;
            this.m_SmartPackingList.Clear();
            var list = dbm.SelectSmartPackingQuery(this.BoxId);
            this.m_SmartPackingList.AddRange(list);
        }
        public void SmartRefresh()
        {
            this.Logger.Write($"SmartRefresh");
            this.BeSearching = false;
            this.BoxId = "";
            this.SelectTodayDatas();
        }
        public void OpenManualPopup()
        {
            this.Logger.Write($"OpenManualPopup");
            if (this.SelectedData == null)
                return;
            this.ShowManualPopup = true;
        }
        public void ManualProcess()
        {
            this.Logger.Write($"ManualProcess");
            if (this.ServerConnection)
            {
                this.server.SmartPackingManualBoxValidationRequest(new SmartPackingManualBoxValidationRequest() { BoxId = this.SelectedData?.BOX_ID, BufferCount = this.ManualAmount, Result = 1 });
                this.ShowManualPopup = false;
            }
            else
            {
                this.ShowErrorMessageBox("서버와 연결되지 않았습니다.");
            }
        }
        #endregion

        #region event handler
        private void EnrollEventHandler()
        {
            this.server.SmartPackingConnectionStateReceived += this.OnSmartPackingConnectionStateReceived;
            this.server.SmartPackingBcrReadReceived += this.OnSmartPackingBcrReadReceived;
        }
        protected override void OnTimeSyncronizeReceived(TimeSyncronize data)
        {
            base.OnTimeSyncronizeReceived(data);
            this.Dispatcher.Invoke(SelectTodayDatas);
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
        private void OnSmartPackingConnectionStateReceived(SmartPackingConnectionState data)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnSmartPackingConnectionStateReceived : {data}");
                this.BcrConnection = data.SmartPackingBcrConnection;
                this.PlcConnection = data.SmartPackingConnection;
            });
        }
        private void OnSmartPackingBcrReadReceived(SmartPackingBcrRead data)
        {

            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnSmartPackingBcrReadReceived : {data}");
                if (this.BeSearching)
                    return;
                var list = dbm.SelectSmartPackingByIndex(data.SmartPackingIndex);
                if (list.Count > 0)
                {
                    var first = list.First();
                    this.m_SmartPackingList.AddOrUpdate(first, d => d.INDEX == first.INDEX);
                }
            });
        }
        #endregion

        #endregion
    }
}
