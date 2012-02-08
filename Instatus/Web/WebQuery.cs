using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Web.Routing;

namespace Instatus.Web
{       
    public class WebQuery : WebSet, ICloneable
    {
        // view or mode
        public WebSort Sort { get; set; }
        public WebMode Mode { get; set; }
        public string Command { get; set; } // edit action
    
        // selection
        public WebContentType ContentType { get; set; }    
        public string User { get; set; }

        // axis
        public string Ancestor { get; set; }
        public string Parent { get; set; }
        
        // taxonomy
        public string Tag { get; set; }
        public string Category { get; set; }

        // search
        public string Term { get; set; }

        public string[] Uri { get; set; }
        public DateTime? StartDate { get; set; }
        public string Filter { get; set; }    

        // geo
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Zoom { get; set; }

        public bool HasCoordinates
        {
            get
            {
                return !(Latitude == 0 && Longitude == 0);
            }
        }

        public bool IsGeospacial
        {
            get
            {
                return Sort == WebSort.Nearby || HasCoordinates;
            }
        }

        // paging
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int MaxPageCount { get; set; }
        public bool CountTotal { get; set; }

        public bool IsDateView
        {
            get
            {
                return Mode == WebMode.Day || Mode == WebMode.Week || Mode == WebMode.Month || Mode == WebMode.Year;
            }
        }

        public WebQuery() {
            PageSize = 10;
            MaxPageCount = 10;
            Sort = WebSort.Recency;
            Mode = WebMode.PagedList;
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
            return new RouteValueDictionary(new Dictionary<string, object>()
                            .AddNonEmptyValue("user", User)
                            .AddNonEmptyValue("tag", Tag)
                            .AddNonEmptyValue("sort", Sort)
                            .AddNonEmptyValue("mode", Mode)
                            .AddNonEmptyValue("contentType", ContentType)
                            .AddNonEmptyValue("pageIndex", PageIndex)
                            .AddNonEmptyValue("pageSize", PageSize)
                            .AddNonEmptyValue("maxPageCount", MaxPageCount)
                            .AddNonEmptyValue("countTotal", CountTotal)
                            .AddNonEmptyValue("ancestor", Ancestor)
                            .AddNonEmptyValue("parent", Parent)
                            .AddNonEmptyValue("startDate", StartDate.HasValue ? StartDate.Value.ToString("yyyy-MM-dd") : null)
                            .AddNonEmptyValue("term", Term)
                            .AddNonEmptyValue("filter", Filter)
                            .AddNonEmptyValue("category", Category)
                            .AddNonEmptyValue("latitude", Latitude)
                            .AddNonEmptyValue("longitude", Longitude)
                            .AddNonEmptyValue("zoom", Zoom)
                            .AddNonEmptyValue("kind", Kind)
                            .AddNonEmptyValue("locale", Locale)
                            .AddNonEmptyValue("expand", string.Join(",", Expand ?? new string[] { })));
        }

        public IDictionary<string, object> ToDataAttributeDictionary()
        {
            return new Dictionary<string, object>()
                .AddNonEmptyValue("data-set-kind", Kind.ToString().ToCamelCase())
                .AddNonEmptyValue("data-query-mode", Mode.ToString().ToCamelCase())
                .AddNonEmptyValue("data-query-pageSize", PageSize)
                .AddNonEmptyValue("data-query-category", Category.ToCamelCase())
                .AddNonEmptyValue("data-query-tag", Tag.ToCamelCase());
        }
    }
}