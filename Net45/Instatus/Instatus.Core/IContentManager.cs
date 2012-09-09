using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface IContentManager
    {
        object Get(string key);
        IEnumerable Query(IFilter filter); // paging applied afterwards if IOrderedQueryable
        void AddOrUpdate(string key, object contentItem);
        void Delete(string key);
    }
}
