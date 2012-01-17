using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using Instatus.Web;

namespace Instatus.Data
{
    public interface IPagedCollection<T> : IEnumerable<T>
    {
        int TotalItemCount { get; }
        int PageIndex { get; }
        int PageSize { get; }
    }    
    
    // http://msdn.microsoft.com/en-us/library/system.componentmodel.ipagedcollectionview(v=vs.95).aspx
    public class PagedCollection<T> : Collection<T>, IPagedCollection<T>
    {
        public int TotalItemCount { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }

        public int TotalPageCount {
            get
            {
                if (TotalItemCount == 0)
                    return 0;

                return (int)Math.Ceiling((double)TotalItemCount / PageSize);
            }
        }

        public PagedCollection(IEnumerable<T> list, int pageSize = 10, int pageIndex = 0, bool count = false)
        {
            if (list is IPagedCollection<T>)
            {
                var pagedCollection = (IPagedCollection<T>)list;
                this.Append(pagedCollection);
                TotalItemCount = pagedCollection.TotalItemCount;
            }
            else if (list is IOrderedQueryable<T> || list is IOrderedEnumerable<T>)
            {
                this.Append(list.Skip(pageIndex * pageSize).Take(pageSize).ToList());
                
                if(count)
                    TotalItemCount = list.Count();
            }
            else
            {
                this.Append(list.AsOrdered().Skip(pageIndex * pageSize).Take(pageSize).ToList());
                
                if(count)
                    TotalItemCount = list.Count();
            }

            PageSize = pageSize;
            PageIndex = pageIndex;
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
    }
}