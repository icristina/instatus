using Instatus.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface IContentManager
    {
        Document Get(string key);
        IEnumerable<Document> Query(IFilter filter); // paging applied afterwards if IOrderedQueryable
        void AddOrUpdate(string key, Document contentItem);
        void Delete(string key);
    }
}
