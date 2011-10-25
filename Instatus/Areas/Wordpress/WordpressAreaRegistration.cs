﻿using System.Web.Mvc;
using Instatus.Web;

namespace Instatus.Areas.Wordpress
{
    public class WordpressAreaRegistration : AreaRegistration
    {       
        public override string AreaName
        {
            get
            {
                return "Wordpress";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            ValueProviderFactories.Factories.Add(new XmlRpcValueProviderFactory());
            
            context.MapRoute(
                "Wordpress",
                "xmlrpc.php",
                new { controller = "WordpressApi" },
                new { action = new XmlRpcRouteConstraint() }
            );  
        }
    }
}
