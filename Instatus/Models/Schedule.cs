using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlTypes;

namespace Instatus.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Data { get; set; }

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
            if (EndTime.HasValue)
                return string.Format("{0} - {1}", StartTime.ToString("f"), EndTime.Value.ToString("f"));
            
            return StartTime.ToString("f");
        }

        public static Schedule Permanent {
            get
            {
                return new Schedule(SqlDateTime.MinValue.Value, SqlDateTime.MaxValue.Value);
            }
        }
    }
}