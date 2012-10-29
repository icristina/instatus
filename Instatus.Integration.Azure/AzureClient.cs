using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Instatus.Core.Models;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using Microsoft.WindowsAzure.Storage.Auth;

namespace Instatus.Integration.Azure
{
    public static class AzureClient
    {    
        public static int TableServiceEntityBufferCount = 25;

        public static TableServiceContext GetTableServiceContext(Credential credential)
        {
            var baseAddress = string.Format("http://{0}.table.core.windows.net", credential.AccountName);
            var storageCredentials = new StorageCredentials(credential.AccountName, credential.PrivateKey);
            var tableClient = new CloudTableClient(new Uri(baseAddress), storageCredentials);

            return tableClient.GetTableServiceContext();
        }

        public static TableServiceContext GetTableServiceContext(IKeyValueStorage<Credential> credentials)
        {
            var credential = credentials.Get(WellKnown.Provider.WindowsAzure);

            return GetTableServiceContext(credential);
        }
    }
}
