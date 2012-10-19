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
        public static TableServiceEntity WithSinglePartitionKey(this TableServiceEntity entity, string partitionKey = null)
        {
            entity.PartitionKey = partitionKey ?? entity.GetType().Name;
            return entity;
        }

        public static TableServiceEntity WithAnnualPartionKey(this TableServiceEntity entity, DateTime? dateTime = null)
        {
            entity.PartitionKey = string.Format(WellKnown.FormatString.Year, dateTime ?? DateTime.UtcNow);
            return entity;
        }

        public static TableServiceEntity WithMonthlyPartionKey(this TableServiceEntity entity, DateTime? dateTime = null)
        {
            entity.PartitionKey = string.Format(WellKnown.FormatString.Month, dateTime ?? DateTime.UtcNow);
            return entity;
        }

        public static TableServiceEntity WithDailyPartionKey(this TableServiceEntity entity, DateTime? dateTime = null)
        {
            entity.PartitionKey = string.Format(WellKnown.FormatString.Date, dateTime ?? DateTime.UtcNow);
            return entity;
        }

        public static TableServiceEntity WithEmptyPartionKey(this TableServiceEntity entity)
        {
            entity.PartitionKey = string.Empty;
            return entity;
        }

        public static TableServiceEntity WithAscendingRowKey(this TableServiceEntity entity, DateTime? dateTime = null)
        {
            entity.RowKey = string.Format(WellKnown.FormatString.TimestampAndGuid, dateTime ?? DateTime.UtcNow, Guid.NewGuid());
            return entity;
        }

        public static TableServiceEntity WithDescendingRowKey(this TableServiceEntity entity, DateTime? dateTime = null)
        {
            entity.RowKey = string.Format("{0:10}-{1}", (DateTime.MaxValue.Ticks - (dateTime ?? DateTime.UtcNow).Ticks), Guid.NewGuid());
            return entity;
        }
    }
}
