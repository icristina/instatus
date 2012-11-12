using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public interface IScheduled
    {
        DateTime Publish { get; set; }
        DateTime Open { get; set; }
        DateTime Close { get; set; }
        DateTime Archive { get; set; }
    }
}