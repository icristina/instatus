using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public enum WebStatus
    {
        Draft,
        PendingApproval,
        Approved,
        Published,
        Archived,
        Spam, // automatically flagged
        Reported, // user reported
        Suspended, // temporary ban
        Banned, // permanent ban
        Deleted
    }
}