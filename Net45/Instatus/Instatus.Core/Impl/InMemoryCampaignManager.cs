using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryCampaignManager : ICampaignManager
    {
        private IEnumerable<InMemoryCampaign> campaigns;
        
        public object GetActiveCampaign()
        {
            var now = DateTime.UtcNow;
            return campaigns.Where(c => c.StartTime <= now && c.EndTime >= now).FirstOrDefault(); 
        }

        public InMemoryCampaignManager(IEnumerable<InMemoryCampaign> campaigns) 
        {
            this.campaigns = campaigns;
        }
    }

    public class InMemoryCampaign
    {
        public string Name { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public InMemoryCampaign(string name, DateTime startTime, DateTime endTime)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
