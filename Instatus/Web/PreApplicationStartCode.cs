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
            if (started || !ConfigurationManager.AppSettings[WebAppSetting.Bootstrap].AsBoolean(true))
            {
                return;
            }

            started = true;

            WebBootstrap.Core();
            WebBootstrap.Routes();
            WebBootstrap.Auth();
            WebBootstrap.Tracing();
            WebBootstrap.Rewriting();
            WebBootstrap.ErrorHandling();
            WebBootstrap.LegacyUserAgents();

            DynamicModuleUtility.RegisterModule(typeof(ConfigurationModule));
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