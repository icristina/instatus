﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Instatus.Web;

namespace Instatus.Web
{
    // http://coderjournal.com/2008/03/force-mvc-route-url-lowercase/
    // http://lowercaseroutesmvc.codeplex.com
    public class LowercaseRoute : Route
    {
        public LowercaseRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        public LowercaseRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var virtualPathData = base.GetVirtualPath(requestContext, values);

            if (virtualPathData != null)
            {
                if (virtualPathData.VirtualPath.Contains('?'))
                {
                    var path = virtualPathData.VirtualPath.SubstringBefore("?");
                    var query = virtualPathData.VirtualPath.SubstringAfter("?");

                    virtualPathData.VirtualPath = string.Format("{0}?{1}", path.ToLowerInvariant(), query);
                }
                else
                {
                    virtualPathData.VirtualPath = virtualPathData.VirtualPath.ToLowerInvariant();
                }
            }

            return virtualPathData;
        }
    }
}

namespace Instatus
{
    public static class LowercaseRouteCollectionExtensions
    {
        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url)
        {
            return MapRouteLowercase(routes, name, url, null, null, null);
        }

        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, object defaults)
        {
            return MapRouteLowercase(routes, name, url, defaults, null, null);
        }

        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, string[] namespaces)
        {
            return MapRouteLowercase(routes, name, url, null, null, namespaces);
        }

        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return MapRouteLowercase(routes, name, url, defaults, constraints, null);
        }

        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return MapRouteLowercase(routes, name, url, defaults, null, namespaces);
        }

        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes[name] != null) // do not throw error on duplicate
                return null;

            var route = new LowercaseRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary(namespaces),
            };

            if (namespaces != null && namespaces.Length > 0)
            {
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return route;
        }
    }

    public static class LowercaseAreaRegistrationContextExtensions
    {
        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url)
        {
            return MapRouteLowercase(context, name, url, null, null, null);
        }

        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url, object defaults)
        {
            return MapRouteLowercase(context, name, url, defaults, null, null);
        }

        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url, string[] namespaces)
        {
            return MapRouteLowercase(context, name, url, null, null, namespaces);
        }

        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url, object defaults, object constraints)
        {
            return MapRouteLowercase(context, name, url, defaults, constraints, null);
        }

        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url, object defaults, string[] namespaces)
        {
            return MapRouteLowercase(context, name, url, defaults, null, namespaces);
        }

        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (context.Routes[name] != null) // do not throw error on duplicate
                return null;            
            
            if (namespaces == null && context.Namespaces != null)
            {
                namespaces = context.Namespaces.ToArray();
            }

            return context.Routes
                        .MapRouteLowercase(name, url, defaults, constraints, namespaces)
                        .AddAreaDataTokens(context.AreaName, namespaces);
        }

        public static Route AddAreaDataTokens(this Route route, string areaName, string[] namespaces = null)
        {
            if (areaName != null)
            {
                route.DataTokens["area"] = areaName;
                route.DataTokens["UseNamespaceFallback"] = (namespaces == null || namespaces.Length == 0);
            }
            return route;
        }
    }
}