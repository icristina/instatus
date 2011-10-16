using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Event : Page
    {
        public virtual Place Place { get; set; }
        public int? PlaceId { get; set; }

        public virtual ICollection<Schedule> Dates { get; set; }    
    }
}