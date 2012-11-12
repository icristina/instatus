using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Score : ICreated
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public int Time { get; set; }

        // ICreated
        public DateTime Created { get; set; }

        // Associations
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        public Score()
        {
            Created = DateTime.UtcNow;
        }
    }
}