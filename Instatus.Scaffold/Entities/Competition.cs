using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Competition : IPage
    {
        // IPage
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public string Locale { get; set; }
        public string Category { get; set; }
        public DateRange Publish { get; set; }
        
        // ICreated
        public DateTime Created { get; set; }

        // IPayload
        public string Data { get; set; }

        public DateRange Valid { get; set; }

        public virtual ICollection<Entry> Entries { get; set; }

        public Competition()
        {
            Publish = new DateRange();
            Created = DateTime.UtcNow;
            Valid = new DateRange();
        }
    }
}