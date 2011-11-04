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
using Instatus.Queries;

namespace Instatus.Data
{   
    public class BaseDataContext : DbContext
    {       
        public static BaseDataContext Instance() {
            return DependencyResolver.Current.GetService<BaseDataContext>();
        }

        public static bool LoggingEnabled { get; set; }

        public IDbSet<Application> Applications { get; set; }
        public IDbSet<Page> Pages { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<Message> Messages { get; set; }
        public IDbSet<Domain> Domains { get; set; }
        public IDbSet<Link> Links { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<Activity> Activities { get; set; }
        public IDbSet<Offer> Offers { get; set; }
        public IDbSet<Coupon> Coupons { get; set; }
        public IDbSet<Source> Sources { get; set; }
        public IDbSet<Organization> Organizations { get; set; }
        public IDbSet<Price> Prices { get; set; }
        public IDbSet<Schedule> Schedules { get; set; }
        public IDbSet<Profile> Profiles { get; set; }
        public IDbSet<Subscription> Subscriptions { get; set; }
        public IDbSet<Restriction> Restrictions { get; set; }
        public IDbSet<List> Lists { get; set; }
        public IDbSet<Selection> Selections { get; set; }
        public IDbSet<Taxonomy> Taxonomies { get; set; }
        public IDbSet<Log> Logs { get; set; }
        public IDbSet<Card> Cards { get; set; }
        public IDbSet<Phrase> Phrases { get; set; }

        public User GetUser(IPrincipal user)
        {
            return Users
                    .Include(u => u.Credentials)
                    .FirstOrDefault(u => u.EmailAddress == user.Identity.Name);
        }

        public User GetCurrentUser()
        {
            return GetUser(HttpContext.Current.User);
        }

        public User GetUser(WebProvider webProvider, string uri)
        {
            var provider = webProvider.ToString();
            return Users
                    .Include(u => u.Credentials)
                    .FirstOrDefault(u => u.Credentials.Any(c => c.Provider == provider && c.Uri == uri));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            
            var page = modelBuilder.Entity<Page>();
            
            page.HasMany(p => p.Related).WithMany(p => p.ParentPages).Map(m => m.ToTable("RelatedPages"));
            page.HasOptional(p => p.Application).WithMany(a => a.Pages);
            page.HasMany(p => p.Activities).WithOptional(a => a.Page);

            var activity = modelBuilder.Entity<Activity>();

            activity.HasMany(a => a.Activities).WithMany(a => a.ParentActivities).Map(m => m.ToTable("RelatedActivities"));
            
            var award = modelBuilder.Entity<Award>();

            award.HasOptional(a => a.Achievement).WithMany(a => a.Awards);
            
            var message = modelBuilder.Entity<Message>();

            message.HasMany(m => m.Replies).WithOptional(m => m.InReplyTo);

            var user = modelBuilder.Entity<User>();

            user.HasMany(u => u.Friends).WithMany(u => u.Relationships).Map(c => c.ToTable("Friends"));
            user.HasMany(u => u.Activities).WithOptional(a => a.User);
        }

        public void SetStatus<T>(int id, WebStatus status) where T : class, IUserGeneratedContent
        {
            Set<T>().Find(id).Status = status.ToString();
        }

        public void MarkAsSpam<T>(int id) where T : class, IUserGeneratedContent
        {
            SetStatus<T>(id, WebStatus.Spam);
        }

        public void MarkAsPublished<T>(int id) where T : class, IUserGeneratedContent
        {
            SetStatus<T>(id, WebStatus.Published);
        }

        public void LogChange(object resource, string action, string uri = null)
        {
            if (!LoggingEnabled)
                return;
            
            var now = DateTime.Now;
            var user = GetCurrentUser();
            var description = resource is string ? resource : string.Format("{0} {1}", ObjectContext.GetObjectType(resource.GetType()).Name, resource.GetKey());
            var message = string.Format("{0} {1} for {2} at {3}",
                user.FullName,
                action,
                description,
                now);

            Logs.Add(new Log()
            {
                Verb = WebVerb.Change.ToString(),
                Uri = uri,
                User = user,
                Message = message,
                CreatedTime = now
            });
        }

        public void LogChange(object resource, string propertyName, object originalValue, object newValue, string uri = null)
        {
            var action = string.Format("changed {0} from {1} to {2}", propertyName, originalValue, newValue);
            LogChange(resource, action, uri);
        }

        public IQueryable<Tag> GetTags(string taxonomyName)
        {
            return Tags.Where(t => t.Taxonomy.Name == taxonomyName).OrderBy(t => t.Name);
        }

        public RecordResult<Post> Post(string message)
        {
            return Post(new WebEntry()
            {
                Description = message
            });
        }

        public RecordResult<Post> Post(WebEntry entry)
        {
            var user = GetCurrentUser();

            if (!user.Can(WebVerb.Post))
                return RecordResult<Post>.Failed;

            var post = new Post()
            {
                Description = entry.Description,
                User = user
            };

            Pages.Add(post);

            return new RecordResult<Post>(post);
        }

        public RecordResult<Activity> Like<T>(int id) where T : class, IUserGeneratedContent
        {
            var user = GetCurrentUser();

            if (!user.Can(WebVerb.Like))
                return RecordResult<Activity>.Failed;

            var userId = user.Id;
            var like = WebVerb.Like.ToString();
            var content = Set<T>().Find(id);

            // Failure:
            // Like own content
            // Duplicate Like
            if (content.User.Id == userId || content.Activities.Any(a => a.Verb == like && a.UserId == userId))
                return RecordResult<Activity>.Failed;

            var activity = new Activity()
            {
                Verb = like,
                User = user
            };

            content.Activities.Add(activity);

            return new RecordResult<Activity>(activity);
        }

        public RecordResult<Comment> Comment<T>(int id, string body) where T : class, IUserGeneratedContent
        {
            var user = GetCurrentUser();

            if (!user.Can(WebVerb.Comment))
                return RecordResult<Comment>.Failed;

            var content = Set<T>().Find(id);

            var comment = new Comment()
            {
                PageId = id,
                User = user,
                Body = body
            };

            Messages.Add(comment);

            return new RecordResult<Comment>(comment);
        }

        public Offer GetLatestOffer()
        {
            var now = DateTime.Now;
            var published = WebStatus.Published.ToString();
            
            return Pages.OfType<Offer>()
                    .Where(o => o.Dates.Any(d => d.StartTime <= now && (!d.EndTime.HasValue || d.EndTime >= now)) && o.Status == published)
                    .FirstOrDefault();
        }

        public T GetLatestAwarded<T>(string achievementSlug) where T : Page
        {
            var award = Activities
                        .OfType<Award>()
                        .OrderByDescending(a => a.CreatedTime)
                        .FirstOrDefault();
           
            return award.IsEmpty() ? null : (T)award.Page;
        }

        public IOrderedQueryable<Activity> GetActivities(WebExpression filter, WebStatus? status = WebStatus.Published)
        {
            return this
                    .DisableProxiesAndLazyLoading()
                    .Activities
                    .Expand(filter.Expand)
                    .FilterActivities(filter, status)
                    .SearchActivities(filter)
                    .SortActivities(filter.Sort);            
        }

        public IOrderedQueryable<T> GetActivities<T>(WebExpression filter, WebStatus? status = WebStatus.Published) where T : Activity
        {
            return this
                    .DisableProxiesAndLazyLoading()
                    .Activities
                    .Expand(filter.Expand)
                    .FilterActivities(filter, status)
                    .SearchActivities(filter)
                    .OfType<T>()
                    .SortActivities(filter.Sort);
        }

        public IOrderedQueryable<Activity> GetActivities(
            string user = null,
            string parent = null,
            string[] expand = null,
            DateTime? startDate = null)
        {
            return GetActivities(new WebExpression()
            {
                User = user,
                Parent = parent,
                Expand = expand,
                StartDate = startDate
            });
        }

        public static string[] DefaultPageExpansions = new string[] { "Restrictions" };

        public T GetPage<T>(string slug, string[] customExpansions = null) where T : Page {
            return this.DisableProxiesAndLazyLoading()
                    .Pages
                    .Expand(DefaultPageExpansions)
                    .Expand(customExpansions)
                    .Where(p => p.Slug == slug)
                    .OfType<T>()
                    .FirstOrDefault();
        }

        public IOrderedQueryable<T> GetPages<T>(WebExpression filter, WebStatus? status = WebStatus.Published) where T : Page
        {
            return this
                    .DisableProxiesAndLazyLoading()
                    .Pages
                    .Expand(filter.Expand)
                    .FilterPages(filter, status)
                    .SearchPages(filter)
                    .OfType<T>()
                    .SortPages(filter.Sort);
        }

        public IOrderedQueryable<Page> GetPages(WebExpression filter, WebStatus? status = WebStatus.Published)
        {
            return this
                    .DisableProxiesAndLazyLoading()
                    .Pages
                    .Expand(filter.Expand)
                    .FilterPages(filter, status)
                    .SearchPages(filter)
                    .OfKind(filter.Kind)
                    .SortPages(filter.Sort);
        }

        public IOrderedQueryable<Page> GetPages(
            WebKind kind = WebKind.Page,
            string[] uri = null,
            WebSort sort = WebSort.Recency,
            string tag = null,
            string user = null,
            string[] expand = null,
            string parent = null,
            DateTime? startDate = null,
            string term = null,
            string locale = null,
            WebMode mode = WebMode.List)
        {
            return GetPages(new WebExpression()
            {
                Kind = kind,
                Uri = uri,
                Sort = sort,
                Tag = tag,
                User = user,
                Expand = expand,
                Parent = parent,
                StartDate = startDate,
                Term = term,
                Locale = locale,
                Mode = mode
            });
        }

        public IQueryable<User> GetUsers(WebExpression filter, WebStatus? status = WebStatus.Published)
        {
            return this
                    .DisableProxiesAndLazyLoading()
                    .Users
                    .Expand(filter.Expand)
                    .FilterUsers(filter, status)
                    .SortUsers(filter.Sort);
        }

        public IQueryable<User> GetUsers(
            int pageSize = 200, 
            int pageIndex = 0, 
            string[] uri = null, 
            WebSort sort = WebSort.Alphabetical,
            string[] expand = null)
        {           
            return GetUsers(new WebQuery()
            {
                Uri = uri,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Sort = sort,
                Expand = expand
            });
        }

        public void LoadArticles(Stream stream)
        {
            var articles = Generator.LoadXml<List<Article>>(stream);

            foreach (var loaded in articles)
            {
                var article = GetPage<Article>(loaded.Slug);

                if (article == null)
                {
                    Pages.Add(loaded);
                }
                else
                {
                    article.Name = loaded.Name;
                    article.Document = loaded.Document;

                    if (loaded.Priority != 0)
                        article.Priority = article.Priority;
                }
            }

            SaveChanges();
        }
    }

    internal static class QueryExtensions
    {
        public static IQueryable<Activity> FilterActivities(this IQueryable<Activity> queryable, WebExpression filter, WebStatus? webStatus)
        {
            var filtered = queryable;

            if (webStatus.HasValue)
            {
                var status = webStatus.ToString();
                filtered = filtered.Where(p => p.Status == status);
            }

            int userId;

            if (!filter.User.IsEmpty() && int.TryParse(filter.User, out userId))
                filtered = filtered.Where(p => p.UserId == userId);

            int parentId;

            if (!filter.Parent.IsEmpty() && int.TryParse(filter.Parent, out parentId))
                filtered = filtered.Where(a => a.PageId == parentId);

            if (filter.StartDate.HasValue || filter.IsDateView)
            {
                DateTime startTime = filter.StartDate ?? StartDate(filter.Mode);
                DateTime? endTime = startTime.EndDate(filter.Mode);

                filter.StartDate = startTime;

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

        public static IQueryable<T> SearchActivities<T>(this IQueryable<T> queryable, WebExpression filter) where T : Activity
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
                case WebKind.CaseStudy:
                    return pages.OfType<CaseStudy>();
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

        public static IQueryable<T> SearchPages<T>(this IQueryable<T> queryable, WebExpression filter) where T : Page
        {
            if(!filter.Filter.IsEmpty()) {
                if(filter.Filter.StartsWith("status:")) {
                    var value = filter.Filter.SubstringAfter("status:");
                    queryable = queryable.Where(p => p.Status == value);
                }
            }

            return queryable;
        }

        public static IQueryable<T> FilterPages<T>(this IQueryable<T> queryable, WebExpression filter, WebStatus? webStatus) where T : Page
        {
            var filtered = queryable;

            if(webStatus.HasValue) {
                var status = webStatus.ToString();
                filtered = filtered.Where(p => p.Status == status);
            }

            if (!filter.Tag.IsEmpty())
                filtered = filtered.Where(p => p.Tags.Any(t => t.Name == filter.Tag));

            if (!filter.Uri.IsEmpty())
                filtered = filtered.Where(p => p.Sources.Any(s => filter.Uri.Contains(s.Uri)));

            int parentId;

            if (!filter.Parent.IsEmpty() && int.TryParse(filter.Parent, out parentId))
                filtered = filtered.Where(p => p.ParentPages.Any(r => r.Id == parentId));

            if (!filter.Term.IsEmpty())
                filtered = filtered.Where(p => p.Name.StartsWith(filter.Term));

            if (!filter.Locale.IsEmpty())
                filtered = filtered.Where(p => p.Locale == filter.Locale);

            int userId;

            if (!filter.User.IsEmpty() && int.TryParse(filter.User, out userId))
                filtered = filtered.Where(p => p.UserId == userId);

            if (filter.StartDate.HasValue || filter.IsDateView) {
                DateTime startTime = filter.StartDate ?? StartDate(filter.Mode);
                DateTime? endTime = startTime.EndDate(filter.Mode);

                filter.StartDate = startTime;

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

        public static IQueryable<User> FilterUsers(this IQueryable<User> queryable, WebExpression filter, WebStatus? webStatus)
        {
            var filtered = queryable;

            if (webStatus.HasValue)
            {
                var status = webStatus.ToString();
                filtered = filtered.Where(u => u.Status == status);
            }

            if (!filter.Uri.IsEmpty())
                filtered = filtered.Where(u => u.Credentials.Any(c => filter.Uri.Contains(c.Uri)));

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
    }
}