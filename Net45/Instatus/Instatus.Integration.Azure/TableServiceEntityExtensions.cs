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
        public static TableServiceEntity WithSinglePartitionKey(this TableServiceEntity tableServiceEntity, string partitionKey = null)
        {
            tableServiceEntity.PartitionKey = partitionKey ?? tableServiceEntity.GetType().Name;
            return tableServiceEntity;
        }

        public static TableServiceEntity WithAnnualPartionKey(this TableServiceEntity tableServiceEntity, DateTime? dateTime = null)
        {
            tableServiceEntity.PartitionKey = string.Format(WellKnown.FormatString.Year, dateTime ?? DateTime.UtcNow);
            return tableServiceEntity;
        }

        public static TableServiceEntity WithMonthlyPartionKey(this TableServiceEntity tableServiceEntity, DateTime? dateTime = null)
        {
            tableServiceEntity.PartitionKey = string.Format(WellKnown.FormatString.Month, dateTime ?? DateTime.UtcNow);
            return tableServiceEntity;
        }

        public static TableServiceEntity WithDailyPartionKey(this TableServiceEntity tableServiceEntity, DateTime? dateTime = null)
        {
            tableServiceEntity.PartitionKey = string.Format(WellKnown.FormatString.Date, dateTime ?? DateTime.UtcNow);
            return tableServiceEntity;
        }

        public static TableServiceEntity WithAscendingRowKey(this TableServiceEntity tableServiceEntity, DateTime? dateTime = null)
        {
            tableServiceEntity.RowKey = string.Format(WellKnown.FormatString.TimestampAndGuid, dateTime ?? DateTime.UtcNow, Guid.NewGuid());
            return tableServiceEntity;
        }

        public static TableServiceEntity WithDescendingRowKey(this TableServiceEntity tableServiceEntity, DateTime? dateTime = null)
        {
            tableServiceEntity.RowKey = string.Format("{0:10}-{1}", (DateTime.MaxValue.Ticks - (dateTime ?? DateTime.UtcNow).Ticks), Guid.NewGuid());
            return tableServiceEntity;
        }
    }
}
