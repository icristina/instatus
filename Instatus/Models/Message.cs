using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Instatus.Models
{
    [KnownType(typeof(Comment))]
    [KnownType(typeof(Review))]
    public class Message : IUserGeneratedContent
    {
        public int Id { get; set; }
        public string ProviderToken { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string Sender { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Status { get; set; }

        public virtual User User { get; set; }
        public int? UserId { get; set; }

        public virtual Page Page { get; set; }
        public int? PageId { get; set; }

        public virtual Activity Activity { get; set; }
        public int? ActivityId { get; set; }
        
        public virtual ICollection<Activity> Activities { get; set; }

        public virtual Message InReplyTo { get; set; }
        public int? InReplyToId { get; set; }

        public virtual ICollection<Message> Replies { get; set; }


        public Message()
        {
            CreatedTime = DateTime.Now;
        }

        public override string ToString()
        {
            return Body;
        }
    }
}