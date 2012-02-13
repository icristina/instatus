using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Instatus.Web
{
    public enum WebVerb
    {
        // general
        [Description("viewed")] 
        View,    
        [Description("is interested in")]
        News,    

        // reading        
        [Description("read")]
        Read,
        [Description("saved")] 
        Save,
        
        // collaboration
        [Description("updated")]
        Change,
        [Description("deleted")]
        Delete,

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
        [Description("achieved a highscore")]
        Highscore,

        // system
        [Description("error")]
        Error
    }
}