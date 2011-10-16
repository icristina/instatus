using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Comment : Message
    {
        public virtual Page Page { get; set; }
        public int? PageId { get; set; }
        
        public virtual ICollection<Activity> Activities { get; set; }
    }
}