using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Instatus.Data;

namespace Instatus.Models
{
    public class InMemoryApplicationContext : IApplicationContext
    {
        public IDbSet<Page> Pages { get; private set; }
        public IDbSet<User> Users { get; private set; }
        public IDbSet<Role> Roles { get; private set; }
        public IDbSet<Preference> Preferences { get; private set; }
        public IDbSet<Message> Messages { get; private set; }
        public IDbSet<Domain> Domains { get; private set; }
        public IDbSet<Link> Links { get; private set; }
        public IDbSet<Tag> Tags { get; private set; }
        public IDbSet<Activity> Activities { get; private set; }
        public IDbSet<Source> Sources { get; private set; }
        public IDbSet<Price> Prices { get; private set; }
        public IDbSet<Schedule> Schedules { get; private set; }
        public IDbSet<Subscription> Subscriptions { get; private set; }
        public IDbSet<Restriction> Restrictions { get; private set; }
        public IDbSet<List> Lists { get; private set; }
        public IDbSet<Selection> Selections { get; private set; }
        public IDbSet<Taxonomy> Taxonomies { get; private set; }
        public IDbSet<Log> Logs { get; private set; }
        public IDbSet<Phrase> Phrases { get; private set; }

        public IDbSet<T> Set<T>() where T : class
        {
            return this.FirstMemberOfType<IDbSet<T>>();
        }

        public void SaveChanges()
        {

        }

        public void Dispose()
        {
            
        }

        public InMemoryApplicationContext()
        {
            Pages = new InMemorySet<Page>();
            Users = new InMemorySet<User>();
            Roles = new InMemorySet<Role>();
            Preferences = new InMemorySet<Preference>();
            Messages = new InMemorySet<Message>();
            Domains = new InMemorySet<Domain>();
            Links = new InMemorySet<Link>();
            Tags = new InMemorySet<Tag>();
            Activities = new InMemorySet<Activity>();
            Sources = new InMemorySet<Source>();
            Prices = new InMemorySet<Price>();
            Schedules = new InMemorySet<Schedule>();
            Subscriptions = new InMemorySet<Subscription>();
            Restrictions = new InMemorySet<Restriction>();
            Lists = new InMemorySet<List>();
            Selections = new InMemorySet<Selection>();
            Taxonomies = new InMemorySet<Taxonomy>();
            Logs = new InMemorySet<Log>();
            Phrases = new InMemorySet<Phrase>();
        }
    }
}