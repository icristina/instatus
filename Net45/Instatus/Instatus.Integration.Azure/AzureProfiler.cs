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
        public const string ProviderName = "AzureTableStorage";
        public const string TableName = "Profiler";
        
        private ICredentialStorage credentialStorage;
        private ICredential credential;

        private InMemoryQueue<BaseEntry> queue;

        public ICredential Credential
        {
            get
            {
                if (credential == null)
                    credential = credentialStorage.GetCredential(ProviderName);

                return credential;
            }
        }

        public IDisposable Step(string label)
        {
            return new AzureTableProfilerStep(label, queue);
        }

        public async void Flush(List<BaseEntry> flushed)
        {
            var baseAddress = string.Format("http://{0}.table.core.windows.net", Credential.AccountName);
            var storageCredentials = new StorageCredentialsAccountAndKey(Credential.AccountName, Credential.PrivateKey);
            var tableClient = new CloudTableClient(baseAddress, storageCredentials);
            var created = await tableClient.CreateTableIfNotExistAsync(TableName);
            var dataContext = tableClient.GetDataServiceContext();

            foreach(var entry in flushed) 
            {
                var profilerEntry = new ProfilerEntity(entry.Text, entry.Timestamp);             
                
                dataContext.AddObject(TableName, profilerEntry);
            }

            await dataContext.SaveChangesWithRetriesAsync();
        }

        public AzureProfiler(ICredentialStorage credentialStorage, int buffer)
        {
            this.credentialStorage = credentialStorage;
            this.queue = new InMemoryQueue<BaseEntry>(buffer, Flush);
        }
    }

    public class ProfilerEntity : TableServiceEntity
    {
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; }

        public ProfilerEntity(string text, DateTime createdTime) 
        {
            Text = text;
            CreatedTime = createdTime;
            PartitionKey = createdTime.ToString("yyyy-MM-dd"); // partition per day
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
