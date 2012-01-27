using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Web;
using Instatus.Data;
using System.Data.Entity;
using System.ServiceModel.Syndication;

namespace Instatus
{
    public static class ContentProviderQueries
    {
        public static IEnumerable<T> GetPages<T>(this IContentProvider contentProvider, WebQuery query = null, bool cache = false, string expand = null, string category = null) where T : Page
        {
            query = query ?? new WebQuery();
            query.Category = category ?? query.Category;
            query.Kind = typeof(T).Name.AsEnum<WebKind>();

            if(!expand.IsEmpty())
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

        public static Page GetPage(this IContentProvider contentProvider, string slug, string locale = null, string expand = null)
        {
            var webSet = new WebSet()
            {
                Locale = locale
            };

            if (!expand.IsEmpty())
                webSet.Expand = expand.ToList().ToArray();
            
            return contentProvider.GetPage(slug, webSet);
        }

        public static void AppendContent<T>(this IContentProvider contentProvider, WebView<T> webView, string slug, WebSet set = null)
        {
            var page = contentProvider.GetPage(slug, set);

            if (page != null)
            {
                webView.Name = page.Name;
                webView.Document = page.Document;

                if (webView.Document != null && !page.Description.IsEmpty() && webView.Document.Description.IsEmpty())
                {
                    webView.Document.Description = page.Description;
                }
            }
        }

        public static IEnumerable<SyndicationItem> AsSyndicationItems(this IEnumerable<Page> pages, string routeName = WebRoute.Post)
        {
            return pages.ToList().Select(s => s.ToSyndicationItem(routeName));
        }

        public static IEnumerable<WebEntry> AsWebEntries(this IEnumerable<Page> pages)
        {
            return pages.Select(SelectWebEntry);
        }

        public static IEnumerable<WebEntry> AsWebEntries(this IEnumerable<Place> places)
        {
            return places.Select(SelectWebGeospatialEntry);
        }

        internal static Func<Page, WebEntry> SelectWebEntry = new Func<Page, WebEntry>(p =>
        {
            return new WebEntry()
            {
                Title = p.Name,
                Description = p.Description,
                Picture = p.Picture
            };
        });

        internal static Func<Place, WebEntry> SelectWebGeospatialEntry = new Func<Place, WebGeospatialEntry>(p =>
        {
            return new WebGeospatialEntry()
            {
                Title = p.Name,
                Caption = p.Address.Locality,
                Description = p.Description,
                Picture = p.Picture,
                Latitude = p.Point.Latitude,
                Longitude = p.Point.Longitude
            };
        });
    }
}