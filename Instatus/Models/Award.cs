using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Web;

namespace Instatus.Models
{
    public class Award : Activity
    {
        public virtual Achievement Achievement { get; set; }
        public int? AchievementId { get; set; }        
        
        public Award()
            : base()
        {
            Verb = WebVerb.Award.ToString();
        }
    }
}
