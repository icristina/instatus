﻿using System;
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
        private ConcurrentDictionary<string, T> cache = new ConcurrentDictionary<string, T>();

        protected virtual T Find(string key)
        {
            return null;
        }

        public T Get(string key)
        {
            T model;

            if (!cache.TryGetValue(key, out model))
            {
                model = Find(key);

                if (model != null)
                {
                    cache.TryAdd(key, model);
                }
            }

            return model;
        }

        public IEnumerable<KeyValue<T>> Query(Criteria criteria)
        {
            return cache.Select(k => new KeyValue<T>() 
            {
                Key = k.Key,
                Value = k.Value
            });
        }

        public void AddOrUpdate(string key, T model)
        {
            cache.AddOrUpdate(key, model, (k, v) => model);
        }

        public void Delete(string key)
        {
            T removedModel;
            cache.TryRemove(key, out removedModel);
        }
    }
}