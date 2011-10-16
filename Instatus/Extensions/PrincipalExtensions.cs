using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Instatus.Web;

namespace Instatus
{
    public static class PrincipalExtensions
    {
        public static bool IsInRole(this IPrincipal principal, WebRole role)
        {
            return principal.IsInRole(role.ToString());
        }
    }
}