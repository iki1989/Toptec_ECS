using ECS.Core.Equipments;
using ECS.Core.Managers;
using ECS.Core.Util;
using ECS.Model.Domain.Touch;
using ECS.Model.Pcs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.ViewModels.Touch
{
    public class BcrLcdViewModel : EcsTouchViewModel
    {
        #region field
        private PagingList<BcrReadData> m_SmartRouteBcrList;
        private PagingList<BcrReadData> m_NormalRouteBcrList;
        private PagingList<BcrReadData> m_SmartPrintBcrList;
        private PagingList<BcrReadData> m_NormalPrintBcrList;
        private PagingList<BcrReadData> m_TopBcrList;
        private PagingList<BcrReadData> m_SmartTopBcrList;
        private PagingList<BcrReadData> m_NormalTopBcrList;
        private PagingList<BcrReadData> m_OutBcrList;
        private PagingList<BcrReadData> m_QueriedOutBcrList;

        private readonly int m_SummaryPrintBcrWindowSize = 8;
        private readonly int m_SummaryTopBcrWindowSize = 10;
        private readonly int m_RouteBcrWindowSize = 16;
        private readonly int m_PrintBcrWindowSize = 13;
        private readonly int m_TopBcrWindowSize = 11;
        private readonly int m_OutBcrWindowSize = 8;

        private TouchParam m_OutSearchParam =
            new TouchParam() { Begin = DateTime.Now, End = DateTime.Now, BoxId = "", InvoiceId = "", CstOrdNo = "", BcrIndex = -1 };
        #endregion

        #region property

        #region connection
        private bool m_RouteBcrConnection;
        public bool RouteBcrConnection
        {
            get => this.m_RouteBcrConnection;
            set
            {
                this.m_RouteBcrConnection = value;
                this.OnPropertyChanged(nameof(this.RouteBcrConnection));
            }
        }


        private bool m_SmartPrintBcrConnection;
        public bool SmartPrintBcrConnection
        {
            get => this.m_SmartPrintBcrConnection;
            set
            {
                this.m_SmartPrintBcrConnection = value;
                this.OnPropertyChanged(nameof(this.SmartPrintBcrConnection));
            }
        }

        private bool m_NormalPrintBcrConnection;
        public bool NormalPrintBcrConnection
        {
            get => this.m_NormalPrintBcrConnection;
            set
            {
                this.m_NormalPrintBcrConnection = value;
                this.OnPropertyChanged(nameof(this.NormalPrintBcrConnection));
            }
        }

        private bool m_TopBcrConnection;
        public bool TopBcrConnection
        {
            get => this.m_TopBcrConnection;
            set
            {
                this.m_TopBcrConnection = value;
                this.OnPropertyChanged(nameof(this.TopBcrConnection));
            }
        }

        private bool m_OutBcrConnection;
        public bool OutBcrConnection
        {
            get => this.m_OutBcrConnection;
            set
            {
                this.m_OutBcrConnection = value;
                this.OnPropertyChanged(nameof(this.OutBcrConnection));
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

        private bool m_SmartPrintConnection;
        public bool SmartPrintConnection
        {
            get => this.m_SmartPrintConnection;
            set
            {
                this.m_SmartPrintConnection = value;
                this.OnPropertyChanged(nameof(this.SmartPrintConnection));
            }
        }

        private bool m_SmartPrintLabelerConnection;
        public bool SmartPrintLabelerConnection
        {
            get => this.m_SmartPrintLabelerConnection;
            set
            {
                this.m_SmartPrintLabelerConnection = value;
                this.OnPropertyChanged(nameof(this.SmartPrintLabelerConnection));
            }
        }

        private bool m_NormalPrintLabelerConnection;
        public bool NormalPrintLabelerConnection
        {
            get => this.m_NormalPrintLabelerConnection;
            set
            {
                this.m_NormalPrintLabelerConnection = value;
                this.OnPropertyChanged(nameof(this.NormalPrintLabelerConnection));
            }
        }

        private bool m_SmartPrintPaperEmptyWarning;
        public bool SmartPrintPaperEmptyWarning
        {
            get => this.m_SmartPrintPaperEmptyWarning;
            set
            {
                this.m_SmartPrintPaperEmptyWarning = value;
                this.OnPropertyChanged(nameof(this.SmartPrintPaperEmptyWarning));
            }
        }

        private bool m_NormalPrintPaperEmptyWarning;
        public bool NormalPrintPaperEmptyWarning
        {
            get => this.m_NormalPrintPaperEmptyWarning;
            set
            {
                this.m_NormalPrintPaperEmptyWarning = value;
                this.OnPropertyChanged(nameof(this.NormalPrintPaperEmptyWarning));
            }
        }

        private bool m_SmartPrintNoReadWarning;
        public bool SmartPrintNoReadWarning
        {
            get => this.m_SmartPrintNoReadWarning;
            set
            {
                this.m_SmartPrintNoReadWarning = value;
                this.OnPropertyChanged(nameof(this.SmartPrintNoReadWarning));
            }
        }

        private bool m_NormalPrintNoReadWarning;
        public bool NormalPrintNoReadWarning
        {
            get => this.m_NormalPrintNoReadWarning;
            set
            {
                this.m_NormalPrintNoReadWarning = value;
                this.OnPropertyChanged(nameof(this.NormalPrintNoReadWarning));
            }
        }

        private bool m_SmartPrintEtcWarning;
        public bool SmartPrintEtcWarning
        {
            get => this.m_SmartPrintEtcWarning;
            set
            {
                this.m_SmartPrintEtcWarning = value;
                this.OnPropertyChanged(nameof(this.SmartPrintEtcWarning));
            }
        }

        private bool m_NormalPrintEtcWarning;
        public bool NormalPrintEtcWarning
        {
            get => this.m_NormalPrintEtcWarning;
            set
            {
                this.m_NormalPrintEtcWarning = value;
                this.OnPropertyChanged(nameof(this.NormalPrintEtcWarning));
            }
        }

        private bool m_SmartPrintError;
        public bool SmartPrintError
        {
            get => this.m_SmartPrintError;
            set
            {
                this.m_SmartPrintError = value;
                this.OnPropertyChanged(nameof(this.SmartPrintError));
            }
        }

        private bool m_NormalPrintError;
        public bool NormalPrintError
        {
            get => this.m_NormalPrintError;
            set
            {
                this.m_NormalPrintError = value;
                this.OnPropertyChanged(nameof(this.NormalPrintError));
            }
        }

        private bool m_SmartPrintNoReadError;
        public bool SmartPrintNoReadError
        {
            get => this.m_SmartPrintNoReadError;
            set
            {
                this.m_SmartPrintNoReadError = value;
                this.OnPropertyChanged(nameof(this.SmartPrintNoReadError));
            }
        }

        private bool m_NormalPrintNoReadError;
        public bool NormalPrintNoReadError
        {
            get => this.m_NormalPrintNoReadError;
            set
            {
                this.m_NormalPrintNoReadError = value;
                this.OnPropertyChanged(nameof(this.NormalPrintNoReadError));
            }
        }

        private bool m_SmartPrintEtcError;
        public bool SmartPrintEtcError
        {
            get => this.m_SmartPrintEtcError;
            set
            {
                this.m_SmartPrintEtcError = value;
                this.OnPropertyChanged(nameof(this.SmartPrintEtcError));
            }
        }

        private bool m_NormalPrintEtcError;
        public bool NormalPrintEtcError
        {
            get => this.m_NormalPrintEtcError;
            set
            {
                this.m_NormalPrintEtcError = value;
                this.OnPropertyChanged(nameof(this.NormalPrintEtcError));
            }
        }



        private PrintInfo m_SmartPrintInfo;
        public PrintInfo SmartPrintInfo
        {
            get => this.m_SmartPrintInfo;
            set
            {
                this.m_SmartPrintInfo = value;
                this.OnPropertyChanged(nameof(this.SmartPrintInfo));
            }
        }

        private PrintInfo m_NormalPrintInfo;
        public PrintInfo NormalPrintInfo
        {
            get => this.m_NormalPrintInfo;
            set
            {
                this.m_NormalPrintInfo = value;
                this.OnPropertyChanged(nameof(this.NormalPrintInfo));
            }
        }
        #endregion

        #region smart route
        public int SmartRouteCount => this.m_SmartRouteBcrList.InnerList.Count;
        public IEnumerable<BcrReadData> SmartRouteBcrList => this.m_SmartRouteBcrList.GetPagedList("");
        #endregion

        #region normal route
        public int NormalRouteCount => this.m_NormalRouteBcrList.InnerList.Count;
        public IEnumerable<BcrReadData> NormalRouteBcrList => this.m_NormalRouteBcrList.GetPagedList("");
        #endregion

        public BcrReadData RecentPrint
            => RecentSmartPrint > RecentNormalPrint ? RecentSmartPrint : RecentNormalPrint;

        #region smart print

        private BcrReadData m_RecentSmartPrint = BcrReadData.None;
        public BcrReadData RecentSmartPrint
        {
            get => this.m_RecentSmartPrint;
            set
            {
                this.m_RecentSmartPrint = value;
                this.OnPropertyChanged(nameof(this.RecentSmartPrint));
                this.OnPropertyChanged(nameof(this.RecentPrint));
            }
        }
        public IEnumerable<BcrReadData> SmartSummaryPrintBcrList => this.m_SmartPrintBcrList.GetPagedList("Summary");
        public IEnumerable<BcrReadData> SmartPrintBcrList => this.m_SmartPrintBcrList.GetPagedList("");
        #endregion

        #region normal print
        private BcrReadData m_RecentNormalPrint = BcrReadData.None;
        public BcrReadData RecentNormalPrint
        {
            get => this.m_RecentNormalPrint;
            set
            {
                this.m_RecentNormalPrint = value;
                this.OnPropertyChanged(nameof(this.RecentNormalPrint));
                this.OnPropertyChanged(nameof(this.RecentPrint));
            }
        }
        public IEnumerable<BcrReadData> NormalSummaryPrintBcrList => this.m_NormalPrintBcrList.GetPagedList("Summary");
        public IEnumerable<BcrReadData> NormalPrintBcrList => this.m_NormalPrintBcrList.GetPagedList("");
        #endregion

        #region top

        private BcrReadData m_RecentTop = BcrReadData.None;
        public BcrReadData RecentTop
        {
            get => this.m_RecentTop;
            set
            {
                this.m_RecentTop = value;
                this.OnPropertyChanged(nameof(this.RecentTop));
            }
        }

        public IEnumerable<BcrReadData> TopBcrList => this.m_TopBcrList.GetPagedList("");
        #endregion

        #region smart top
        public IEnumerable<BcrReadData> SmartTopBcrList => this.m_SmartTopBcrList.GetPagedList("");
        #endregion

        #region normal top
        public IEnumerable<BcrReadData> NormalTopBcrList => this.m_NormalTopBcrList.GetPagedList("");
        #endregion

        #region out
        public DateTime OutBcrBegin
        {
            get => this.m_OutSearchParam.Begin;
            set
            {
                if (this.OutBcrEnd < value)
                    value = this.OutBcrEnd;
                this.m_OutSearchParam.Begin = value;
                this.OnPropertyChanged(nameof(this.OutBcrBegin));
            }
        }

        public DateTime OutBcrEnd
        {
            get => this.m_OutSearchParam.End;
            set
            {
                if (value < this.OutBcrBegin)
                    value = this.OutBcrBegin;
                this.m_OutSearchParam.End = value;
                this.OnPropertyChanged(nameof(this.OutBcrEnd));
            }
        }

        public string OutBcrId
        {
            get => this.m_OutSearchParam.BoxId;
            set
            {
                this.m_OutSearchParam.BoxId = value;
                this.OnPropertyChanged(nameof(this.OutBcrId));
            }
        }


        private bool m_BeSearching = false;
        private bool BeSearching
        {
            get => m_BeSearching;
            set
            {
                this.m_BeSearching = value;
                this.OnPropertyChanged(nameof(this.OutBcrList));
            }
        }

        public IEnumerable<BcrReadData> OutBcrList => this.BeSearching ? this.m_QueriedOutBcrList.GetPagedList("") : this.m_OutBcrList.GetPagedList("");
        #endregion

        #endregion


        #region constructor
        public BcrLcdViewModel() : base()
        {
            this.Logger.Write($"BcrLcdViewModel Start");
            this.InitPagingLists();
            this.EnrollEventHandler();
            this.SelectTodayInvoiceBcr();
        }
        #endregion

        #region method

        #region private

        private void InitRouteBcrList()
        {
            //smart route
            this.m_SmartRouteBcrList = new PagingList<BcrReadData>(() =>
            {
                this.OnPropertyChanged(nameof(this.SmartRouteCount));
            });
            this.m_SmartRouteBcrList.AddPaging("", m_RouteBcrWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.SmartRouteBcrList));
            });
            //normal route
            this.m_NormalRouteBcrList = new PagingList<BcrReadData>(() =>
            {
                this.OnPropertyChanged(nameof(this.NormalRouteCount));
            });
            this.m_NormalRouteBcrList.AddPaging("", m_RouteBcrWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.NormalRouteBcrList));
            });
        }
        private void InitPrintBcrList()
        {
            //smart print
            this.m_SmartPrintBcrList = new PagingList<BcrReadData>(() => { });
            this.m_SmartPrintBcrList.AddPaging("", m_PrintBcrWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.SmartPrintBcrList));
            });
            this.m_SmartPrintBcrList.AddPaging("Summary", m_SummaryPrintBcrWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.SmartSummaryPrintBcrList));
            });
            //normal print
            this.m_NormalPrintBcrList = new PagingList<BcrReadData>(() => { });
            this.m_NormalPrintBcrList.AddPaging("", m_PrintBcrWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.NormalPrintBcrList));
            });
            this.m_NormalPrintBcrList.AddPaging("Summary", m_SummaryPrintBcrWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.NormalSummaryPrintBcrList));
            });
        }
        private void InitTopBcrList()
        {
            //top
            this.m_TopBcrList = new PagingList<BcrReadData>(() =>
            {
                this.OnPropertyChanged(nameof(this.TopBcrList));
            });
            this.m_TopBcrList.AddPaging("", m_TopBcrWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.TopBcrList));
            });
            //smart top
            this.m_SmartTopBcrList = new PagingList<BcrReadData>(() =>
            {
                this.OnPropertyChanged(nameof(this.SmartTopBcrList));
            });
            this.m_SmartTopBcrList.AddPaging("", m_SummaryTopBcrWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.SmartTopBcrList));
            });
            //normal top
            this.m_NormalTopBcrList = new PagingList<BcrReadData>(() =>
            {
                this.OnPropertyChanged(nameof(this.NormalTopBcrList));
            });
            this.m_NormalTopBcrList.AddPaging("", m_SummaryTopBcrWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.NormalTopBcrList));
            });
        }
        private void InitOutBcrList()
        {
            //out
            this.m_OutBcrList = new PagingList<BcrReadData>(() =>
            {
                this.OnPropertyChanged(nameof(this.OutBcrList));
            });
            this.m_OutBcrList.AddPaging("", m_OutBcrWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.OutBcrList));
            });
            //queried out
            this.m_QueriedOutBcrList = new PagingList<BcrReadData>(() =>
            {
                this.OnPropertyChanged(nameof(this.OutBcrList));
            });
            this.m_QueriedOutBcrList.AddPaging("", m_OutBcrWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.OutBcrList));
            }, true, true);
        }
        private void InitPagingLists()
        {
            this.Logger.Write($"InitPagingLists");
            this.InitRouteBcrList();
            this.InitPrintBcrList();
            this.InitTopBcrList();
            this.InitOutBcrList();
        }
        private void SelectTodayInvoiceBcr()
        {
            this.Logger.Write($"SelectTodayInvoiceBcr");
            this.Clear();
            var list = dbm.SelectTodayInvoiceBcr();
            this.AddBcrReadDatas(list);

        }
        private void AddBcrReadDatas(List<BcrReadData> list)
        {
            this.Logger.Write($"AddBcrReadData");
            var datas = list.GroupBy(d => d.BcrType).ToDictionary(g => g.Key);

            if (datas.ContainsKey(BcrType.ROUTE))
                this.AddRouteBcrs(datas[BcrType.ROUTE]);

            if (datas.ContainsKey(BcrType.PRINT))
                this.AddPrintBcrs(datas[BcrType.PRINT]);

            if (datas.ContainsKey(BcrType.TOP))
                this.AddTopBcrs(datas[BcrType.TOP]);

            if (datas.ContainsKey(BcrType.OUT))
                this.AddOutBcrs(datas[BcrType.OUT]);
        }
        private void AddRouteBcrs(IEnumerable<BcrReadData> datas)
        {
            this.Logger.Write($"AddRouteBcrs");
            this.m_SmartRouteBcrList.AddRange(datas.Where(d => d.Line == LineType.SMART));
            this.m_NormalRouteBcrList.AddRange(datas.Where(d => d.Line == LineType.NORMAL));
        }
        private void AddPrintBcrs(IEnumerable<BcrReadData> datas)
        {
            this.Logger.Write($"AddSmartPrintBcrs");

            var smartDatas = datas.Where(d => d.Line == LineType.SMART);
            this.m_SmartPrintBcrList.AddRange(smartDatas);
            this.RecentSmartPrint = smartDatas.Count() > 0 ? smartDatas.Last() : BcrReadData.None;

            var normalDatas = datas.Where(d => d.Line == LineType.NORMAL);
            this.m_NormalPrintBcrList.AddRange(normalDatas);
            this.RecentNormalPrint = normalDatas.Count() > 0 ? normalDatas.Last() : BcrReadData.None;
        }
        private void AddTopBcrs(IEnumerable<BcrReadData> datas)
        {
            this.Logger.Write($"AddTopBcrs");
            this.m_TopBcrList.AddRange(datas);
            this.m_SmartTopBcrList.AddRange(datas.Where(d => d.Line == LineType.SMART));
            this.m_NormalTopBcrList.AddRange(datas.Where(d => d.Line == LineType.NORMAL));
            this.RecentTop = datas.Count() > 0 ? datas.Last() : BcrReadData.None;
        }
        private void AddOutBcrs(IEnumerable<BcrReadData> datas)
        {
            this.Logger.Write($"AddOutBcrs");
            this.m_OutBcrList.AddRange(datas);
            if (this.BeSearching)
                this.m_QueriedOutBcrList.AddRange(datas);
        }
        private void AddQueriedOutBcr(List<BcrReadData> list)
        {
            this.Logger.Write($"AddQueriedOutBcr");
            this.m_QueriedOutBcrList.AddRange(list.Select(d =>
            {
                d.Queried = "O";
                return d;
            }));
            this.m_QueriedOutBcrList.Reset();
        }
        public void BcrAlarmReset(BcrAlarmSetReset bcrAlarmSetReset)
        {
            this.Logger.Write($"BcrAlarmReset : {bcrAlarmSetReset}");
            this.server.BcrAlarmResetRequest(bcrAlarmSetReset.Reason, bcrAlarmSetReset.AlarmResult);
        }
        #endregion

        #region public
        public void SmartSummaryPrintBcrMove(bool isUp) => this.m_SmartPrintBcrList.PageMove("Summary", isUp);
        public void SmartSummaryPrintBcrMoveTop() => this.m_SmartPrintBcrList.PageMoveTop("Summary");
        public void SmartSummaryPrintBcrMoveBottom() => this.m_SmartPrintBcrList.PageMoveBottom("Summary");
        public void SmartTopBcrMove(bool isUp) => this.m_SmartTopBcrList.PageMove("", isUp);
        public void SmartTopBcrMoveTop() => this.m_SmartTopBcrList.PageMoveTop("");
        public void SmartTopBcrMoveBottom() => this.m_SmartTopBcrList.PageMoveBottom("");
        public void NormalSummaryPrintBcrMove(bool isUp) => this.m_NormalPrintBcrList.PageMove("Summary", isUp);
        public void NormalSummaryPrintBcrMoveTop() => this.m_NormalPrintBcrList.PageMoveTop("Summary");
        public void NormalSummaryPrintBcrMoveBottom() => this.m_NormalPrintBcrList.PageMoveBottom("Summary");
        public void NormalTopBcrMove(bool isUp) => this.m_NormalTopBcrList.PageMove("", isUp);
        public void NormalTopBcrMoveTop() => this.m_NormalTopBcrList.PageMoveTop("");
        public void NormalTopBcrMoveBottom() => this.m_NormalTopBcrList.PageMoveBottom("");
        public void SmartRouteBcrMove(bool isUp) => this.m_SmartRouteBcrList.PageMove("", isUp);
        public void SmartRouteBcrMoveTop() => this.m_SmartRouteBcrList.PageMoveTop("");
        public void SmartRouteBcrMoveBottom() => this.m_SmartRouteBcrList.PageMoveBottom("");
        public void NormalRouteBcrMove(bool isUp) => this.m_NormalRouteBcrList.PageMove("", isUp);
        public void NormalRouteBcrMoveTop() => this.m_NormalRouteBcrList.PageMoveTop("");
        public void NormalRouteBcrMoveBottom() => this.m_NormalRouteBcrList.PageMoveBottom("");
        public void SmartPrintBcrMove(bool isUp) => this.m_SmartPrintBcrList.PageMove("", isUp);
        public void SmartPrintBcrMoveTop() => this.m_SmartPrintBcrList.PageMoveTop("");
        public void SmartPrintBcrMoveBottom() => this.m_SmartPrintBcrList.PageMoveBottom("");
        public void NormalPrintBcrMove(bool isUp) => this.m_NormalPrintBcrList.PageMove("", isUp);
        public void NormalPrintBcrMoveTop() => this.m_NormalPrintBcrList.PageMoveTop("");
        public void NormalPrintBcrMoveBottom() => this.m_NormalPrintBcrList.PageMoveBottom("");
        public void TopBcrMove(bool isUp) => this.m_TopBcrList.PageMove("", isUp);
        public void TopBcrMoveTop() => this.m_TopBcrList.PageMoveTop("");
        public void TopBcrMoveBottom() => this.m_TopBcrList.PageMoveBottom("");
        public void OutBcrMove(bool isUp)
        {
            if (this.BeSearching)
                this.m_QueriedOutBcrList.PageMove("", isUp);
            else
                this.m_OutBcrList.PageMove("", isUp);
        }
        public void OutBcrMoveTop()
        {
            if (this.BeSearching)
                this.m_QueriedOutBcrList.PageMoveTop("");
            else
                this.m_OutBcrList.PageMoveTop("");
        }
        public void OutBcrMoveBottom()
        {
            if (this.BeSearching)
                this.m_QueriedOutBcrList.PageMoveBottom("");
            else
                this.m_OutBcrList.PageMoveBottom("");
        }

        public void OutBcrSearch()
        {
            this.Logger.Write($"OutBcrSearch : param = {this.m_OutSearchParam}");
            this.BeSearching = true;
            m_QueriedOutBcrList.Clear();
            var list = this.dbm.SelectOutBcrQuery(this.m_OutSearchParam);
            this.AddQueriedOutBcr(list);
        }
        public void OutBcrRefresh()
        {
            this.Logger.Write($"OutBcrRefresh");
            this.BeSearching = false;
            this.m_OutBcrList.Reset();
        }
        public void ScrollReset()
        {
            this.Logger.Write($"ScrollReset");
            this.m_SmartRouteBcrList.Reset();
            this.m_NormalRouteBcrList.Reset();
            this.m_SmartPrintBcrList.Reset();
            this.m_NormalPrintBcrList.Reset();
            this.m_TopBcrList.Reset();
            this.m_SmartTopBcrList.Reset();
            this.m_NormalTopBcrList.Reset();
            this.m_OutBcrList.Reset();
            this.BeSearching = false;
        }
        public void Clear()
        {
            this.Logger.Write($"Clear");
            this.m_SmartRouteBcrList.Clear();
            this.m_NormalRouteBcrList.Clear();
            this.m_SmartPrintBcrList.Clear();
            this.m_NormalPrintBcrList.Clear();
            this.m_TopBcrList.Clear();
            this.m_SmartTopBcrList.Clear();
            this.m_NormalTopBcrList.Clear();
            this.m_OutBcrList.Clear();
            this.BeSearching = false;
        }
        #endregion

        #region event handler
        private void EnrollEventHandler()
        {
            this.server.InvoiceBcrReadReceived += this.OnInvoiceBcrReadReceived;
            this.server.InvoiceBcrStateReceived += this.OnInvoiceBcrStateReceived;
        }
        protected override void OnTimeSyncronizeReceived(TimeSyncronize data)
        {
            base.OnTimeSyncronizeReceived(data);
            this.Dispatcher.Invoke(SelectTodayInvoiceBcr);
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
        private void OnInvoiceBcrReadReceived(InvoiceBcrRead data)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnInvoiceBcrReadReceived : {data}");
                var list = dbm.SelectInvoiceBcrByIndex(new TouchParam() { BcrIndex = data.InvoiceBcrIndex });
                this.AddBcrReadDatas(list);
            });
        }
        private void OnInvoiceBcrStateReceived(InvoiceBcrState data)
        {

            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnInvoiceBcrStateReceived : {data}");
                this.RouteBcrConnection = data.RouteBcrConnection;
                this.SmartPrintBcrConnection = data.PrintInfoList[0].PrintBcrConnection;
                this.NormalPrintBcrConnection = data.PrintInfoList[1].PrintBcrConnection;
                this.TopBcrConnection = data.TopBcrConnection;
                this.OutBcrConnection = data.OutBcrConnection;
                this.PlcConnection = data.PlcConnection;
                this.SmartPrintLabelerConnection = data.PrintInfoList[0].LabellerConnection;
                this.NormalPrintLabelerConnection = data.PrintInfoList[1].LabellerConnection;
                this.SmartPrintPaperEmptyWarning = data.PrintInfoList[0].LabelerState.HasFlag(LabelerStateFlag.PaperEmptyWarning);
                this.NormalPrintPaperEmptyWarning = data.PrintInfoList[1].LabelerState.HasFlag(LabelerStateFlag.PaperEmptyWarning);
                this.SmartPrintEtcWarning = data.PrintInfoList[0].LabelerState.HasFlag(LabelerStateFlag.EtcWarning);
                this.NormalPrintEtcWarning = data.PrintInfoList[1].LabelerState.HasFlag(LabelerStateFlag.EtcWarning);
                this.SmartPrintError = data.PrintInfoList[0].LabelerState.HasFlag(LabelerStateFlag.PrintError);
                this.NormalPrintError = data.PrintInfoList[1].LabelerState.HasFlag(LabelerStateFlag.PrintError);
                this.SmartPrintEtcError = data.PrintInfoList[0].LabelerState.HasFlag(LabelerStateFlag.EtcError);
                this.NormalPrintEtcError = data.PrintInfoList[1].LabelerState.HasFlag(LabelerStateFlag.EtcError);
            });
        }
        #endregion

        #endregion
    }
}
