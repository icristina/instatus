using Instatus.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core.Extensions;

namespace Instatus.Core.Impl
{
    public class AppSettingsStorage<T> : InMemoryKeyValueStorage<T> where T : class
    {
        private IHosting hosting;

        protected override T Find(string key)
        {
            var setting = hosting.GetAppSetting(typeof(T).Name.WithNamespace(key));

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
