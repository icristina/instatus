using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual Event Event { get; set; }
        public int EventId { get; set; }

        public Schedule() { }

        public Schedule(DateTime startTime)
            : this(startTime, null)
        {

        }

        public Schedule(DateTime startTime, DateTime? endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public override string ToString()
        {
            return StartTime.ToLongTimeString();
        }
    }
}