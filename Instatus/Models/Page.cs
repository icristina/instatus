﻿using System;
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
    [KnownType(typeof(Directory))]
    [KnownType(typeof(Event))]
    [KnownType(typeof(Group))]
    [KnownType(typeof(Post))]
    [KnownType(typeof(Place))]
    [KnownType(typeof(Product))]
    [KnownType(typeof(Offer))]
    [KnownType(typeof(Organization))]
    [KnownType(typeof(Job))]
    [KnownType(typeof(Achievement))]
    public class Page : IUserGeneratedContent
    {
        public int Id { get; set; }
        public string Locale { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Permissions { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? PublishedTime { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }

        public virtual Application Application { get; set; }
        public int? ApplicationId { get; set; }
        
        public virtual User User { get; set; }
        public int? UserId { get; set; }

        public virtual Photo Picture { get; set; }
        public int? PictureId { get; set; }

        public virtual Card Card { get; set; }
        public int? CardId { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Link> Links { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Selection> Selections { get; set; }
        public virtual ICollection<Restriction> Restrictions { get; set; }
        public virtual ICollection<Source> Sources { get; set; }
        public virtual ICollection<Log> Logs { get; set; }

        public virtual ICollection<Page> Related { get; set; }
        public virtual ICollection<Page> ParentPages { get; set; }

        [NotMapped]
        public WebDocument Document { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public dynamic Extensions { get; set; }

        [IgnoreDataMember]
        [ScaffoldColumn(false)]
        public byte[] Data
        {
            get
            {
                return Document.Serialize();
            }
            set
            {
                Document = value.Deserialize<WebDocument>();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public Page()
        {
            CreatedTime = DateTime.Now;
            UpdatedTime = DateTime.Now;
            PublishedTime = DateTime.Now;
            Extensions = new ExpandoObject();
            Status = WebStatus.Published.ToString();
            Document = new WebDocument();
        }

        public Page(string name) : this() {
            Name = name;
        }

        public SiteMapNode ToSiteMapNode(SiteMapProvider sitemap)
        {
            var routeData = new { slug = Slug };
            var routeName = MicrositeAreaRegistration.PageRouteName;
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
                    ((IPayload)restrictionEvaluator).Data = restriction.Data;
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

        public Page ProcessIncludes(BaseDataContext dataContext = null)
        {
            if (Document == null && Document.Parts == null)
                return this;

            if (dataContext == null)
                dataContext = BaseDataContext.Instance();

            // can be fixed size array
            Document.Parts = new List<WebPart>().Append(Document.Parts);

            foreach (var include in Document.Parts.OfType<WebInclude>().ToList())
            {
                Document.Parts.Remove(include);
                
                var page = dataContext.GetPage<Page>(include.Uri);

                if (page.Document == null)
                    break;

                if (page.Document.Body != null)
                    Document.Parts.Add(new WebSection()
                    {
                        Body = page.Document.Body
                    });

                if (page.Document.Parts != null)
                    Document.Parts.Append(page.Document.Parts);
            }

            return this;
        }
    }
}