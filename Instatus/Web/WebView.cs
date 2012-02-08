using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using System.Web.Mvc;
using Instatus.Models;
using System.Collections;
using System.Dynamic;

namespace Instatus.Web
{
    public interface IContentSource
    {
        WebDocument Document { get; }
        IList<WebFeed> Feeds { get; }
    }
    
    public interface IWebView : IEnumerable, IViewModel
    {
        int TotalItemCount { get; }
        int TotalPageCount { get; }         
        WebQuery Query { get; }
        SelectList Tags { get; }
        SelectList Filter { get; }
        SelectList Mode { get; }
        SelectList Sort { get; }
        ICollection<IWebCommand> Commands { get; }
        bool Can(object action);
        SiteMapNodeCollection Navigation { get; }
        dynamic CurrentRow { get; set; }
        WebDocument Document { get; }
    }

    public class WebView<T> : PagedCollection<T>, IWebView, IContentSource, IExtensionPoint
    {
        public string Name { get; set; }
        public object Context { get; set; }
        public WebDocument Document { get; set; }
        public IList<WebFeed> Feeds { get; set; }
        public SelectList Tags { get; set; }
        public SelectList Filter { get; set; }
        public SelectList Mode { get; set; }
        public SelectList Sort { get; set; }
        public ICollection<WebLink> Links { get; set; }
        public WebQuery Query { get; set; }
        public IList Permissions { get; set; }
        public ICollection<IWebCommand> Commands { get; set; }
        public SiteMapNodeCollection Navigation { get; set; }
        public dynamic CurrentRow { get; set; }
        public WebStep Step { get; set; }
        public dynamic Extensions { get; set; }

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
                return TotalItemCount > (PageIndex + 1) * PageSize;
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

        private void Init()
        {
            Extensions = new ExpandoObject();
            Feeds = new List<WebFeed>();
        }

        public WebView(IEnumerable<T> list, WebQuery query)
            : base(list, query.PageSize, query.PageIndex, query.CountTotal)
        {
            Init();
            Query = query;
        }

        public WebView(IQueryable<Record<T>> queryable, Func<Record<T>, T> projection, WebQuery query)
            : base(queryable, projection, query.PageSize, query.PageIndex, query.CountTotal)
        {
            Init();
            Query = query;
        }

        private SelectList CreateSelectList<TSelectItem>(IEnumerable<TSelectItem> data, object selectedValue = null, IEnumerable<string> labels = null, string allLabel = "All", string prefix = "") {
            var items = new List<WebParameter>();

            if (!allLabel.IsEmpty())
                items.Add(new WebParameter("all", allLabel));

            for (var i = 0; i < data.Count(); i++)
            {
                items.Add(new WebParameter(prefix, data.ElementAt(i), labels.IsEmpty() ? data.ElementAt(i).ToString() : labels.ElementAt(i)));
            }

            return new SelectList(items, "Name", "Content", selectedValue);
        }

        public void WebStatusList(IEnumerable<WebStatus> statuses, IEnumerable<string> labels = null) {
            Filter = CreateSelectList<WebStatus>(statuses, Query.Filter, labels, prefix: "status");
        }

        public void WebModeList(IEnumerable<WebMode> modes, IEnumerable<string> labels = null)
        {
            Mode = CreateSelectList<WebMode>(modes, Query.Filter, labels);
        }

        public override string ToString()
        {
            if (!Name.IsEmpty())
                return Name;

            if (Document != null && !Document.Title.IsEmpty())
                return Document.Title;
            
            if (Query != null)
                return Query.Kind.ToDescriptiveString().ToPlural();

            return base.ToString();
        }
    }
}