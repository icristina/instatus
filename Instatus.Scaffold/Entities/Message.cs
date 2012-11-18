using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Message : ICreated, IPayload
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        // ICreated
        public DateTime Created { get; set; }

        // IPayload
        public string Data { get; set; }

        // Associations
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public Message()
        {
            Created = DateTime.UtcNow;
        }
    }
}