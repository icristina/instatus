using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus;

namespace Instatus.Web
{
    public static class WebConfiguration
    {
        public static bool Setting(string name, bool ensureDebug = false) {
            return (ensureDebug && HttpContext.Current.IsDebuggingEnabled) &&
                HttpContext.Current.ApplicationInstance.Setting<bool>(name);
        }
    }
}