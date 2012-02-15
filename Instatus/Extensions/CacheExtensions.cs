using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Instatus
{
    public static class CacheExtensions
    {
        private static object cacheLock = new object();

        // http://stackoverflow.com/questions/39112/what-is-the-best-way-to-lock-cache-in-asp-net
        public static T Value<T>(this Cache cache, Func<T> getter, string key = null, int duration = 60) where T : class
        {
            if(key == null)
                key = getter.GetHashCode().ToString(); // hash of getter, is hashcode of the return type

            T cachedValue = cache[key] as T;

            if (cachedValue == null)
            {
                lock (cacheLock)
                {
                    cachedValue = cache[key] as T; // recheck if available in cache during locking

                    if (cachedValue == null)
                    {
                        cachedValue = getter();
                        cache.Insert(key, cachedValue, null, DateTime.Now.AddSeconds(duration), TimeSpan.Zero);
                    }
                }
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