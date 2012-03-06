using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using System.Data.Entity;
using Instatus;
using Instatus.Web;
using Instatus.Data;

namespace Instatus
{
    public static class UserQueries
    {
        public static bool Can(this User user, WebVerb verb)
        {
            return !(user == null || user.IsBanned());
        }
        
        public static bool IsBanned(this User user)
        {
            return user.Status.AsEnum<WebStatus>() == WebStatus.Banned;
        }
        
        public static bool IsSelf(this User user)
        {
            var identity = HttpContext.Current.User.Identity;
            return identity.IsAuthenticated && user.EmailAddress == identity.Name;
        }
        
        public static int Checkins(this User user)
        {
            return user.Activities.OfType<Checkin>().Count();
        }
        
        public static Checkin LastCheckin(this User user)
        {
            return user.Activities
                    .OfType<Checkin>()
                    .OrderByDescending(a => a.CreatedTime)
                    .FirstOrDefault();
        }
        
        public static Achievement Reputation(this User user)
        {
            var reputation = WebVerb.Reputation.ToString();
            
            return user.Activities
                    .OfType<Award>()
                    .Where(a => a.Verb == reputation)
                    .OrderByDescending(a => a.CreatedTime)
                    .FirstOrDefault().Page as Achievement;
        }

        private static Credential Credentials(this User user, WebProvider webProvider)
        {
            var provider = webProvider.ToString();
            return user.Credentials.FirstOrDefault(c => c.Provider == provider);
        }

        public static string UserId(this User user, WebProvider webProvider)
        {
            var credentials = user.Credentials(webProvider);
            return credentials != null ? credentials.Uri : string.Empty;
        }

        public static string AccessToken(this User user, WebProvider webProvider)
        {
            var credentials = user.Credentials(webProvider);
            return credentials != null ? credentials.AccessToken : string.Empty;
        }

        public static T GetLatestAwarded<T>(this IApplicationContext context, string achievementSlug) where T : Page
        {
            var award = context.Activities
                        .OfType<Award>()
                        .OrderByDescending(a => a.CreatedTime)
                        .FirstOrDefault();

            return award.IsEmpty() ? null : (T)award.Page;
        }

        public static IEnumerable<User> GetUsers(this IApplicationContext context, WebQuery query)
        {
            return context
                    .SerializationSafe()
                    .Users
                    .Expand(query.Expand)
                    .Filter(query)
                    .Sort(query.Sort);
        }

        public static IQueryable<User> Filter(this IQueryable<User> queryable, WebQuery query)
        {
            var filtered = queryable;

            var status = query.Status.ToString();

            filtered = filtered.Where(u => u.Status == status);

            if (!query.Uri.IsEmpty())
                filtered = filtered.Where(u => u.Credentials.Any(c => query.Uri.Contains(c.Uri)));

            return filtered;
        }

        public static IOrderedQueryable<User> Sort(this IQueryable<User> queryable, WebSort sort)
        {
            switch (sort)
            {
                case WebSort.Recency:
                    return queryable.OrderByDescending(u => u.CreatedTime);
                default:
                    return queryable.OrderBy(u => u.FullName);
            }
        }
    }
}