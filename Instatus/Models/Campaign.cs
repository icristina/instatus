using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Models
{
    public class Campaign
    {
        public int Id { get; set; }

        // content
        public string Title { get; set; }

        // publishing
        public string Locale { get; set; }
        public State State { get; set; }
        public DateTime Created { get; set; }

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
        public Schedule Schedule { get; set; }

        // restrictions
        public string LocaleRestriction { get; set; }
        public int AgeRestriction { get; set; }
        public bool EnableMultipleEntries { get; set; }

        // associations
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public Campaign()
        {
            Created = DateTime.UtcNow;
            Schedule = new Schedule();

            // initialize collections
            Pages = new List<Page>();
            Entries = new List<Entry>();
        }
    }
}
