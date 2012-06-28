using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryCampaign : ICampaign
    {
        private ISchedule schedule;
        
        public ISchedule GetSchedule()
        {
            return schedule;
        }

        public void Subscribe(IUser user)
        {
            throw new NotImplementedException();
        }

        public InMemoryCampaign(ISchedule schedule) 
        {
            this.schedule = schedule;
        }
    }

    public class InMemorySchedule : ISchedule
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public InMemorySchedule(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
