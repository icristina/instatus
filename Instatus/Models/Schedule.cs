using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Models
{
    public class Schedule
    {
        public DateTime Publish { get; set; }
        public DateTime Start { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }
        public DateTime End { get; set; }
        public DateTime Archive { get; set; }

        public Recurrence Recurrence { get; set; }

        public Schedule()
        {
            var now = DateTime.UtcNow;
            var twelveMonths = now.AddYears(1);

            Publish = now;
            Start = now;
            Open = now;
            Close = twelveMonths;
            End = twelveMonths;
            Archive = twelveMonths;

            Recurrence = Recurrence.Single;      
        }
    }
}
