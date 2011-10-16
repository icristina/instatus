﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections;
using System.Diagnostics;

namespace Instatus
{
    public static class HttpApplicationExtensions
    {
        public static bool IsDebug(this HttpApplication application)
        {
            return Debugger.IsAttached;
        }
        
        public static T Setting<T>(this HttpApplication application, string name)
        {                      
            return ConfigurationManager.AppSettings.Value<T>(name);
        }
    }
}