﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core;

namespace Instatus.Integration.Mvc
{
    public class PagedViewModel<T> : IPaged<T>
    {
        private IList<T> results;

        public int TotalItemCount { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }

        public int TotalPageCount
        {
            get
            {
                if (TotalItemCount == 0)
                    return 0;

                return (int)Math.Ceiling((double)TotalItemCount / PageSize);
            }
        }

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
                return TotalItemCount > (PageIndex + 1) * PageSize;
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

        public PagedViewModel(IOrderedQueryable<T> orderedQueryable, int pageIndex, int pageSize)
        {
            this.results = orderedQueryable.Skip(pageSize * pageIndex).Take(pageSize).ToList();

            TotalItemCount = orderedQueryable.Count();
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
