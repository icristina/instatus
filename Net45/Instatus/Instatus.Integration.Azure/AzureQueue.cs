using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Instatus.Core;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Instatus.Integration.Azure
{
    public class AzureQueue<T> : IQueue<T>
    {
        public const string ProviderName = "AzureQueueStorage";

        private ICredentialStorage credentialStorage;
        private ICredential credential;

        public ICredential Credential
        {
            get
            {
                return credential ?? (credential = credentialStorage.GetCredential(ProviderName));
            }
        }

        public CloudQueue GetQueue()
        {
            var baseUri = string.Format("http://{0}.queue.core.windows.net", Credential.AccountName);
            var storageCredential = new StorageCredentialsAccountAndKey(Credential.AccountName, Credential.PrivateKey);
            var client = new CloudQueueClient(baseUri, storageCredential);

            return client.GetQueueReference(typeof(T).FullName);
        }

        public void Enqueue(T message)
        {
            var queue = GetQueue();
            var bytes = Serialize(message);
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

            message = Deserialize(queueMessage.AsBytes);
            return true;
        }

        public byte[] Serialize(T graph, IEnumerable<Type> knownTypes = null)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T), knownTypes);
                serializer.WriteObject(stream, graph);
                return stream.ToArray();
            }
        }

        public T Deserialize(byte[] bytes, IEnumerable<Type> knownTypes = null)
        {
            if (bytes == null)
                return default(T);

            using (var stream = new MemoryStream(bytes))
            {
                stream.Position = 0;
                var serializer = new DataContractSerializer(typeof(T), knownTypes);
                return (T)serializer.ReadObject(stream);
            }
        }

        public AzureQueue(ICredentialStorage credentialStorage)
        {
            this.credentialStorage = credentialStorage;
        }
    }
}
