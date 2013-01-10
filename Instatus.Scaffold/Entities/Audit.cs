using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Audit : ICreated, IPayload
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Uri { get; set; }

        // ICreated
        public DateTime Created { get; set; }

        // IPayload
        public string Data { get; set; }

        // Associations
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public Audit()
        {
            Created = DateTime.UtcNow;
        }
    }
}