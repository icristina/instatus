using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryEntityStorage : IEntityStorage
    {
        private IDictionary<Type, object> entitySets = new ConcurrentDictionary<Type, object>();
        
        public IEntitySet<T> Set<T>() where T : class
        {
            if (!entitySets.ContainsKey(typeof(T)))
            {
                entitySets.Add(typeof(T), new InMemoryEntitySet<T>());
            }

            return entitySets[typeof(T)] as IEntitySet<T>;
        }

        public void SaveChanges()
        {
            // do nothing
        }
    }
}
