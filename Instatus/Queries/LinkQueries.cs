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
        public static IEnumerable<Link> WithContentType(this IEnumerable<Link> links, string contentType)
        {
            return links.Where(l => l.ContentType == contentType);
        }

        private static IEnumerable<Link> Filter(this IEnumerable<Link> links, string contentType, string rel)
        {
            links = links.WithContentType(contentType);

            if (rel.NonEmpty())
                links = links.Where(l => l.Rel == rel);

            return links;
        }

        public static string Uri(this IEnumerable<Link> links, string contentType = WebConstant.ContentType.Html, string rel = null)
        {
            return links.Filter(contentType, rel).Select(l => l.Uri).FirstOrDefault();
        }

        public static string Title(this IEnumerable<Link> links, string contentType = WebConstant.ContentType.Html, string rel = null)
        {
            return links.Filter(contentType, rel).Select(l => l.Title).FirstOrDefault();
        }
    }
}