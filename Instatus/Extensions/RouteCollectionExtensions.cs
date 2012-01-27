using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Instatus.Areas.Microsite;
using Instatus.Web;

namespace Instatus
{
    public static class RouteCollectionExtensions
    {
        // http://johncoder.com/Post/register-single-area-in-aspnet-mvc2
        public static void RegisterArea<T>(this RouteCollection routes) where T : AreaRegistration
        {
            var area = Activator.CreateInstance<T>();
            var context = new AreaRegistrationContext(area.AreaName, routes);

            area.RegisterArea(context);
        }

        // http://haacked.com/archive/2009/11/04/routehandler-for-http-handlers.aspx
        public static void MapHttpHandler<THandler>(this RouteCollection routes,
          string url) where THandler : IHttpHandler, new()
        {
            routes.MapHttpHandler<THandler>(null, url, null, null);
        }

        public static void MapHttpHandler<THandler>(this RouteCollection routes,
            string name, string url, object defaults, object constraints)
            where THandler : IHttpHandler, new()
        {
            var route = new Route(url, new HttpHandlerRouteHandler<THandler>());
            route.Defaults = new RouteValueDictionary(defaults);
            route.Constraints = new RouteValueDictionary(constraints);
            routes.Add(name, route);
        }

        public class HttpHandlerRouteHandler<THandler>
            : IRouteHandler where THandler : IHttpHandler, new()
        {
            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new THandler();
            }
        }

        public static string GetVirtualPath(this RouteCollection routes, string routeName, object routeData)
        {
            return routes.GetVirtualPath(null, routeName, new RouteValueDictionary(routeData)).VirtualPath;
        }

        public static RouteCollection RemoveRoute(this RouteCollection routes, string routeName)
        {
            var route = routes[routeName];

            if(route != null)
                routes.Remove(route);

            return routes;
        }

        public static Route MapHomeRoute(this RouteCollection routes, string controllerName = "Home", string actionName = "Index", string areaName = null)
        {
            return routes
                .RemoveRoute(WebRoute.Home)
                .MapRouteLowercase(
                    WebRoute.Home,
                    "",
                    new
                    {
                        controller = controllerName,
                        action = actionName,
                        area = areaName
                    },
                    new
                    {
                        controller = controllerName,
                        action = actionName
                    })
                .AddAreaDataTokens(areaName);
        }

        public static Route MapPageRoute(this RouteCollection routes, string prefix, string controllerName = "Page", string actionName = "Details", string areaName = null)
        {
            return routes
                .RemoveRoute(WebRoute.Page)
                .MapRouteLowercase(
                    WebRoute.Page, 
                    prefix + "/{slug}",
                    new
                    {
                        controller = controllerName,
                        action = actionName,
                        slug = UrlParameter.Optional,
                        area = areaName
                    })
                .AddAreaDataTokens(areaName);
        }

        public static Route MapPostRoute(this RouteCollection routes, string controllerName = "Post", string actionName = "Details", string areaName = null)
        {
            return routes
                .RemoveRoute(WebRoute.Post)
                .MapRouteLowercase(
                    WebRoute.Post,
                    controllerName + "/" + actionName + "/{slug}",
                    new
                    {
                        controller = controllerName,
                        action = actionName,
                        area = areaName
                    })
                .AddAreaDataTokens(areaName);
        }

        public static Route MapPagesRoute(this RouteCollection routes, string prefix, string[] slugs, string controllerName = "Page", string actionName = "Details", string areaName = null)
        {
            return routes
                .RemoveRoute(WebRoute.Page)
                .MapRouteLowercase(
                    WebRoute.Page,
                    prefix + "/{slug}",
                    new
                    {
                        controller = controllerName,
                        action = actionName,
                        slug = UrlParameter.Optional,
                        area = areaName
                    },
                    new
                    {
                        slug = string.Join("|", slugs)
                    })
                .AddAreaDataTokens(areaName);
        }

        public static Route MapDefaultRoute(this RouteCollection routes, string controllerName = "Home", string actionName = "Index", string areaName = null, string[] excludeControllerNames = null)
        {
            return routes
                .RemoveRoute(WebRoute.Default)
                .MapRouteLowercase(
                    WebRoute.Default,
                    "{controller}/{action}/{slug}",
                    new
                    {
                        controller = controllerName,
                        action = actionName,
                        slug = UrlParameter.Optional,
                        area = areaName
                    },
                    new
                    {
                        controller = new ExcludeConstraint(excludeControllerNames)
                    })
                .AddAreaDataTokens(areaName);
        }

        public static Route MapSingleActionRoute(this RouteCollection routes, string controllerName, string actionName, string areaName = null, string routeName = null)
        {
            return routes.MapRouteLowercase(
                routeName ?? string.Format("{0}-{1}", controllerName, actionName),
                string.Format("{0}/{1}/{{slug}}", controllerName, actionName),
                new
                {
                    controller = controllerName,
                    action = actionName,
                    slug = UrlParameter.Optional,
                    area = areaName
                },
                new
                {
                    controller = controllerName,
                    action = actionName
                }
            ).AddAreaDataTokens(areaName);
        }
    }
}