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
            if (started || !WebApp.IsEnabled(WebAppSetting.Bootstrap))
            {
                return;
            }

            started = true;

            WebBootstrap.Core();
            WebBootstrap.Routes();
            WebBootstrap.Auth();
            WebBootstrap.Rewriting();
            WebBootstrap.ErrorHandling();
            WebBootstrap.LegacyUserAgents();
            WebBootstrap.ViewLocation();
            WebBootstrap.RemoveServerFingerprint();
        }
    }
}