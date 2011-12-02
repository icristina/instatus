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

        public static WebAction WebAction(this RouteData routeData)
        {
            return routeData.ActionName().AsEnum<WebAction>();
        }

        public static WebAction WebAction(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.ActionName.AsEnum<WebAction>();
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
    }
}