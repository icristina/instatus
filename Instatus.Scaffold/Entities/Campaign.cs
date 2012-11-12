using Instatus.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Campaign : IPage, IScheduled, IModerated, ICreated, IPayload
    {
        public int Id { get; set; }
        public Mechanic Mechanic { get; set; }
        public Gate Gate { get; set; }
        
        // IPage
        public string Slug { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public string Category { get; set; }
        
        // IScheduled
        public DateTime Publish { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }
        public DateTime Archive { get; set; }

        // IModerated
        public State State { get; set; }

        // ICreated
        public DateTime Created { get; set; }

        // IPayload
        public string Data { get; set; }

        // Associations
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
        public virtual ICollection<Map> Maps { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }

        public Campaign()
        {
            
            Publish = SqlDateTime.MinValue.Value;
            Open = SqlDateTime.MinValue.Value;
            Close = SqlDateTime.MaxValue.Value;
            Archive = SqlDateTime.MaxValue.Value;
            State = State.Approved;
            Created = DateTime.UtcNow;
            Mechanic = Mechanic.Promotion;
            Gate = Gate.None;
        }
    }
}