using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Instatus.Integration.Facebook
{
    public static class FacebookPrincipalExtensions
    {
        public static string GetAccessToken(this IPrincipal principal)
        {
            var formsIdentity = principal.Identity as FormsIdentity;

            if (formsIdentity != null)
                return formsIdentity.Ticket.UserData;

            return null;
        }
    }
}
