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
        private IHosting hosting;
        private ICache cache;
        
        public T Get(string partitionKey, string rowKey)
        {
            return cache.Get<T>(partitionKey, rowKey, () =>
            {
                try
                {
                    var virtualPath = ResolveVirtualPath(partitionKey, rowKey);
                    
                    using (var fileStream = localStorage.OpenRead(virtualPath))
                    {
                        return handler.Read(fileStream);
                    }
                }
                catch
                {
                    return default(T);
                }
            });
        }

        public IEnumerable<KeyValue<T>> Query(string partitionKey, Criteria criteria)
        {
            return localStorage.Query("~/App_Data/", partitionKey + "." + handler.FileExtension)
                        .Select(f => {
                            var rowKey = Path.GetFileName(f).Split('.')[0];
                            return new KeyValue<T>()
                            {
                                Key = rowKey,
                                Value = Get(partitionKey, rowKey)
                            };
                        })
                        .ToList();
        }

        public void AddOrUpdate(string partitionKey, string rowKey, T model)
        {
            var virtualPath = ResolveVirtualPath(partitionKey, rowKey);

            using (var inputStream = localStorage.OpenWrite(virtualPath, null))
            {
                handler.Write(model, inputStream);
            }
        }

        public void Delete(string partitionKey, string rowKey)
        {
            var virtualPath = ResolveVirtualPath(partitionKey, rowKey);

            localStorage.Delete(virtualPath);
        }

        private string ResolveVirtualPath(string partitionKey, string rowKey)
        {
            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                return string.Format("~/App_Data/{0}.{1}.{2}", rowKey, partitionKey, handler.FileExtension);
            }
            else
            {
                return string.Format("~/App_Data/{0}.{2}", rowKey, handler.FileExtension);
            }
        }

        public AppDataStorage(IHandler<T> handler, IBlobStorage localStorage, IHosting hosting, ICache cache)
        {
            this.handler = handler;
            this.localStorage = localStorage;
            this.hosting = hosting;
            this.cache = cache;
        }
    }
}
