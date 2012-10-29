using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Instatus.Core;
using Microsoft.WindowsAzure;
using Instatus.Core.Extensions;
using Instatus.Core.Models;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Instatus.Integration.Azure
{
    public class AzureQueue<T> : IQueue<T>
    {
        private IKeyValueStorage<Credential> credentials;
        private ISerializer serializer;

        public CloudQueue GetQueue()
        {
            var credential = credentials.Get(WellKnown.Provider.WindowsAzure);
            var baseUri = string.Format("http://{0}.queue.core.windows.net", credential.AccountName);
            var storageCredential = new StorageCredentials(credential.AccountName, credential.PrivateKey);
            var client = new CloudQueueClient(new Uri(baseUri), storageCredential);

            return client.GetQueueReference(typeof(T).FullName);
        }

        public void Enqueue(T message)
        {
            var queue = GetQueue();
            var bytes = serializer.Serialize(message);
            var cloudQueueMessage = new CloudQueueMessage(bytes);

            queue.AddMessage(cloudQueueMessage);
        }

        public bool TryDequeue(out T message)
        {
            var queue = GetQueue();
            var queueMessage = queue.GetMessage();

            if (queueMessage == null)
            {
                message = default(T);
                return false;
            }

            message = serializer.Deserialize<T>(queueMessage.AsBytes);
            return true;
        }

        public AzureQueue(IKeyValueStorage<Credential> credentials, ISerializer serializer)
        {
            this.credentials = credentials;
            this.serializer = serializer;
        }
    }
}
