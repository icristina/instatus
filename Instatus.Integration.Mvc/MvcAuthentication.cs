using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Instatus.Integration.Mvc
{
    public static class MvcAuthentication
    {
        public static void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            var httpContext = HttpContext.Current;
            var authCookie = FormsAuthentication.GetAuthCookie(userName, createPersistentCookie);
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var roles = Roles.GetRolesForUser(userName);

            httpContext.Response.Cookies.Add(authCookie);
            httpContext.User = new GenericPrincipal(new FormsIdentity(authTicket), roles);
        }
    }
}
