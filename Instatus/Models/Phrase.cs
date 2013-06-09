using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Models
{
    public class Phrase
    {
        public int Id { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        // associations
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}
