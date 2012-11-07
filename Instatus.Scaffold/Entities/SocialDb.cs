using Instatus.Core;
using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Instatus.Core.Extensions;

namespace Instatus.Scaffold.Entities
{
    public class SocialDb : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Marker> Markers { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Taxonomy> Taxonomies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        
        public SocialDb(string connectionName)
            : base(connectionName)
        {

        }
    }
}