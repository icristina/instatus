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
using System.Threading.Tasks.Dataflow;

namespace Instatus.Integration.Azure
{
    public class AzureProfiler : IProfiler
    {
        public const string TableName = "Profiler";
        
        private IKeyValueStorage<Credential> credentials;
        private BatchBlock<AzureProfilerEntity> buffer;

        public IDisposable Step(string label)
        {
            return new AzureTableProfilerStep(label, buffer);
        }

        public async void SaveBatch(AzureProfilerEntity[] flushed)
        {
            var credential = credentials.Get(WellKnown.Provider.WindowsAzure);
            var dataContext = await AzureClient.GetTableServiceContext(credential, TableName, false);

            foreach(var entry in flushed) 
            {
                dataContext.AddObject(TableName, entry);
            }

            await dataContext.SaveChangesWithRetriesAsync();
        }

        public AzureProfiler(IKeyValueStorage<Credential> credentials)
        {
            this.credentials = credentials;
            this.buffer = new BatchBlock<AzureProfilerEntity>(AzureClient.TableServiceEntityBufferCount);
            this.buffer.LinkTo(new ActionBlock<AzureProfilerEntity[]>(batch => SaveBatch(batch)));
        }
    }

    public class AzureProfilerEntity : TableServiceEntity, ICreated
    {
        public string Text { get; set; }
        public DateTime Created { get; set; }

        public AzureProfilerEntity(string text) 
        {
            Text = text;
            Created = DateTime.UtcNow;

            this.WithMonthlyPartionKey().WithDescendingRowKey();
        }
    }

    internal class AzureTableProfilerStep : ProfilerStep
    {
        private BatchBlock<AzureProfilerEntity> buffer;
        
        public override void WriteStart(string message)
        {
            // only save completion profiler messages
        }

        public override void WriteEnd(string message)
        {
            buffer.Post(new AzureProfilerEntity(message));
        }

        public AzureTableProfilerStep(string stepName, BatchBlock<AzureProfilerEntity> buffer)
            : base(stepName)
        {
            this.buffer = buffer;
        }
    }
}
