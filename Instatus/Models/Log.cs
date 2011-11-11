using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Verb { get; set; }
        public string Message { get; set; }

        public virtual User User { get; set; }
        public int? UserId { get; set; }

        public virtual Activity Activity { get; set; }
        public int? ActivityId { get; set; }

        public virtual Page Page { get; set; }
        public int? PageId { get; set; }

        public Log()
        {
            CreatedTime = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}