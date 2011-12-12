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
using System.Net.Mail;
using System.Text;
using System.ServiceModel.Web;
using System.Linq.Expressions;

namespace Instatus.Data
{
    public class BaseDataContext : DbContext, IBaseDataContext, IContentProvider
    {       
        public static BaseDataContext Instance() {
            return DependencyResolver.Current.GetService<BaseDataContext>();
        }

        public static IBaseDataContext BaseInstance()
        {
            return DependencyResolver.Current.GetService<IBaseDataContext>();
        }

        public static bool LoggingEnabled { get; set; }

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

        public User GetUser(IPrincipal user)
        {
            return GetUser(user.Identity.Name);
        }

        public User GetUser(string userName)
        {
            if (userName.IsEmpty())
                return null;
            
            if (userName.Contains("@"))
            {
                return Users
                    .Include(u => u.Credentials)
                    .Include(u => u.Roles)
                    .FirstOrDefault(u => u.EmailAddress == userName);
            }

            var parts = userName.ToList(':');

            if (parts.Count != 3 || parts[0] != "urn")
                return null;

            var provider = parts[1].AsEnum<WebProvider>();
            var uri = parts[2];

            return GetUser(provider, uri);
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
                    .Include(u => u.Roles)
                    .FirstOrDefault(u => u.Credentials.Any(c => c.Provider == provider && c.Uri == uri));
        }

        public IQueryable<User> GetUsers(WebRole webRole)
        {
            var roleName = webRole.ToString();
            return Users.Where(u => u.Roles.Any(r => r.Name == roleName));
        }

        public List<MailAddress> GetMailAddresses(WebRole webRole)
        {
            return GetUsers(webRole)
                    .ToList()
                    .Select(u => new MailAddress(u.EmailAddress, u.FullName))
                    .ToList();
        }

        public Application GetCurrentApplication()
        {
            return Pages.OfType<Application>().First();
        }

        public Brand GetCurrentBrand()
        {
            var brand = Pages
                    .Include(p => p.Links)
                    .OfType<Brand>()
                    .FirstOrDefault(); 
            
            if(brand != null)
                return brand;

            var application = GetCurrentApplication();

            return new Brand()
            {
                Name = application.Name,
                Picture = "~/Content/logo.png"
            };
        }

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

        public void LogError(Exception error)
        {
            if (!LoggingEnabled)
                return;

            var message = new StringBuilder();
            var uri = string.Empty;

            message.AppendSection("Message", error.Message);
            message.AppendSection("Stack Trace", error.StackTrace);

            var innerException = error.InnerException;

            if (innerException != null)
            {
                message.AppendSection("Inner Exception Message", innerException.Message);
                message.AppendSection("Inner Exception Stack Trace", innerException.StackTrace);
            }

            if (HttpContext.Current.Request != null)
            {
                uri = HttpContext.Current.Request.RawUrl;

                if (WebOperationContext.Current == null)
                {
                    message.AppendSection("Server Variables", HttpContext.Current.Request.ServerVariables["ALL_RAW"]);
                }                
            }

            Logs.Add(new Log()
            {
                Verb = WebVerb.Error.ToString(),
                Uri = uri,
                Message = message.ToString()
            });
        }

        public void LogChange(object resource, string action, string uri = null)
        {
            if (!LoggingEnabled)
                return;
            
            var now = DateTime.UtcNow;
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

        public Credential GetApplicationCredentials(WebProvider webProvider, string environment = null)
        {
            if (environment == null)
                environment = HttpContext.Current.ApplicationInstance.Setting<string>("Environment");

            var provider = webProvider.ToString();

            return Sources
                    .OfType<Credential>()
                    .FirstOrDefault(s => s.Provider == provider && s.Application != null && s.Environment == environment);
        }

        public Message GetApplicationMessage()
        {
            var published = WebStatus.Published.ToString();
            return Messages
                    .Where(m => m.Page is Application && m.Status == published)
                    .OrderByDescending(m => m.CreatedTime)
                    .FirstOrDefault();
        }

        public Offer GetLatestOffer()
        {
            var now = DateTime.UtcNow;
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

        public static string[] DefaultPageExpansions = new string[] { "Restrictions", "Links", "Tags.Taxonomy", "User" };

        public Page GetPage(string slug, string[] customExpansions = null)
        {
            return this.DisableProxiesAndLazyLoading()
                    .Pages
                    .Expand(DefaultPageExpansions)
                    .Expand(customExpansions)
                    .Where(p => p.Slug == slug)
                    .FirstOrDefault();
        }

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

        public IOrderedQueryable<Link> GetLinks(WebExpression filter)
        {
            return this
                    .DisableProxiesAndLazyLoading()
                    .Links
                    .FilterLinks(filter)
                    .SortLinks(filter.Sort);
        }

        public IOrderedQueryable<T> GetTemplates<T>(string locale = "") where T : Page 
        {
            return GetPages<T>(new WebQuery() {
                Locale = locale
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

        public void LoadPages(Stream stream)
        {
            var pages = Generator.LoadXml<List<Page>>(stream);

            foreach (var loaded in pages)
            {
                var page = GetPage<Page>(loaded.Slug);
                
                loaded.Tags = loaded.Tags.Synchronize(tag => Tags.FirstOrDefault(t => t.Name == tag.Name));

                if (page == null)
                {
                    Pages.Add(loaded);
                }
                else
                {
                    page.Name = loaded.Name;
                    page.Description = loaded.Description;
                    page.Document = loaded.Document;
                    page.Tags = loaded.Tags;
                    page.Category = loaded.Category;

                    if (!loaded.Links.IsEmpty())
                    {
                        this.MarkDeleted(page.Links);
                        page.Links = loaded.Links;
                    }   

                    if (loaded.Priority != 0)
                        page.Priority = page.Priority;

                    if (loaded is Application)
                    {
                        var application = (Application)loaded;
                        
                        application.Taxonomies = application.Taxonomies.Synchronize(tn => Taxonomies.FirstOrDefault(t => t.Name == tn.Name));

                        if (!application.Taxonomies.IsEmpty())
                        {
                            foreach (var taxonomy in application.Taxonomies)
                            {
                                taxonomy.Tags = taxonomy.Tags.Synchronize(tag => Tags.FirstOrDefault(t => t.Name == tag.Name));
                            }
                        }

                        Entry((Application)page).Replace(a => a.Taxonomies, application.Taxonomies);
                    }
                }

                SaveChanges();
            }
        }
    }

    internal static class DbEntryExtensions
    {
        public static void Replace<T, TNavigation>(this DbEntityEntry<T> entry, Expression<Func<T, ICollection<TNavigation>>> predicate, ICollection<TNavigation> navigation) where TNavigation : class where T : class
        {
            entry.Collection(predicate).Load();
            entry.Collection(predicate).CurrentValue.Clear();
            entry.Collection(predicate).CurrentValue = navigation;
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

            if(!filter.Category.IsEmpty())
                filtered = filtered.Where(p => p.Category == filter.Category);

            if (!filter.Uri.IsEmpty())
                filtered = filtered.Where(p => p.Sources.Any(s => filter.Uri.Contains(s.Uri)));

            int parentId;

            if (!filter.Parent.IsEmpty() && int.TryParse(filter.Parent, out parentId))
                filtered = filtered.Where(p => p.Parents.Any(r => r.Id == parentId));

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

        public static IQueryable<Link> FilterLinks(this IQueryable<Link> queryable, WebExpression filter)
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