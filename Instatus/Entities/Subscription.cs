using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public User User { get; set; }
        public Page Page { get; set; }

        public Subscription()
        {
            CreatedTime = DateTime.UtcNow;
        }
    }
}
