using Instatus.Core.Models;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryContentManager : IContentManager
    {
        private ISessionData sessionData;

        private static IDictionary<Tuple<string, string>, Document> content = new ConcurrentDictionary<Tuple<string, string>, Document>();

        private Tuple<string, string> GetContentKey(string key)
        {
            return new Tuple<string, string>(sessionData.Locale, key);
        }

        public Document Get(string key)
        {
            return content[GetContentKey(key)];
        }

        public IEnumerable<Document> Query(IFilter filter)
        {
            return content.Select(c => c.Value);
        }

        public void AddOrUpdate(string key, Document contentItem)
        {
            content[GetContentKey(key)] = contentItem;
        }

        public void Delete(string key)
        {
            content.Remove(GetContentKey(key));
        }

        public InMemoryContentManager(ISessionData sessionData)
        {
            this.sessionData = sessionData;
        }
    }
}
