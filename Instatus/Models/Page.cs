using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Models
{
    public class Page
    {
        public int Id { get; set; }
        public Kind Kind { get; set; }

        // content
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public string Text { get; set; }

        // publishing
        public string Locale { get; set; }
        public State State { get; set; }
        public DateTime Created { get; set; }

        // schedule
        public Schedule Schedule { get; set; }

        // associations
        public int? UserId { get; set; }
        [IgnoreDataMember]
        public virtual User User { get; set; }
        public int? CampaignId { get; set; }
        [IgnoreDataMember]
        public virtual Campaign Campaign { get; set; }
        public int? PlaceId { get; set; }
        [IgnoreDataMember]
        public virtual Place Place { get; set; }
        [IgnoreDataMember]
        public virtual ICollection<Tag> Tags { get; set; }
        [IgnoreDataMember]
        public virtual ICollection<Comment> Comments { get; set; }

        public Page()
        {
            Created = DateTime.UtcNow;
            Schedule = new Schedule();
            
            // initialize collections
            Tags = new List<Tag>();
            Comments = new List<Comment>();
        }
    }
}
