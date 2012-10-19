using Autofac;
using Autofac.Integration.Mvc;
using Elmah;
using Instatus.Core;
using Instatus.Core.Models;
using Instatus.Integration.Azure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.StorageClient;

namespace Instatus.Integration.Elmah
{
    public class AzureErrorLog : ErrorLog
    {
        private const string TableName = "ElmahLog";
        
        public override ErrorLogEntry GetError(string id)
        {
            using (ILifetimeScope container = AutofacDependencyResolver.Current.ApplicationContainer.BeginLifetimeScope())
            {
                var credentials = container.Resolve<IKeyValueStorage<Credential>>();
                var tableServiceContext = AzureClient.GetTableServiceContext(credentials);
                var error = tableServiceContext.CreateQuery<AzureErrorLogEntity>(TableName)
                            .Where(e => e.PartitionKey == string.Empty && e.RowKey == id)
                            .Single();

                return new ErrorLogEntry(this, id, ErrorXml.DecodeString(error.Xml));
            }
        }

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            using (ILifetimeScope container = AutofacDependencyResolver.Current.ApplicationContainer.BeginLifetimeScope())
            {
                var credentials = container.Resolve<IKeyValueStorage<Credential>>();
                var tableServiceContext = AzureClient.GetTableServiceContext(credentials);
                var errors = tableServiceContext.CreateQuery<AzureErrorLogEntity>(TableName)
                                .Where(e => e.PartitionKey == string.Empty)
                                .Take(pageSize)
                                .AsTableServiceQuery()
                                .Execute();

                foreach (var error in errors)
                {
                    errorEntryList.Add(new ErrorLogEntry(this, error.RowKey, ErrorXml.DecodeString(error.Xml)));
                }

                return errors.Count();
            }
        }

        public override string Log(Error error)
        {
            using (ILifetimeScope container = AutofacDependencyResolver.Current.ApplicationContainer.BeginLifetimeScope())
            {
                var credentials = container.Resolve<IKeyValueStorage<Credential>>();
                var tableServiceContext = AzureClient.GetTableServiceContext(credentials);
                var now = DateTime.UtcNow;
                var errorEntity = new AzureErrorLogEntity()
                {
                    Created = now,
                    Xml = ErrorXml.EncodeString(error)
                }
                .WithEmptyPartionKey()
                .WithDescendingRowKey(now);

                tableServiceContext.AddObject(TableName, errorEntity);
                tableServiceContext.SaveChangesWithRetries();

                return errorEntity.RowKey;
            }
        }

        // constructors required by Elmah
        public AzureErrorLog(IDictionary config)
        {

        }

        public AzureErrorLog(string connectionString)
        {

        }
    }
}
