using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Data;

namespace Instatus.Entities
{
    public class InMemoryApplicationModel : IApplicationModel
    {
        static InMemoryApplicationModel() {
            applications = new InMemorySet<Application>();
            credentials = new InMemorySet<Credential>();
            pages = new InMemorySet<Page>();
            users = new InMemorySet<User>();
            tags = new InMemorySet<Tag>();
            taxonomies = new InMemorySet<Taxonomy>();
            activities = new InMemorySet<Activity>();
            logs = new InMemorySet<Log>();
            subscriptions = new InMemorySet<Subscription>();
            redirects = new InMemorySet<Redirect>();
            lists = new InMemorySet<List>();
            selections = new InMemorySet<Selection>();
            domains = new InMemorySet<Domain>();
            comments = new InMemorySet<Comment>();
            associations = new InMemorySet<Association>();
            phrases = new InMemorySet<Phrase>();
        }

        public IDbSet<T> Set<T>() where T : class
        {
            return this.FirstMemberOfType<IDbSet<T>>();
        }

        public void SaveChanges()
        {

        }
        
        private static IDbSet<Application> applications;

        public IDbSet<Application> Applications
        {
            get
            {
                return applications;
            }
        }

        private static IDbSet<Credential> credentials;

        public IDbSet<Credential> Credentials
        {
            get
            {
                return credentials;
            }
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

        private static IDbSet<Tag> tags;

        public IDbSet<Tag> Tags
        {
            get
            {
                return tags;
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

        private static IDbSet<Activity> activities;

        public IDbSet<Activity> Activities
        {
            get
            {
                return activities;
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

        private static IDbSet<Subscription> subscriptions;

        public IDbSet<Subscription> Subscriptions
        {
            get
            {
                return subscriptions;
            }
        }

        private static IDbSet<Redirect> redirects;

        public IDbSet<Redirect> Redirects
        {
            get
            {
                return redirects;
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

        private static IDbSet<Domain> domains;

        public IDbSet<Domain> Domains
        {
            get
            {
                return domains;
            }
        }

        private static IDbSet<Comment> comments;

        public IDbSet<Comment> Comments
        {
            get
            {
                return comments;
            }
        }

        private static IDbSet<Association> associations;

        public IDbSet<Association> Associations
        {
            get
            {
                return associations;
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
    }
}
