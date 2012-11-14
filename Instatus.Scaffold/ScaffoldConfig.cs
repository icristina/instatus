using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections.Specialized;
using Instatus.Core;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Web.WebPages;
using Instatus.Integration.Mvc;

namespace Instatus.Scaffold
{
    public static class ScaffoldConfig
    {
        public static void RegisterBlogRoutes(RouteCollection routes, string controllerName = "Blog")
        {
            routes.MapRoute(
                name: WellKnown.RouteName.Blog,
                url: "blog/{pageIndex}/{tag}",
                defaults: new { controller = controllerName, action = "Index" },
                constraints: new { pageIndex = WellKnown.RegularExpression.Number }
            );

            routes.MapRoute(
                name: WellKnown.RouteName.BlogPost,
                url: "blog/{slug}",
                defaults: new { controller = controllerName, action = "Details" }
            );
        }
    }
}
