using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Instatus.Web;
using System.Web.Security;

namespace Instatus
{
    public static class PrincipalExtensions
    {
        public static bool IsInRole(this IPrincipal principal, WebRole role)
        {
            return principal.IsInRole(role.ToString());
        }

        // http://abadjimarinov.net/blog/2010/01/24/RenewUserInTheSameRequestInAspdotNET.xhtml
        public static void RefreshFromFormsCookie(this IPrincipal principal)
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null && !authTicket.Expired)
                {
                    FormsAuthenticationTicket newAuthTicket = authTicket;

                    if (FormsAuthentication.SlidingExpiration)
                    {
                        newAuthTicket = FormsAuthentication.RenewTicketIfOld(authTicket);
                    }

                    string userData = newAuthTicket.UserData;
                    string[] roles = userData.Split(',');

                    HttpContext.Current.User = new GenericPrincipal(new FormsIdentity(newAuthTicket), roles);
                }
            }
        }
    }
}