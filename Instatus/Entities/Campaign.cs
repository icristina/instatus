using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Campaign
    {
        public int Id { get; set; }

        public string Title { get; set; }

        // features
        public Mechanic Mechanic { get; set; }
        public Gate Gate { get; set; }
        public Recurrence Recurrence { get; set; }
        public bool EnableMultipleEntries { get; set; }

        // social
        public string GoogleAnalyticsProfileId { get; set; }
        public string FacebookUri { get; set; }
        public string FacebookAppId { get; set; }
        public string YouTubeUri { get; set; }
        public string TwitterUri { get; set; }
        public string TwitterHashTag { get; set; }

        // links
        public string HomeUri { get; set; }
        public string TermsUri { get; set; }
        public string PrivacyUri { get; set; }

        // schedule
        public DateTime Publish { get; set; }
        public DateTime Open { get; set; }
        public DateTime Repeat { get; set; }
        public DateTime Close { get; set; }
        public DateTime Archive { get; set; }

        // associations
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
    }
}
