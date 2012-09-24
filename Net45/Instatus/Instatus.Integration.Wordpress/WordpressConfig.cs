using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Instatus.Integration.Wordpress
{
    public class WordpressConfig
    {
        // http://cookcomputing.com/blog/archives/Implementing%20an%20xml-rpc-service-with-asp-net-mvc
        public static void RegisterRoutes(RouteCollection routes)
        {
            var dataTokens = new RouteValueDictionary(new
            {
                Namespaces = new string[] { "Instatus.Integration.Wordpress" }
            });
            
            routes.Add(new Route("xmlrpc.php", null, null, dataTokens, new WordpressRouteHandler()));
        }
    }
}
