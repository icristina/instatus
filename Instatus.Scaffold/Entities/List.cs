using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class List
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public List()
        {
            // initialize collections
            Subscriptions = new List<Subscription>();
        }
    }
}