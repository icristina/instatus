using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Instatus.Web;
using System.IO;

namespace Instatus.Data
{
    // http://stackoverflow.com/questions/536350/sql-server-2005-drop-all-the-tables-stored-procedures-triggers-constriants-a
    // http://blogs.msdn.com/b/onoj/archive/2008/02/26/incorrect-syntax-near-go-sqlcommand-executenonquery.aspx
    public class DropTablesInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        public void InitializeDatabase(T context)
        {
            var objectContext = context.ObjectContext();
            var sql = File.ReadAllText(WebPath.Server("~/Data/DropTables.sql"));

            // split on GO statements, ensure that final GO has line break or space after it
            foreach (var command in sql.Split(new string[] { "GO\r\n", "GO ", "GO\t" }, StringSplitOptions.RemoveEmptyEntries))
            {
                context.Database.ExecuteSqlCommand(command);
            }

            context.Database.ExecuteSqlCommand(objectContext.CreateDatabaseScript());

            Seed(context);
        }

        protected virtual void Seed(T context)
        {
            context.SaveChanges();
        }
    }
}