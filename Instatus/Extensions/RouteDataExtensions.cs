using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Instatus.Web;
using System.Web.Mvc;

namespace Instatus
{
    public static class RouteDataExtensions
    {
        public static string ActionName(this RouteData routeData)
        {
            return routeData.GetRequiredString("action");
        }

        public static string AreaName(this RouteData routeData)
        {
            return routeData.DataTokens["area"].AsString();
        }

        public static string ControllerName(this RouteData routeData)
        {
            return routeData.GetRequiredString("controller");
        }

        public static int Id(this RouteData routeData)
        {
            return routeData.Values["id"].AsInteger();
        }

        public static string Slug(this RouteData routeData)
        {
            return routeData.Values["slug"].AsString();
        }

        public static string ToUniqueId(this RouteData routeData)
        {
            var id = routeData.Values["id"] ?? routeData.Values["slug"] ?? string.Empty;
            return id.ToString();
        }

        public static string ToClassName(this RouteData routeData)
        {
            return string.Format("{0} {1} {2}",
                    routeData.AreaName().ToCamelCase(),
                    routeData.ControllerName().ToCamelCase(),
                    routeData.ActionName().ToCamelCase())
                    .Trim()
                    .RemoveDoubleSpaces();
        }

        public static Dictionary<string, object> ToDataAttributeDictionary(this RouteData routeData) {
            return new Dictionary<string, object>()
                {
                    { "data-route-id", routeData.ToUniqueId().ToCamelCase() },
                    { "data-route-area", routeData.AreaName().ToCamelCase() },
                    { "data-route-controller", routeData.ControllerName().ToCamelCase() },
                    { "data-route-action", routeData.ActionName().ToCamelCase() }
                };
        }

        public static bool IsAncestorOrSelf(this SiteMapNode siteMapNode)
        {
            var url = HttpContext.Current.Request.Url.AbsolutePath;
            var routeData = HttpContext.Current.Request.RequestContext.RouteData;
            return siteMapNode.Url.Match(url) || siteMapNode.Key.Match(routeData.Slug()) || siteMapNode.Key.Match(routeData.ControllerName());
        }
    }
}