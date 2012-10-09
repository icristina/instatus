using Instatus.Core;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Integration.Azure
{
    public static class TableServiceEntityExtensions
    {
        public static TableServiceEntity SetSinglePartitionKey(this TableServiceEntity tableServiceEntity)
        {
            tableServiceEntity.PartitionKey = tableServiceEntity.GetType().Name;
            return tableServiceEntity;
        }
        
        public static TableServiceEntity SetMonthPartionKey(this TableServiceEntity tableServiceEntity, DateTime? dateTime = null)
        {
            tableServiceEntity.PartitionKey = string.Format(WellKnown.FormatString.Month, dateTime ?? DateTime.UtcNow);
            return tableServiceEntity;
        }

        public static TableServiceEntity SetAscendingRowKey(this TableServiceEntity tableServiceEntity, DateTime? dateTime = null)
        {
            tableServiceEntity.RowKey = string.Format(WellKnown.FormatString.TimestampAndGuid, dateTime ?? DateTime.UtcNow, Guid.NewGuid());
            return tableServiceEntity;
        }

        public static TableServiceEntity SetDescendingRowKey(this TableServiceEntity tableServiceEntity, DateTime? dateTime = null)
        {
            tableServiceEntity.RowKey = string.Format("{0}-{1}", (DateTime.MaxValue.Ticks - (dateTime ?? DateTime.UtcNow).Ticks), Guid.NewGuid());
            return tableServiceEntity;
        }
    }
}
