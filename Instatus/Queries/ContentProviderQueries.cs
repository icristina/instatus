using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Web;
using Instatus.Data;
using System.Data.Entity;

namespace Instatus
{
    public static class ContentProviderQueries
    {
        public static IEnumerable<T> GetPages<T>(this IContentProvider contentProvider, WebQuery query, string category = null) where T : Page
        {
            query.Category = category ?? query.Category;
            query.Kind = typeof(T).Name.AsEnum<WebKind>();
            return contentProvider.GetPages(query).Cast<T>();
        }

        public static T GetPage<T>(this IContentProvider contentProvider, string slug, WebSet set = null) where T : Page
        {
            return contentProvider.GetPage(slug, set) as T;
        }
    }
}