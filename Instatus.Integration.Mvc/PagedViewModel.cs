using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core;
using Instatus.Core.Extensions;
using Instatus.Core.Models;

namespace Instatus.Integration.Mvc
{
    public class PagedViewModel<T> : IPaged, IList<T>
    {
        private IList<T> results;

        public int PageIndex { get; private set; }
        public int TotalPageCount { get; private set; }
        public int TotalItemCount { get; private set; }

        public Document Document { get; set; }

        public bool HasPreviousPage
        {
            get
            {
                return PageIndex > 0;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageIndex + 1 < TotalPageCount;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return results.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get
            {
                return results[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                return results.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public PagedViewModel(IOrderedQueryable<T> orderedQueryable, int pageIndex, int pageSize)
        {
            results = orderedQueryable.Skip(pageSize * pageIndex).Take(pageSize).ToList();

            TotalItemCount = orderedQueryable.Count();

            if (TotalItemCount == 0)
            {
                TotalPageCount = 0;
            }
            else
            {
                TotalPageCount = (int)Math.Ceiling((double)TotalItemCount / pageSize);
            }

            PageIndex = pageIndex;
        }

        public PagedViewModel(IOrderedQueryable<T> orderedQueryable, int pageIndex, int firstPageSize, int defaultPageSize)
        {
            if (pageIndex == 0)
            {
                results = orderedQueryable.Take(firstPageSize).ToList();
            }
            else
            {
                results = orderedQueryable.Skip(firstPageSize + (defaultPageSize * (pageIndex - 1))).Take(defaultPageSize).ToList();
            }

            TotalItemCount = orderedQueryable.Count();

            if (TotalItemCount == 0)
            {
                TotalPageCount = 0;
            }
            else if (TotalItemCount <= firstPageSize)
            {
                TotalPageCount = 1;
            }
            else
            {
                TotalPageCount = (int)Math.Ceiling((double)(TotalItemCount - firstPageSize) / defaultPageSize) + 1;
            }

            PageIndex = pageIndex;
        }
    }
}
