using ECS.Core.Equipments;
using ECS.Core.Managers;
using ECS.Core.Util;
using ECS.Model.Domain.Touch;
using ECS.Model.LabelPrinter;
using ECS.Model.Pcs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECS.Core.ViewModels.Touch
{
    public class InvoiceRejectViewModel : EcsTouchViewModel
    {
        #region field

        private string m_SearchedBoxId = "";

        private PagingList<InvoiceData> m_SameOrderInvoiceList;
        private PagingList<InvoiceReprintData> m_InvoiceReprintList;

        private readonly int m_SameOrderInvoiceWindowSize = 4;
        private readonly int m_InvoiceReprintWindowSize = 8;

        private HostStatusReturnArg printerState;
        private TouchParam m_InvoiceRejectParam =
            new TouchParam() { Begin = DateTime.Now, End = DateTime.Now, InvoiceId = "", CstOrdNo = "", BoxId = "", BcrIndex = -1 };
        #endregion

        #region property
        private LabelPrinterZebraZt411Equipment printer { get; }

        private bool m_PrinterConnection;
        public bool PrinterConnection
        {
            get => this.m_PrinterConnection;
            set
            {
                this.Logger.Write($"PrinterConnection Changed : {value}");
                this.m_PrinterConnection = value;

                if (this.m_PrinterConnection)
                {
                    this.printer.HostStatusReturnSend();
                    this.printer.CancelAllSend();
                }

                this.OnPropertyChanged(nameof(this.PrinterConnection));
            }
        }


        private InvoiceData m_SearchedInvoice = InvoiceData.None;
        public InvoiceData SearchedInvoice
        {
            get => this.m_SearchedInvoice;
            set
            {
                this.Logger.Write($"SearchedInvoice Changed : {value}");
                this.m_SearchedInvoice = value;
                this.OnPropertyChanged(nameof(this.SearchedInvoice));
            }
        }

        private InvoicePrintData m_BcrInfo = InvoicePrintData.None;
        public InvoicePrintData BcrInfo
        {
            get => this.m_BcrInfo;
            set
            {
                this.Logger.Write($"BcrInfo Changed : {value}");
                this.m_BcrInfo = value;
                this.OnPropertyChanged(nameof(this.BcrInfo));
            }
        }

        public IEnumerable<InvoiceData> SameOrderInvoiceList => this.m_SameOrderInvoiceList.GetPagedList("");
        public IEnumerable<InvoiceReprintData> InvoiceReprintList => this.m_InvoiceReprintList.GetPagedList("");


        private bool m_ShowManualVerification;
        public bool ShowManualVerification
        {
            get => this.m_ShowManualVerification;
            set
            {
                this.m_ShowManualVerification = value;
                this.OnPropertyChanged(nameof(this.ShowManualVerification));
            }
        }


        private string m_VerificationBoxId;
        public string VerificationBoxId
        {
            get => this.m_VerificationBoxId;
            set
            {
                this.m_VerificationBoxId = value;
                this.OnPropertyChanged(nameof(this.VerificationBoxId));
            }
        }

        private string m_VerificationInvoiceId;
        public string VerificationInvoiceId
        {
            get => this.m_VerificationInvoiceId;
            set
            {
                this.m_VerificationInvoiceId = value;
                this.OnPropertyChanged(nameof(this.VerificationInvoiceId));
            }
        }


        public InvoiceData m_VerifiedInvoice = InvoiceData.None;
        public InvoiceData VerifiedInvoice
        {
            get => this.m_VerifiedInvoice;
            set
            {
                this.Logger.Write($"VerifiedInvoice Changed : {value}");
                this.m_VerifiedInvoice = value;
                this.OnPropertyChanged(nameof(this.VerifiedInvoice));
            }
        }


        public string BoxId
        {
            get => this.m_InvoiceRejectParam.BoxId;
            set
            {
                this.m_InvoiceRejectParam.BoxId = value;
                this.OnPropertyChanged(nameof(this.BoxId));
            }
        }

        #endregion

        #region constructor
        public InvoiceRejectViewModel() : base()
        {
            this.Logger.Write("InvoiceRejectViewModel Start");
            this.printer = EcsTouchAppManager.Instance.Equipments.GetByEquipmentType<LabelPrinterZebraZt411Equipment>();
            if (printer == null)
            {
                this.ShowErrorMessageBox($"프린터 정보가 없습니다.(Setting 오류)");
            }
            else
                this.PrinterConnection = this.printer.Communicator.TcpConnectionState == Urcis.SmartCode.Net.Tcp.TcpConnectionStateEnum.Connected;

            this.InitPagingLists();
            this.EnrollEventHandler();
        }
        #endregion

        #region method

        #region private
        private void InitPagingLists()
        {
            this.Logger.Write("InvoiceRejectViewModel : InitPagingLists");
            //same order invoice
            this.m_SameOrderInvoiceList = new PagingList<InvoiceData>(() => { });
            this.m_SameOrderInvoiceList.AddPaging("", m_SameOrderInvoiceWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.SameOrderInvoiceList));
            }, false);
            //same order invoice
            this.m_InvoiceReprintList = new PagingList<InvoiceReprintData>(() => { });
            this.m_InvoiceReprintList.AddPaging("", m_InvoiceReprintWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.InvoiceReprintList));
            }, false);
        }
        private void Clear()
        {
            this.Logger.Write("InvoiceRejectViewModel : Clear");
            this.m_SameOrderInvoiceList.Clear();
            this.m_InvoiceReprintList.Clear();

            this.SearchedInvoice = InvoiceData.None;
            this.VerifiedInvoice = InvoiceData.None;
            this.BcrInfo = InvoicePrintData.None;
        }
        private bool AddSameOrders(List<InvoiceData> sameOrders)
        {
            this.Logger.Write("InvoiceRejectViewModel : AddSameOrders");
            if (sameOrders.Count == 0) return false;
            this.m_SameOrderInvoiceList.AddRange(sameOrders);
            this.SearchedInvoice = sameOrders.First(d => d.BoxId == this.m_SearchedBoxId);
            return this.m_SameOrderInvoiceList.InnerList.Count != 0;
        }
        private void AddInvoiceReprints(List<InvoiceReprintData> list)
        {
            this.Logger.Write("InvoiceRejectViewModel : AddInvoiceReprints");
            this.m_InvoiceReprintList.AddRange(list);
        }
        private void SetBcrInfo(List<InvoicePrintData> list)
        {
            this.Logger.Write("InvoiceRejectViewModel : SetBcrInfo");
            if (list.Count == 0)
                return;
            this.BcrInfo = list.First();
        }
        #endregion

        #region public
        public bool Search()
        {
            this.Logger.Write($"InvoiceRejectViewModel : Search({this.BoxId})");
            this.Clear();

            if (string.IsNullOrEmpty(this.BoxId))
                return false;

            this.m_SearchedBoxId = this.BoxId;

            var sameOrders = dbm.SelectInvoicesByOrder(this.m_InvoiceRejectParam);
            if (sameOrders != null)
            {
                if (!this.AddSameOrders(sameOrders))
                {
                    this.ShowErrorMessageBox("일치하는 송장이 없습니다.");
                    return false;
                }
            }

            this.m_InvoiceRejectParam.InvoiceId = this.SearchedInvoice.InvoiceId;
            var invoiceReprints = dbm.SelectInvoiceReprint(this.m_InvoiceRejectParam);
            if (invoiceReprints != null)
                this.AddInvoiceReprints(invoiceReprints);

            var bcrInfo = dbm.SelectBcrInfoById(this.m_InvoiceRejectParam);
            this.SetBcrInfo(bcrInfo);

            return true;
        }
        public bool Reprint()
        {
            this.Logger.Write($"InvoiceRejectViewModel : Reprint)");
            if (!this.PrinterConnection)
            {
                this.ShowErrorMessageBox($"프린터가 연결되지 않았습니다.");
                return false;
            }
            if (SearchedInvoice.Zpl == null)
            {
                this.ShowErrorMessageBox($"발행 가능한 송장이 없습니다.");
                return false;
            }

            bool printRequest = false;
            printRequest = printer.HostStatusReturnSend();
            if (printRequest == false)
            {
                this.ShowErrorMessageBox($"프린터 상태가 정상이 아닙니다.");
                return false;
            }

            Thread.Sleep(100); //프린터가 딜레이가 있어야 제대로 수신받는듯하다.

            if (this.printerState == null
                || this.printerState.PaperOutFlag
                || this.printerState.PauseFlag)
            {
                this.ShowErrorMessageBox($"프린터 상태가 정상이 아닙니다.");
                return false;
            }

             //printRequest = printer.ZplPrintSend(SearchedInvoice.Zpl);
            var dynamicResults = dbm.SelectWieghtSearch(new TouchParam() {Begin = DateTime.Now, End = DateTime.Now, InvoiceId = "", CstOrdNo = "", BoxId = this.BoxId, BcrIndex = -1 });
            List<WeightCheckData> data = new List<WeightCheckData>();

            foreach (WeightCheckData item in dynamicResults)
            {
                if (this.BoxId.Equals(item.BoxId))
                {
                    data.Add(item);
                }
            }

            if (data.Count == 0)
            {
                printRequest = printer.InvoicePrintSend($"{ResultType.NOWEIGHT}");
                this.ShowErrorMessageBox($"중량 검증 무게가 없습니다.");
                return false;
            }
            else
            {
                var latest = data.OrderBy(e => e.CaseErectedAt).Last();

                if (latest.Verification == "정상")
                    printRequest = printer.ZplPrintSend(SearchedInvoice.Zpl);
                else
                {
                    printRequest = printer.InvoicePrintSend($"{ResultType.WEIGHT_FAIL}");
                    this.ShowErrorMessageBox($"중량 검증이 실패되었습니다.");
                    return false;
                }
            }

            if (printRequest == false)
            {
                this.ShowErrorMessageBox($"발행 요청이 정상적으로 되지 않았습니다.");
                return false;
            }

            this.dbm.InsertInvoiceReprint(new TouchParam() { BcrIndex = this.BcrInfo.TopBcrIndex, InvoiceId = this.SearchedInvoice.InvoiceId });
            this.Search();

            return true;
        }
        public bool ManualVerification()
        {
            if (string.IsNullOrEmpty(this.SearchedInvoice.InvoiceId)
                || string.IsNullOrEmpty(this.VerificationInvoiceId))
                return false;

            if (this.SearchedInvoice.InvoiceId == this.VerificationInvoiceId)
            {
                this.server.InvoiceReprintRequest(this.SearchedInvoice.BoxId);
                this.VerificationBoxId = string.Empty;
                this.VerificationInvoiceId = string.Empty;

                this.ShowManualVerification = false;
                return true;
            }
            else
            {
                this.ShowErrorMessageBox($"송장 번호가 일치하지 않습니다.");
                return false;
            }
        }
        public void SameOrderInvoiceMove(bool isUp) => this.m_SameOrderInvoiceList.PageMove("", isUp);
        public void InvoiceReprintMove(bool isUp) => this.m_InvoiceReprintList.PageMove("", isUp);
        public void InvoiceReprintMoveTop() => this.m_InvoiceReprintList.PageMoveTop("");
        public void InvoiceReprintMoveBottom() => this.m_InvoiceReprintList.PageMoveBottom("");
        #endregion

        #region event handler
        private void EnrollEventHandler()
        {
            this.PrinterConnection = printer.Communicator.TcpConnectionState == Urcis.SmartCode.Net.Tcp.TcpConnectionStateEnum.Connected;
            printer.Communicator.TcpConnectionStateChanged += OnPrinterConnectionChanged;
            printer.Communicator.HostStatusReturnRecived += OnCommunicator_HostStatusReturnRecived;
            server.InvoiceReprintResponseReceived += OnInvoiceReprintResponseReceived;
        }

        private void OnPrinterConnectionChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnPrinterConnectionChanged : {e}");
                this.PrinterConnection = e.Current == Urcis.SmartCode.Net.Tcp.TcpConnectionStateEnum.Connected;
            });
        }
        private void OnCommunicator_HostStatusReturnRecived(object sender, HostStatusReturnArg e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnCommunicator_HostStatusReturnRecived : {e}");
                this.printerState = e;
            });
        }
        private void OnInvoiceReprintResponseReceived(InvoiceReprintResponse data)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnInvoiceReprintResponseReceived : {data}");
                this.Search();
                if (data.Result)
                {
                    this.ShowErrorMessageBox($"'{SearchedInvoice.InvoiceId}'의 검증이 성공했습니다.");
                    this.VerifiedInvoice = this.SearchedInvoice;
                    this.BoxId = "";
                }
                else
                {
                    this.ShowErrorMessageBox($"'{SearchedInvoice.InvoiceId}'의 검증이 실패했습니다.", CjRed);
                }
                this.ShowManualVerification = false;
            });
        }

        #endregion

        #endregion
    }
}
