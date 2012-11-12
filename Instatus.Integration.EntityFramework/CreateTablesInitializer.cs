using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Integration.EntityFramework
{
    public class CreateTablesInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        public void InitializeDatabase(T context)
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;

            try
            {
                context.Database.ExecuteSqlCommand(objectContext.CreateDatabaseScript());
            }
            catch
            {
                // database already exists
            }

            Seed(context);
        }

        protected virtual void Seed(T context)
        {
            context.SaveChanges();
        }
    }
}
