using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;

namespace Instatus.Integration.EntityFramework
{
    public class EfEntityStorage<TContext> : IEntityStorage, IDisposable where TContext : DbContext
    {
        private DbContext context;

        public DbContext Context
        {
            get
            {
                return context ?? (context = Activator.CreateInstance<TContext>());
            }
        }
        
        public IEntitySet<T> Set<T>() where T : class
        {
            return new EfEntitySet<T>(Context.Set<T>());
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            using(context) { // try dispose

            }
        }

        public EfEntityStorage()
        {

        }

        public EfEntityStorage(TContext context)
        {
            this.context = context;
        }
    }
}
