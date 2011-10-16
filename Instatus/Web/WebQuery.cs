using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Web.Routing;

namespace Instatus.Web
{
    public class WebExpression
    {
        public string User { get; set; }
        public string[] Expand { get; set; }
        public string Parent { get; set; }
        public string Tag { get; set; }
        public WebSort Sort { get; set; }
        public WebMode Mode { get; set; }
        public WebKind Kind { get; set; }
        public string[] Uri { get; set; }
        public string Term { get; set; }
        public string Locale { get; set; }
        public DateTime? StartDate { get; set; }
        public string Filter { get; set; }

        public bool IsDateView
        {
            get
            {
                return Mode == WebMode.Day || Mode == WebMode.Week || Mode == WebMode.Month || Mode == WebMode.Year;
            }
        }
    }
    
    public class WebQuery : WebExpression, ICloneable
    {
        public WebContentType ContentType { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int MaxPageCount { get; set; }
        public bool CountTotal { get; set; }

        public WebQuery() {
            PageSize = 10;
            MaxPageCount = 10;
            Sort = WebSort.Recency;
            Mode = WebMode.List;
            CountTotal = true;
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }

        public WebQuery WithPageSize(int size)
        {
            var query = (WebQuery)this.Clone();
            query.PageSize = size;
            return query;
        }

        public WebQuery WithPageIndex(int index) {
            var query = (WebQuery)this.Clone();
            query.PageIndex = index;
            return query;
        }

        public WebQuery WithTag(string tag)
        {
            var query = (WebQuery)this.Clone();
            query.PageIndex = 0;
            query.Tag = tag;
            return query;
        }

        public WebQuery WithStartDate(DateTime startDate)
        {
            var query = (WebQuery)this.Clone();
            query.PageIndex = 0;
            query.StartDate = startDate;
            return query;
        }

        public override string ToString()
        {
            if (!Tag.IsEmpty())
                return string.Format("tagged as {0}", Tag);

            return string.Empty;
        }

        public RouteValueDictionary ToRouteValueDictionary()
        {
            return new RouteValueDictionary()
                        .AddNonEmptyValue("user", User)
                        .AddNonEmptyValue("tag", Tag)
                        .AddNonEmptyValue("sort", Sort)
                        .AddNonEmptyValue("mode", Mode)
                        .AddNonEmptyValue("kind", Kind)
                        .AddNonEmptyValue("contentType", ContentType)
                        .AddNonEmptyValue("pageIndex", PageIndex)
                        .AddNonEmptyValue("pageSize", PageSize)
                        .AddNonEmptyValue("maxPageCount", MaxPageCount)
                        .AddNonEmptyValue("countTotal", CountTotal)
                        .AddNonEmptyValue("locale", Locale)
                        .AddNonEmptyValue("parent", Parent)
                        .AddNonEmptyValue("startDate", StartDate.HasValue ? StartDate.Value.ToString("yyyy-MM-dd") : null)
                        .AddNonEmptyValue("term", Term)
                        .AddNonEmptyValue("expand", string.Join(",", Expand ?? new string[] {}))
                        .AddNonEmptyValue("filter", Filter);
        }
    }
}