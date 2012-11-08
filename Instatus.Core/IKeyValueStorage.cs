using Instatus.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface IKeyValueStorage<T>
    {
        T Get(string partitionKey, string rowKey);
        IEnumerable<KeyValue<T>> Query(string partitionKey, Criteria criteria);
        void AddOrUpdate(string partitionKey, string rowKey, T value);
        void Delete(string partitionKey, string rowKey);
    }
}
