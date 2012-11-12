using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Marker : ICreated, IModerated
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DbGeography Point { get; set; }

        // ICreated
        public DateTime Created { get; set; }

        // IModerated
        public State State { get; set; }

        // Associations
        public int MapId { get; set; }
        public virtual Map Map { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public Marker()
        {
            Created = DateTime.UtcNow;
            State = State.Approved;
        }
    }
}