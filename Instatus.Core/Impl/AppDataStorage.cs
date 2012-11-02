using Instatus.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Core.Extensions;

namespace Instatus.Core.Impl
{
    public class AppDataStorage<T> : IKeyValueStorage<T> where T : class
    {
        private IBlobStorage localStorage;
        private IHandler<T> handler;
        private IPreferences preferences;
        private IHosting hosting;
        private ICache cache;
        
        public T Get(string key)
        {
            return cache.Get<T>(preferences.Locale, key, () =>
            {
                foreach (var virtualPath in ResolveVirtualPaths(key))
                {
                    try
                    {
                        using (var fileStream = localStorage.OpenRead(virtualPath))
                        {
                            return handler.Read(fileStream);
                        }
                    }
                    catch
                    {

                    }
                }

                return default(T);
            });
        }

        public IEnumerable<KeyValue<T>> Query(Criteria criteria)
        {
            return localStorage.Query("~/App_Data/", preferences.Locale + "." + handler.FileExtension)
                        .Select(f => {
                            var key = Path.GetFileName(f).Split('.')[0];
                            return new KeyValue<T>()
                            {
                                Key = key,
                                Value = Get(key)
                            };
                        })
                        .ToList();
        }

        public void AddOrUpdate(string key, T model)
        {
            var virtualPath = ResolveVirtualPaths(key).First();

            using (var inputStream = localStorage.OpenWrite(virtualPath, null))
            {
                handler.Write(model, inputStream);
            }
        }

        public void Delete(string key)
        {
            foreach (var path in ResolveVirtualPaths(key))
            {
                try
                {
                    localStorage.Delete(path);
                }
                catch
                {

                }
            }
        }

        private string[] ResolveVirtualPaths(string key)
        {
            return new string[] 
            {
                string.Format("~/App_Data/{0}.{1}.{2}", key, preferences.Locale, handler.FileExtension),
                string.Format("~/App_Data/{0}.{1}.{2}", key, hosting.DefaultCulture.Name, handler.FileExtension),
                string.Format("~/App_Data/{0}.{1}", key, handler.FileExtension)
            };
        }

        public AppDataStorage(IHandler<T> handler, IBlobStorage localStorage, IPreferences preferences, IHosting hosting, ICache cache)
        {
            this.handler = handler;
            this.localStorage = localStorage;
            this.preferences = preferences;
            this.hosting = hosting;
            this.cache = cache;
        }
    }
}
