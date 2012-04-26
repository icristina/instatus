using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus;
using Instatus.Web;
using System.Diagnostics;
using System.Configuration;
using Instatus.Services;
using System.Web.Mvc;
using Instatus.Data;
using System.Net;
using Instatus.Models;

namespace Instatus.Web
{
    public static class WebApp
    {       
        public static T GetService<T>()
        {
            T service = DependencyResolver.Current.GetService<T>();

            if (service == null && !typeof(T).IsInterface)
            {
                service = Activator.CreateInstance<T>();
            }            
            
            return service;
        }

        public static IEnumerable<T> GetServices<T>()
        {
            return DependencyResolver.Current.GetServices<T>();
        }

        public static HttpApplication Instance {
            get 
            {
                return HttpContext.Current.ApplicationInstance;
            }
        }

        public static T Setting<T>(string name)
        {
            return ConfigurationManager.AppSettings.Value<T>(name.ToString());
        }

        public static Deployment Environment
        {
            get
            {
                return Setting<string>(WebConstant.AppSetting.Environment).AsEnum<Deployment>();
            }
        }

        public static bool IsDebug
        {
            get
            {
                return Debugger.IsAttached;
            }
        }

        public static bool IsDebugOrLocal
        {
            get
            {
                return IsDebug || HttpContext.Current.Request.IsLocal;
            }
        }

        public static bool IsEnabled(string name, bool defaultValue = true) // defaults to enabled
        {
            return ConfigurationManager.AppSettings[name].AsBoolean(defaultValue);
        }

        private static IMessageBus messageBus = new InMemoryMessageBus();

        public static void Subscribe<T>(Action<T> action)
        {
            messageBus.Subscribe<T>(action);
        }

        public static void Publish<T>(T message)
        {
            messageBus.Publish(message);
        }

        public static void OnReset(Action action)
        {
            Subscribe<WebAppReset>(a => action());
        }

        public static void Reset()
        {
            Publish(new WebAppReset());
        }

        public static IHasPermission GlobalPermission { get; set; } 

        public static bool Can(string action, object instance)
        {
            return GlobalPermission == null || GlobalPermission.Can(action, instance);
        }

        public static string PingUrl { get; set; }

        // ping server every minute
        // http://www.west-wind.com/weblog/posts/2007/May/10/Forcing-an-ASPNET-Application-to-stay-alive
        public static void KeepAlive()
        {
            TaskExtensions.Repeat(() =>
            {
                new WebClient().DownloadString(PingUrl ?? WebPath.BaseUri.ToString());
            }, 60 * 1000 * 1); 
        }
    }

    internal class WebAppReset
    {

    }
}