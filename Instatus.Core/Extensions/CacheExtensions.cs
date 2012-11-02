using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Extensions
{
    public static class CacheExtensions
    {
        public static T Get<T>(this ICache cache, string key, Func<T> regenerate) where T : class
        {
            T value = cache.Get(key) as T ?? regenerate();

            if (value != null)
            {
                cache.AddOrUpdate(key, value);
            }

            return value;
        }

        public static T Get<T>(this ICache cache, string partitionKey, string rowKey, Func<T> regenerate) where T : class
        {
            return cache.Get<T>(string.Format("{0}-{1}", partitionKey, rowKey), regenerate);
        }
    }
}
