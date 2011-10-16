using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

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
    }
}