using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Instatus.Data;

namespace Instatus.Models
{
    public interface IApplicationContext : IDbContext, IDisposable
    {
        IDbSet<Page> Pages { get; }
        IDbSet<User> Users { get; }
        IDbSet<Role> Roles { get; }
        IDbSet<Preference> Preferences { get; }
        IDbSet<Message> Messages { get; }
        IDbSet<Domain> Domains { get; }
        IDbSet<Link> Links { get; }
        IDbSet<Tag> Tags { get; }
        IDbSet<Activity> Activities { get; }
        IDbSet<Source> Sources { get; }
        IDbSet<Price> Prices { get; }
        IDbSet<Schedule> Schedules { get; }
        IDbSet<Subscription> Subscriptions { get; }
        IDbSet<Restriction> Restrictions { get; }
        IDbSet<List> Lists { get; }
        IDbSet<Selection> Selections { get; }
        IDbSet<Taxonomy> Taxonomies { get; }
        IDbSet<Log> Logs { get; }
        IDbSet<Phrase> Phrases { get; }
    }
}