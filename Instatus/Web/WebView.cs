using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using System.Web.Mvc;
using Instatus.Models;
using System.Collections;

namespace Instatus.Web
{
    public interface IWebView : IEnumerable
    {
        int TotalItemCount { get; }
        int TotalPageCount { get; }         
        WebQuery Query { get; }
        SelectList Tags { get; }
        SelectList Filter { get; }
        SelectList Mode { get; }
        ICollection<IWebCommand> Commands { get; }
        bool Can(object action);
        SiteMapNodeCollection Navigation { get; }
        dynamic CurrentRow { get; set; }
    }

    public class WebView<T> : PagedCollection<T>, IWebView
    {
        public string Name { get; set; }
        public object Context { get; set; }
        public WebDocument Document { get; set; }
        public SelectList Tags { get; set; }
        public SelectList Filter { get; set; }
        public SelectList Mode { get; set; }
        public ICollection<WebLink> Links { get; set; }
        public WebQuery Query { get; set; }
        public IList Permissions { get; set; }
        public ICollection<IWebCommand> Commands { get; set; }
        public SiteMapNodeCollection Navigation { get; set; }
        public dynamic CurrentRow { get; set; }

        public bool Can(object action)
        {
            return !Permissions.IsEmpty() && Permissions.Contains(action);
        }

        public bool HasPrevious
        {
            get
            {
                return PageIndex > 0;
            }
        }

        public bool HasNext
        {
            get
            {
                return TotalItemCount >= (PageIndex + 1) * PageSize;
            }
        }

        public WebQuery Previous()
        {
            return HasPrevious ? Query.WithPageIndex(PageIndex - 1) : null;
        }

        public WebQuery Next()
        {
            return HasNext ? Query.WithPageIndex(PageIndex + 1) : null;
        }

        public WebView(IEnumerable<T> list, WebQuery query, int totalItemCount = 0)
            : base(list, query.PageSize, query.PageIndex, totalItemCount)
        {
            Query = query;
        }

        public WebView(IQueryable<T> queryable, WebQuery query)
            : base(queryable, query.PageSize, query.PageIndex, query.CountTotal)
        {
            Query = query;
        }

        public WebView(IQueryable<Record<T>> queryable, Func<Record<T>, T> projection, WebQuery query)
            : base(queryable, projection, query.PageSize, query.PageIndex, query.CountTotal)
        {
            Query = query;
        }
    }
}