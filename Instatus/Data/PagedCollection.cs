using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using Instatus.Web;

namespace Instatus.Data
{
    // http://msdn.microsoft.com/en-us/library/system.componentmodel.ipagedcollectionview(v=vs.95).aspx
    public class PagedCollection<T> : ReadOnlyCollection<T>
    {
        public int TotalItemCount { get; private set; }
        public int PageIndex { get; set; }
        public int PageSize { get; private set; }

        public int TotalPageCount {
            get
            {
                if (TotalItemCount == 0)
                    return 0;

                return (int)Math.Ceiling((double)TotalItemCount / PageSize);
            }
        }

        public PagedCollection(IEnumerable<T> list, int pageSize = 10, int pageIndex = 0, int totalItemCount = 0)
            : base(list.ToList())
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalItemCount = totalItemCount;
        }

        public PagedCollection(IQueryable<Record<T>> queryable, Func<Record<T>, T> mapping, int pageSize = 10, int pageIndex = 0, bool count = false)
            : base(queryable.Skip(pageIndex * pageSize).Take(pageSize).ToList().Select(mapping).ToList())
        {
            PageSize = pageSize;
            PageIndex = pageIndex;

            if (count)
            {
                TotalItemCount = queryable.Count();
            }   
        }

        public PagedCollection(IQueryable<Record<T, WebMetrics>> queryable, Func<Record<T, WebMetrics>, T> mapping, int pageSize = 10, int pageIndex = 0, bool count = false)
            : base(queryable.Skip(pageIndex * pageSize).Take(pageSize).ToList().Select(mapping).ToList())
        {
            PageSize = pageSize;
            PageIndex = pageIndex;

            if (count)
            {
                TotalItemCount = queryable.Count();
            }
        }

        public PagedCollection(IQueryable<T> queryable, int pageSize = 10, int pageIndex = 0, bool count = false) 
            : base(queryable.Skip(pageIndex * pageSize).Take(pageSize).ToList())
        {
            PageSize = pageSize;
            PageIndex = pageIndex;

            if (count)
            {
                TotalItemCount = queryable.Count();
            }        
        }
    }
}