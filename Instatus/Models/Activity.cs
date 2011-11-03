using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Instatus.Web;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Web.Mvc;

namespace Instatus.Models
{
    [KnownType(typeof(Award))]
    [KnownType(typeof(Highscore))]
    [KnownType(typeof(Checkin))]
    [KnownType(typeof(Coupon))]
    public class Activity : IUserGeneratedContent
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Verb { get; set; }
        public string Message { get; set; }
        public int? Rating { get; set; }
        public string Permissions { get; set; }
        public string Status { get; set; }
        public string Data { get; set; }

        public virtual Source Source { get; set; }
        public int? SourceId { get; set; }
        
        public virtual User User { get; set; }
        public int? UserId { get; set; }

        public virtual Page Page { get; set; }
        public int? PageId { get; set; }

        public virtual ICollection<Message> Replies { get; set; }
        public virtual ICollection<Log> Logs { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Activity> ParentActivities { get; set; }

        [NotMapped]
        public dynamic Extensions { get; set; }

        public Activity()
        {
            CreatedTime = DateTime.Now;
            Extensions = new ExpandoObject();
            Status = WebStatus.Published.ToString();
        }

        public Activity(WebVerb verb, Page parent, User user = null)
            : this()
        {
            Verb = verb.ToString();
            Page = parent;
            User = user;
        }

        public override string ToString()
        {
            return string.Format("{0} at {1}", Verb, CreatedTime);
        }
    }
}