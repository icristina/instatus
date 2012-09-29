using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core;
using Instatus.Core.Impl;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;
using Instatus.Core.Models;

namespace Instatus.Integration.Azure
{
    public class AzureLogger : ILogger
    {
        public const string TableName = "Logger";
        
        private IKeyValueStorage<Credential> credentials;
        private InMemoryQueue<AzureLoggerEntity> queue;

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
            
            queue.Enqueue(azureLoggerEntity);
        }

        public void Flush()
        {
            queue.Flush();
        }

        public async void Flush(List<AzureLoggerEntity> flushed)
        {
            var credential = credentials.Get(WellKnown.Provider.WindowsAzure);
            var dataContext = await AzureClient.GetTableServiceContext(credential, TableName);

            foreach (var entry in flushed)
            {
                dataContext.AddObject(TableName, entry);
            }

            await dataContext.SaveChangesWithRetriesAsync();
        }

        public AzureLogger(IKeyValueStorage<Credential> credentials)
        {
            this.credentials = credentials;
            this.queue = new InMemoryQueue<AzureLoggerEntity>(AzureClient.TableServiceEntityBufferCount, Flush);
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
            Created = created;
            PartitionKey = string.Format(WellKnown.FormatString.Date, created);
            RowKey = string.Format(WellKnown.FormatString.TimestampAndGuid, created, Guid.NewGuid());
        }
    }
}
