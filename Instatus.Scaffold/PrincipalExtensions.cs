using Instatus.Scaffold.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Instatus.Scaffold
{
    public static class PrincipalExtensions
    {
        public static bool IsInRole(this IPrincipal principal, Role role)
        {
            return principal.IsInRole(role.ToString());
        }
    }
}