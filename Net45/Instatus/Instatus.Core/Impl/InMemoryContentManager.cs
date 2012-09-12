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
        private ISession session;

        private static IDictionary<Tuple<string, string>, object> content = new ConcurrentDictionary<Tuple<string, string>, object>();

        private Tuple<string, string> GetContentKey(string key)
        {
            return new Tuple<string, string>(session.Locale, key);
        }

        public object Get(string key)
        {
            return content[GetContentKey(key)];
        }

        public IEnumerable Query(IFilter filter)
        {
            return content.Select(c => c.Value);
        }

        public void AddOrUpdate(string key, object contentItem)
        {
            content[GetContentKey(key)] = contentItem;
        }

        public void Delete(string key)
        {
            content.Remove(GetContentKey(key));
        }

        public InMemoryContentManager(ISession session)
        {
            this.session = session;
        }
    }
}
