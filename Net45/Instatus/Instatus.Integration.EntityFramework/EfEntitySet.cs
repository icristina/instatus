using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;

namespace Instatus.Integration.EntityFramework
{
    public class EfEntitySet<T> : IEntitySet<T>
    {
        private IDbSet<T> dbset;
        
        public T Find(object key)
        {
            return dbset.Find(key);
        }

        public void Delete(object key)
        {
            var instance = dbset.Find(key);

            dbset.Remove(instance);
        }

        public T Create()
        {
            var instance = Activator.CreateInstance<T>();

            dbset.Add(instance);

            return instance;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return dbset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dbset.GetEnumerator();
        }

        public Type ElementType
        {
            get {
                return dbset.ElementType;
            }
        }

        public Expression Expression
        {
            get {
                return dbset.Expression;
            }
        }

        public IQueryProvider Provider
        {
            get {
                return dbset.Provider; 
            }
        }

        public EfEntitySet(IDbSet<T> dbset)
        {
            this.dbset = dbset;
        }
    }
}
