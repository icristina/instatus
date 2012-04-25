using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Data;

namespace Instatus.Entities
{
    public interface IApplicationModel : IDbContext
    {
        IDbSet<Application> Applications { get; }
        IDbSet<Credential> Credentials { get; }
        IDbSet<Page> Pages { get; }
        IDbSet<User> Users { get; }
        IDbSet<Tag> Tags { get; }
        IDbSet<Taxonomy> Taxonomies { get; }
        IDbSet<Activity> Activities { get; }
        IDbSet<Log> Logs { get; }
        IDbSet<Subscription> Subscriptions { get; }
        IDbSet<Redirect> Redirects { get; }
        IDbSet<List> Lists { get; }
        IDbSet<Selection> Selections { get; }
        IDbSet<Domain> Domains { get; }
        IDbSet<Comment> Comments { get; }
        IDbSet<Association> Associations { get; }
        IDbSet<Phrase> Phrases { get; }
    }
}
