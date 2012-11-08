using Instatus.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core.Extensions;

namespace Instatus.Core.Impl
{
    public class AppSettingsStorage<T> : InMemoryKeyValueStorage<T>, ILookup<T> where T : class
    {
        private IHosting hosting;
        
        public T Get(string key)
        {
            return Get(key, typeof(T).Name);
        }
        
        protected override T Find(string partitionKey, string rowKey)
        {
            var setting = hosting.GetAppSetting(rowKey.WithNamespace(partitionKey));

            if (setting == null)
                return default(T);

            return AppContext.Binders[typeof(T)](setting.AsDictionary()) as T;
        }

        public AppSettingsStorage(IHosting hosting)
        {
            this.hosting = hosting;
        }
    }
}
