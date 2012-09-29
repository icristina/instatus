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
        T Get(string key);
        IEnumerable<T> Query(Criteria criteria);
        void AddOrUpdate(string key, T model);
        void Delete(string key);
    }
}
