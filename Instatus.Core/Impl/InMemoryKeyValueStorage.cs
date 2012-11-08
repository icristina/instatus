using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core.Extensions;
using Instatus.Core.Models;
using System.Collections;

namespace Instatus.Core.Impl
{
    public abstract class InMemoryKeyValueStorage<T> : IKeyValueStorage<T> where T : class
    {
        private ConcurrentDictionary<Tuple<string, string>, T> cache = new ConcurrentDictionary<Tuple<string, string>, T>();

        protected virtual T Find(string partitionKey, string rowKey)
        {
            return null;
        }

        public T Get(string partitionKey, string rowKey)
        {
            T model;

            var compositeKey = new Tuple<string, string>(partitionKey, rowKey);

            if (!cache.TryGetValue(compositeKey, out model))
            {
                model = Find(partitionKey, rowKey);

                if (model != null)
                {
                    cache.TryAdd(compositeKey, model);
                }
            }

            return model;
        }

        public IEnumerable<KeyValue<T>> Query(string partitionKey, Criteria criteria)
        {
            return cache
                .Where(k => k.Key.Item1 == partitionKey)
                .Select(k => new KeyValue<T>() 
                {
                    Key = k.Key.Item2,
                    Value = k.Value
                });
        }

        public void AddOrUpdate(string partitionKey, string rowKey, T model)
        {
            cache.AddOrUpdate(new Tuple<string, string>(partitionKey, rowKey), model, (k, v) => model);
        }

        public void Delete(string partitionKey, string rowKey)
        {
            T removedModel;
            cache.TryRemove(new Tuple<string, string>(partitionKey, rowKey), out removedModel);
        }
    }
}
