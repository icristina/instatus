using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Data;

namespace Instatus.Entities
{
    public class DbApplicationModel : DbContext, IApplicationModel
    {
        public DbApplicationModel(string connectionName)
            : base(connectionName)
        {

        }
        
        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public new void SaveChanges()
        {
            foreach (var deletedEntity in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted && e.Entity is Page))
            {
                var page = (Page)deletedEntity.Entity;

                this.ClearCollection(page, p => p.Parents);
            }
            
            base.SaveChanges();
        }
        
        public IDbSet<Application> Applications { get; set; }
        public IDbSet<Credential> Credentials { get; set; }
        public IDbSet<Page> Pages { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<Taxonomy> Taxonomies { get; set; }
        public IDbSet<Activity> Activities { get; set; }
        public IDbSet<Log> Logs { get; set; }
        public IDbSet<Subscription> Subscriptions { get; set; }
        public IDbSet<Redirect> Redirects { get; set; }
        public IDbSet<List> Lists { get; set; }
        public IDbSet<Selection> Selections { get; set; }
        public IDbSet<Domain> Domains { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<Association> Associations { get; set; }
        public IDbSet<Phrase> Phrases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // association, cascade delete on only one end of relationship
            modelBuilder.Entity<Association>().HasRequired(a => a.Parent).WithMany(a => a.Children).HasForeignKey(a => a.ParentId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Association>().HasRequired(a => a.Child).WithMany(a => a.Parents).HasForeignKey(a => a.ChildId).WillCascadeOnDelete(false);
        
            // complex types, negates use of System.ComponentModel.DataAnnotations.Schema in core
            modelBuilder.ComplexType<Source>();
            modelBuilder.ComplexType<Identity>();
            modelBuilder.ComplexType<Schedule>();
            modelBuilder.ComplexType<Segment>();
            modelBuilder.ComplexType<Location>();
            modelBuilder.ComplexType<Card>();
            modelBuilder.ComplexType<Availability>();

            // not mapped
            modelBuilder.Entity<Page>().Ignore(p => p.Document);
            modelBuilder.Entity<Page>().Ignore(p => p.Fields);
            modelBuilder.Entity<User>().Ignore(u => u.FullName);
        }
    }
}
