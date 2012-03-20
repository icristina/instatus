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
        static InMemoryApplicationContext()
        {
            pages = new InMemorySet<Page>();
            users = new InMemorySet<User>();
            roles = new InMemorySet<Role>();
            preferences = new InMemorySet<Preference>();
            messages = new InMemorySet<Message>();
            domains = new InMemorySet<Domain>();
            links = new InMemorySet<Link>();
            tags = new InMemorySet<Tag>();
            activities = new InMemorySet<Activity>();
            sources = new InMemorySet<Source>();
            prices = new InMemorySet<Price>();
            schedules = new InMemorySet<Schedule>();
            subscriptions = new InMemorySet<Subscription>();
            restrictions = new InMemorySet<Restriction>();
            lists = new InMemorySet<List>();
            selections = new InMemorySet<Selection>();
            taxonomies = new InMemorySet<Taxonomy>();
            logs = new InMemorySet<Log>();
            phrases = new InMemorySet<Phrase>();
        }

        private static IDbSet<Page> pages;

        public IDbSet<Page> Pages
        {
            get
            {
                return pages;
            }
        }

        private static IDbSet<User> users;

        public IDbSet<User> Users
        {
            get
            {
                return users;
            }
        }

        private static IDbSet<Role> roles;

        public IDbSet<Role> Roles
        {
            get
            {
                return roles;
            }
        }

        private static IDbSet<Preference> preferences;

        public IDbSet<Preference> Preferences
        {
            get
            {
                return preferences;
            }
        }

        private static IDbSet<Message> messages;

        public IDbSet<Message> Messages
        {
            get
            {
                return messages;
            }
        }

        private static IDbSet<Domain> domains;

        public IDbSet<Domain> Domains
        {
            get
            {
                return domains;
            }
        }

        private static IDbSet<Link> links;

        public IDbSet<Link> Links
        {
            get
            {
                return links;
            }
        }

        private static IDbSet<Tag> tags;

        public IDbSet<Tag> Tags
        {
            get
            {
                return tags;
            }
        }

        private static IDbSet<Activity> activities;

        public IDbSet<Activity> Activities
        {
            get
            {
                return activities;
            }
        }

        private static IDbSet<Source> sources;

        public IDbSet<Source> Sources
        {
            get
            {
                return sources;
            }
        }

        private static IDbSet<Price> prices;

        public IDbSet<Price> Prices
        {
            get
            {
                return prices;
            }
        }

        private static IDbSet<Schedule> schedules;

        public IDbSet<Schedule> Schedules
        {
            get
            {
                return schedules;
            }
        }

        private static IDbSet<Subscription> subscriptions;

        public IDbSet<Subscription> Subscriptions
        {
            get
            {
                return subscriptions;
            }
        }

        private static IDbSet<Restriction> restrictions;

        public IDbSet<Restriction> Restrictions
        {
            get
            {
                return restrictions;
            }
        }

        private static IDbSet<List> lists;

        public IDbSet<List> Lists
        {
            get
            {
                return lists;
            }
        }

        private static IDbSet<Selection> selections;

        public IDbSet<Selection> Selections
        {
            get
            {
                return selections;
            }
        }

        private static IDbSet<Taxonomy> taxonomies;

        public IDbSet<Taxonomy> Taxonomies
        {
            get
            {
                return taxonomies;
            }
        }

        private static IDbSet<Log> logs;

        public IDbSet<Log> Logs
        {
            get
            {
                return logs;
            }
        }

        private static IDbSet<Phrase> phrases;

        public IDbSet<Phrase> Phrases
        {
            get
            {
                return phrases;
            }
        }

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
    }
}