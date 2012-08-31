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

namespace Instatus.Integration.Azure
{
    public class AzureProfiler : IProfiler
    {
        public const string TableName = "Profiler";
        
        private ICredentialStorage credentialStorage;
        private InMemoryQueue<BaseEntry> queue;

        public IDisposable Step(string label)
        {
            return new AzureTableProfilerStep(label, queue);
        }

        public void Flush()
        {
            queue.Flush();
        }

        public async void Flush(List<BaseEntry> flushed)
        {
            var credential = credentialStorage.GetCredential(AzureClient.TableProviderName);
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
            this.queue = new InMemoryQueue<BaseEntry>(AzureClient.TableServiceEntityBufferCount, Flush);
        }
    }

    public class AzureProfilerEntity : TableServiceEntity
    {
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; }

        public AzureProfilerEntity(string text, DateTime createdTime) 
        {
            Text = text;
            CreatedTime = createdTime;
            PartitionKey = createdTime.ToString(AzureClient.PartitionKeyFormatString);
            RowKey = Guid.NewGuid().ToString();
        }
    }

    internal class AzureTableProfilerStep : AbstractProfilerStep
    {
        private IQueue<BaseEntry> queue;
        
        public override void WriteStart(string message)
        {
            // only save completion profiler messages
        }

        public override void WriteEnd(string message)
        {
            queue.Enqueue(new BaseEntry(message));
        }

        public AzureTableProfilerStep(string stepName, IQueue<BaseEntry> queue)
            : base(stepName)
        {
            this.queue = queue;
        }
    }
}
