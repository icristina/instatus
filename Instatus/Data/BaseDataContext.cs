using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Instatus.Models;
using System.Data.Entity.Infrastructure;
using System.Security.Principal;
using Instatus.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using System.Web.Routing;
using System.IO;
using Instatus;
using System.Data.Objects;
using System.Net.Mail;
using System.Text;
using System.ServiceModel.Web;
using System.Linq.Expressions;

namespace Instatus.Data
{
    public class BaseDataContext : DbContext, IBaseDataContext, IContentProvider
    {
        public static BaseDataContext Instance()
        {
            return DependencyResolver.Current.GetService<BaseDataContext>();
        }

        public static IBaseDataContext BaseInstance()
        {
            return DependencyResolver.Current.GetService<IBaseDataContext>();
        }

        public IDbSet<Page> Pages { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<Message> Messages { get; set; }
        public IDbSet<Domain> Domains { get; set; }
        public IDbSet<Link> Links { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<Activity> Activities { get; set; }
        public IDbSet<Source> Sources { get; set; }
        public IDbSet<Price> Prices { get; set; }
        public IDbSet<Schedule> Schedules { get; set; }
        public IDbSet<Subscription> Subscriptions { get; set; }
        public IDbSet<Restriction> Restrictions { get; set; }
        public IDbSet<List> Lists { get; set; }
        public IDbSet<Selection> Selections { get; set; }
        public IDbSet<Taxonomy> Taxonomies { get; set; }
        public IDbSet<Log> Logs { get; set; }
        public IDbSet<Phrase> Phrases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            var page = modelBuilder.Entity<Page>();

            page.HasMany(p => p.Pages).WithMany(p => p.Parents).Map(m => m.ToTable("RelatedPages"));
            page.HasOptional(p => p.Application).WithMany(a => a.Content);

            var activity = modelBuilder.Entity<Activity>();

            activity.HasMany(a => a.Activities).WithMany(a => a.Parents).Map(m => m.ToTable("RelatedActivities"));

            var award = modelBuilder.Entity<Award>();

            award.HasOptional(a => a.Achievement).WithMany(a => a.Awards);

            var message = modelBuilder.Entity<Message>();

            message.HasMany(m => m.Replies).WithOptional(m => m.InReplyTo);

            var user = modelBuilder.Entity<User>();

            user.HasMany(u => u.Friends).WithMany(u => u.Relationships).Map(c => c.ToTable("Friends"));
            user.HasMany(u => u.Activities).WithOptional(a => a.User);
            user.HasOptional(u => u.Source);
            user.HasMany(u => u.Credentials).WithOptional(c => c.User);
        }

        public IOrderedQueryable<Activity> GetActivities(WebQuery query)
        {
            return this
                    .DisableProxiesAndLazyLoading()
                    .Activities
                    .Expand(query.Expand)
                    .FilterActivities(query)
                    .SearchActivities(query)
                    .SortActivities(query.Sort);
        }

        public IOrderedQueryable<T> GetActivities<T>(WebQuery query) where T : Activity
        {
            return this
                    .DisableProxiesAndLazyLoading()
                    .Activities
                    .Expand(query.Expand)
                    .FilterActivities(query)
                    .SearchActivities(query)
                    .OfType<T>()
                    .SortActivities(query.Sort);
        }

        public static string[] DefaultPageExpansions = new string[] { "Restrictions", "Links", "Tags.Taxonomy", "User" };

        public Page GetPage(string slug, WebSet set = null)
        {
            set = set ?? new WebSet();

            var page = this.DisableProxiesAndLazyLoading()
                    .Pages
                    .Expand(DefaultPageExpansions)
                    .FilterPagesBySet(set)
                    .Where(p => p.Slug == slug)
                    .FirstOrDefault();

            return ExpandNavigationProperties(page, set);
        }

        private T ExpandNavigationProperties<T>(T page, WebSet set) where T : Page
        {
            if (page != null)
            {
                foreach (var navigationProperty in set.Expand)
                {
                    Entry(page).Collection(navigationProperty).Load();
                }
            }

            return page;
        }

        public IEnumerable<Page> GetPages(WebQuery query)
        {
            return this
                    .DisableProxiesAndLazyLoading()
                    .Pages
                    .Expand(query.Expand)
                    .FilterPagesBySet(query)
                    .FilterPages(query)
                    .SearchPages(query)
                    .OfKind(query.Kind)
                    .SortPages(query.Sort);
        }

        public IOrderedQueryable<Link> GetLinks(WebQuery filter)
        {
            return this
                    .DisableProxiesAndLazyLoading()
                    .Links
                    .FilterLinks(filter)
                    .SortLinks(filter.Sort);
        }

        public IQueryable<User> GetUsers(WebQuery query)
        {
            return this
                    .DisableProxiesAndLazyLoading()
                    .Users
                    .Expand(query.Expand)
                    .FilterUsers(query)
                    .SortUsers(query.Sort);
        }
    }

    internal static class QueryExtensions
    {
        public static IQueryable<Activity> FilterActivities(this IQueryable<Activity> queryable, WebQuery query)
        {
            var filtered = queryable;

            var status = query.Status.ToString();

            filtered = filtered.Where(p => p.Status == status);

            int userId;

            if (!query.User.IsEmpty() && int.TryParse(query.User, out userId))
                filtered = filtered.Where(p => p.UserId == userId);

            int parentId;

            if (!query.Parent.IsEmpty() && int.TryParse(query.Parent, out parentId))
                filtered = filtered.Where(a => a.PageId == parentId);

            if (query.StartDate.HasValue || query.IsDateView)
            {
                DateTime startTime = query.StartDate ?? StartDate(query.Mode);
                DateTime? endTime = startTime.EndDate(query.Mode);

                query.StartDate = startTime;

                filtered = filtered.Where(p => p.CreatedTime >= startTime);

                if (endTime.HasValue)
                {
                    filtered = filtered.Where(p => p.CreatedTime < endTime.Value);
                }
            }

            return filtered;
        }

        public static DateTime StartDate(WebMode mode)
        {
            var today = DateTime.Today;
            
            switch (mode)
            {
                case WebMode.Year:
                    return new DateTime(today.Year, 1, 1);
                case WebMode.Month:
                    return new DateTime(today.Year, today.Month, 1);
                case WebMode.Week:
                    return today.AddDays((int)today.DayOfWeek);
                default:
                    return today;
            }
        }

        public static IOrderedQueryable<T> SortActivities<T>(this IQueryable<T> queryable, WebSort sort) where T : Activity
        {
            return queryable.OrderByDescending(a => a.CreatedTime);
        }

        public static IQueryable<T> SearchActivities<T>(this IQueryable<T> queryable, WebQuery filter) where T : Activity
        {
            if (!filter.Filter.IsEmpty())
            {
                if (filter.Filter.StartsWith("status:"))
                {
                    var value = filter.Filter.SubstringAfter("status:");
                    queryable = queryable.Where(p => p.Status == value);
                }
            }

            return queryable;
        }

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
                default:
                    return pages;
            }
        }

        public static DateTime? EndDate(this DateTime startTime, WebMode mode)
        {
            switch(mode) {
                case WebMode.Day:
                    return startTime.AddDays(1);
                case WebMode.Week:
                    return startTime.AddDays(7);
                case WebMode.Month:
                    return startTime.AddMonths(1);
                case WebMode.Year:
                    return startTime.AddYears(1);
                default:
                    return null;
            }
        }

        public static IQueryable<T> SearchPages<T>(this IQueryable<T> queryable, WebQuery filter) where T : Page
        {
            if(!filter.Filter.IsEmpty()) {
                if(filter.Filter.StartsWith("status:")) {
                    var value = filter.Filter.SubstringAfter("status:");
                    queryable = queryable.Where(p => p.Status == value);
                }
            }

            return queryable;
        }

        public static IQueryable<T> FilterPagesBySet<T>(this IQueryable<T> queryable, WebSet query) where T : Page
        {
            var filtered = queryable;

            var status = query.Status.ToString();

            filtered = filtered.Where(p => p.Status == status);

            if (!query.Locale.IsEmpty())
                filtered = filtered.Where(p => p.Locale == query.Locale);

            return filtered;
        }

        public static IQueryable<T> FilterPages<T>(this IQueryable<T> queryable, WebQuery query) where T : Page
        {
            var filtered = queryable;

            if (!query.Tag.IsEmpty())
                filtered = filtered.Where(p => p.Tags.Any(t => t.Name == query.Tag));

            if(!query.Category.IsEmpty())
                filtered = filtered.Where(p => p.Category == query.Category);

            if (!query.Uri.IsEmpty())
                filtered = filtered.Where(p => p.Sources.Any(s => query.Uri.Contains(s.Uri)));            

            if (!query.Parent.IsEmpty())
            {
                if (query.Parent.IsNumeric())
                {
                    int parentId = query.Parent.AsInteger();
                    filtered = filtered.Where(p => p.Parents.Any(r => r.Id == parentId));
                }
                else
                {
                    filtered = filtered.Where(p => p.Parents.Any(r => r.Slug == query.Parent));
                }
            }

            if (!query.Ancestor.IsEmpty())
            {
                if (query.Ancestor.IsNumeric())
                {
                    int ancestorId = query.Ancestor.AsInteger();
                    filtered = filtered.Where(p => p.Parents.Any(r => r.Parents.Any(a => a.Id == ancestorId)));
                }
                else
                {
                    filtered = filtered.Where(p => p.Parents.Any(r => r.Parents.Any(a => a.Slug == query.Ancestor)));
                }
            }     

            if (!query.Term.IsEmpty())
                filtered = filtered.Where(p => p.Name.StartsWith(query.Term));

            int userId;

            if (!query.User.IsEmpty() && int.TryParse(query.User, out userId))
                filtered = filtered.Where(p => p.UserId == userId);

            if (query.StartDate.HasValue || query.IsDateView) {
                DateTime startTime = query.StartDate ?? StartDate(query.Mode);
                DateTime? endTime = startTime.EndDate(query.Mode);

                query.StartDate = startTime;

                if (queryable is IQueryable<Event>)
                {
                    var events = (IQueryable<Event>)queryable;

                    if (endTime.HasValue)
                    {
                        events = events.Where(e => e.Dates.Any(s => s.StartTime >= startTime && s.StartTime < endTime.Value || (s.EndTime.HasValue && (s.EndTime.Value >= startTime && s.EndTime.Value < endTime.Value))));
                    }
                    else
                    {
                        events = events.Where(e => e.Dates.Any(s => s.StartTime >= startTime || (s.EndTime.HasValue && s.EndTime >= startTime)));
                    }
                    
                    filtered = (IQueryable<T>)events;
                }
                else
                {
                    filtered = filtered.Where(p => p.CreatedTime >= startTime);

                    if (endTime.HasValue)
                    {
                        filtered = filtered.Where(p => p.CreatedTime < endTime.Value);
                    }
                }
            }

            return filtered;
        }

        public static IOrderedQueryable<T> SortPages<T>(this IQueryable<T> queryable, WebSort sort) where T : Page
        {
            var like = WebVerb.Like.ToString();
            
            switch (sort)
            {
                case WebSort.Priority:
                    return queryable.OrderBy(p => p.Priority).ThenByDescending(p => p.CreatedTime);
                case WebSort.Alphabetical:
                    return queryable.OrderBy(p => p.Name).ThenByDescending(p => p.CreatedTime);
                case WebSort.Likes:
                    return queryable.OrderByDescending(p => p.Activities.Where(a => a.Verb == like).Count()).ThenByDescending(p => p.CreatedTime);
                case WebSort.Comments:
                    return queryable.OrderByDescending(p => p.Replies.Count()).ThenByDescending(p => p.CreatedTime);
                default:
                    return queryable.OrderByDescending(p => p.PublishedTime);
            }
        }

        public static IQueryable<User> FilterUsers(this IQueryable<User> queryable, WebQuery query)
        {
            var filtered = queryable;

            var status = query.Status.ToString();

            filtered = filtered.Where(u => u.Status == status);

            if (!query.Uri.IsEmpty())
                filtered = filtered.Where(u => u.Credentials.Any(c => query.Uri.Contains(c.Uri)));

            return filtered;
        }

        public static IOrderedQueryable<User> SortUsers(this IQueryable<User> queryable, WebSort sort)
        {
            switch (sort)
            {
                case WebSort.Recency:
                    return queryable.OrderByDescending(u => u.CreatedTime);
                default:
                    return queryable.OrderBy(u => u.FullName);
            }
        }

        public static IQueryable<Link> FilterLinks(this IQueryable<Link> queryable, WebQuery filter)
        {
            var filtered = queryable;

            if (!filter.Category.IsEmpty())
                filtered = filtered.Where(l => l.Rel == filter.Category);

            return filtered;
        }

        public static IOrderedQueryable<Link> SortLinks(this IQueryable<Link> queryable, WebSort sort)
        {
            switch (sort)
            {
                case WebSort.Alphabetical:
                    return queryable.OrderByDescending(l => l.Name);
                default:
                    return queryable.OrderBy(l => l.Priority);
            }
        }
    }
}