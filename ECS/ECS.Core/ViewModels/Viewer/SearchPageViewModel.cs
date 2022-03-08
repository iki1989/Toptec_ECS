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
    public class SearchPageViewModel : EcsViewerViewModel
    {
        protected event Action PageSizeChanged;
        protected event Action PageChanged;
        #region field
        protected SearchParam m_Param = new SearchParam()
        {
            Begin = DateTime.Now,
            End = DateTime.Now,
            WaveNo = "",
            CstCd = "",
            CstOrdNo = "",
            BoxId = "",
            InvoiceId = ""
        };
        #endregion

        #region property
        public DateTime Begin
        {
            get => this.m_Param.Begin;
            set
            {
                this.m_Param.Begin = value;
                this.End = this.End;
                this.OnPropertyChanged(nameof(this.Begin));
            }
        }

        public DateTime End
        {
            get => this.m_Param.End;
            set
            {
                if (value > this.Begin.AddDays(7))
                    value = this.Begin.AddDays(7);
                else if (value < this.Begin)
                    value = this.Begin;
                this.m_Param.End = value;
                this.OnPropertyChanged(nameof(this.End));
            }
        }

        public string WaveNo
        {
            get => this.m_Param.WaveNo;
            set
            {
                this.m_Param.WaveNo = value;
                this.OnPropertyChanged(nameof(this.WaveNo));
            }
        }

        public string CstCd
        {
            get => this.m_Param.CstCd;
            set
            {
                this.m_Param.CstCd = value;
                this.OnPropertyChanged(nameof(this.CstCd));
            }
        }

        public string CstOrdNo
        {
            get => this.m_Param.CstOrdNo;
            set
            {
                this.m_Param.CstOrdNo = value;
                this.OnPropertyChanged(nameof(this.CstOrdNo));
            }
        }

        public string BoxId
        {
            get => this.m_Param.BoxId;
            set
            {
                this.m_Param.BoxId = value;
                this.OnPropertyChanged(nameof(this.BoxId));
            }
        }

        public string InvoiceId
        {
            get => this.m_Param.InvoiceId;
            set
            {
                this.m_Param.InvoiceId = value;
                this.OnPropertyChanged(nameof(this.InvoiceId));
            }
        }

        protected int m_PageSize = 20;
        public int PageSize
        {
            get => this.m_PageSize;
            set
            {
                var prev = this.m_PageSize;
                this.m_PageSize = value;
                this.OnPropertyChanged(nameof(this.PageSize));
                this.PageSizeChanged?.Invoke();
                this.Page = (this.Page - 1) * prev / value + 1;
            }
        }

        protected int m_Page = 0;
        public int Page
        {
            get => this.m_Page;
            set
            {
                if (value < 1 || MaxPage != 0 && value > MaxPage)
                    return;
                this.m_Page = value;
                this.OnPropertyChanged(nameof(this.Page));
                this.PageChanged?.Invoke();
            }
        }

        protected int m_MaxPage = 0;
        public int MaxPage
        {
            get => this.m_MaxPage;
            set
            {
                this.m_MaxPage = value;
                this.OnPropertyChanged(nameof(this.MaxPage));
            }
        }
        #endregion

        #region method
        protected void SetMaxPage(int count)
            => this.MaxPage = count == 0 ? 0 : (count - 1) / this.PageSize + 1;
        protected IEnumerable<T> GetPagedList<T>(IEnumerable<T> list)
            => list.Skip((this.Page - 1) * this.PageSize).Take(this.PageSize).AsParallel();
        protected void Export<T>(string filename, IEnumerable<T> list)
        {
            List<string> lsFields = new List<string>();
            CsvFile cf = new CsvFile();
            var type = typeof(T);

            if (cf.Open(filename) == true)
            {
                lsFields.Clear();

                // Part: Title !!
                foreach (var f in typeof(T).GetProperties())
                {
                    lsFields.Add(f.Name);
                }

                cf.AddRowNoQuotations(lsFields);
                lsFields.Clear();

                // Part: Cell Data !!
                foreach (T c in list)
                {
                    foreach (var f in typeof(T).GetProperties())
                    {
                        lsFields.Add(f.GetValue(c)?.ToString()??"");
                    }

                    cf.AddRowNoQuotations(lsFields);
                    lsFields.Clear();
                }
            }

            cf.Write();
            cf.Close();

            MessageBox.Show("data exporting completed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public virtual void Clear()
        {
            this.Begin = DateTime.Today;
            this.End = DateTime.Today;
            this.WaveNo = "";
            this.CstCd = "";
            this.CstOrdNo = "";
            this.BoxId = "";
            this.InvoiceId = "";
            this.Page = 0;
        }
        #endregion

    }
}
