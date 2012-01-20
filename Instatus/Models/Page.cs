using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus.Areas.Microsite;
using Instatus.Web;
using System.Dynamic;
using Instatus.Data;

namespace Instatus.Models
{   
    [KnownType(typeof(Application))]
    [KnownType(typeof(Article))]
    [KnownType(typeof(Brand))]
    [KnownType(typeof(CaseStudy))]
    [KnownType(typeof(Catalog))]
    [KnownType(typeof(Event))]
    [KnownType(typeof(Group))]
    [KnownType(typeof(Post))]
    [KnownType(typeof(Place))]
    [KnownType(typeof(Product))]
    [KnownType(typeof(Offer))]
    [KnownType(typeof(Organization))]
    [KnownType(typeof(Job))]
    [KnownType(typeof(Achievement))]
    public class Page : IUserGeneratedContent, IExtensionPoint, INavigableContent
    {
        public int Id { get; set; }
        public string Locale { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Permissions { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? PublishedTime { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string Data { get; set; }

        public virtual Application Application { get; set; }
        public int? ApplicationId { get; set; }
        
        public virtual User User { get; set; }
        public int? UserId { get; set; }

        public Card Card { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Message> Replies { get; set; }
        public virtual ICollection<Link> Links { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Selection> Selections { get; set; }
        public virtual ICollection<Restriction> Restrictions { get; set; }
        public virtual ICollection<Source> Sources { get; set; }
        public virtual ICollection<Log> Logs { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Page> Parents { get; set; }

        [NotMapped]
        public WebDocument Document
        {
            get
            {
                return Fields["Document"] as WebDocument;
            }
            set
            {
                Fields["Document"] = value;
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public dynamic Extensions { get; set; }

        [NotMapped]
        public Dictionary<WebVerb, WebInsight> Insights { get; private set; }

        [NotMapped]
        [IgnoreDataMember]
        public Dictionary<string, object> Fields { get; private set; }

        private Type[] knownTypes = new Type[] { typeof(WebDocument) };

        [IgnoreDataMember]
        public byte[] Payload
        {
            get
            {
                return Fields.Serialize(knownTypes);
            }
            set
            {
                Fields = value.Deserialize<Dictionary<string, object>>(knownTypes);
            }
        }

        public override string ToString()
        {
            if (Document != null && !Document.Title.IsEmpty())
                return Document.Title;

            return Name ?? Description;
        }

        public Page()
        {
            Fields = new Dictionary<string, object>();
            
            CreatedTime = DateTime.UtcNow;
            UpdatedTime = DateTime.UtcNow;
            PublishedTime = DateTime.UtcNow;
            Extensions = new ExpandoObject();
            Status = WebStatus.Published.ToString();
            Document = new WebDocument();
            Card = new Card();
            Insights = new Dictionary<WebVerb, WebInsight>();
        }

        public Page(string name) : this() {
            Name = name;
            Slug = name.ToSlug();
        }

        public virtual WebEntry ToWebEntry()
        {
            return ContentProviderQueries.SelectWebEntry(this);
        }

        public SiteMapNode ToSiteMapNode(SiteMapProvider sitemap)
        {
            var routeData = new { slug = Slug };
            var routeName = RouteCollectionExtensions.NavigableRouteName;
            var virtualPath = RouteTable.Routes.GetVirtualPath(null, routeName, new RouteValueDictionary(routeData)).VirtualPath;
            return new SiteMapNode(sitemap, Slug, virtualPath, Name);
        }

        public RestrictionResultCollection ValidateRestrictions(BaseDataContext context = null, User user = null, Activity trigger = null, bool saveChanges = true)
        {
            var restrictionContext = new RestrictionContext()
            {
                DataContext = context ?? BaseDataContext.Instance(),
                Page = this,
                Trigger = trigger
            };

            restrictionContext.User = user ?? restrictionContext.DataContext.GetCurrentUser();

            var restrictionEvaluators = DependencyResolver.Current.GetServices<IRestrictionEvaluator>();
            var restrictionResults = new RestrictionResultCollection(restrictionContext);

            foreach (var restriction in Restrictions.OrderBy(r => r.Priority))
            {
                var restrictionEvaluator = restrictionEvaluators.First(s => s.Name == restriction.Name);

                if (restrictionEvaluator is IPayload)
                {
                    ((IPayload)restrictionEvaluator).Payload = restriction.Payload;
                }

                var restrictionResult = restrictionEvaluator.Evaluate(restrictionContext);

                restrictionResults.Add(restrictionResult);

                if (restrictionResult.Continue == false)
                {
                    break;
                }
            }

            if (restrictionResults.IsValid && saveChanges)
            {
                restrictionResults.SaveActivities();
            }

            return restrictionResults;
        }

        public Page ProcessIncludes(BaseDataContext dataContext = null, RouteData routeData = null)
        {
            if (Document == null)
                Document = new WebDocument();

            var scope = new List<string>() { GetType().Name };

            if(routeData != null) {
                scope.Add(routeData.ActionName());
                scope.Add(routeData.AreaName());
                scope.Add(routeData.ToUniqueId());
            }

            // include WebParts that are unscoped or scope matches routeData parameter
            Document.Parts.AddRange(WebPart.Catalog.Where(p => p.Scope.IsEmpty() || scope.Contains(p.Scope)));

            if (dataContext == null)
                dataContext = BaseDataContext.Instance();

            foreach (var include in Document.Parts.OfType<WebInclude>().ToList())
            {
                Document.Parts.Remove(include);
                
                var page = dataContext.GetPage<Page>(include.Uri);

                if (page.Document == null)
                    break;

                if (page.Document.Body != null)
                    Document.Parts.Add(new WebSection()
                    {
                        Heading = page.Document.Title,
                        Abstract = page.Document.Description,
                        Body = page.Document.Body
                    });

                if (page.Document.Parts != null)
                    Document.Parts.Append(page.Document.Parts);
            }

            return this;
        }

        public static Page Instance(WebKind kind)
        {
            switch (kind)
            {
                case WebKind.Application:
                    return new Application();
                case WebKind.Achievement:
                    return new Achievement();
                case WebKind.Article:
                    return new Article();
                case WebKind.Brand:
                    return new Brand();
                case WebKind.CaseStudy:
                    return new CaseStudy();
                case WebKind.Catalog:
                    return new Catalog();
                case WebKind.Event:
                    return new Event();
                case WebKind.Group:
                    return new Group();
                case WebKind.Job:
                    return new Job();
                case WebKind.Post:
                    return new Post();
                case WebKind.Place:
                    return new Place();
                case WebKind.Product:
                    return new Product();
                case WebKind.Offer:
                    return new Offer();
                case WebKind.Organization:
                    return new Organization();
                default:
                    return new Page();
            }
        }
    }
}