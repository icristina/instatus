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

        public static WebEnvironment Environment
        {
            get 
            {
                return Setting<string>(WebAppSetting.Environment).AsEnum<WebEnvironment>();
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

        private static IMessageBus messageBus = new MessageBus();

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
    }

    internal class WebAppReset
    {

    }
}