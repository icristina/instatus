using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using System.Data.Entity;
using Instatus;
using Instatus.Web;
using Instatus.Data;

namespace Instatus.Queries
{
    public static class UserQueries
    {
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
    }
}