using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public enum Step
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