using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Entry : ICreated, IPayload
    {
        public int Id { get; set; }

        // competition
        public bool IsVoid { get; set; }
        public bool IsWinner { get; set; }

        // entry details
        public string ProofOfPurchase { get; set; }
        public string Text { get; set; }
        public int? Answer { get; set; }
        public int? Score { get; set; }

        // ICreated
        public DateTime Created { get; set; }

        // IPayload
        public string Data { get; set; }

        // Associations
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int? PostId { get; set; }
        public virtual Post Post { get; set; }

        public Entry()
        {
            IsVoid = false;
            IsWinner = false;
            Created = DateTime.UtcNow;
        }
    }
}