using Instatus.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Server
{
    public class InstatusDb : DbContext
    {
        public IDbSet<Campaign> Campaigns { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<Entry> Entries { get; set; }
        public IDbSet<Log> Logs { get; set; }
        public IDbSet<Page> Pages { get; set; }
        public IDbSet<Phrase> Phrases { get; set; }
        public IDbSet<Place> Places { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<Address>();
            modelBuilder.ComplexType<Point>();
            modelBuilder.ComplexType<Schedule>();
            
            base.OnModelCreating(modelBuilder);
        }

        public InstatusDb(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
    }
}
