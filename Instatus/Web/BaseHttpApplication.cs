using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace Instatus.Web
{
    public class BaseHttpApplication : HttpApplication
    {
        public static WebEnvironment GetEnvironment()
        {
            return HttpContext.Current.ApplicationInstance.Setting<string>(WebAppSetting.Environment).AsEnum<WebEnvironment>();
        }
        
        public bool IsDebug
        {
            get
            {
                return HttpApplicationExtensions.IsDebug(this);
            }
        }

        public T Setting<T>(string name)
        {
            return HttpApplicationExtensions.Setting<T>(this, name);
        }

        public bool IsEnabled(string name, bool ensureDebug = false)
        {
            return (ensureDebug && IsDebug) && Setting<bool>(name);
        }
    }
}