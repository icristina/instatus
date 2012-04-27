using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Web;

namespace Instatus
{
    public static class LinkQueries
    {
        public static IEnumerable<Link> WithContentType(this IEnumerable<Link> links, WebContentType webContentType)
        {
            var contentType = webContentType.ToMimeType();
            return links.Where(l => l.ContentType == contentType);
        }

        public static string Uri(this IEnumerable<Link> links, WebContentType webContentType = WebContentType.Html)
        {
            return links.WithContentType(webContentType).Select(l => l.Uri).FirstOrDefault();
        }

        public static string Uri(this IEnumerable<Link> links, string rel)
        {
            return links.Where(l => l.Rel.Match(rel)).Select(l => l.Uri).FirstOrDefault();
        }

        //public static IEnumerable<Link> Redirects(this IEnumerable<Link> links)
        //{
        //    return links.Where(l => l.Location != null && l.HttpStatusCode > 300 && l.HttpStatusCode < 303);
        //}

        //public static IEnumerable<Link> GetLinks(this IApplicationModel context, Query filter)
        //{
        //    return context
        //            .SerializationSafe()
        //            .Links
        //            .Filter(filter)
        //            .Sort(filter.Sort);
        //}

        //public static IQueryable<Link> Filter(this IQueryable<Link> queryable, Query filter)
        //{
        //    var filtered = queryable;

        //    if (!filter.Category.IsEmpty())
        //        filtered = filtered.Where(l => l.Rel == filter.Category);

        //    return filtered;
        //}

        //public static IQueryable<Link> Sort(this IQueryable<Link> queryable, WebSort sort)
        //{
        //    switch (sort)
        //    {
        //        case WebSort.Alphabetical:
        //            return queryable.OrderByDescending(l => l.Name);
        //        default:
        //            return queryable.OrderBy(l => l.Priority);
        //    }
        //}
    }
}