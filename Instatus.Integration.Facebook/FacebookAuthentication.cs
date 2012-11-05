using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Instatus.Integration.Facebook
{
    public static class FacebookAuthentication
    {
        // http://www.danharman.net/2011/07/07/storing-custom-data-in-forms-authentication-tickets/
        public static void SetAuthCookie(long userId, string accessToken)
        {
            var userName = userId.ToString();
            var userData = accessToken;
            var httpContext = HttpContext.Current;
            var authCookie = FormsAuthentication.GetAuthCookie(userName, false);
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);            
            var facebookAuthTicket = new FormsAuthenticationTicket(authTicket.Version, authTicket.Name, authTicket.IssueDate, authTicket.Expiration, authTicket.IsPersistent, userData, authTicket.CookiePath);
            var encryptedFacebookAuthTicket = FormsAuthentication.Encrypt(facebookAuthTicket);

            authCookie.Value = encryptedFacebookAuthTicket;

            httpContext.Response.Cookies.Add(authCookie);
            httpContext.User = new GenericPrincipal(new FormsIdentity(facebookAuthTicket), null);
        }
    }
}
