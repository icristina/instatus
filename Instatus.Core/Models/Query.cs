using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Query : Set
    {
        public SortOrder SortOrder { get; set; }
        public ViewMode ViewMode { get; set; }
        public string Command { get; set; }
        public string ContentType { get; set; }
        public string User { get; set; }
        public string Ancestor { get; set; }
        public string Parent { get; set; }
        public string Tag { get; set; }
        public string Category { get; set; }
        public string Term { get; set; }
        public string[] Uri { get; set; }
        public DateTime? StartDate { get; set; }
        public string Filter { get; set; }
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
                return SortOrder == SortOrder.Nearby || HasCoordinates;
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
                return ViewMode == ViewMode.Day || ViewMode == ViewMode.Week || ViewMode == ViewMode.Month || ViewMode == ViewMode.Year;
            }
        }

        public Query() {
            PageSize = 10;
            MaxPageCount = 10;
            SortOrder = SortOrder.Recency;
            ViewMode = ViewMode.PagedList;
            CountTotal = true;
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }

        public Query PreviousPage()
        {
            return this.WithPageIndex(PageIndex - 1);
        }

        public Query NextPage()
        {
            return this.WithPageIndex(PageIndex + 1);
        }

        public Query WithPageSize(int size)
        {
            var query = (Query)this.Clone();
            query.PageSize = size;
            return query;
        }

        public Query WithPageIndex(int index) {
            var query = (Query)this.Clone();
            query.PageIndex = index;
            return query;
        }

        public Query WithTag(string tag)
        {
            var query = (Query)this.Clone();
            query.PageIndex = 0;
            query.Tag = tag;
            return query;
        }

        public Query WithStartDate(DateTime startDate)
        {
            var query = (Query)this.Clone();
            query.PageIndex = 0;
            query.StartDate = startDate;
            return query;
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Tag))
                return string.Format("tagged as {0}", Tag);

            return string.Empty;
        }
    }
}
