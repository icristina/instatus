using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Instatus.Web;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Instatus.Startup), "PreApplicationStart")]
[assembly: WebActivator.PostApplicationStartMethod(typeof(Instatus.Startup), "PostApplicationStart")]

namespace Instatus
{
    public static class Startup
    {
        public static Action<ContainerBuilder> Build { get; set; }
        
        private static void PreApplicationStart()
        {
            WebBootstrap.Core();
            WebBootstrap.Routes();
            WebBootstrap.Auth();
            WebBootstrap.ErrorHandling();
            WebBootstrap.ViewLocation();
        }

        private static void PostApplicationStart()
        {                      
            // autofac dependency resolver
            if (Build != null)
            {
                var builder = new ContainerBuilder();

                Build(builder);

                DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
            }

            // default route
            RouteTable.Routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }
            );

            // bundles, cannot be added during PreApplicationStart
            BundleTable.Bundles.AddScriptsBundle("bootstrap", "jquery-1.7.2.js", "jquery.validate.js", "bootstrap.js");
            BundleTable.Bundles.AddStylesBundle("bootstrap", "bootstrap.css", "bootstrap-responsive.css"); 
        }
    }
}
