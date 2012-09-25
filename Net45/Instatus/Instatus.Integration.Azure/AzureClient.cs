using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Instatus.Core.Models;

namespace Instatus.Integration.Azure
{
    public static class AzureClient
    {    
        public static int TableServiceEntityBufferCount = 25;

        public static async Task<TableServiceContext> GetTableServiceContext(Credential credential, string tableName)
        {
            var baseAddress = string.Format("http://{0}.table.core.windows.net", credential.AccountName);
            var storageCredentials = new StorageCredentialsAccountAndKey(credential.AccountName, credential.PrivateKey);
            var tableClient = new CloudTableClient(baseAddress, storageCredentials);
            var created = await tableClient.CreateTableIfNotExistAsync(tableName);
            return tableClient.GetDataServiceContext();
        }
    }
}
