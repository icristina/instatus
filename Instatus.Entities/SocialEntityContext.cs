using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class SocialEntityContext : DbContext
    {
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
    }
}
