using Instatus.Core.Models;
using System;
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
                    using (var inputStream = new MemoryStream())
                    {
                        localStorage.Stream(virtualPath, inputStream);
                        return handler.Read(inputStream);
                    }
                }
                catch
                {

                }
            }

            return default(T);
        }

        public IEnumerable<T> Query(Criteria criteria)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(string key, T model)
        {
            var virtualPath = ResolveVirtualPaths(key).First();

            using (var inputStream = new MemoryStream())
            {
                handler.Write(model, inputStream);
                localStorage.Save(virtualPath, inputStream);
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
