using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Instatus.Integration.Wordpress
{
    public class WordpressConfig
    {
        public const string XmlRpcUrl = "xmlrpc.php";
        
        // http://cookcomputing.com/blog/archives/Implementing%20an%20xml-rpc-service-with-asp-net-mvc
        public static void RegisterRoutes(RouteCollection routes)
        {
            var dataTokens = new RouteValueDictionary(new
            {
                Namespaces = new string[] { "Instatus.Integration.Wordpress" }
            });

            var constraints = new RouteValueDictionary(new
            {
                Controller = string.Empty
            });
            
            routes.Add(new Route(XmlRpcUrl, null, constraints, dataTokens, new WordpressRouteHandler()));
        }
    }
}
