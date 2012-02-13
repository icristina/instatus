using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Instatus
{
    public static class CacheExtensions
    {
        public static T Value<T>(this Cache cache, Func<T> getter, string key = null, int duration = 60) where T : class
        {
            if(key == null)
                key = getter.GetHashCode().ToString();

            T cachedValue = cache[key] as T;

            if (cachedValue == null)
            {
                cachedValue = getter();
                cache.Insert(key, cachedValue, null, DateTime.Now.AddSeconds(duration), TimeSpan.Zero);
            }

            return cachedValue;
        }

        // http://stackoverflow.com/questions/2109928/how-to-turn-output-caching-off-for-authenticated-users-in-asp-net-mvc
        public static void IgnoreThisRequest(this HttpCachePolicyBase cache)
        {
            cache.AddValidationCallback(IgnoreThisRequestCallback, null);
        }

        private static void IgnoreThisRequestCallback(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = HttpValidationStatus.IgnoreThisRequest;
        }
    }
}