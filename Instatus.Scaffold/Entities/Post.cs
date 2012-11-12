using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Post : IPage, IModerated, ICreated, IPayload
    {
        public int Id { get; set; }

        // IPage
        public string Slug { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public string Category { get; set; }

        // IModerated
        public State State { get; set; }

        // ICreated
        public DateTime Created { get; set; }

        // IPayload
        public string Data { get; set; }

        // Associations
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int? CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        public Post()
        {
            Created = DateTime.UtcNow;
        }
    }
}