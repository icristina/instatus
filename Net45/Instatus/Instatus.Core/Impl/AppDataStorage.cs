using Instatus.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class AppDataStorage<T> : IKeyValueStorage<T>
    {
        private IHandler<T> handler;
        private ILocalStorage localStorage;
        private ISessionData sessionData;        
        
        public T Get(string key)
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
        }

        public IEnumerable<KeyValue<T>> Query(Criteria criteria)
        {
            return localStorage.Query("~/App_Data/", sessionData.Locale + "." + handler.FileExtension)
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

            using (var inputStream = localStorage.OpenWrite(virtualPath))
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
                string.Format("~/App_Data/{0}.{1}.{2}", key, sessionData.Locale, handler.FileExtension),
                string.Format("~/App_Data/{0}.{1}.{2}", key, WellKnown.Locale.UnitedStates, handler.FileExtension),
                string.Format("~/App_Data/{0}.{1}", key, handler.FileExtension)
            };
        }

        public AppDataStorage(IHandler<T> handler, ILocalStorage localStorage, ISessionData sessionData)
        {
            this.handler = handler;
            this.localStorage = localStorage;
            this.sessionData = sessionData;
        }
    }
}
