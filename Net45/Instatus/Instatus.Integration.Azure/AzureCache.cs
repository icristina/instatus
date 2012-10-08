using Instatus.Core;
using Microsoft.ApplicationServer.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Integration.Azure
{
    public class AzureCache : ICache
    {
        private DataCache cache = new DataCacheFactory().GetDefaultCache();
        
        public object Get(string key)
        {
            return cache.Get(key);
        }

        public void AddOrUpdate(string key, object value)
        {
            cache.Put(key, value);
        }
    }
}
