﻿using System;
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

[assembly: WebActivator.PreApplicationStartMethod(typeof(Instatus.Web.Startup), "PreApplicationStart")]
[assembly: WebActivator.PostApplicationStartMethod(typeof(Instatus.Web.Startup), "PostApplicationStart")]

namespace Instatus.Web
{
    public static class Startup
    {
        public static Action<ContainerBuilder> Build { get; set; }
        
        private static void PreApplicationStart()
        {
            // app settings
            ConfigurationManager.AppSettings["webpages:Version"] = "1.0.0.0";

            // client validation
            HtmlHelper.ClientValidationEnabled = false;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = false;

            // script bundles
            BundleTable.Bundles.AddScripts("jquery", "jquery-1.7.2.min.js", "jquery.validate.min.js");       
        }

        private static void PostApplicationStart()
        {                      
            // dependency resolver
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
        }
    }
}
