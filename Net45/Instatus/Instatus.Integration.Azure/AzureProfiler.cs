using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;
using Instatus.Core.Impl;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Instatus.Core.Models;

namespace Instatus.Integration.Azure
{
    public class AzureProfiler : IProfiler
    {
        public const string TableName = "Profiler";
        
        private ICredentialStorage credentialStorage;
        private InMemoryQueue<Entry> queue;

        public IDisposable Step(string label)
        {
            return new AzureTableProfilerStep(label, queue);
        }

        public void Flush()
        {
            queue.Flush();
        }

        public async void Flush(List<Entry> flushed)
        {
            var credential = credentialStorage.GetCredential(WellKnown.Provider.WindowsAzure);
            var dataContext = await AzureClient.GetTableServiceContext(credential, TableName);

            foreach(var entry in flushed) 
            {
                var profilerEntry = new AzureProfilerEntity(entry.Text, entry.Created);             
                
                dataContext.AddObject(TableName, profilerEntry);
            }

            await dataContext.SaveChangesWithRetriesAsync();
        }

        public AzureProfiler(ICredentialStorage credentialStorage)
        {
            this.credentialStorage = credentialStorage;
            this.queue = new InMemoryQueue<Entry>(AzureClient.TableServiceEntityBufferCount, Flush);
        }
    }

    public class AzureProfilerEntity : TableServiceEntity, ICreated
    {
        public string Text { get; set; }
        public DateTime Created { get; set; }

        public AzureProfilerEntity(string text, DateTime created) 
        {
            Text = text;
            Created = created;
            PartitionKey = string.Format(WellKnown.FormatString.Date, created);
            RowKey = string.Format(WellKnown.FormatString.TimestampAndGuid, created, Guid.NewGuid());
        }
    }

    internal class AzureTableProfilerStep : AbstractProfilerStep
    {
        private IQueue<Entry> queue;
        
        public override void WriteStart(string message)
        {
            // only save completion profiler messages
        }

        public override void WriteEnd(string message)
        {
            queue.Enqueue(new Entry(message));
        }

        public AzureTableProfilerStep(string stepName, IQueue<Entry> queue)
            : base(stepName)
        {
            this.queue = queue;
        }
    }
}
