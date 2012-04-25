using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using Instatus.Web;
using Instatus.Models;
using System.Security.Principal;
using Instatus.Data;
using Instatus;
using System.Data.Entity;
using Instatus.Entities;

namespace Instatus
{
    public static class MembershipQueries
    {
        public static User GetUser(this IApplicationModel context, IPrincipal user)
        {
            return GetUser(context, user.Identity.Name);
        }

        public static User GetUser(this IApplicationModel context, string userName)
        {
            if (userName.IsEmpty())
                return null;

            if (userName.Contains("@"))
            {
                userName = userName.ToLower(); // normalize email address
                return context.Users.FirstOrDefault(u => u.EmailAddress == userName);
            }

            var parts = userName.ToList(':');

            if (parts.Count != 3 || parts[0] != "urn")
                return null;

            var provider = parts[1].AsEnum<Provider>();
            var uri = parts[2];

            return GetUser(context, provider, uri);
        }

        public static User GetCurrentUser(this IApplicationModel context)
        {
            return GetUser(context, HttpContext.Current.User);
        }

        public static User GetUser(this IApplicationModel context, Provider webProvider, string uri)
        {
            var provider = webProvider.ToString();
            return context.Users.FirstOrDefault(u => u.Identity.Provider == provider && u.Identity.UserId == uri);
        }

        //public static IQueryable<User> GetUsers(this IApplicationModel context, WebRole webRole)
        //{
        //    var roleName = webRole.ToString();
        //    return context.Users.Where(u => u.Roles.Any(r => r.Name == roleName));
        //}

        //public static List<MailAddress> GetMailAddresses(this IApplicationModel context, WebRole webRole)
        //{
        //    return context.GetUsers(webRole)
        //            .ToList()
        //            .Select(u => new MailAddress(u.EmailAddress, u.FullName))
        //            .ToList();
        //}
    }
}