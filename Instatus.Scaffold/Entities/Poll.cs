﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Poll : IPage
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

        public virtual ICollection<Vote> Votes { get; set; }

        public Poll()
        {
            Publish = new DateRange();
            Created = DateTime.UtcNow;
        }
    }
}