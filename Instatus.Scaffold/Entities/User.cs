﻿using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class User : ICreated
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string FacebookId { get; set; }
        public string TwitterId { get; set; }
        public string Locale { get; set; }
        public DateTime Created { get; set; }
        public Role Role { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
        public virtual ICollection<Marker> Markers { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public Address Address { get; set; }
        public bool IsOptedIn { get; set; }
        public bool IsVerified { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }

        public User()
        {
            Created = DateTime.UtcNow;
            Address = new Address();
            Role = Role.Member;
        }
    }
}