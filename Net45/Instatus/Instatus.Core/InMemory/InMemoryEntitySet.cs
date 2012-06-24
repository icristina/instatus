using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.InMemory
{
    public class InMemoryEntitySet<T> : IEntitySet<T>
    {
        private IList<T> list = new List<T>();
        
        public T Find(object key)
        {
            return list.FirstOrDefault(i => (i as dynamic).Id.Equals(key));
        }

        public void Delete(object key)
        {
            var item = Find(key);

            if (item != null)
                list.Remove(item);
        }

        public T Create()
        {
            var item = Activator.CreateInstance<T>();

            list.Add(item);
            
            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        public Expression Expression
        {
            get
            {
                return list.AsQueryable().Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return list.AsQueryable().Provider;
            }
        }
    }
}
