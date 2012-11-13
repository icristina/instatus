using Instatus.Integration.Mvc;
using Instatus.Integration.Server;
using Instatus.Scaffold;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Instatus.Sample
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            BaseMvcConfig.RegisterIgnoreRoutes(routes);
            BaseMvcConfig.RegisterImageHandlerRoute(routes);
            ScaffoldConfig.RegisterBlogRoutes(routes);
            BaseMvcConfig.RegisterPageRoute(routes);
            BaseMvcConfig.RegisterDefaultRoute(routes);
        }
    }
}