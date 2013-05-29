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

        // configuration
        public string GoogleAnalyticsId { get; set; }
        public string FacebookAppId { get; set; }

        // social
        public string FacebookUri { get; set; }
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
        public DateTime Draw { get; set; }
        public Recurrence Recurrence { get; set; }
        public DateTime Close { get; set; }
        public DateTime Archive { get; set; }

        // restrictions
        public string LocaleRestriction { get; set; }
        public int AgeRestriction { get; set; }
        public bool EnableMultipleEntries { get; set; }

        // associations
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }

        public Campaign()
        {
            var now = DateTime.UtcNow;
            var twelveMonths = now.AddYears(1);

            Publish = now;
            Open = now;
            Draw = twelveMonths;
            Recurrence = Recurrence.Single;
            Close = twelveMonths;
            Archive = twelveMonths;

            // initialize collections
            Posts = new List<Post>();
            Entries = new List<Entry>();
        }
    }
}
