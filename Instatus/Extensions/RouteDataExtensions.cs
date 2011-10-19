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

        public static WebAction WebAction(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.ActionName.AsEnum<WebAction>();
        }
    }
}