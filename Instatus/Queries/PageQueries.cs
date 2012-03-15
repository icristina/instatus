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
    public static class PageQueries
    {
        public static IEnumerable<T> GetPages<T>(this IPageContext content, WebQuery query = null, bool cache = false, string expand = null, string category = null, WebSort sort = WebSort.Recency, string cacheKey = null, int cacheDuration = WebCache.Duration) where T : Page
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

        public static T GetPage<T>(this IPageContext content, string slug, string locale = null, string expand = null) where T : Page
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

        public static T ApplyAdapters<T>(this T contentItem, string hint = null) where T : IContentItem
        {
            if (contentItem == null)
                return contentItem;
            
            foreach(var contentAdapter in WebApp.GetServices<IContentAdapter>()) 
            {
                contentAdapter.Process(contentItem, hint);
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

        public static IQueryable<Page> OfKind(this IQueryable<Page> pages, WebKind kind)
        {
            switch (kind)
            {
                case WebKind.Application:
                    return pages.OfType<Application>();
                case WebKind.Article:
                    return pages.OfType<Article>();
                case WebKind.Brand:
                    return pages.OfType<Brand>();
                case WebKind.CaseStudy:
                    return pages.OfType<CaseStudy>();
                case WebKind.Catalog:
                    return pages.OfType<Catalog>();
                case WebKind.Event:
                    return pages.OfType<Event>();
                case WebKind.Group:
                    return pages.OfType<Group>();
                case WebKind.Job:
                    return pages.OfType<Job>();
                case WebKind.Post:
                    return pages.OfType<Post>();
                case WebKind.Place:
                    return pages.OfType<Place>();
                case WebKind.Product:
                    return pages.OfType<Product>();
                case WebKind.Offer:
                    return pages.OfType<Offer>();
                case WebKind.Organization:
                    return pages.OfType<Organization>();
                case WebKind.Region:
                    return pages.OfType<Region>();
                case WebKind.News:
                    return pages.OfType<News>();
                case WebKind.Profile:
                    return pages.OfType<Profile>();
                case WebKind.Listing:
                    return pages.OfType<Listing>();
                default:
                    return pages;
            }
        }

        public static IQueryable<T> FilterBySet<T>(this IQueryable<T> pages, WebSet query) where T : Page
        {
            var status = query.Status.ToString();

            pages = pages.Where(p => p.Status == status);

            if (!query.Locale.IsEmpty())
                pages = pages.Where(p => p.Locale == query.Locale);

            return pages;
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> pages, WebQuery query) where T : Page
        {
            if (!query.Tag.IsEmpty())
                pages = pages.Where(p => p.Tags.Any(t => t.Name == query.Tag));

            if (!query.Category.IsEmpty())
                pages = pages.Where(p => p.Category == query.Category);

            if (!query.Uri.IsEmpty())
                pages = pages.Where(p => p.Sources.Any(s => query.Uri.Contains(s.Uri)));

            if (!query.Parent.IsEmpty())
            {
                if (query.Parent.IsNumeric())
                {
                    int parentId = query.Parent.AsInteger();
                    pages = pages.Where(p => p.Parents.Any(r => r.Id == parentId));
                }
                else
                {
                    pages = pages.Where(p => p.Parents.Any(r => r.Slug == query.Parent));
                }
            }

            if (!query.Ancestor.IsEmpty())
            {
                if (query.Ancestor.IsNumeric())
                {
                    int ancestorId = query.Ancestor.AsInteger();
                    pages = pages.Where(p => p.Parents.Any(r => r.Parents.Any(a => a.Id == ancestorId)));
                }
                else
                {
                    pages = pages.Where(p => p.Parents.Any(r => r.Parents.Any(a => a.Slug == query.Ancestor)));
                }
            }

            if (!query.Term.IsEmpty())
                pages = pages.Where(p => p.Name.StartsWith(query.Term));

            int userId;

            if (!query.User.IsEmpty() && int.TryParse(query.User, out userId))
                pages = pages.Where(p => p.UserId == userId);

            if (query.StartDate.HasValue || query.IsDateView)
            {
                DateTime startTime = query.StartDate ?? DateTimeExtensions.StartDate(query.Mode);
                DateTime? endTime = startTime.EndDate(query.Mode);

                query.StartDate = startTime;

                if (pages is IQueryable<Event>)
                {
                    var events = (IQueryable<Event>)pages;

                    if (endTime.HasValue)
                    {
                        events = events.Where(e => e.Dates.Any(s => s.StartTime >= startTime && s.StartTime < endTime.Value || (s.EndTime.HasValue && (s.EndTime.Value >= startTime && s.EndTime.Value < endTime.Value))));
                    }
                    else
                    {
                        events = events.Where(e => e.Dates.Any(s => s.StartTime >= startTime || (s.EndTime.HasValue && s.EndTime >= startTime)));
                    }

                    pages = (IQueryable<T>)events;
                }
                else
                {
                    pages = pages.Where(p => p.CreatedTime >= startTime);

                    if (endTime.HasValue)
                    {
                        pages = pages.Where(p => p.CreatedTime < endTime.Value);
                    }
                }
            }

            return pages;
        }

        public static IQueryable<Page> Sort(this IQueryable<Page> pages, WebSort sort)
        {
            var like = WebVerb.Like.ToString();

            switch (sort)
            {
                case WebSort.Priority:
                    return pages.OrderBy(p => p.Priority).ThenByDescending(p => p.CreatedTime);
                case WebSort.Alphabetical:
                    return pages.OrderBy(p => p.Name).ThenByDescending(p => p.CreatedTime);
                case WebSort.Likes:
                    return pages.OrderByDescending(p => p.Activities.Where(a => a.Verb == like).Count()).ThenByDescending(p => p.CreatedTime);
                case WebSort.Comments:
                    return pages.OrderByDescending(p => p.Replies.Count()).ThenByDescending(p => p.CreatedTime);
                default:
                    return pages.OrderByDescending(p => p.PublishedTime);
            }
        }
    }
}