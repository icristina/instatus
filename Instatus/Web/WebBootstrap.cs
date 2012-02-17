using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Configuration;
using System.Web.Security;
using System.Collections.Specialized;
using System.Web.Routing;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.ComponentModel;

namespace Instatus.Web
{
    public static class WebBootstrap
    {
        public static void Core()
        {
            // app settings
            ConfigurationManager.AppSettings["webpages:Version"] = "1.0.0.0";
            ConfigurationManager.AppSettings["ClientValidationEnabled"] = "false";
            ConfigurationManager.AppSettings["UnobtrusiveJavaScriptEnabled"] = "false";            
            
            // model metadata provider
            ModelMetadataProviders.Current = new ExtendedModelMetadataProvider();

            // dependency injection
            DependencyResolver.SetResolver(new MefDependencyResolver());
            DynamicModuleUtility.RegisterModule(typeof(MefDependencyResolverModule));

            // virtual path providers
            HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedVirtualPathProvider());
            HostingEnvironment.RegisterVirtualPathProvider(new PackageVirtualPathProvider());
        }

        public static void Auth()
        {
            Membership.Providers.Clear();
            Membership.Providers.Add(new DbMembershipProvider());

            FormsAuthentication.EnableFormsAuthentication(new NameValueCollection() {
                    { "loginUrl", "Auth/Account/LogOn" }
            });

            Roles.Enabled = true;
            Roles.Providers.Clear();
            Roles.Providers.Add(new DbRoleProvider());
        }

        public static void Routes()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("{location}/{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("Scripts/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("Content/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("favicon.ico");
        }

        public static void Rewriting()
        {
            DynamicModuleUtility.RegisterModule(typeof(RedirectModule));
        }

        public static void ErrorHandling()
        {
            GlobalFilters.Filters.Add(new LogErrorAttribute());
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }

        public static void LegacyUserAgents()
        {
            GlobalFilters.Filters.Add(new UserAgentAttribute("IE", 7, true));
        }

        // http://blogs.msdn.com/b/marcinon/archive/2011/08/16/optimizing-mvc-view-lookup-performance.aspx
        public static void ViewLocation()
        {
            ViewEngines.Engines.Clear();

            var razorViewEngine = new RazorViewEngine();
            razorViewEngine.ViewLocationCache = new TwoLevelViewCache(razorViewEngine.ViewLocationCache);

            ViewEngines.Engines.Add(razorViewEngine);
        }

        public static bool LoggingEnabled { get; set; }
    }
}