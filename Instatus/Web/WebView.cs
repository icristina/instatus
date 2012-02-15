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
    public interface IContentItem
    {
        WebDocument Document { get; set; }
        IDictionary<WebVerb, IWebFeed> Feeds { get; }
    }
    
    public interface IWebView : IEnumerable, IViewModel, IHasPermission
    {
        int TotalItemCount { get; }
        int TotalPageCount { get; }         
        WebQuery Query { get; }
        SelectList Tags { get; }
        SelectList Filter { get; }
        SelectList Mode { get; }
        SelectList Sort { get; }
        ICollection<IWebCommand> Commands { get; }
        SiteMapNodeCollection Navigation { get; }
        dynamic CurrentRow { get; set; }
        WebDocument Document { get; }
        string[] Columns { get; }
    }

    public class WebView<T> : PagedCollection<T>, IWebView, IContentItem, IExtensionPoint, IHasPermission
    {
        public string Name { get; set; }
        public object Context { get; set; }
        public WebDocument Document { get; set; }
        public IDictionary<WebVerb, IWebFeed> Feeds { get; private set; }
        public SelectList Tags { get; set; }
        public SelectList Filter { get; set; }
        public SelectList Mode { get; set; }
        public SelectList Sort { get; set; }
        public ICollection<WebLink> Links { get; set; }
        public WebQuery Query { get; set; }
        public ICollection<IWebCommand> Commands { get; set; }
        public SiteMapNodeCollection Navigation { get; set; }
        public dynamic CurrentRow { get; set; }
        public WebStep Step { get; set; }
        public dynamic Extensions { get; set; }
        public string[] Columns { get; set; }

        private List<string> permissions;

        public IList Permissions
        {
            get
            {
                return permissions;
            }
            set
            {
                permissions = value.ToStringList(); // IHasPermissions typically checks against a string
            }
        }

        public bool Can(string action, object instance)
        {
            return !Permissions.IsEmpty() && Permissions.Contains(action) && WebApp.Can(action, instance);
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
            Feeds = new Dictionary<WebVerb, IWebFeed>();
            Columns = new string[] { };
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