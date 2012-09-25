using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryCampaignManager : ICampaignManager
    {
        private IEnumerable<Campaign> campaigns;
        
        public Campaign GetActiveCampaign()
        {
            var now = DateTime.UtcNow;
            return campaigns.Where(c => c.StartTime <= now && c.EndTime >= now).FirstOrDefault(); 
        }

        public InMemoryCampaignManager(IEnumerable<Campaign> campaigns) 
        {
            this.campaigns = campaigns;
        }
    }
}
