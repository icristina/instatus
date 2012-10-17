using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core;
using Instatus.Core.Impl;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;
using Instatus.Core.Models;
using Instatus.Integration.Azure;
using Instatus.Core.Extensions;
using System.Threading.Tasks.Dataflow;

namespace Instatus.Integration.Azure
{
    public class AzureLogger : ILogger
    {
        public const string TableName = "Logger";
        
        private IKeyValueStorage<Credential> credentials;
        private BatchBlock<AzureLoggerEntity> buffer;

        public void Log(Exception exception, IDictionary<string, string> properties)
        {
            var azureLoggerEntity = new AzureLoggerEntity(DateTime.UtcNow)
            {
                Exception = exception.Message,
                StackTrace = exception.StackTrace,
                Properties = JsonConvert.SerializeObject(properties)
            };

            if (exception.InnerException != null)
            {
                azureLoggerEntity.InnerException = exception.InnerException.Message;
            }
            
            buffer.Post(azureLoggerEntity);
        }

        public async void SaveBatch(AzureLoggerEntity[] flushed)
        {
            var credential = credentials.Get(WellKnown.Provider.WindowsAzure);
            var dataContext = await AzureClient.GetTableServiceContext(credential, TableName, false);

            foreach (var entry in flushed)
            {
                dataContext.AddObject(TableName, entry);
            }

            await dataContext.SaveChangesWithRetriesAsync();
        }

        public AzureLogger(IKeyValueStorage<Credential> credentials)
        {
            this.credentials = credentials;
            this.buffer = new BatchBlock<AzureLoggerEntity>(AzureClient.TableServiceEntityBufferCount);
            this.buffer.LinkTo(new ActionBlock<AzureLoggerEntity[]>(batch => SaveBatch(batch)));
        }
    }

    public class AzureLoggerEntity : TableServiceEntity, ICreated
    {
        public string Exception { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string Properties { get; set; }
        public DateTime Created { get; set; }

        public AzureLoggerEntity(DateTime created)
        {
            Created = DateTime.UtcNow;

            this.WithMonthlyPartionKey().WithDescendingRowKey();
        }
    }
}
