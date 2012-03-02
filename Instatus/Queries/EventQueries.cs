using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Data;

namespace Instatus
{
    public static class EventQueries
    {
        public static bool IsScheduledNow(this Event evnt)
        {
            return evnt.IsScheduledForDate(DateTime.UtcNow);
        }
        
        public static bool IsScheduledForDate(this Event evnt, DateTime dateTime)
        {
            return evnt.Dates.Any(d => d.StartTime <= dateTime && d.EndTime.HasValue && d.EndTime >= dateTime);
        }
    }
}