using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Invite
    {
        // Associations
        [Key, Column(Order = 0)]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}