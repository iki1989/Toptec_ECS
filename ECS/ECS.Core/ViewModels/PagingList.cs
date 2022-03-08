using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.ViewModels
{
    public class PagingList<T>
    {
        private class Paging
        {
            private PagingList<T> m_Parent;
            public int WindowSize { get; set; }
            public int Index { get; set; }
            public bool IsReverse { get; set; }
            public bool ScrollRock { get; set; } = false;
            public IEnumerable<T> PagedList
            {
                get
                {
                    if (IsReverse)
                        return ((IEnumerable<T>)this.m_Parent.InnerList).Reverse().Skip(this.Index).Take(this.WindowSize);
                    return this.m_Parent.InnerList.Skip(this.Index).Take(this.WindowSize);
                }
            }

            public Action PageChanged;

            public Paging(PagingList<T> parent, int windowSize, bool isReverse, bool scrollRock)
            {
                this.m_Parent = parent;
                this.WindowSize = windowSize;
                this.IsReverse = isReverse;
                this.ScrollRock = scrollRock;
            }
            public void Move(bool isUp)
            {
                this.Index += this.WindowSize * (isUp ? -1 : 1);
                if (this.m_Parent.InnerList.Count < this.Index + this.WindowSize)
                    this.Index = this.m_Parent.InnerList.Count - this.WindowSize;
                if (this.Index < 0)
                    this.Index = 0;
                PageChanged?.Invoke();
            }
            public void MoveTop()
            {
                this.Index = 0;
                PageChanged?.Invoke();
            }
            public void MoveBottom()
            {
                this.Index = this.m_Parent.InnerList.Count - this.WindowSize;
                PageChanged?.Invoke();
            }
        }
        public event Action ListChanged;
        public List<T> InnerList { get; private set; } = new List<T>();
        private Dictionary<string, Paging> m_PagingMap = new Dictionary<string, Paging>();
        public PagingList(Action onListChanged) => this.ListChanged = onListChanged;
        public void Add(T data)
        {
            lock (this.InnerList)
            {
                this.InnerList.Add(data);
                foreach (Paging p in m_PagingMap.Values)
                {
                    if (p.ScrollRock || p.Index > 0)
                        ++p.Index;
                    p.PageChanged?.Invoke();
                }
                this.ListChanged?.Invoke();
            }
        }
        public void AddRange(IEnumerable<T> list)
        {
            foreach (T item in list)
            {
                this.InnerList.Add(item);
                foreach (Paging p in m_PagingMap.Values)
                {
                    if (p.ScrollRock || p.Index > 0)
                        ++p.Index;
                }
            }
            foreach (Paging p in m_PagingMap.Values)
            {
                p.PageChanged?.Invoke();
            }
            this.ListChanged?.Invoke();
        }
        public void AddOrUpdate(T data, Predicate<T> match)
        {
            var index = this.InnerList.FindIndex(match);
            if (index > 0)
            {
                this.InnerList[index] = data;
                this.ListChanged?.Invoke();
            }
            else
                this.Add(data);
        }
        public void Clear()
        {
            lock (this.InnerList)
            {
                InnerList.Clear();
                this.ListChanged?.Invoke();
                this.Reset();
            }
        }
        public void Reset()
        {
            foreach (Paging p in m_PagingMap.Values)
            {
                p.Index = 0;
                p.PageChanged?.Invoke();
            }
        }
        public void AddPaging(string key, int windowSize, Action onPageChanged, bool isReverse = true, bool scrollRock = false)
        {
            var p = new Paging(this, windowSize, isReverse, scrollRock);
            p.PageChanged += onPageChanged;
            this.m_PagingMap[key] = p;
        }
        public IEnumerable<T> GetPagedList(string key) => this.m_PagingMap[key].PagedList;
        public void PageMove(string key, bool isUp) => this.m_PagingMap[key].Move(isUp);
        public void PageMoveTop(string key) => this.m_PagingMap[key].MoveTop();
        public void PageMoveBottom(string key) => this.m_PagingMap[key].MoveBottom();
    }
}
