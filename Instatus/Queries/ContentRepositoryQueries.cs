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
    public static class ContentRepositoryQueries
    {
        public static IEnumerable<T> GetPages<T>(this IContentRepository content, WebQuery query = null, bool cache = false, string expand = null, string category = null, WebSort sort = WebSort.Recency, string cacheKey = null, int cacheDuration = 60) where T : Page
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
                return HttpRuntime.Cache.Value(() => content.GetPages(query).Cast<T>().ToList(), cacheKey, cacheDuration); // cache = true, currently returns list only
            }
            else
            {
                return content.GetPages(query).Cast<T>();
            }
        }

        public static T GetPage<T>(this IContentRepository content, string slug, WebSet set = null) where T : Page
        {
            return content.GetPage(slug, set) as T;
        }

        public static Page GetPage(this IContentRepository content, string slug, string locale = null, string expand = null)
        {
            var webSet = new WebSet()
            {
                Locale = locale
            };

            if (!expand.IsEmpty())
                webSet.Expand = expand.ToList().ToArray();
            
            return content.GetPage(slug, webSet);
        }

        public static void AppendContent<T>(this IContentRepository content, WebView<T> webView, string slug, WebSet set = null)
        {
            var page = content.GetPage(slug, set);

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

        public static void AppendContent(this IContentRepository content, IContentItem contentItem, string slug = null, string expand = null)
        {
            Page page = !slug.IsEmpty() ? content.GetPage(slug, expand: expand) : contentItem as Page;
            
            if (page != null) {
                contentItem.Document = page.Document;

                if (page.Replies != null)
                {
                    contentItem.Feeds.Add(WebVerb.Notification, new DeferredWebFeed(page.Replies.OfType<Notification>()));
                    contentItem.Feeds.Add(WebVerb.Comment, new DeferredWebFeed(page.Replies.OfType<Comment>()));
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