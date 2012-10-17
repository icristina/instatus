using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Models
{
    public class Campaign
    {
        public string Name { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public Campaign(string name, DateTime startTime, DateTime endTime)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
