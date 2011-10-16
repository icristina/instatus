using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public enum WebVerb
    {
        // reading
        View,
        Save,
        
        // collaboration
        Change,

        // social
        Post,
        Like,
        Checkin,
        Comment,
        Status,
        Report,
        Share,
        Subscribe,

        // community level
        Reputation,

        // gaming
        Award,
        Highscore,

        // system
        Error
    }
}