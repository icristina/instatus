using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Instatus.Web;

namespace Instatus.Models
{
    public class DbApplicationContext : DbContext, IApplicationContext, IPageContext
    {
        public DbApplicationContext(string connectionName)
            : base(connectionName)
        {

        }
        
        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }  
        
        public IDbSet<Page> Pages { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<Preference> Preferences { get; set; }
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

        public new void SaveChanges()
        {
            // Entity<Page>().Relationship.WithCascadeOnDelete() could be used in model builder
            foreach (var deletedEntity in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
            {
                deletedEntity.State = EntityState.Modified;

                if (deletedEntity.Entity is Page)
                {
                    var page = (Page)deletedEntity.Entity;

                    page.Links.Clear();
                    page.Tags.Clear();
                    page.Parents.Clear();
                    page.Pages.Clear();
                }

                if (deletedEntity.Entity is Tag)
                {
                    var tag = (Tag)deletedEntity.Entity;

                    tag.Pages.Clear();
                }

                deletedEntity.State = EntityState.Deleted;
            }
            
            base.SaveChanges();
        }

        public static string[] DefaultPageExpansions = new string[] { "Restrictions", "Links", "Tags.Taxonomy", "User" };

        public Page GetPage(string slug, WebSet set = null)
        {
            set = set ?? new WebSet();

            var page = this.DisableProxiesAndLazyLoading()
                    .Pages
                    .Expand(DefaultPageExpansions)
                    .FilterBySet(set)
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
                    .FilterBySet(query)
                    .Filter(query)
                    .OfKind(query.Kind)
                    .Sort(query.Sort);
        }
    }
}