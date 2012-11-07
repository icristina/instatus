using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Marker : ICreated
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int MapId { get; set; }
        public virtual Map Map { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime Created { get; set; }
        public DbGeography Point { get; set; }

        public Marker()
        {
            Created = DateTime.UtcNow;
        }
    }
}