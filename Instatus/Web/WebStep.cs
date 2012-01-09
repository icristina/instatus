using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Instatus.Web
{
    public enum WebStep
    {
        Start,
        Teaser,
        Error,
        Confirm,
        Complete,
        Cancelled,
        Closed
    } 
}