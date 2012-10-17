using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Instatus.Integration.Azure
{
    public static class AzureExtensions
    {
        public static Task<bool> CreateTableIfNotExistAsync(this CloudTableClient client, string tableName)
        {
            return Task.Factory.FromAsync<string, bool>(client.BeginCreateTableIfNotExist, client.EndCreateTableIfNotExist, tableName, null);
        }

        public static Task<DataServiceResponse> SaveChangesWithRetriesAsync(this TableServiceContext context)
        {
            return Task.Factory.FromAsync<SaveChangesOptions, DataServiceResponse>(context.BeginSaveChangesWithRetries, context.EndSaveChanges, SaveChangesOptions.Batch, null);
        }
    }
}
