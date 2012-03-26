using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Instatus.Web;

namespace Instatus.Models
{
    public class DbApplicationContext : DbContext, IApplicationContext
    {
        public DbApplicationContext(string connectionName)
            : base(connectionName)
        {

        }
        
        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public IDbSet<Page> Pages
        {
            get
            {
                return Set<Page>(); // no public setter
            }
        }

        public IDbSet<User> Users
        {
            get
            {
                return Set<User>();
            }
        }

        public IDbSet<Role> Roles
        {
            get
            {
                return Set<Role>();
            }
        }

        public IDbSet<Preference> Preferences
        {
            get
            {
                return Set<Preference>();
            }
        }

        public IDbSet<Message> Messages
        {
            get
            {
                return Set<Message>();
            }
        }

        public IDbSet<Domain> Domains
        {
            get
            {
                return Set<Domain>();
            }
        }

        public IDbSet<Link> Links
        {
            get
            {
                return Set<Link>();
            }
        }

        public IDbSet<Tag> Tags
        {
            get
            {
                return Set<Tag>();
            }
        }

        public IDbSet<Activity> Activities
        {
            get
            {
                return Set<Activity>();
            }
        }

        public IDbSet<Source> Sources
        {
            get
            {
                return Set<Source>();
            }
        }

        public IDbSet<Price> Prices
        {
            get
            {
                return Set<Price>();
            }
        }

        public IDbSet<Schedule> Schedules
        {
            get
            {
                return Set<Schedule>();
            }
        }

        public IDbSet<Subscription> Subscriptions
        {
            get
            {
                return Set<Subscription>();
            }
        }

        public IDbSet<Restriction> Restrictions
        {
            get
            {
                return Set<Restriction>();
            }
        }

        public IDbSet<List> Lists
        {
            get
            {
                return Set<List>();
            }
        }

        public IDbSet<Selection> Selections
        {
            get
            {
                return Set<Selection>();
            }
        }

        public IDbSet<Taxonomy> Taxonomies
        {
            get
            {
                return Set<Taxonomy>();
            }
        }

        public IDbSet<Log> Logs
        {
            get
            {
                return Set<Log>();
            }
        }

        public IDbSet<Phrase> Phrases
        {
            get
            {
                return Set<Phrase>();
            }
        }

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
    }
}