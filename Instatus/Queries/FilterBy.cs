using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Instatus.Entities;
using Instatus.Models;

namespace Instatus
{
    public class FilterBy
    {
        public static Expression<Func<Page, bool>> Current
        {
            get
            {
                var now = DateTime.UtcNow;
                return page => (page.Schedule.StartTime.HasValue && page.Schedule.StartTime <= now) && (page.Schedule.EndTime.HasValue && page.Schedule.EndTime >= now);
            }
        }

        public static Expression<Func<Activity, bool>> Verb(Verb verb)
        {
            var verbName = verb.ToString();
            return activity => activity.Verb == verbName;
        }

        public static Expression<Func<Page, bool>> Kind(Kind kind)
        {
            var kindName = kind.ToString();
            return page => page.Kind == kindName;
        }

        public static Expression<Func<User, bool>> Me
        {
            get 
            {
                var identity = HttpContext.Current.User.Identity;

                if (!identity.IsAuthenticated)
                    return user => false;

                return UserName(identity.Name);
            }
        }

        public static Expression<Func<User, bool>> UserName(string userName)
        {
            if (userName.IsEmpty())
                return user => false;

            var accountId = userName.SubstringAfter(":");

            if (userName.StartsWith("facebook"))
                return user => user.FacebookAccount == accountId;

            if (userName.StartsWith("twitter"))
                return user => user.TwitterAccount == accountId;

            if (userName.Contains('@'))
                return user => user.EmailAddress == userName;

            return user => user.UserName == userName;
        }

        public static Expression<Func<Activity, bool>> Friends(IEnumerable<string> friends)
        {
            return activity => friends.Contains(activity.User.Identity.Key);
        }
    }
}