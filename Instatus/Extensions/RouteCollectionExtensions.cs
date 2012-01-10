﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Instatus.Areas.Microsite;

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

        public const string HomeRouteName = "HomeRoute";
        public const string NavigableRouteName = "NavigableRoute";
        public const string HomeSlug = "home";

        public static void MapHomeRoute(this RouteCollection routes, string controllerName = "Page", string actionName = "Details", string areaName = null)
        {
            routes.MapRoute(
                HomeRouteName,
                "",
                new
                {
                    controller = controllerName,
                    action = actionName,
                    area = areaName
                }
            );
        }

        public static void MapNavigableRoute(this RouteCollection routes, string prefix, string controllerName = "Page", string actionName = "Details", string areaName = null)
        {
            routes.MapRouteLowercase(
                NavigableRouteName, 
                prefix + "/{slug}",
                new
                {
                    controller = controllerName,
                    action = actionName,
                    slug = UrlParameter.Optional,
                    area = areaName
                }
            );
        }
    }
}