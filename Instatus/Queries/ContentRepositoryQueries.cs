using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Web;
using Instatus.Data;
using System.Data.Entity;
using System.ServiceModel.Syndication;
using Instatus.Adapters;

namespace Instatus
{
    public static class ContentRepositoryQueries
    {
        public static IEnumerable<T> GetPages<T>(this IContentRepository content, WebQuery query = null, bool cache = false, string expand = null, string category = null, WebSort sort = WebSort.Recency, string cacheKey = null, int cacheDuration = WebCache.Duration) where T : Page
        {
            query = query ?? new WebQuery();
            query.Category = category ?? query.Category;
            query.Kind = typeof(T).Name.AsEnum<WebKind>();

            if (sort != WebSort.Recency)
                query.Sort = sort;

            if (!expand.IsEmpty())
                query.Expand = expand.ToList().ToArray();

            if (cache)
            {
                content.SerializationSafe();
                return WebCache.Value(() => content.GetPages(query).Cast<T>().ToList(), cacheKey, cacheDuration);
            }
            else
            {
                return content.GetPages(query).Cast<T>();
            }
        }

        public static T GetPage<T>(this IContentRepository content, string slug, string locale = null, string expand = null) where T : Page
        {
            var webSet = new WebSet()
            {
                Locale = locale,
                Kind = typeof(T).Name.AsEnum<WebKind>()
            };

            if (!expand.IsEmpty())
                webSet.Expand = expand.ToList().ToArray();
            
            return content.GetPage(slug, webSet) as T;
        }

        public static T ApplyAdapters<T>(this T contentItem, IContentRepository contentRepository, string hint = null) where T : IContentItem
        {
            if (contentItem == null)
                return contentItem;
            
            foreach(var contentAdapter in WebApp.GetServices<IContentAdapter>()) 
            {
                contentAdapter.Process(contentItem, contentRepository, hint);
            }

            return contentItem;
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