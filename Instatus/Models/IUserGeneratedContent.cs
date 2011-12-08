using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;

namespace Instatus.Models
{
    public interface IUserGeneratedContent
    {
        string Status { get; set; }
        DateTime CreatedTime { get; set; }
        User User { get; set; }
        ICollection<Message> Replies { get; set; }
        ICollection<Activity> Activities { get; set; }
        Dictionary<WebVerb, WebInsight> Insights { get; }
    }
}