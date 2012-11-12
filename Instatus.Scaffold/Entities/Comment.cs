using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Comment : ICreated, IModerated
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        // ICreated
        public DateTime Created { get; set; }

        // IModerated
        public State State { get; set; }

        // Associations
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public Comment()
        {
            Created = DateTime.UtcNow;
            State = State.Approved;
        }
    }
}