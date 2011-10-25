using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition.Hosting;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Web.Routing;
using System.Configuration;
using System.Web.Security;
using System.Collections.Specialized;
using System.Web.Hosting;

namespace Instatus.Web
{
    public static class PreApplicationStartCode
    {
        private static bool started = false;
        
        public static void Start()
        {
            if (started)
            {
                return;
            }

            started = true;

            // model metadata provider
            ModelMetadataProviders.Current = new ExtendedModelMetadataProvider();

            // dependency injection
            DependencyResolver.SetResolver(new MefDependencyResolver());

            // modules
            DynamicModuleUtility.RegisterModule(typeof(ConfigurationModule));
            DynamicModuleUtility.RegisterModule(typeof(PermanentRedirectModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpTraceModule));

            // routes
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("{location}/{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("Scripts/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("Content/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("favicon.ico");

            // action filters
            GlobalFilters.Filters.Add(new LogErrorAttribute());
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            GlobalFilters.Filters.Add(new UserAgentAttribute("IE", 7, true));

            // app settings
            ConfigurationManager.AppSettings["webpages:Version"] = "1.0.0.0";
            ConfigurationManager.AppSettings["ClientValidationEnabled"] = "false";
            ConfigurationManager.AppSettings["UnobtrusiveJavaScriptEnabled"] = "false";

            // membership and roles
            Membership.Providers.Clear();
            Membership.Providers.Add(new DbMembershipProvider());

            FormsAuthentication.EnableFormsAuthentication(new NameValueCollection() {
                    { "loginUrl", "Auth/Account/LogOn" }
            });

            Roles.Enabled = true;
            Roles.Providers.Clear();
            Roles.Providers.Add(new DbRoleProvider());

            // virtual path providers
            HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedVirtualPathProvider());
            HostingEnvironment.RegisterVirtualPathProvider(new PackageVirtualPathProvider());
        }

        private class ConfigurationModule : IHttpModule
        {
            public void Dispose()
            {
                
            }

            public void Init(HttpApplication context)
            {
                var ns = context.GetType().BaseType.Namespace + ".Controllers";
                
                ControllerBuilder.Current.DefaultNamespaces.Add(ns);
            }
        }
    }
}