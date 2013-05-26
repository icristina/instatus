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

        // competition and recurrence
        public Recurrence Recurrence { get; set; }
        public DateTime Draw { get; set; }
        public bool EnableMultipleEntries { get; set; }

        // social
        public string GoogleAnalyticsProfileId { get; set; }
        public string FacebookUri { get; set; }
        public string FacebookAppId { get; set; }
        public string YouTubeUri { get; set; }
        public string TwitterUri { get; set; }
        public string TwitterHashTag { get; set; }

        // target
        public string GeographicRestriction { get; set; }
        public int AgeRestriction { get; set; }

        // legal
        public string TermsUri { get; set; }
        public string PrivacyUri { get; set; }

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
        public virtual ICollection<Message> Messages { get; set; }

        public Campaign()
        {
            Draw = SqlDateTime.MaxValue.Value;
            Recurrence = Recurrence.Single;
            EnableMultipleEntries = true;

            Publish = SqlDateTime.MinValue.Value;
            Open = SqlDateTime.MinValue.Value;
            Close = SqlDateTime.MaxValue.Value;
            Archive = SqlDateTime.MaxValue.Value;
            State = State.Approved;
            Created = DateTime.UtcNow;
            Mechanic = Mechanic.Promotion;
            Gate = Gate.None;

            // initialize collections
            Posts = new List<Post>();
            Scores = new List<Score>();
            Votes = new List<Vote>();
            Entries = new List<Entry>();
            Maps = new List<Map>();
            Invites = new List<Invite>();
            Messages = new List<Message>();
        }
    }
}