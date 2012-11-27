using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class User : ICreated, IPayload
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string FacebookId { get; set; }
        public string TwitterId { get; set; }
        public string Locale { get; set; }
        public string Picture { get; set; }
        public Role Role { get; set; }
        public Address Address { get; set; }
        public bool IsOptedIn { get; set; }
        public bool IsVerified { get; set; }
        public string VerificationToken { get; set; }
        public string Password { get; set; }

        // ICreated
        public DateTime Created { get; set; }

        // IPayload
        public string Data { get; set; }

        // Associations
        public virtual ICollection<Vote> Votes { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
        public virtual ICollection<Marker> Markers { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public User()
        {
            Created = DateTime.UtcNow;
            Address = new Address();
            Role = Role.Member;

            // initialize collections
            Votes = new List<Vote>();
            Scores = new List<Score>();
            Markers = new List<Marker>();
            Entries = new List<Entry>();
            Posts = new List<Post>();
            Likes = new List<Like>();
            Comments = new List<Comment>();
            Subscriptions = new List<Subscription>();
            Invites = new List<Invite>();
            Messages = new List<Message>();
        }
    }
}