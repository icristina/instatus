using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Instatus.Web
{
    public enum WebVerb
    {
        // reading        
        [Description("viewed")] 
        View,
        [Description("saved")] 
        Save,
        
        // collaboration
        [Description("updated")]
        Change,

        // social general
        [Description("is interested in")]
        News,

        // social action
        [Description("posted")]
        Post,
        [Description("liked")]
        Like,
        [Description("checked into")]
        Checkin,
        [Description("commented on")]
        Comment,
        [Description("updated status")]
        Status,
        [Description("reported")]
        Report,
        [Description("shared")]
        Share,
        [Description("subscribed to")]
        Subscribe,
        [Description("travelled")]
        Journey,
        [Description("mentioned")]
        Mention,
        [Description("invited")]
        Invite,
        [Description("friended")]
        Friend,
        [Description("voted for")]
        Vote,
        [Description("earned")]
        Reputation,
        [Description("notified")]
        Notification,

        // gaming
        [Description("awarded")]
        Award,
        [Description("achieved")]
        Highscore,

        // system
        [Description("error")]
        Error
    }
}