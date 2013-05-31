using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public bool IsVoid { get; set; }
        public bool IsWinner { get; set; }
        public string ProofOfPurchase { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
        public int? Answer { get; set; }
        public int? Score { get; set; }
        public State State { get; set; }

        // associations
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public Entry()
        {
            Created = DateTime.UtcNow;
        }
    }
}
