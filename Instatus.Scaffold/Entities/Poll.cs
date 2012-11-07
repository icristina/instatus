using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Poll : IPage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public DateTime Active { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Locale { get; set; }
        public string Category { get; set; }
        public DateTime Created { get; set; }
        public string Data { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}