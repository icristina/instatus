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
        public IDbSet<Entry> Entries { get; set; }
        public IDbSet<Place> Places { get; set; }
        public IDbSet<Post> Posts { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<Address>();
            modelBuilder.ComplexType<Point>();
            
            base.OnModelCreating(modelBuilder);
        }

        public InstatusDb(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
    }
}
