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

        public static string Uri(this IEnumerable<Link> links, WebContentType webContentType)
        {
            var link = links.WithContentType(webContentType).FirstOrDefault();
            return link != null ? link.Uri : string.Empty;
        }
    }
}