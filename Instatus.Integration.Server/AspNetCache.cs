using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Instatus.Integration.Server
{
    public class AspNetCache : ICache
    {
        public object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        public void AddOrUpdate(string key, object value)
        {
            HttpRuntime.Cache[key] = value;
        }

        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
    }
}
