using Instatus.Integration.Mvc;
using Instatus.Integration.Server;
using Instatus.Integration.Wordpress;
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
            BaseMvcConfig.RegisterLocalizedContentPageRoute(routes);
            BaseMvcConfig.RegisterDefaultRoute(routes);
            WordpressConfig.RegisterRoutes(routes);
        }
    }
}