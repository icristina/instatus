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
        IEnumerable<Document> Query(Filter filter); // paging applied afterwards if IOrderedQueryable
        void AddOrUpdate(string key, Document document);
        void Delete(string key);
    }
}
