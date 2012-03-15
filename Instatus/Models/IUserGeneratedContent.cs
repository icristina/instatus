using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using Instatus.Web;

namespace Instatus.Models
{
    public interface IUserGeneratedContent : ITimestamp
    {
        string Status { get; set; }
        User User { get; set; }
        ICollection<Message> Replies { get; set; }
        ICollection<Activity> Activities { get; set; }
        IDictionary<WebVerb, WebInsight> Insights { get; }
    }
}