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
        private DbContext dbcontext;

        public DbContext Context
        {
            get
            {
                if (dbcontext == null)
                    dbcontext = Activator.CreateInstance<TContext>();

                return dbcontext;
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
            using(dbcontext) { // try dispose

            }
        }
    }
}
