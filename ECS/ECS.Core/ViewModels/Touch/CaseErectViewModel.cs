using ECS.Core.Managers;
using ECS.Model.Domain.Touch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using ECS.Core.Equipments;
using ECS.Model.Pcs;
using ECS.Core.Util;
using ECS.Model.LabelPrinter;
using System.Threading;

namespace ECS.Core.ViewModels.Touch
{
    public class CaseErectViewModel : EcsTouchViewModel
    {
        #region field
        private PagingList<CaseErectData> m_CaseErectList1;
        private PagingList<CaseErectData> m_CaseErectList2;

        private PagingList<BoxInfoData> m_BoxInfoList;
        private Dictionary<string, string> m_BoxNameMap = new Dictionary<string, string>();

        private PagingList<CaseErectData> m_CaseErectSearchList;
        private PagingList<CaseErectData> m_CaseErectRejectSearchList;
        private PagingList<CaseErectData> m_NonVerifiedBoxList;

        private int m_CaseErectWindowSize = 13;
        private int m_CaseErectSearchWindowSize = 10;
        private int m_BoxInfoWindowSize = 20;
        private int m_NonVerifiedBoxWindowSize = 5;

        private HostStatusReturnArg printerState;

        private TouchParam m_CaseErectParam =
            new TouchParam() { Begin = DateTime.Now, End = DateTime.Now, InvoiceId = "", CstOrdNo = "", BoxId = "", BcrIndex = -1 };
        #endregion

        #region property
        private LabelPrinterZebraZt411Equipment printer { get; }

        public IEnumerable<string> BoxTypeList => this.m_BoxInfoList.InnerList.Select((data) => data.BoxTypeCd);

        #region connection
        private bool m_Erector1Connection;
        public bool Erector1Connection
        {
            get => this.m_Erector1Connection;
            set
            {
                this.Logger.Write($"Erector1Connection Changed : {value}");
                this.m_Erector1Connection = value;
                this.OnPropertyChanged(nameof(this.Erector1Connection));
            }
        }

        private bool m_Erector2Connection = false;
        public bool Erector2Connection
        {
            get => this.m_Erector2Connection;
            set
            {
                this.Logger.Write($"Erector2Connection Changed : {value}");
                this.m_Erector2Connection = value;
                this.OnPropertyChanged(nameof(this.Erector2Connection));
            }
        }

        private bool m_Inkjet1Connection = false;
        public bool Inkjet1Connection
        {
            get => this.m_Inkjet1Connection;
            set
            {
                this.Logger.Write($"Inkjet1Connection Changed : {value}");
                this.m_Inkjet1Connection = value;
                this.OnPropertyChanged(nameof(this.Inkjet1Connection));
            }
        }

        private bool m_Inkjet2Connection = false;
        public bool Inkjet2Connection
        {
            get => this.m_Inkjet2Connection;
            set
            {
                this.Logger.Write($"Inkjet2Connection Changed : {value}");
                this.m_Inkjet2Connection = value;
                this.OnPropertyChanged(nameof(this.Inkjet2Connection));
            }
        }

        private bool m_Bcr1Connection = false;
        public bool Bcr1Connection
        {
            get => this.m_Bcr1Connection;
            set
            {
                this.Logger.Write($"Bcr1Connection Changed : {value}");
                this.m_Bcr1Connection = value;
                this.OnPropertyChanged(nameof(this.Bcr1Connection));
            }
        }

        private bool m_Bcr2Connection = false;
        public bool Bcr2Connection
        {
            get => this.m_Bcr2Connection;
            set
            {
                this.Logger.Write($"Bcr2Connection Changed : {value}");
                this.m_Bcr2Connection = value;
                this.OnPropertyChanged(nameof(this.Bcr2Connection));
            }
        }

        private bool m_PrinterConnection = false;
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
        #endregion

        #region case erector 1 
        private CaseErectData m_RecentErectData1;
        public CaseErectData RecentErectData1
        {
            get => this.m_RecentErectData1;
            set
            {
                this.Logger.Write($"RecentErectData1 Changed : {value}");
                this.m_RecentErectData1 = value;
                this.OnPropertyChanged(nameof(this.RecentErectData1));
            }
        }
        public IEnumerable<CaseErectData> CaseErectList1 => this.m_CaseErectList1.GetPagedList("");

        private int m_InkRatio1 = 100;
        public int InkRatio1
        {
            get => this.m_InkRatio1;
            set
            {
                this.Logger.Write($"InkRatio1 Changed : {value}");
                this.m_InkRatio1 = value;
                this.OnPropertyChanged(nameof(this.InkRatio1));
            }
        }


        private bool m_IgnoreInkLeak1 = false;
        public bool IgnoreInkLeak1
        {
            get => this.m_IgnoreInkLeak1;
            set
            {
                this.Logger.Write($"IgnoreInkLeak1 Changed : {value}");
                this.m_IgnoreInkLeak1 = value;
                this.OnPropertyChanged(nameof(this.IgnoreInkLeak1));
            }
        }
        #endregion

        #region case erector 2
        private CaseErectData m_RecentErectData2;
        public CaseErectData RecentErectData2
        {
            get => this.m_RecentErectData2;
            set
            {
                this.Logger.Write($"RecentErectData2 Changed : {value}");
                this.m_RecentErectData2 = value;
                this.OnPropertyChanged(nameof(this.RecentErectData2));
            }
        }
        public IEnumerable<CaseErectData> CaseErectList2 => this.m_CaseErectList2.GetPagedList("");

        private int m_InkRatio2 = 100;
        public int InkRatio2
        {
            get => this.m_InkRatio2;
            set
            {
                this.Logger.Write($"InkRatio2 Changed : {value}");
                this.m_InkRatio2 = value;
                this.OnPropertyChanged(nameof(this.InkRatio2));
            }
        }

        private bool m_IgnoreInkLeak2 = false;
        public bool IgnoreInkLeak2
        {
            get => this.m_IgnoreInkLeak2;
            set
            {
                this.Logger.Write($"IgnoreInkLeak2 Changed : {value}");
                this.m_IgnoreInkLeak2 = value;
                this.OnPropertyChanged(nameof(this.IgnoreInkLeak2));
            }
        }

        #endregion

        #region password

        private bool m_ShowPasswordBox;
        public bool ShowPasswordBox
        {
            get => this.m_ShowPasswordBox;
            set
            {
                this.m_ShowPasswordBox = value;
                this.OnPropertyChanged(nameof(this.ShowPasswordBox));
            }
        }
        #endregion

        #region numbering popup
        private bool m_ShowNumberingPopup = false;
        public bool ShowNumberingPopup
        {
            get => this.m_ShowNumberingPopup;
            set
            {
                this.Logger.Write($"ShowNumberingPopup Changed : {value}");
                this.NumberingPopupRefresh();
                this.m_ShowNumberingPopup = value;
                this.OnPropertyChanged(nameof(this.ShowNumberingPopup));
            }
        }
        public IEnumerable<BoxInfoData> BoxInfoList => this.m_BoxInfoList.GetPagedList("");

        private BoxInfoData m_SelectedBoxInfo = BoxInfoData.None;
        public BoxInfoData SelectedBoxInfo
        {
            get => this.m_SelectedBoxInfo;
            set
            {
                this.Logger.Write($"SelectedBoxInfo Changed : {value}");
                this.m_SelectedBoxInfo = value;
                this.OnPropertyChanged(nameof(this.SelectedBoxInfo));
            }
        }
        #endregion

        #region message box
        private bool m_ShowNumberingMessageBox;
        public bool ShowNumberingMessageBox
        {
            get => this.m_ShowNumberingMessageBox;
            set
            {
                this.Logger.Write($"ShowNumberingMessageBox Changed : {value}");
                this.m_ShowNumberingMessageBox = value;
                this.OnPropertyChanged(nameof(this.ShowNumberingMessageBox));
            }
        }

        private bool m_IsInsert;
        public bool IsInsert
        {
            get => this.m_IsInsert;
            set
            {
                this.Logger.Write($"IsInsert Changed : {value}");
                this.m_IsInsert = value;
                this.OnPropertyChanged(nameof(this.IsInsert));
            }
        }

        #region insert or update target
        private BoxInfoData m_TargetBoxInfo;
        public BoxInfoData TargetBoxInfo
        {
            get => this.m_TargetBoxInfo;
            set
            {
                this.Logger.Write($"TargetBoxInfo Changed : {value}");
                this.m_TargetBoxInfo = value;
                this.OnPropertyChanged(nameof(this.TargetName));
                this.OnPropertyChanged(nameof(this.TargetWeight));
                this.OnPropertyChanged(nameof(this.TargetType));
                this.OnPropertyChanged(nameof(this.TargetLength));
                this.OnPropertyChanged(nameof(this.TargetWidth));
                this.OnPropertyChanged(nameof(this.TargetHeight));
                this.OnPropertyChanged(nameof(this.TargetHeightSensor));
                this.OnPropertyChanged(nameof(this.TargetNormalFrom));
                this.OnPropertyChanged(nameof(this.TargetNormalTo));
                this.OnPropertyChanged(nameof(this.TargetManualFrom));
                this.OnPropertyChanged(nameof(this.TargetManualTo));
                this.OnPropertyChanged(nameof(this.TargetBoxInfo));
            }
        }
        public string TargetName
        {
            get => this.m_TargetBoxInfo.Name;
            set
            {
                this.m_TargetBoxInfo.Name = value;
                this.OnPropertyChanged(nameof(this.TargetName));
            }
        }
        public double TargetWeight
        {
            get => this.m_TargetBoxInfo.Weight;
            set
            {
                this.m_TargetBoxInfo.Weight = value;
                this.OnPropertyChanged(nameof(this.TargetWeight));
            }
        }
        public string TargetType
        {
            get => this.m_TargetBoxInfo.BoxTypeCd;
            set
            {
                this.m_TargetBoxInfo.BoxTypeCd = value;
                this.OnPropertyChanged(nameof(this.TargetType));
            }
        }
        public double TargetLength
        {
            get => this.m_TargetBoxInfo.Length;
            set
            {
                this.m_TargetBoxInfo.Length = value;
                this.OnPropertyChanged(nameof(this.TargetLength));
            }
        }
        public double TargetWidth
        {
            get => this.m_TargetBoxInfo.Width;
            set
            {
                this.m_TargetBoxInfo.Width = value;
                this.OnPropertyChanged(nameof(this.TargetWidth));
            }
        }
        public double TargetHeight
        {
            get => this.m_TargetBoxInfo.Height;
            set
            {
                this.m_TargetBoxInfo.Height = value;
                this.OnPropertyChanged(nameof(this.TargetHeight));
            }
        }

        public int TargetHeightSensor
        {
            get => this.m_TargetBoxInfo.HeightSensor;
            set
            {
                this.m_TargetBoxInfo.HeightSensor = value;
                this.OnPropertyChanged(nameof(this.TargetHeightSensor));
            }
        }

        public int TargetNormalFrom
        {
            get => this.m_TargetBoxInfo.FirstNormalFrom;
            set
            {
                this.m_TargetBoxInfo.FirstNormalFrom = value;
                this.OnPropertyChanged(nameof(this.TargetNormalFrom));
            }
        }
        public int TargetNormalTo
        {
            get => this.m_TargetBoxInfo.FirstNormalTo;
            set
            {
                this.m_TargetBoxInfo.FirstNormalTo = value;
                this.OnPropertyChanged(nameof(this.TargetNormalTo));
            }
        }
        public int TargetManualFrom
        {
            get => this.m_TargetBoxInfo.ManualFrom;
            set
            {
                this.m_TargetBoxInfo.ManualFrom = value;
                this.OnPropertyChanged(nameof(this.TargetManualFrom));
            }
        }
        public int TargetManualTo
        {
            get => this.m_TargetBoxInfo.ManualTo;
            set
            {
                this.m_TargetBoxInfo.ManualTo = value;
                this.OnPropertyChanged(nameof(this.TargetManualTo));
            }
        }
        #endregion

        #endregion

        #region reprint
        private bool m_DisplayReject = false;
        public bool DisplayReject
        {
            get => this.m_DisplayReject;
            set
            {
                this.Logger.Write($"DisplayReject Changed : {value}");
                this.m_DisplayReject = value;
                this.OnPropertyChanged(nameof(this.CaseErectSearchList));
                this.OnPropertyChanged(nameof(this.DisplayReject));
            }
        }

        private string m_SelectedBoxType;
        public string SelectedBoxType
        {
            get => this.m_SelectedBoxType;
            set
            {
                this.Logger.Write($"SelectedBoxType Changed : {value}");
                this.m_SelectedBoxType = value;
                this.OnPropertyChanged(nameof(this.SelectedBoxType));
            }
        }

        private string m_BoxNumber = "";
        public string BoxNumber
        {
            get => this.m_BoxNumber;
            set
            {
                this.Logger.Write($"BoxNumber Changed : {value}");
                this.m_BoxNumber = value;
                this.OnPropertyChanged(nameof(this.BoxNumber));
            }
        }

        public DateTime SearchBegin
        {
            get => this.m_CaseErectParam.Begin;
            set
            {
                this.Logger.Write($"SearchBegin Changed : {value}");
                this.m_CaseErectParam.Begin = value;
                this.OnPropertyChanged(nameof(this.SearchBegin));
            }
        }

        public DateTime SearchEnd
        {
            get => this.m_CaseErectParam.End;
            set
            {
                this.Logger.Write($"SearchEnd Changed : {value}");
                this.m_CaseErectParam.End = value;
                this.OnPropertyChanged(nameof(this.SearchEnd));
            }
        }

        public IEnumerable<CaseErectData> CaseErectSearchList
            => this.DisplayReject ? this.m_CaseErectRejectSearchList.GetPagedList("") : this.m_CaseErectSearchList.GetPagedList("");

        private string m_NewPrintCount;
        public string NewPrintCount
        {
            get => this.m_NewPrintCount;
            set
            {
                this.m_NewPrintCount = value;
                this.OnPropertyChanged(nameof(this.NewPrintCount));
            }
        }

        private bool m_ShowNewPrint;
        public bool ShowNewPrint
        {
            get => this.m_ShowNewPrint;
            set
            {
                this.m_ShowNewPrint = value;
                this.OnPropertyChanged(nameof(this.ShowNewPrint));
            }
        }

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
        public IEnumerable<CaseErectData> NonVerifiedBoxList
            => this.m_NonVerifiedBoxList.GetPagedList("");
        #endregion

        #endregion

        #region constructor
        public CaseErectViewModel() : base()
        {
            this.Logger.Write($"CaseErectViewModel Start");
            this.InitPagingLists();
            this.printer = EcsTouchAppManager.Instance.Equipments.GetByEquipmentType<LabelPrinterZebraZt411Equipment>();
            if (this.printer == null)
            {
                this.ShowErrorMessageBox($"프린터 정보가 없습니다.(Setting 오류)");
            }
            else
                this.PrinterConnection = this.printer.Communicator.TcpConnectionState == Urcis.SmartCode.Net.Tcp.TcpConnectionStateEnum.Connected;

            this.EnrollEventHandler();
            this.NumberingPopupRefresh();
            this.SelectTodayData();
        }
        #endregion

        #region method

        #region private
        private void InitPagingLists()
        {
            this.Logger.Write($"InitPagingLists");
            this.m_CaseErectList1 = new PagingList<CaseErectData>(() => { });
            this.m_CaseErectList1.AddPaging("", m_CaseErectWindowSize, () =>
             {
                 this.OnPropertyChanged(nameof(this.CaseErectList1));
             });
            this.m_CaseErectList2 = new PagingList<CaseErectData>(() => { });
            this.m_CaseErectList2.AddPaging("", m_CaseErectWindowSize, () =>
             {
                 this.OnPropertyChanged(nameof(this.CaseErectList2));
             });
            this.m_BoxInfoList = new PagingList<BoxInfoData>(() => { });
            this.m_BoxInfoList.AddPaging("", m_BoxInfoWindowSize, () =>
             {
                 this.OnPropertyChanged(nameof(this.BoxInfoList));
             }, false);
            this.m_CaseErectSearchList = new PagingList<CaseErectData>(() => { });
            this.m_CaseErectSearchList.AddPaging("", m_CaseErectSearchWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.CaseErectSearchList));
            });
            this.m_CaseErectRejectSearchList = new PagingList<CaseErectData>(() => { });
            this.m_CaseErectRejectSearchList.AddPaging("", m_CaseErectSearchWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.CaseErectSearchList));
            });
            this.m_NonVerifiedBoxList = new PagingList<CaseErectData>(() => { });
            this.m_NonVerifiedBoxList.AddPaging("", m_NonVerifiedBoxWindowSize, () =>
            {
                this.OnPropertyChanged(nameof(this.NonVerifiedBoxList));
            });
        }
        private void SelectTodayData()
        {
            this.Logger.Write($"SelectTodayData");
            this.m_CaseErectList1.Clear();
            this.m_CaseErectList2.Clear();
            var list = dbm.SelectTodayErect();
            this.AddCaseErectData(list);
        }
        private void AddCaseErectData(List<CaseErectData> list)
        {
            this.Logger.Write($"AddCaseErectData");
            var datas = list.Select(data =>
            {
                if (this.m_BoxNameMap.ContainsKey(data.BoxType))
                    data.BoxName = this.m_BoxNameMap[data.BoxType];
                return data;
            });

            var firstDatas = datas.Where(d => d.ErectorType == "1호기");
            this.m_CaseErectList1.AddRange(firstDatas);
            this.RecentErectData1 = firstDatas.Count() > 0 ? firstDatas.Last() : CaseErectData.None;

            var secondDatas = datas.Where(d => d.ErectorType == "2호기");
            this.m_CaseErectList2.AddRange(secondDatas);
            this.RecentErectData2 = secondDatas.Count() > 0 ? secondDatas.Last() : CaseErectData.None;
        }
        private void AddBoxInfoData(List<BoxInfoData> list)
        {
            this.Logger.Write($"AddBoxInfoData");
            int no = 1;
            var datas = list.Select(d =>
            {
                d.No = no++;
                return d;
            });
            this.m_BoxInfoList.AddRange(datas);
            foreach (var d in datas)
                this.m_BoxNameMap[d.BoxTypeCd] = d.Name;
        }
        private void AddCaseErectSearchData(List<CaseErectData> list)
        {
            this.Logger.Write($"AddCaseErectSearchData");
            this.m_CaseErectRejectSearchList.AddRange(list.Where(d => d.Verification != "Y"));
            this.m_CaseErectSearchList.AddRange(list);
        }
        private void AddNonVerifiedBoxData(List<CaseErectData> list)
        {
            this.Logger.Write($"AddNonVerifiedBoxData");
            this.m_NonVerifiedBoxList.AddRange(list);
        }
        #endregion

        #region public

        #region case erect
        public void CaseErect1Move(bool isUp) => this.m_CaseErectList1.PageMove("", isUp);
        public void CaseErect1MoveTop() => this.m_CaseErectList1.PageMoveTop("");
        public void CaseErect1MoveBottom() => this.m_CaseErectList1.PageMoveBottom("");
        public void CaseErect2Move(bool isUp) => this.m_CaseErectList2.PageMove("", isUp);
        public void CaseErect2MoveTop() => this.m_CaseErectList2.PageMoveTop("");
        public void CaseErect2MoveBottom() => this.m_CaseErectList2.PageMoveBottom("");

        public void InkjetResume(int line)
        {
            this.Logger.Write($"InkjetResume : {line}");
            this.server.InkjectResumeSend(line);
            this.ShowErrorMessageBox($"잉크젯 {line}호기 재가동을 요청했습니다.");
        }
        public void NumberingPopupRefresh()
        {
            this.Logger.Write($"NumberingPopupRefresh");
            this.m_BoxInfoList.Clear();
            this.m_BoxNameMap.Clear();
            var list = dbm.SelectBoxInfos();
            this.AddBoxInfoData(list);
            this.OnPropertyChanged(nameof(this.BoxTypeList));
        }
        #endregion

        #region Numbering
        public void NumberingPopupMove(bool isUp) => this.m_BoxInfoList.PageMove("", isUp);
        public void NumberingPopupMoveTop() => this.m_BoxInfoList.PageMoveTop("");
        public void NumberingPopupMoveBottom() => this.m_BoxInfoList.PageMoveBottom("");
        public void ShowMessageBox(int erectorType, bool isInsert)
        {
            this.Logger.Write($"ShowMessageBox : {erectorType}, {isInsert}");
            this.IsInsert = isInsert;
            if (isInsert)
                this.TargetBoxInfo = BoxInfoData.None;
            else if (this.SelectedBoxInfo.Equals(BoxInfoData.None))
                return;
            else
            {
                this.TargetBoxInfo = this.SelectedBoxInfo;
                if (erectorType == 1)
                {
                    this.TargetNormalFrom = this.TargetBoxInfo.SecondNormalFrom;
                    this.TargetNormalTo = this.TargetBoxInfo.SecondNormalTo;
                }
            }
            this.ShowNumberingMessageBox = true;
        }
        public void DeleteBoxInfo()
        {
            this.Logger.Write($"DeleteBoxInfo");
            if (!this.SelectedBoxInfo.Equals(BoxInfoData.None))
            {
                dbm.DeleteBoxInfo(this.SelectedBoxInfo.BoxTypeCd);
                this.ShowErrorMessageBox("박스 정보가 삭제되었습니다.");
                this.NumberingPopupRefresh();
            }
        }
        public void SubmitMessageBox(int erectorType)
        {
            this.Logger.Write($"SubmitMessageBox : {erectorType}");
            this.TargetType = this.TargetType.Trim();
            if (this.TargetType == "")
                return;
            this.m_TargetBoxInfo.SecondNormalFrom = this.TargetNormalFrom;
            this.m_TargetBoxInfo.SecondNormalTo = this.TargetNormalTo;
            if (this.IsInsert)
            {
                if (dbm.InsertBoxInfo(this.TargetBoxInfo))
                    this.ShowErrorMessageBox("박스 정보가 추가되었습니다.");
                else
                    this.ShowErrorMessageBox("이미 존재하는 박스 타입입니다.");
            }
            else
            {
                if (dbm.UpdateBoxInfo(this.SelectedBoxInfo.BoxTypeCd, this.TargetBoxInfo, erectorType == 0))
                {
                    this.ShowErrorMessageBox("박스 정보가 수정되었습니다.");
                    server.BoxNumberUpdate(erectorType + 1);
                }
                else
                    this.ShowErrorMessageBox("이미 존재하는 박스 타입입니다.");
            }
            this.ShowNumberingMessageBox = false;
            this.NumberingPopupRefresh();
        }
        #endregion

        #region newPrint
        public void OpenNewPrint()
        {
            this.Logger.Write($"OpenNewPrint");
            this.ShowNewPrint = true;
        }
        #endregion

        #region reprint
        public void CaseErectSearchMove(bool isUp)
        {
            if (this.DisplayReject)
                this.m_CaseErectRejectSearchList.PageMove("", isUp);
            else
                this.m_CaseErectSearchList.PageMove("", isUp);
        }
        public void CaseErectSearchMoveTop()
        {
            if (this.DisplayReject)
                this.m_CaseErectRejectSearchList.PageMoveTop("");
            else
                this.m_CaseErectSearchList.PageMoveTop("");
        }
        public void CaseErectSearchMoveBottom()
        {
            if (this.DisplayReject)
                this.m_CaseErectRejectSearchList.PageMoveBottom("");
            else
                this.m_CaseErectSearchList.PageMoveBottom("");
        }
        public void Search()
        {
            this.Logger.Write($"Search");
            this.Clear();
            this.BoxNumber = this.BoxNumber.ToUpper();
            var boxNumber = this.BoxNumber;
            if (boxNumber.Length == 10)
            {
                this.SelectedBoxType = this.BoxNumber.Substring(1, 1);
                boxNumber = this.BoxNumber.Substring(3, 7);
            }
            //Textbox의 MaxLength=10
            this.m_CaseErectParam.BoxId = boxNumber;
            var list = dbm.SelectCaseErectQuery(this.m_CaseErectParam, this.SelectedBoxType);
            this.AddCaseErectSearchData(list);

        }
        public void Clear()
        {
            this.Logger.Write($"Clear");
            this.m_CaseErectSearchList.Clear();
            this.m_CaseErectRejectSearchList.Clear();
        }
        public void Refresh()
        {
            this.Logger.Write($"Refresh");
            this.Clear();
            this.SelectedBoxType = this.BoxTypeList.First();
            this.BoxNumber = "";
            this.SearchBegin = DateTime.Now;
            this.SearchEnd = DateTime.Now;
        }
        public bool NewPrint()
        {
            this.Logger.Write($"NewPrint");
            int nextNumber = dbm.GetBoxNumber(this.SelectedBoxType, "0");
            if (nextNumber == -1)
            {
                this.ShowErrorMessageBox("존재하지 않는 박스 타입 입니다.");
                this.NumberingPopupRefresh();
                return false;
            }

            //10자리 = 층,박스타입, 수기(0), 박스번호(7자리)
            string boxId = $"2{this.SelectedBoxType}0{nextNumber}";

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

            printRequest = printer.RejectBoxPrintSend(boxId);
            if (printRequest)
            {
                dbm.PrintBox(boxId);
                this.ShowErrorMessageBox($"'{boxId}'의 신규발행이 요청 되었습니다.");
                this.BoxNumber = "";
                //this.Search();

                Thread.Sleep(3000); //프린터가 인쇄할 시간을 벌어야함

                this.ManualVerification(boxId, this.SelectedBoxType);
            }
            else
            {
                this.ShowErrorMessageBox($"발행 요청이 정상적으로 되지 않았습니다.");
                return false;
            }

            return true;
        }

        public void NewPrint(int count)
        {
            this.Logger.Write($"NewPrint Count = {count}");

            for (int i = 0; i < count; i++)
            {
                if (this.NewPrint() == false)
                    break;
            }
        }
        public void Reprint(string boxId)
        {
            this.Logger.Write($"Reprint : {boxId}");
            bool printRequest = false;
            printRequest = printer.HostStatusReturnSend();
            if (printRequest == false)
            {
                this.ShowErrorMessageBox($"프린터 상태가 정상이 아닙니다.");
                return;
            }

            if (this.printerState == null
               || this.printerState.PaperOutFlag
               || this.printerState.PauseFlag)
            {
                this.ShowErrorMessageBox($"프린터 상태가 정상이 아닙니다.");
                return;
            }

            Thread.Sleep(100); //프린터가 딜레이가 있어야 제대로 수신받는듯하다.

            printRequest = printer.RejectBoxPrintSend(boxId);
            if (printRequest)
            {
                dbm.ReprintBox(boxId);
                this.ShowErrorMessageBox($"'{boxId}'의 재발행이 요청 되었습니다.");
                this.Search();
            }
            else
                this.ShowErrorMessageBox($"발행 요청이 정상적으로 되지 않았습니다.");
        }
        public void OpenManualVerification()
        {
            this.Logger.Write($"OpenManualVerification");
            this.m_NonVerifiedBoxList.Clear();
            this.ShowManualVerification = true;
        }
        public void SearchNonVerifiedBox(string boxId)
        {
            this.Logger.Write($"SearchNonVerifiedBox : {boxId}");
            this.m_NonVerifiedBoxList.Clear();
            var list = dbm.SelectNonVerifiedBox(boxId);
            this.AddNonVerifiedBoxData(list);
            if (this.m_NonVerifiedBoxList.InnerList.Count == 0)
                this.ShowErrorMessageBox("검증할 수 있는 박스ID가 없습니다.");
            this.BoxNumber = boxId;
            this.Search();
        }
        public void ManualVerificationMove(bool isUp)
            => this.m_NonVerifiedBoxList.PageMove("", isUp);
        public void ManualVerification(string boxId, string boxType)
        {
            this.Logger.Write($"ManualVerification : {boxId}, {boxType}");
            this.server.ManualBoxValidationRequestSend(boxId, boxType);
            this.ShowErrorMessageBox($"'{boxId}'를 검증 요청했습니다.");
            this.BoxNumber = boxId;
            this.Search();
        }
        #endregion

        #endregion

        #region event handler
        private void EnrollEventHandler()
        {
            this.Logger.Write($"EnrollEventHandler");
            printer.Communicator.TcpConnectionStateChanged += OnPrinterConnectionChanged;
            printer.Communicator.HostStatusReturnRecived += OnCommunicator_HostStatusReturnRecived;
            server.CaseErectBcrReceived += OnCaseErectBcrReceived;
            server.ErectorStateReceived += OnErectorStateReceived;
            server.InkjetInkReceived += OnInkjetInkReceived;
            server.ManualBoxValidationResponseReceived += OnManualBoxValidationResponseReceived;
        }

        private void OnPrinterConnectionChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.PrinterConnection = e.Current == Urcis.SmartCode.Net.Tcp.TcpConnectionStateEnum.Connected;
            });
        }
        private void OnCommunicator_HostStatusReturnRecived(object sender, HostStatusReturnArg e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.printerState = e;
            });
        }

        protected override void OnTimeSyncronizeReceived(TimeSyncronize data)
        {
            base.OnTimeSyncronizeReceived(data);
            this.Dispatcher.Invoke(() =>
            {
                this.SelectTodayData();
            });
        }
        private void OnCaseErectBcrReceived(CaseErectBcrRead data)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnCaseErectBcrReceived : {data}");
                var list = dbm.SelectErectByIndex(new TouchParam() { BcrIndex = data.CaseErectIndex });
                this.AddCaseErectData(list);
            });
        }
        private void OnErectorStateReceived(ErectorConnectionState data)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnErectorStateReceived : {data}");
                this.Erector1Connection = data.Erector1Connection;
                this.Erector2Connection = data.Erector2Connection;
                this.Inkjet1Connection = data.Inkjet1Connection;
                this.Inkjet2Connection = data.Inkjet2Connection;
                this.Bcr1Connection = data.ErectorBcr1Connection;
                this.Bcr2Connection = data.ErectorBcr2Connection;
            });
        }
        private void OnInkjetInkReceived(InkjectInk data)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnInkjetInkReceived : {data}");
                if (data.Line == 1)
                {
                    this.InkRatio1 = data.InkPercent;
                    if (data.InkPercent <= 10 && !this.IgnoreInkLeak1)
                        this.ShowErrorMessageBox($"잉크젯 1호기의 잉크가 부족합니다.");
                }
                else if (data.Line == 2)
                {
                    this.InkRatio2 = data.InkPercent;
                    if (data.InkPercent <= 10 && !this.IgnoreInkLeak2)
                        this.ShowErrorMessageBox($"잉크젯 2호기의 잉크가 부족합니다.");
                }
            });
        }
        private void OnManualBoxValidationResponseReceived(ManualBoxValidationResponse data)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnManualBoxValidationResponseReceived : {data}");
                if (data.Result)
                    this.ShowErrorMessageBox($"'{this.BoxNumber}'의 검증이 성공했습니다.");
                else
                    this.ShowErrorMessageBox($"'{this.BoxNumber}'의 검증이 실패했습니다.");
                this.Search();
            });
        }
        #endregion

        #endregion
    }
}
