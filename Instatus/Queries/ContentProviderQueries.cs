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
        public static IEnumerable<T> GetPages<T>(this IContentProvider contentProvider, WebQuery query = null, bool cache = false, string expand = null, string category = null) where T : Page
        {
            query = query ?? new WebQuery();
            query.Category = category ?? query.Category;
            query.Kind = typeof(T).Name.AsEnum<WebKind>();

            if (!expand.IsEmpty())
                query.Expand = expand.ToList().ToArray();

            if (cache)
            {
                return HttpRuntime.Cache.Value(() => contentProvider.GetPages(query).Cast<T>().ToList()); // cache = true, currently returns list only
            }
            else
            {
                return contentProvider.GetPages(query).Cast<T>();
            }
        }

        public static T GetPage<T>(this IContentProvider contentProvider, string slug, WebSet set = null) where T : Page
        {
            return contentProvider.GetPage(slug, set) as T;
        }
    }
}