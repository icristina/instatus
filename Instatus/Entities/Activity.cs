using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Data;

namespace Instatus.Entities
{
    public class Activity : ITimestamp
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
        public Page Page { get; set; }
        public int Rating { get; set; }
        public int Score { get; set; }
        public int Duration { get; set; }
        public Location Location { get; set; }
        public Application Application { get; set; }
#if NET45
        public Verb Verb { get; set; }
#else
        public string Verb { get; set; }
#endif

        public override string ToString()
        {
            return Description ?? base.ToString();
        }

        public Activity()
        {
            CreatedTime = DateTime.UtcNow;
            Location = new Location();
        }
    }

    public enum Verb
    {
        Award,
        Highscore,
        Checkin,
        Coupon,
        Journey,
        Vote,
        Like,
        Read,
        Post,
        Custom1,
        Custom2,
        Custom3
    }
}
