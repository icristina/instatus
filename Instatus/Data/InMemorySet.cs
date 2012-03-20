using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Collections;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace Instatus.Data
{
    public class InMemorySet<T> : IDbSet<T> where T : class
    {
        private IList<T> list = new List<T>();

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
            get {
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

        public InMemorySet()
        {

        }

        public InMemorySet(IEnumerable<T> items)
        {
            list = items.ToList();
        }

        public T Add(T entity)
        {
            list.Add(entity);
            return entity;
        }

        public T Attach(T entity)
        {
            var current = Find(entity.GetKey());
            Remove(current);
            Add(entity);
            return entity;
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public T Find(params object[] keyValues)
        {
            foreach(var item in list) {
                if (item.GetKey().Equals(keyValues[0]))
                    return item;
            }
            return null;
        }

        public ObservableCollection<T> Local
        {
            get { throw new NotImplementedException(); }
        }

        public T Remove(T entity)
        {
            list.Remove(entity);
            return entity;
        }
    }
}