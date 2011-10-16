using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Instatus.Tasks;

namespace Instatus.Data
{
    // Based on: https://gist.github.com/895730
    public class DropTablesInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {       
        public void InitializeDatabase(T context)
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;

            objectContext.ExecuteStoreCommand(dropConstraints);
            TaskProvider.Retry(() => objectContext.ExecuteStoreCommand(deleteTables), 10);
            objectContext.ExecuteStoreCommand(objectContext.CreateDatabaseScript());

            Seed(context);
        }

        protected virtual void Seed(T context)
        {
            context.SaveChanges();
        }

        private const string dropAll =
            @"DECLARE @Sql NVARCHAR(500) DECLARE @Cursor CURSOR

                SET @Cursor = CURSOR FAST_FORWARD FOR

                SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + ']'

                FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1
                LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME =rc1.CONSTRAINT_NAME

                OPEN @Cursor FETCH NEXT FROM @Cursor INTO @Sql

                WHILE (@@FETCH_STATUS = 0)
                BEGIN
                Exec SP_EXECUTESQL @Sql

                FETCH NEXT FROM @Cursor INTO @Sql

                END
                CLOSE @Cursor DEALLOCATE @Cursor

                GO
                EXEC sp_MSForEachTable 'DROP TABLE ?'

                GO";

        private const string dropConstraints =
        @"select  
                'ALTER TABLE ' + so.table_name + ' DROP CONSTRAINT ' 
                + so.constraint_name  
                from INFORMATION_SCHEMA.TABLE_CONSTRAINTS so";

        private const string deleteTables =
            @"declare @cmd varchar(4000)
                declare cmds cursor for 
                Select
                    'drop table [' + Table_Name + ']'
                From
                    INFORMATION_SCHEMA.TABLES
 
                open cmds
                while 1=1
                begin
                    fetch cmds into @cmd
                    if @@fetch_status != 0 break
                    print @cmd
                    exec(@cmd)
                end
                close cmds
                deallocate cmds";
    }
}