using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public static class WebCache
    {
        public const int Duration = 600; // 10 minutes
        public const string VaryByParam = "none"; // option to make this a querystring value to clear caches

        public static T Value<T>(Func<T> getter, string key = null, int duration = Duration) where T : class
        {
            return HttpRuntime.Cache.Value(getter, key, duration);
        }
    }
}